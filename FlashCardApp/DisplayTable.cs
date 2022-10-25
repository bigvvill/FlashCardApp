using FlashCardApp.Dtos.CardStack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp
{
    internal class DisplayTable
    {
        private string connectionString = @"Data Source=WILL-PC\NEW2019;Initial Catalog=FlashCardDb;Integrated Security=True";

        internal void DisplayStack()
        {
            try
            {
                List<CardStackReadOnlyDto> tableData = new List<CardStackReadOnlyDto>();
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var tableCmd = connection.CreateCommand())
                    {
                        connection.Open();
                        tableCmd.CommandText = "SELECT * FROM cardstacks";

                        using (var stackReader = tableCmd.ExecuteReader())
                        {
                            if (stackReader.HasRows)
                            {
                                while (stackReader.Read())
                                {
                                    tableData.Add(
                                    new CardStackReadOnlyDto
                                    {
                                        CardStackName = stackReader.GetString(1)
                                    });
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nNo rows found.\n");
                            }
                        }
                    }

                    Console.Clear();
                    FormatTable.ShowStackTable(tableData);
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
