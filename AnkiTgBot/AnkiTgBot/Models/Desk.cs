using System;
using System.Collections.Generic;
using System.Text;

namespace AnkiTgBot.Models
{
    public class Desk
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
        public virtual User User { get; set; }

        public int UserId { get; set; }
    }
}
