using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace Hurace.Dal.Common
{
    public class AdoTemplate
    {
        private readonly IConnectionFactory connectionFactory;

        public IConnectionFactory ConnectionFactory => connectionFactory;

        public AdoTemplate(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        private void AddParameters(DbCommand command, QueryParameter[] parameters)
        {
            foreach (var p in parameters)
            {
                DbParameter dbParam = command.CreateParameter();
                dbParam.ParameterName = p.Name;
                dbParam.Value = p.Value;
                command.Parameters.Add(dbParam);
            }
        }

        public IEnumerable<T> Query<T>(string sql, RowMapper<T> rowMapper, params QueryParameter[] parameters)
        {
            using DbConnection connection = connectionFactory.CreateConnection();
            using DbCommand command = connection.CreateCommand();
            command.CommandText = sql;
            AddParameters(command, parameters);

            var items = new List<T>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    items.Add(rowMapper(reader));
                }
            }
            return items;
        }

        public T QueryById<T>(string sql, RowMapper<T> rowMapper, params QueryParameter[] parameters)
        {
            return Query(sql, rowMapper, parameters).SingleOrDefault();
        }

        public int Execute(string sql, params QueryParameter[] parameters)
        {
            int retval = 0;
            using DbConnection connection = connectionFactory.CreateConnection();
            connection.Open();

            using DbCommand command = connection.CreateCommand();
            command.Connection = connection;
            DbTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;
                    
            try
            {
                command.CommandText = sql;
                AddParameters(command, parameters);
                retval = Convert.ToInt32(command.ExecuteScalar());
                transaction.Commit();
                connection.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);

                // Attempt to roll back the transaction.
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
            }
                    
            return retval;
        }
    }
}
