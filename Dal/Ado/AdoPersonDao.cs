﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
 using System.Data;
using System.Data.Common;
using System.Linq;
 using Hurace.Dal.Common;
 using Hurace.Dal.Interface;
 using Hurace.Domain;

 namespace Hurace.Dal.Ado
{
    public class AdoPersonDao : IPersonDao
    {
        private readonly AdoTemplate template;

        public AdoPersonDao(IConnectionFactory connectionFactory)
        {
            this.template = new AdoTemplate(connectionFactory);
        }

        private Person MapRowToPerson(IDataRecord row)
        {
            return new Person
            {
                Id = (int)row["id"],
                FirstName = (string)row["first_name"],
                LastName = (string)row["last_name"],
                DateOfBirth = (DateTime)row["date_of_birth"]
            };
        }

        public bool Update(Person person)
        {
            return template.Execute(@"update person set first_name=@fn, last_name=@ln, date_of_birth=@dob 
                                             where id=@id",
                                             new QueryParameter("@id", person.Id),
                                             new QueryParameter("@fn", person.FirstName),
                                             new QueryParameter("@ln", person.LastName),
                                             new QueryParameter("@dob", person.DateOfBirth)) == 1;
        }

        public IEnumerable<Person> FindAll()
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
            //        command.CommandText = "select * from person";

            //        var items = new List<Person>();
            //        using (DbDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                items.Add(new Person
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

            return template.Query("select * from person", MapRowToPerson);
        }

        public Person FindById(int id)
        {
            return template.QueryById("select * from person where id=@id",
                MapRowToPerson,
                new QueryParameter("@id", id));
        }
    }
}
