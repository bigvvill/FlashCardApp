using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp.Dtos.CardStack
{
    internal class CardListReadOnlyDto
    {
        public int CardLabelNumber { get; set; }

        public string CardFront { get; set; }

        public string CardBack { get; set; }
    }
}
