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

        }
    }
}
