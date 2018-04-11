using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;
using System.Data;
using System.IO;

namespace DataAcessLibrary
{
    public class FilesService
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

        public void AddFile()
        {
            using(SqlConnection connection = CreateConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO Files VALUES (@name)";
                    command.Parameters.Add("@name", SqlDbType.Image, 1000000);

                    string filename = @"C:\itstep.bmp";

                    byte[] imageData;
                    using (System.IO.FileStream fs = new System.IO.FileStream(filename, FileMode.Open))
                    {
                        imageData = new byte[fs.Length];
                        fs.Read(imageData, 0, imageData.Length);
                    }
                    // передаем данные в команду через параметры
                    command.Parameters["@name"].Value = imageData;

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
    }
}
