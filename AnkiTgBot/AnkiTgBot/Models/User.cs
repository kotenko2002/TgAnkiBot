using System;
using System.Collections.Generic;
using System.Text;

namespace AnkiTgBot.Models
{
    public class User
    {
        public int Id { get; set; }
        public int TgId { get; set; }

        public string Lang { get; set; } = "ru";
        public virtual ICollection<Desk> Desk { get; set; }
    }
}
