using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AlphaOneA.Model;
using System.Data.SqlClient;

namespace AlphaOneA
{ public class SqliteDatabaseService
    {
        private readonly ILogger<SqliteDatabaseService> _logger;
        private readonly AppDbContext _dbContext;

        string myDb1ConnString = "Server=localhost;Database=mydb;User=root;Password=admin;";

        public SqliteDatabaseService(ILogger<SqliteDatabaseService> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public void DeleteOldData()
        {

/*            MySqlConnection connection = new MySqlConnection(myDb1ConnString);
            connection.Open();
            string sqlTrunc = "TRUNCATE TABLE " + yourTableName
            
            SqlCommand cmd = new SqlCommand(sqlTrunc, connection);
            
            cmd.ExecuteNonQuery();*/

        }

        public void StoreData(string data)
        {
            MySqlConnection connection = new MySqlConnection(myDb1ConnString);
            connection.Open();
            StreamReader reader = new StreamReader(data);
            
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');

                        using (MySqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "INSERT INTO Trade (TradeID, ISIN, Notional) VALUES (@col1, @col2, @col3)";
                            command.Parameters.AddWithValue("@col1", values[0]);
                            command.Parameters.AddWithValue("@col2", values[1]);
                            command.Parameters.AddWithValue("@col3", values[2]);

                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    Console.WriteLine("CSV data has been inserted into the database.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("An error occurred: " + ex.Message);
                }

            }
            connection.Close();
            reader.Close();
        }
        } } 
