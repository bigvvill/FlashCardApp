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

            //Console.WriteLine($"{currentStack} - {currentStackId}");
            Console.WriteLine($"\nCreate new Flashcard in the {currentStack} stack\n");
            Console.WriteLine("Enter question text for front of the card or 0 to go back to Menu");
            string frontText = Console.ReadLine();
            Console.WriteLine("Enter answer text for the back of the card or 0 to go back to Menu");
            string backText = Console.ReadLine();

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            string insertQuery = $"INSERT INTO cards(cardfront,cardback,stackid) VALUES ('{frontText}','{backText}',{currentStackId});";
            SqlCommand insertCard = new SqlCommand(insertQuery, sqlConnection);
            insertCard.ExecuteNonQuery();
            sqlConnection.Close();

            Console.WriteLine("New card created. Press Enter...");
            Console.ReadLine();
            getUserInput.CardMenu(currentStack, currentStackId);
        }

        //TODO : EDIT CARD FUNCTION
        public void EditCard(string currentStack, int currentStackId)
        {
            GetUserInput getUserInput = new GetUserInput();
            DisplayTable displayTable = new DisplayTable();

            Console.Clear();
            displayTable.DisplayCardList(currentStackId);

            Console.WriteLine("Choose a card Id to edit:");
            string cardId = Console.ReadLine();


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
                        tableCmd.CommandText = $"SELECT * FROM cards WHERE stackid = {currentStackId};";

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

                    string insertQuery = $"DELETE FROM cards WHERE Id = {cardList[cardId - 1].Id};";
                    SqlCommand insertCard = new SqlCommand(insertQuery, connection);
                    insertCard.ExecuteNonQuery();
                    connection.Close();

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
