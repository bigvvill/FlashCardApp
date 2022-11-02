using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp.Models
{
    internal class StudySession
    {
        public int Id { get; set; }
        public DateTime SessionDate { get; set; }
        public int CardsTried { get; set; }
        public int CardsCorrect { get; set; }
        public int Stack { get; set; }
    }
}
