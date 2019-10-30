using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak_Card_Game
{
    public class Card
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
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace,
    }
}
