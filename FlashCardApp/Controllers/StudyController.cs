using FlashCardApp.Models;
using System.Data.SqlClient;


namespace FlashCardApp.Controllers
{
    internal class StudyController
    {
        private string connectionString = @"Data Source=WILL-PC\NEW2019;Initial Catalog=FlashCardDb;Integrated Security=True";

        public void StudyCards(string stackSelection, int stackSelectionId)
        {
            GetUserInput getUserInput = new GetUserInput();
            getUserInput.StudyMenu(stackSelection, stackSelectionId);
        }

        public void StudyFront(string stackSelection, int stackSelectionId, int numberCorrect, int numberTotal)
        {
            DisplayTable displayTable = new DisplayTable();
            GetUserInput getUserInput = new GetUserInput();

            List<Card> cards = new List<Card>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM cards WHERE stackid = {stackSelectionId};";

                    using (var cardReader = tableCmd.ExecuteReader())
                    {
                        if (cardReader.HasRows)
                        {
                            while (cardReader.Read())
                            {
                                cards.Add(
                                new Card
                                {
                                    Id = cardReader.GetInt32(0),
                                    CardFront = cardReader.GetString(1),
                                    CardBack = cardReader.GetString(2),
                                    CardStackId = cardReader.GetInt32(3)
                                });
                            }
                        }

                        else
                        {
                            Console.WriteLine("\nNo rows found.\n");
                            Console.ReadLine();

                            getUserInput.StudyMenu(stackSelection, stackSelectionId);
                        }
                    }
                }
            }
            Random rnd = new Random();
            int currentCard = rnd.Next(cards.Count);

            string cardFront = cards[currentCard].CardFront;
            string cardBack = cards[currentCard].CardBack;

            Console.Clear();
            displayTable.DisplayFrontCard(stackSelection, cardFront, cardBack, stackSelectionId, numberCorrect, numberTotal);
        }

        public void StudyBack(string stackSelection, int stackSelectionId, int numberCorrect, int numberTotal)
        {
            DisplayTable displayTable = new DisplayTable();
            GetUserInput getUserInput = new GetUserInput();

            List<Card> cards = new List<Card>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM cards WHERE stackid = {stackSelectionId};";

                    using (var cardReader = tableCmd.ExecuteReader())
                    {
                        if (cardReader.HasRows)
                        {
                            while (cardReader.Read())
                            {
                                cards.Add(
                                new Card
                                {
                                    Id = cardReader.GetInt32(0),
                                    CardFront = cardReader.GetString(1),
                                    CardBack = cardReader.GetString(2),
                                    CardStackId = cardReader.GetInt32(3)
                                });
                            }
                        }

                        else
                        {
                            Console.WriteLine("\nNo rows found.\n");
                            Console.ReadLine();

                            getUserInput.StudyMenu(stackSelection, stackSelectionId);
                        }
                    }
                }
            }
            Random rnd = new Random();
            int currentCard = rnd.Next(cards.Count);

            string cardFront = cards[currentCard].CardFront;
            string cardBack = cards[currentCard].CardBack;

            Console.Clear();
            displayTable.DisplayBackCard(stackSelection, cardFront, cardBack, stackSelectionId, numberCorrect, numberTotal);
        }

        public void GetUserInput(string cardSide, string stackSelection, string cardFront, string cardBack, int stackSelectionId, int numberCorrect, int numberTotal)
        {
            string playAgain = "";
            DateTime sessionTime = DateTime.Now;

            Console.WriteLine("Input your answer to this card or 0 to go back to Menu");
            string cardAnswer = Console.ReadLine();

            while (string.IsNullOrEmpty(cardAnswer))
            {
                Console.WriteLine("\nInvalid Entry. Input your answer or 0 to go back to Menu.\n");
                cardAnswer = Console.ReadLine();
            }

            if (cardAnswer == "0")
            {
                StudyCards(stackSelection, stackSelectionId);
            }

            if (cardSide == "front")
            {
                if (cardAnswer == cardBack)
                {
                    sessionTime = DateTime.Now;
                    numberCorrect++;
                    numberTotal++;

                    Console.WriteLine("Your answer was correct!\nPress Enter to try another card or 0 to go back to Menu...");
                    playAgain = Console.ReadLine();

                    if (playAgain == "0")
                    {
                        SaveSession(sessionTime, numberCorrect, numberTotal, stackSelection, stackSelectionId);
                    }

                    else
                    {
                        StudyFront(stackSelection, stackSelectionId, numberCorrect, numberTotal);
                    }
                }

                else
                {
                    numberTotal++;

                    Console.WriteLine("Your answer was wrong.\n");
                    Console.WriteLine($"You answered {cardAnswer}\n");
                    Console.WriteLine($"The correct answer was {cardBack}\nPress Enter to try another card or 0 to go back to Menu...");
                    playAgain = Console.ReadLine();

                    if (playAgain == "0")
                    {
                        SaveSession(sessionTime, numberCorrect, numberTotal, stackSelection, stackSelectionId);
                    }

                    else
                    {
                        StudyFront(stackSelection, stackSelectionId, numberCorrect, numberTotal);
                    }
                }
            }

            else
            {
                if (cardAnswer == cardFront)
                {
                    numberCorrect++;
                    numberTotal++;

                    Console.WriteLine("Your answer was correct!\nPress Enter to try another card or 0 to go back to Menu...");
                    playAgain = Console.ReadLine();

                    if (playAgain == "0")
                    {
                        SaveSession(sessionTime, numberCorrect, numberTotal, stackSelection, stackSelectionId);
                    }

                    else
                    {
                        StudyBack(stackSelection, stackSelectionId, numberCorrect, numberTotal);
                    }
                }

                else
                {
                    numberTotal++;

                    Console.WriteLine("Your answer was wrong.\n");
                    Console.WriteLine($"You answered {cardAnswer}\n");
                    Console.WriteLine($"The correct answer was {cardFront}\nPress Enter to try another card or 0 to go back to Menu...");
                    playAgain = Console.ReadLine();


                    if (playAgain == "0")
                    {
                        SaveSession(sessionTime, numberCorrect, numberTotal, stackSelection, stackSelectionId);
                    }

                    else
                    {
                        StudyBack(stackSelection, stackSelectionId, numberCorrect, numberTotal);
                    }
                }
            }
        }

        public void SaveSession(DateTime sessionTime, int numberCorrect, int numberTotal, string stackSelection, int stackSelectionId)
        {
            string commandText = "INSERT INTO sessions(sessiontime,numbercorrect,numbertotal,stack, sessionstackid) VALUES (@sessionTime,@numberCorrect,@numberTotal,@stackSelection, @stackSelectionId);";

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand command = new SqlCommand(commandText, sqlConnection))
                    {
                        command.Parameters.Add(new SqlParameter("@sessionTime", sessionTime));
                        command.Parameters.Add(new SqlParameter("@numberCorrect", numberCorrect));
                        command.Parameters.Add(new SqlParameter("@numberTotal", numberTotal));
                        command.Parameters.Add(new SqlParameter("@stackSelection", stackSelection));
                        command.Parameters.Add(new SqlParameter("@stackSelectionid", stackSelectionId));

                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine($"You got {numberCorrect} right out of {numberTotal}. Press Enter...");
                Console.ReadLine();

                StudyCards(stackSelection, stackSelectionId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}