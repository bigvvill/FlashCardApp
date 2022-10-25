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
        CodeController codeController = new CodeController();

        internal void MainMenu()
        {            
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
                        ManageStacks();
                        break;
                    //case "2":
                    //    ManageCards();
                    //    break;
                    //case "2":
                    //    StudySession();
                    //    break;
                    //case "3":
                    //    ViewData();
                    //    break;
                    default:
                        Console.WriteLine("\nInvalid Selection. Please type a number from 0 to 3.\nPress Enter...\n");
                        Console.ReadLine();
                        MainMenu();
                        break;
                }
            }            
        }

        public void ManageStacks()
        {
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
                    codeController.CreateStack();
                    break;
                case "2":
                    codeController.DeleteStack();
                    break;
                //case "3":
                //    UpdateStack();
                //    break;
                default:
                    Console.WriteLine("\nInvalid Selection. Please type a number from 0 to 3.\nPress Enter...\n");
                    Console.ReadLine();
                    ManageStacks();
                    break;
            }
        }

        //public void ManageCards()
        //{
        //    GetUserInput getUserInput = new GetUserInput();
        //    DisplayTable displayTable = new DisplayTable();
        //    displayTable.DisplayStack();            

        //    Console.WriteLine("\nWhich stack would you like to delete? Type 0 to go back to Menu.");
        //    string stackSelection = Console.ReadLine();

        //    while (string.IsNullOrEmpty(stackSelection))
        //    {
        //        Console.WriteLine("\nInvalid Entry. Please enter the stack name or 0 to go back to Menu.\n");
        //        stackSelection = Console.ReadLine();
        //    }

        //    if (stackSelection == "0")
        //    {
        //        getUserInput.ManageStacks();
        //    }
        //}


        //}
    }
}
