using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp.Dtos.CardDtos
{
    internal class CardListReadOnlyDto
    {
        
        public int Id { get; set; }

        
        public string Front { get; set; }

        
        public string Back { get; set; }
    }
}
