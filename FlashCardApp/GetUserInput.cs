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
        private SqlConnection sqlConnection;
        private string connectionString = @"Data Source=WILL-PC\NEW2019;Initial Catalog=FlashCardDb;Integrated Security=True";        

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
                Console.WriteLine("2 - Study");
                Console.WriteLine("3 - View Study Session Data");
                Console.WriteLine("---------------------------");

                string menuSelection = Console.ReadLine();

                while (string.IsNullOrEmpty(menuSelection))
                {
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 3.\n");
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
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 3.\n");
                        Console.ReadLine();
                        break;
                }
            }            
        }

        private void ManageStacks()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("---STACKS----");
                Console.WriteLine("-------------");

                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                string displayStackQuery = $"SELECT * FROM cardstacks";
                SqlCommand getStacks = new SqlCommand(displayStackQuery, sqlConnection);
                SqlDataReader reader = getStacks.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader.GetValue(1)}");
                    Console.WriteLine("-------------");
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //Console.WriteLine("-------------");
            Console.WriteLine("\n");

            Console.WriteLine("0 - Back To Main Menu");
            Console.WriteLine("1 - Create a New Stack");
            Console.WriteLine("2 - Delete Stack");
            Console.WriteLine("3 - Interact with Stack");

            string stackInput = Console.ReadLine();

            while (string.IsNullOrEmpty(stackInput))
            {
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 3.\n");
                stackInput = Console.ReadLine();
            }

            switch (stackInput)
            {
                case "0":
                    MainMenu();
                    break;
                case "1":
                    CreateStack();
                    break;
                case "2":
                    DeleteStack();
                    break;
                //case "3":
                //    UpdateStack();
                //    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 3.\n");
                    Console.ReadLine();
                    break;
            }
        }

        private void DeleteStack()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("---STACKS----");
                Console.WriteLine("-------------");

                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                string displayStackQuery = $"SELECT * FROM cardstacks";
                SqlCommand getStacks = new SqlCommand(displayStackQuery, sqlConnection);
                SqlDataReader reader = getStacks.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader.GetValue(1)}");
                    Console.WriteLine("-------------");
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            

            Console.WriteLine("Which stack would you like to delete? Type 0 to go back to Menu.");
            string stackToDelete = Console.ReadLine();

            while (string.IsNullOrEmpty(stackToDelete))
            {
                Console.WriteLine("\nInvalid Command. Please enter the stack name or 0 to go back to Menu.\n");
                stackToDelete = Console.ReadLine();
            }

            try
            {

                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();



                string displayStackQuery = $"DELETE FROM cardstacks WHERE cardstackname ='{stackToDelete}' select @@ROWCOUNT;";
                SqlCommand getStacks = new SqlCommand(displayStackQuery, sqlConnection); 
                int rc = getStacks.ExecuteNonQuery();
                sqlConnection.Close();

                if (rc == 0)
                {
                    while (stackToDelete != "0")
                    {
                        Console.WriteLine($"There is no stack named {stackToDelete}. Please enter a valid stack.");
                        stackToDelete = Console.ReadLine();
                        DeleteStack();
                    }
                    
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        private void CreateStack()
        {
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                Console.WriteLine("Enter Name for New Stack:");
                string newStackName = Console.ReadLine();

                string insertQuery = $"INSERT INTO cardstacks(cardstackname) VALUES('{newStackName}')";
                SqlCommand insertStackName = new SqlCommand(insertQuery, sqlConnection);
                insertStackName.ExecuteNonQuery();
                sqlConnection.Close();

                Console.WriteLine($"New stack {newStackName} has been created.");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
