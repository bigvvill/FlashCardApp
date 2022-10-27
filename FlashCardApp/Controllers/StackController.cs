using FlashCardApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp.Controllers
{
    internal class StackController
    {
        private string connectionString = @"Data Source=WILL-PC\NEW2019;Initial Catalog=FlashCardDb;Integrated Security=True";

        public void CreateStack()
        {
            GetUserInput getUserInput = new GetUserInput();
            string newStackName = "";

            DisplayTable displayTable = new DisplayTable();
            displayTable.DisplayStack();

            Console.WriteLine("\nEnter Name for New Stack or 0 to go back to Menu");
            newStackName = Console.ReadLine();

            while (string.IsNullOrEmpty(newStackName))
            {
                Console.WriteLine("\nInvalid Entry. Please enter the stack name or 0 to go back to Menu.\n");
                newStackName = Console.ReadLine();
            }

            if (newStackName == "0")
            {
                getUserInput.StackMenu();
            }

            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                string insertQuery = $"INSERT INTO cardstacks(cardstackname) VALUES('{newStackName}')";
                SqlCommand insertStackName = new SqlCommand(insertQuery, sqlConnection);
                insertStackName.ExecuteNonQuery();
                sqlConnection.Close();

                Console.WriteLine($"New stack {newStackName} has been created.\nPress Enter...");
                Console.ReadLine();

                CreateStack();
            }
            catch (Exception)
            {
                Console.WriteLine($"Stack Name {newStackName} already exists. Please use another name.\nPress Enter...");
                Console.ReadLine();
                CreateStack();
            }
        }

        public void DeleteStack()
        {
            GetUserInput getUserInput = new GetUserInput();
            DisplayTable displayTable = new DisplayTable();
            displayTable.DisplayStack();

            string stackToDelete = "";

            Console.WriteLine("\nWhich stack would you like to delete? Type 0 to go back to Menu.");
            stackToDelete = Console.ReadLine();

            while (string.IsNullOrEmpty(stackToDelete))
            {
                Console.WriteLine("\nInvalid Entry. Please enter the stack name or 0 to go back to Menu.\n");
                stackToDelete = Console.ReadLine();
            }

            if (stackToDelete == "0")
            {
                getUserInput.StackMenu();
            }
            Console.WriteLine("WARNING! This will delete all associated cards!");
            Console.WriteLine($"Are you sure you want to delete the stack {stackToDelete}?\nTypo \"Yes\" to delete or any other key to go back.");
            string confirmDelete = Console.ReadLine();

            if (confirmDelete == "Yes")
            {
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
                            Console.WriteLine($"There is no stack named {stackToDelete}. Please enter a valid stack.\nPress Enter...");
                            Console.ReadLine();
                            DeleteStack();
                        }
                    }

                    Console.WriteLine($"Stack Name {stackToDelete} has been deleted.\nPress Enter...");
                    Console.ReadLine();
                    DeleteStack();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else DeleteStack();
        }
    }
}
