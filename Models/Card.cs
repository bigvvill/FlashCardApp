using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp.Models
{
    internal class Card
    {
        public int Id { get; set; }

        public string CardFront { get; set; }

        public string CardBack { get; set; }

        public int CardStackId { get; set; }        
    }
}
