using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp.Dtos.CardDtos
{
    internal class CardListReadOnlyDto
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Front")]
        public string Front { get; set; }

        [Display(Name = "Back")]
        public string Back { get; set; }
    }
}
