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

        public IEnumerable<T> Query<T>(string sql, RowMapper<T> rowMapper, 
            params QueryParameter[] parameters)
        {
            // @ so we dont need to escape
            //var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Sebastian\Documents\FH\5_Semester\SWKUE\4_UE\adonet\PersonAdmin\Db\person_db.mdf;Integrated Security=True;Connect Timeout=30";
            //not good hardcoded connection string
            //using (DbConnection connection = new Microsoft.Data.SqlClient.SqlConnection())

            //better: user config, still bad that this is in this method, see connection facotry
            //(string connectionString, string providerName) =
            //    ConfigurationUtil.GetConnectionParameters("PersonDbConnection");

            //DbProviderFactory dbFactory = DbUtil.GetDbProviderFactory(providerName);

            using (DbConnection connection = connectionFactory.CreateConnection())
            {
                //connection.ConnectionString = connectionString;
                //connection.Open();

                using (DbCommand command = connection.CreateCommand())
                {
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
            }
        }

        public T QueryById<T>(string sql, RowMapper<T> rowMapper,
            params QueryParameter[] parameters)
        {
            return Query(sql, rowMapper, parameters).SingleOrDefault();
        }

        public int Execute(string sql, params QueryParameter[] parameters)
        {
            using (DbConnection connection = connectionFactory.CreateConnection())
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Transaction = connection.BeginTransaction();
                    DbTransaction transaction = connection.BeginTransaction();
                    AddParameters(command, parameters);

                    int rows = command.ExecuteNonQuery();
                    transaction.Commit();
                    
                    return rows;
                }
            }
        }
    }
}
