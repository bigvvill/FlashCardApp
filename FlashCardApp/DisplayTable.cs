﻿using FlashCardApp.Controllers;
using FlashCardApp.Dtos.CardDtos;
using FlashCardApp.Dtos.CardStack;
using FlashCardApp.Dtos.StudyCardDtos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal void DisplayCardList(int selection)
        {
            try
            {
                List<CardListReadOnlyDto> tableData = new List<CardListReadOnlyDto>();
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var tableCmd = connection.CreateCommand())
                    {
                        connection.Open();
                        tableCmd.CommandText = $"SELECT * FROM cards WHERE stackId = {selection};";

                        using (var stackReader = tableCmd.ExecuteReader())
                        {
                            int listNumber = 1;

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
        }

        internal void DisplayFrontCard(string stackSelection, string cardFront, string cardBack, int stackSelectionId)
        {
            StudyController studyController = new StudyController();

            List<CardFrontDto> tableData = new List<CardFrontDto>();         

            tableData.Add(
            new CardFrontDto
            {                                        
                Front = cardFront                                        
            });      
                
            FormatTable.ShowFrontCard(stackSelection, tableData);
            studyController.GetUserInput("front", stackSelection, cardFront, cardBack, stackSelectionId);            
        }

        internal void DisplayBackCard(string stackSelection, string cardFront, string cardBack, int stackSelectionId)
        {
            StudyController studyController = new StudyController();

            List<CardFrontDto> tableData = new List<CardFrontDto>();            

            tableData.Add(
            new CardFrontDto
            {
                Front = cardBack
            });

            FormatTable.ShowFrontCard(stackSelection, tableData);
            studyController.GetUserInput("back", stackSelection, cardFront, cardBack, stackSelectionId);
        }

        internal void DisplayData()
        {

        }
    }
}
