using FlashCardApp.Dtos.CardDtos;
using FlashCardApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp.Controllers
{
    internal class CardController
    {
        private string connectionString = @"Data Source=WILL-PC\NEW2019;Initial Catalog=FlashCardDb;Integrated Security=True";        

        public void CreateCard(string currentStack, int currentStackId)
        {
            GetUserInput getUserInput = new GetUserInput();
            DisplayTable displayTable = new DisplayTable();

            Console.Clear();
            displayTable.DisplayCardList(currentStackId);
            
            Console.WriteLine($"\nCreate new Flashcard in the {currentStack} stack\n");
            Console.WriteLine("Enter question text for front of the card or 0 to go back to Menu");
            string frontText = Console.ReadLine();
            Console.WriteLine("Enter answer text for the back of the card or 0 to go back to Menu");
            string backText = Console.ReadLine();

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            string insertQuery = $"INSERT INTO cards(cardfront,cardback,stackid) VALUES ('{frontText}','{backText}',{currentStackId});"; // TODO : parameters
            SqlCommand insertCard = new SqlCommand(insertQuery, sqlConnection);
            insertCard.ExecuteNonQuery();
            sqlConnection.Close();

            Console.WriteLine("New card created. Press Enter...");
            Console.ReadLine();
            getUserInput.CardMenu(currentStack, currentStackId);
        }

        public void EditCard(string currentStack, int currentStackId)
        {
            GetUserInput getUserInput = new GetUserInput();
            DisplayTable displayTable = new DisplayTable();

            Console.Clear();
            displayTable.DisplayCardList(currentStackId);

            Console.WriteLine("Choose a card Id to edit or 0 to return to Menu");
            string cardToEdit = Console.ReadLine();

            bool isNumber = Int32.TryParse(cardToEdit, out int cardId);

            while (string.IsNullOrEmpty(cardToEdit) || !isNumber)
            {
                Console.WriteLine("\nInvalid Selection. Please type a Card Id or 0 to return to Menu\n");
                cardToEdit = Console.ReadLine();
                isNumber = Int32.TryParse(cardToEdit, out cardId);
            }

            if (cardToEdit == "0")
            {
                getUserInput.CardMenu(currentStack, currentStackId);
            }

            try
            {
                List<CardListEditDto> cardList = new List<CardListEditDto>();
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var tableCmd = connection.CreateCommand())
                    {
                        connection.Open();
                        tableCmd.CommandText = $"SELECT * FROM cards WHERE stackid = {currentStackId};"; // TODO : parameters

                        using (var cardReader = tableCmd.ExecuteReader())
                        {
                            if (cardReader.HasRows)
                            {
                                while (cardReader.Read())
                                {
                                    cardList.Add(
                                    new CardListEditDto
                                    {
                                        Id = cardReader.GetInt32(0),
                                        CardFront = cardReader.GetString(1),
                                        CardBack = cardReader.GetString(2)
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

                    Console.WriteLine("Enter question text for front of the card or Enter to leave as is");
                    Console.WriteLine($"Current Text: {cardList[cardId-1].CardFront}");
                    string frontText = Console.ReadLine();
                    Console.WriteLine("Enter answer text for the back of the card or Enter to leave as is");
                    Console.WriteLine($"Current Text: {cardList[cardId - 1].CardBack}");
                    string backText = Console.ReadLine();

                    if (string.IsNullOrEmpty(frontText) && string.IsNullOrEmpty(backText))
                    {
                        EditCard(currentStack, currentStackId);
                    }

                    else if (string.IsNullOrEmpty(frontText) && !string.IsNullOrEmpty(backText))
                    {
                        string frontQuery = $"UPDATE cards SET cardback = '{backText}' WHERE Id = {cardList[cardId - 1].Id};"; // TODO : parameters
                        SqlCommand insertFront = new SqlCommand(frontQuery, connection);
                        insertFront.ExecuteNonQuery();
                    }

                    else if (!string.IsNullOrEmpty(frontText) && string.IsNullOrEmpty(backText))
                    {
                        string frontQuery = $"UPDATE cards SET cardfront = '{frontText}' WHERE Id = {cardList[cardId - 1].Id};"; // TODO : parameters
                        SqlCommand insertBack = new SqlCommand(frontQuery, connection);
                        insertBack.ExecuteNonQuery();
                    }

                    else 
                    {
                        string frontQuery = $"UPDATE cards SET cardfront = '{frontText}', cardback = '{backText}' WHERE Id = {cardList[cardId - 1].Id};"; // TODO : parameters
                        SqlCommand insertBoth = new SqlCommand(frontQuery, connection);
                        insertBoth.ExecuteNonQuery();
                    }

                    Console.WriteLine("Card Updated. Press Enter...");
                    Console.ReadLine();
                    EditCard(currentStack, currentStackId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        public void DeleteCard(string currentStack, int currentStackId)
        {
            GetUserInput getUserInput = new GetUserInput();
            DisplayTable displayTable = new DisplayTable();

            Console.Clear();
            displayTable.DisplayCardList(currentStackId);

            Console.WriteLine("Choose a card Id to delete or 0 to return to Menu");
            string cardToDelete = Console.ReadLine();

            bool isNumber = Int32.TryParse(cardToDelete, out int cardId);

            while (string.IsNullOrEmpty(cardToDelete) || !isNumber)
            {
                Console.WriteLine("\nInvalid Selection. Please type a Card Id or 0 to return to Menu\n");
                cardToDelete = Console.ReadLine();
                isNumber = Int32.TryParse(cardToDelete, out cardId);
            }

            if (cardToDelete == "0")
            {
                getUserInput.CardMenu(currentStack, currentStackId);
            }

            try
            {
                List<CardListDeleteDto> cardList = new List<CardListDeleteDto>();
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var tableCmd = connection.CreateCommand())
                    {
                        connection.Open();
                        tableCmd.CommandText = $"SELECT * FROM cards WHERE stackid = {currentStackId};"; // TODO : parameters

                        using (var cardReader = tableCmd.ExecuteReader())
                        {
                            if (cardReader.HasRows)
                            {
                                while (cardReader.Read())
                                {
                                    cardList.Add(
                                    new CardListDeleteDto
                                    {
                                        Id = cardReader.GetInt32(0)
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

                    string insertQuery = $"DELETE FROM cards WHERE Id = {cardList[cardId - 1].Id};"; // TODO : parameters
                    SqlCommand insertCard = new SqlCommand(insertQuery, connection);
                    insertCard.ExecuteNonQuery();
                    //connection.Close();

                    Console.WriteLine("Card Deleted. Press Enter...");
                    Console.ReadLine();
                    DeleteCard(currentStack, currentStackId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }            
        }
    }
}
