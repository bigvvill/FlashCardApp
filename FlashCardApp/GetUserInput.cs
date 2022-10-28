using FlashCardApp.Controllers;
using FlashCardApp.Dtos.CardStack;
using FlashCardApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp
{
    internal class GetUserInput
    {        
        private string connectionString = @"Data Source=WILL-PC\NEW2019;Initial Catalog=FlashCardDb;Integrated Security=True";

        internal void MainMenu()
        {
            DisplayTable displayTable = new DisplayTable();
            StudyController studyController = new StudyController();

            bool closeApp = false;
            while (!closeApp)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Flashcards! It's STUDY TIME!");
                Console.WriteLine("\n");
                Console.WriteLine("---------------------------");
                Console.WriteLine("0 - Exit");
                Console.WriteLine("1 - Manage Stacks");
                Console.WriteLine("2 - Manage Cards");
                Console.WriteLine("3 - Study");
                Console.WriteLine("4 - View Study Session Data");
                Console.WriteLine("---------------------------");

                string menuSelection = Console.ReadLine();

                while (string.IsNullOrEmpty(menuSelection))
                {
                    Console.WriteLine("\nInvalid Selection. Please type a number from 0 to 3.\n");
                    menuSelection = Console.ReadLine();
                }

                switch (menuSelection)
                {
                    case "0":
                        Console.WriteLine("Good Bye!");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        StackMenu();
                        break;
                    case "2":
                        SelectStack();
                        break;
                    case "3":
                        studyController.StudySession(); // TODO : Create Study Session
                        break;
                    case "4":
                        displayTable.DisplayData();  // TODO : Create Display Data
                        break;
                    default:
                        Console.WriteLine("\nInvalid Selection. Please type a number from 0 to 4.\nPress Enter...\n");
                        Console.ReadLine();
                        MainMenu();
                        break;
                }
            }            
        }

        public void StackMenu()
        {
            StackController stackController = new StackController();

            DisplayTable displayTable = new DisplayTable();
            displayTable.DisplayStack();

            //Console.WriteLine("-------------");
            Console.WriteLine("\n");

            Console.WriteLine("0 - Back To Main Menu");
            Console.WriteLine("1 - Create a New Stack");
            Console.WriteLine("2 - Delete Stack");
            Console.WriteLine("3 - Interact with Stack");

            string stackInput = Console.ReadLine();

            while (string.IsNullOrEmpty(stackInput))
            {
                Console.WriteLine("\nInvalid Selection. Please type a number from 0 to 3.\n");
                stackInput = Console.ReadLine();
            }

            switch (stackInput)
            {
                case "0":
                    MainMenu();
                    break;
                case "1":
                    stackController.CreateStack();
                    break;
                case "2":
                    stackController.DeleteStack();
                    break;
                case "3":
                    SelectStack();
                    break;
                default:
                    Console.WriteLine("\nInvalid Selection. Please type a number from 0 to 3.\nPress Enter...\n");
                    Console.ReadLine();
                    StackMenu();
                    break;
            }
        }        

        public void CardMenu(string stackSelection, int stackSelectionId)
        {
            CardController cardController = new CardController();
            DisplayTable displayTable = new DisplayTable();

            Console.Clear();
            displayTable.DisplayCardList(stackSelectionId);

            Console.WriteLine($"\nCurrent working stack: {stackSelection}\n");

            Console.WriteLine("0 - Return to Main Menu");
            Console.WriteLine("1 - Change Current Stack");
            Console.WriteLine("2 - Create a Flashcard in Current Stack");
            Console.WriteLine("3 - Edit a Flashcard in Current Stack");
            Console.WriteLine("4 - Delete a Flashcard in Current Stack");  

            string cardInput = Console.ReadLine();

            while (string.IsNullOrEmpty(cardInput))
            {
                Console.WriteLine("\nInvalid Selection. Please type a number from 0 to 3.\n");
                cardInput = Console.ReadLine();
            }

            switch (cardInput)
            {
                case "0":
                    MainMenu();
                    break;
                case "1":
                    SelectStack();
                    break;
                case "2":
                    cardController.CreateCard(stackSelection, stackSelectionId);
                    break;
                case "3":
                    cardController.EditCard(stackSelection,stackSelectionId);
                    break;
                case "4":
                    cardController.DeleteCard(stackSelection, stackSelectionId);
                    break;
                default:
                    Console.WriteLine("\nInvalid Selection. Please type a number from 0 to 4.\nPress Enter...\n");
                    Console.ReadLine();
                    StackMenu();
                    break;
            }
        }

        public void SelectStack()
        {
            GetUserInput getUserInput = new GetUserInput();
            DisplayTable displayTable = new DisplayTable();
            displayTable.DisplayStack();

            Console.WriteLine("\nWhich stack would you like to manage? Type 0 to go back to Menu.");
            string stackSelection = Console.ReadLine();

            if (stackSelection == "0")
            {
                getUserInput.MainMenu();
            }

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
                SelectStack();
            }

            try
            {
                displayTable.DisplayCardList(stackSelectionId);
            }
            catch (Exception)
            {
                Console.WriteLine("\nInvalid Entry. Please enter the stack name or 0 to go back to Menu.\n");
                stackSelection = Console.ReadLine();

                if (stackSelection == "0")
                {
                    getUserInput.MainMenu();
                }
            }

            CardMenu(stackSelection, stackSelectionId);
        }

    }
}

