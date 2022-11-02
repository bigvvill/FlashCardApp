using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardApp.Dtos.SessionDtos
{
    internal class SessionStatsDto
    {
        public string Stack { get; set; }
        public int Correct { get; set; }
        public int Total { get; set; }
        public decimal Percentage { get; set; }
    }
}