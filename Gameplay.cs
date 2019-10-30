using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Durak_Card_Game
{
    public class Gameplay
    {
        public List<Card> GameDeck { get; set; }
        public List<Card> ShuffledDeck { get; set; }
        public List<Card> Player = new List<Card>();
        public List<Card> AIplayer = new List<Card>();
        public List<Card> Swap = new List<Card>();

        public void GetGameDeck()
        {
            GameDeck = new List<Card>();
            foreach (Suits suit in Enum.GetValues(typeof(Suits)))
            {
                foreach (Faces face in Enum.GetValues(typeof(Faces)))
                {
                    GameDeck.Add(new Card { Suit = suit, Face = face });
                }
            }

            //(un)comment to (see)hide deck display
            Console.WriteLine("\nGot deck is:\n");
            foreach (Card card in GameDeck)
            {
                Console.WriteLine(card.ShowCard());
            }
        }

        public void Shuffle()
        {
            ShuffledDeck = GameDeck.OrderBy(c => Guid.NewGuid()).ToList();
            ShowShuffledDeck(); //(un)comment to (see)hide shuffled deck display
        }
        public void DealHands()
        {
            while (Player.Count < 6 && AIplayer.Count < 6)
            {
                Player.Add(ShuffledDeck[0]);
                ShuffledDeck.RemoveAt(0);
                AIplayer.Add(ShuffledDeck[0]);
                ShuffledDeck.RemoveAt(0);
            }
        }
        public void GetTrump()
        {
            Swap.Add(ShuffledDeck[0]);
            ShuffledDeck.RemoveAt(0);
            ShuffledDeck.AddRange(Swap);
            Swap.Clear();
            ShowTrump();
        }
        public void ShowTrump()
        {
            Card trump = ShuffledDeck[ShuffledDeck.Count - 1];
            Console.WriteLine($"\n{trump.Suit} are trumps for this game.");
        }
        public void Play()
        {
            Console.WriteLine("Let's start the game. Checking for the lowest trump or value in hands...");
            Thread.Sleep(1500);
            Card trump = ShuffledDeck[ShuffledDeck.Count - 1];
            List<Card> playerHand = Player;
            List<Card> aiHand = AIplayer;
            if (playerHand.Any(f => f.Face == 0)) && playerHand.Any(s => s.Suit == trump.Suit))
            {

            }
            while (ShuffledDeck.Count != 0)
            {
                //place for gameplay
            }
            if (Player.Count == 0)
            {
                Console.WriteLine("You won!");
            }
            else
            {
                Console.WriteLine("You lost the game. Replay? (Y/N)");
                string answer = Console.ReadLine();
                answer.ToLower();
                while (true)
                    switch (answer)
                    {
                        case "y":
                            {
                                Play();
                                break;
                            }
                        case "n":
                            {
                                Console.WriteLine("Bye!");
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Press Y or N");
                                break;
                            }
                    }
            }
        }




        //(un)comment to (see)hide display of hands and(or) deck. don't forget to insert/remove the methods' calls!
        public void ShowPlayerHand()
        {
            Console.WriteLine("\nYour hand is:\n");
            foreach (Card pCards in Player)
            {
                Console.WriteLine(pCards.ShowCard());
            }
        }
        public void ShowAIplayerHand()
        {
            Console.WriteLine("\nAI's hand is:\n");
            foreach (Card aiCards in AIplayer)
            {
                Console.WriteLine(aiCards.ShowCard());
            }
        }
        public void ShowShuffledDeck()
        {
            Console.WriteLine("\nShuffled deck is:\n");
            foreach (Card card in ShuffledDeck)
            {
                Console.WriteLine(card.ShowCard());
            }
        }
    }
}
