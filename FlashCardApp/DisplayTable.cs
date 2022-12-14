using FlashCardApp.Controllers;
using FlashCardApp.Dtos.CardDtos;
using FlashCardApp.Dtos.CardStack;
using FlashCardApp.Dtos.SessionDtos;
using FlashCardApp.Dtos.StudyCardDtos;
using System.Data.SqlClient;

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
                                        Name = stackReader.GetString(1)
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

        internal int DisplayCardList(int currentStackId)
        {
            int listNumber = 1;

            try
            {
                List<CardListReadOnlyDto> tableData = new List<CardListReadOnlyDto>();
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var tableCmd = connection.CreateCommand())
                    {
                        connection.Open();
                        tableCmd.CommandText = $"SELECT * FROM cards WHERE stackId = {currentStackId};";

                        using (var stackReader = tableCmd.ExecuteReader())
                        {
                            

                            if (stackReader.HasRows)
                            {
                                while (stackReader.Read())
                                {

                                    tableData.Add(
                                    new CardListReadOnlyDto
                                    {
                                        Id = listNumber,
                                        Front = stackReader.GetString(1),
                                        Back = stackReader.GetString(2)
                                    });

                                    listNumber++;
                                }
                            }

                            else
                            {
                                Console.WriteLine("\nNo rows found.\n");
                                listNumber = 0;
                            }

                            
                        }
                    }

                    Console.Clear();
                    FormatTable.ShowCardTable(tableData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return listNumber;
        }

        internal void DisplayFrontCard(string stackSelection, string cardFront, string cardBack, int stackSelectionId, int numberCorrect, int numberTotal)
        {
            StudyController studyController = new StudyController();

            List<CardFrontDto> tableData = new List<CardFrontDto>();

            tableData.Add(
            new CardFrontDto
            {
                Front = cardFront
            });

            FormatTable.ShowFrontCard(stackSelection, tableData);
            studyController.GetUserInput("front", stackSelection, cardFront, cardBack, stackSelectionId, numberCorrect, numberTotal);
        }

        internal void DisplayBackCard(string stackSelection, string cardFront, string cardBack, int stackSelectionId, int numberCorrect, int numberTotal)
        {
            StudyController studyController = new StudyController();

            List<CardFrontDto> tableData = new List<CardFrontDto>();

            tableData.Add(
            new CardFrontDto
            {
                Front = cardBack
            });

            FormatTable.ShowFrontCard(stackSelection, tableData);
            studyController.GetUserInput("back", stackSelection, cardFront, cardBack, stackSelectionId, numberCorrect, numberTotal);
        }

        internal void DisplayData()
        {
            try
            {
                List<SessionStatsDto> tableData = new List<SessionStatsDto>();
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var tableCmd = connection.CreateCommand())
                    {
                        connection.Open();
                        tableCmd.CommandText = "SELECT stack, SUM(numbercorrect) as correct, SUM(numbertotal) as total, CAST(CAST(SUM(numbercorrect) as DECIMAL(4,2))/CAST(SUM(numbertotal) as DECIMAL(4,2)) as DECIMAL(6,2))*100 AS Result FROM sessions GROUP BY stack;";

                        using (var stackReader = tableCmd.ExecuteReader())
                        {
                            if (stackReader.HasRows)
                            {
                                while (stackReader.Read())
                                {

                                    tableData.Add(
                                    new SessionStatsDto
                                    {
                                        Stack = stackReader.GetString(0),
                                        Correct = stackReader.GetInt32(1),
                                        Total = stackReader.GetInt32(2),
                                        Percentage = stackReader.GetDecimal(3)
                                    }); ;


                                }
                            }
                            else
                            {
                                Console.WriteLine("\nNo rows found.\n");
                            }
                        }
                    }

                    Console.Clear();
                    FormatTable.ShowSessionTable(tableData);

                    Console.WriteLine("Press Enter to return to Menu...");
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}