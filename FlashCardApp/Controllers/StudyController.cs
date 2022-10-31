using FlashCardApp.Dtos.CardDtos;
using FlashCardApp.Dtos.StudyCardDtos;
using FlashCardApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp.Controllers
{
    internal class StudyController
    {
        private string connectionString = @"Data Source=WILL-PC\NEW2019;Initial Catalog=FlashCardDb;Integrated Security=True";

        //public void StudySession()
        //{
        //    GetUserInput getUserInput = new GetUserInput();
        //    getUserInput.SelectStack("study");
        //}

        public void StudyCards(string stackSelection, int stackSelectionId)
        {
            GetUserInput getUserInput = new GetUserInput();
            getUserInput.StudyMenu(stackSelection, stackSelectionId);
        }

        public void StudyFront(string stackSelection, int stackSelectionId, int numberCorrect, int numberTotal) 
        {
            DisplayTable displayTable = new DisplayTable();

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
            //int numberCorrect = 0;
            //int numberTotal = 0;


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
                        SqlConnection sqlConnection = new SqlConnection(connectionString);
                        sqlConnection.Open();

                        string insertQuery = $"INSERT INTO sessions(sessiontime,numbercorrect,numbertotal, stack) VALUES ('{sessionTime}',{numberCorrect},{numberTotal}, '{stackSelection}');";
                        SqlCommand insertCard = new SqlCommand(insertQuery, sqlConnection);
                        insertCard.ExecuteNonQuery();
                        sqlConnection.Close();

                        Console.WriteLine($"You got {numberCorrect} right out of {numberTotal}. Press Enter...");
                        Console.ReadLine();

                        StudyCards(stackSelection, stackSelectionId);
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
                        SqlConnection sqlConnection = new SqlConnection(connectionString);
                        sqlConnection.Open();

                        string insertQuery = $"INSERT INTO sessions(sessiontime,numbercorrect,numbertotal, stack) VALUES ('{sessionTime}',{numberCorrect},{numberTotal}, '{stackSelection}');";
                        SqlCommand insertCard = new SqlCommand(insertQuery, sqlConnection);
                        insertCard.ExecuteNonQuery();
                        sqlConnection.Close();

                        Console.WriteLine($"You got {numberCorrect} right out of {numberTotal}. Press Enter...");
                        Console.ReadLine();

                        StudyCards(stackSelection, stackSelectionId);                        
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
                        SqlConnection sqlConnection = new SqlConnection(connectionString);
                        sqlConnection.Open();

                        string insertQuery = $"INSERT INTO sessions(sessiontime,numbercorrect,numbertotal, stack) VALUES ('{sessionTime}',{numberCorrect},{numberTotal}, '{stackSelection}');";
                        SqlCommand insertCard = new SqlCommand(insertQuery, sqlConnection);
                        insertCard.ExecuteNonQuery();
                        sqlConnection.Close();

                        Console.WriteLine($"You got {numberCorrect} right out of {numberTotal}. Press Enter...");
                        Console.ReadLine();

                        StudyCards(stackSelection, stackSelectionId);
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
                        SqlConnection sqlConnection = new SqlConnection(connectionString);
                        sqlConnection.Open();

                        string insertQuery = $"INSERT INTO sessions(sessiontime,numbercorrect,numbertotal, stack) VALUES ('{sessionTime}',{numberCorrect},{numberTotal}, '{stackSelection}');";
                        SqlCommand insertCard = new SqlCommand(insertQuery, sqlConnection);
                        insertCard.ExecuteNonQuery();
                        sqlConnection.Close();

                        Console.WriteLine($"You got {numberCorrect} right out of {numberTotal}. Press Enter...");
                        Console.ReadLine();

                        StudyCards(stackSelection, stackSelectionId);
                    }

                    else
                    {
                        StudyBack(stackSelection, stackSelectionId, numberCorrect, numberTotal);
                    }

                }
            }
        }
    }
}
