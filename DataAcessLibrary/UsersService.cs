using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;
namespace DataAcessLibrary
{
    public class UsersService
    {
        private SqlConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection();
            string connStr = @"Data Source=A-305-04;
                            Initial Catalog=Cloud;
                            Integrated Security=True;
                            MultipleActiveResultSets=True";
            connection.ConnectionString = connStr;
            return connection;
        }

        public void CreateUser(Users user)
        {
            using(SqlConnection connection = CreateConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlCommand command = connection.CreateCommand();
                    SqlParameter nameParameter = command.CreateParameter();
                    nameParameter.DbType = System.Data.DbType.String;
                    nameParameter.IsNullable = false;
                    nameParameter.ParameterName = "@name";
                    nameParameter.Value = user.Name;
                    command.Parameters.Add(nameParameter);

                    SqlParameter passwordParameter = command.CreateParameter();
                    passwordParameter.DbType = System.Data.DbType.String;
                    passwordParameter.IsNullable = false;
                    passwordParameter.ParameterName = "@password";
                    passwordParameter.Value = user.Password;
                    command.Parameters.Add(passwordParameter);

                    SqlParameter ageParameter = command.CreateParameter();
                    ageParameter.DbType = System.Data.DbType.Int32;
                    ageParameter.IsNullable = false;
                    ageParameter.ParameterName = "@age";
                    ageParameter.Value = user.Age;
                    command.Parameters.Add(ageParameter);

                    command.CommandText = @"INSERT INTO [dbo].[users]
                                            ([name],[password],[age]) VALUES (@name,@password,@age)";

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch(SqlException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
    }
}
