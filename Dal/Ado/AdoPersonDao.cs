﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
 using System.Data;
using System.Data.Common;
using System.Linq;
 using Hurace.Dal.Common;
 using Hurace.Dal.Domain;
 using Hurace.Dal.Interface;

 namespace Hurace.Dal.Ado
{
    public class AdoPersonDao : IPersonDao
    {
        private readonly AdoTemplate template;

        public AdoPersonDao(IConnectionFactory connectionFactory)
        {
            this.template = new AdoTemplate(connectionFactory);
        }

        private Skier MapRowToPerson(IDataRecord row)
        {
            return new Skier
            {
                Id = (int)row["id"],
                FirstName = (string)row["first_name"],
                LastName = (string)row["last_name"],
                DateOfBirth = (DateTime)row["date_of_birth"]
            };
        }

        public bool Update(Skier skier)
        {
            return template.Execute(@"update skier set first_name=@fn, last_name=@ln, date_of_birth=@dob 
                                             where id=@id",
                                             new QueryParameter("@id", skier.Id),
                                             new QueryParameter("@fn", skier.FirstName),
                                             new QueryParameter("@ln", skier.LastName),
                                             new QueryParameter("@dob", skier.DateOfBirth)) == 1;
        }

        public IEnumerable<Skier> FindAll()
        {
            //// @ so we dont need to escape
            ////var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Sebastian\Documents\FH\5_Semester\SWKUE\4_UE\adonet\PersonAdmin\Db\person_db.mdf;Integrated Security=True;Connect Timeout=30";
            ////not good hardcoded connection string
            ////using (DbConnection connection = new Microsoft.Data.SqlClient.SqlConnection())

            ////better: user config, still bad that this is in this method
            //(string connectionString, string providerName) =
            //    ConfigurationUtil.GetConnectionParameters("PersonDbConnection");

            //DbProviderFactory dbFactory = DbUtil.GetDbProviderFactory(providerName);

            //using (DbConnection connection = dbFactory.CreateConnection())
            //{
            //    connection.ConnectionString = connectionString;
            //    connection.Open();

            //    using (DbCommand command = connection.CreateCommand())
            //    {
            //        command.CommandText = "select * from skier";

            //        var items = new List<Skier>();
            //        using (DbDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                items.Add(new Skier
            //                {
            //                    Id = (int)reader["id"],
            //                    FirstName = (string)reader["first_name"],
            //                    LastName = (string)reader["last_name"],
            //                    DateOfBirth = (DateTime)reader["date_of_birth"]
            //                });
            //            }
            //        }
            //        return items;
            //    }
            //}

            return template.Query("select * from skier", MapRowToPerson);
        }

        public Skier FindById(int id)
        {
            return template.QueryById("select * from skier where id=@id",
                MapRowToPerson,
                new QueryParameter("@id", id));
        }
    }
}
