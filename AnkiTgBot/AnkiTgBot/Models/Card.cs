using System;
using System.Collections.Generic;
using System.Text;

namespace AnkiTgBot.Models
{
    public class Card
    {
        public int Id { get; set; }

        public string Front { get; set; }
        public string Back { get; set; }

        public bool Favorite { get; set; }

        public int DeskId { get; set; }
        public virtual Desk Desk { get; set; }

        public Card(string front, string back, int deskId)
        {
            Front = front;
            Back = back;
            DeskId = deskId;
        }
    }
}
