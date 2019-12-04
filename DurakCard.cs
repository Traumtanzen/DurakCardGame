using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak_Card_Game
{
    public class DurakCard
    {
        public Suits Suit { get; set; }
        public Faces Face { get; set; }

        public string ShowCard()
        {
            return $"{Face} of {Suit}";
        }
    }
    public enum Suits
    {
        Hearts,
        Diamonds,
        Spades,
        Clubs
    }
    public enum Faces
    {
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14,
    }
}
