using FlashCardApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp
{
    internal class CodeController
    {
        private string connectionString = @"Data Source=WILL-PC\NEW2019;Initial Catalog=FlashCardDb;Integrated Security=True";

        internal void GetStacks()
        {
            List<CardStack> tableData = new List<CardStack>();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "SELECT * FROM cardstacks";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tableData.Add(
                                new CardStack
                                {
                                    CardStackName = reader.GetString(1)                                    
                                });
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nNo rows found.\n");
                        }
                    }
                }
                Console.WriteLine("\n\n");
            }
            FormatTable.ShowStackTable(tableData);
        }


    }
}
