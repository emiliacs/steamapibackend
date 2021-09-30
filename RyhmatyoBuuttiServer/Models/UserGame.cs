using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RyhmatyoBuuttiServer.Models
{
    public class UserGame
    {
        public int Id { get; set; }
        public int PlayedHours { get; set; }
        public Game Game { get; set; }
    }
}
