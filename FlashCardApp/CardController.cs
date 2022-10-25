using FlashCardApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp
{
    internal class CardController
    {
        private string connectionString = @"Data Source=WILL-PC\NEW2019;Initial Catalog=FlashCardDb;Integrated Security=True";

        public void ManageCards()
        {
            GetUserInput getUserInput = new GetUserInput();
            DisplayTable displayTable = new DisplayTable();
            displayTable.DisplayStack();

            Console.WriteLine("\nWhich stack would you like to manage? Type 0 to go back to Menu.");
            string stackSelection = Console.ReadLine();

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            string insertQuery = $"SELECT * FROM cardstacks;";
            SqlCommand getStackName = new SqlCommand(insertQuery, sqlConnection);
            SqlDataReader reader = getStackName.ExecuteReader();

            List<CardStack> stacks = new List<CardStack>();
            bool cardStackExists = false;
            int stackSelectionId = 0;

            while (reader.Read())
            {
                CardStack cardStack = new CardStack();
                cardStack.CardStackName = (string)reader["CardStackName"];
                cardStack.Id = (int)reader["Id"];
                stacks.Add(cardStack);
            }
            sqlConnection.Close();

            foreach (var cardStack in stacks)
            {
                if (cardStack.CardStackName == stackSelection)
                {
                    cardStackExists = true;
                    stackSelectionId = cardStack.Id;
                }
            }

            while (string.IsNullOrEmpty(stackSelection) || cardStackExists == false)
            {
                Console.WriteLine("\nInvalid Entry. Stack does not exist.\nPress Enter...\n");
                stackSelection = Console.ReadLine();
                ManageCards();
            }

            if (stackSelection == "0")
            {
                getUserInput.MainMenu();
            }

            //sqlConnection = new SqlConnection(connectionString);
            //sqlConnection.Open();

            //string insertIdQuery = $"SELECT stackid FROM cards;";
            //SqlCommand getStackId = new SqlCommand(insertIdQuery, sqlConnection);
            //reader = getStackId.ExecuteReader();

            //while (reader.Read())
            //{
            //    //TODO: CREATE LIST
            //    Card card = new Card();
            //    stackSelectionId = card.CardStackId;
            //}

            //sqlConnection.Close();

            try
            {
                displayTable.DisplayCardList(stackSelectionId);
            }
            catch (Exception)
            {
                Console.WriteLine("\nInvalid Entry. Please enter the stack name or 0 to go back to Menu.\n");
                stackSelection = Console.ReadLine();
            }

            Console.WriteLine("worked");
            Console.ReadLine();

        }
    }
}
