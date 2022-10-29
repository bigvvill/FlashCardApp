using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp.Controllers
{
    internal class StudyController
    {
        public void StudySession()
        {
            GetUserInput getUserInput = new GetUserInput();
            getUserInput.SelectStack("study");
        }

        public void StudyCards(string stackSelection, int stackSelectionId)
        {
            GetUserInput getUserInput = new GetUserInput();
            getUserInput.StudyMenu(stackSelection, stackSelectionId);
        }

        public void StudyFront(string stackSelection, int stackSelectionId) // TODO : Create StudyFront
        {
            DisplayTable displayTable = new DisplayTable();
            displayTable.DisplayFrontCard(stackSelectionId); // TODO : Get data to randomize card
        }

        public void StudyBack(string stackSelection, int stackSelectionId) // TODO : Create StudyBack
        {
            DisplayTable displayTable = new DisplayTable();
            displayTable.DisplayBackCard(stackSelectionId);  // TODO : Get data to randomize card
        }
    }
}
