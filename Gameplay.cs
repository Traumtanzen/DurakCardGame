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
        public Card trump = new Card();
        public Card aiBid = new Card();
        public Card playerBid = new Card();


        public void Play()
        {
            Console.WriteLine("Let's start the game!");
            GetGameDeck();
            Shuffle();
            DealHands();
            ShowTrump();

            while ((Player.Count != 0) || (AIplayer.Count != 0))
            {
                TrumpCheckAndStart();
            }
            if (Player.Count == 0)
            {
                Console.WriteLine("\nYou won!");
                AskForReplay();
            }
            else
            {
                Console.WriteLine("\nYou lost the game.");
                AskForReplay();
            }
        }


        public void TrumpCheckAndStart()
        {
            Console.WriteLine("\nChecking for the lowest trump or value in hands...");
            Thread.Sleep(500);
            var playerTrump = Player.OrderBy(o => o.Face).FirstOrDefault(s => s.Suit == trump.Suit);
            var aiTrump = AIplayer.OrderBy(o => o.Face).FirstOrDefault(s => s.Suit == trump.Suit);
            if (playerTrump == null)
            {
                AIstarts();
            }
            else
            {
                if (aiTrump == null)
                {
                    PlayerStarts();
                }
                else
                {
                    if (playerTrump.Face < aiTrump.Face)
                    {
                        PlayerStarts();
                    }
                    else
                    {
                        AIstarts();
                    }
                }
            }
        }


        //Turns
        public void AIstarts()
        {
            Console.WriteLine("\nAI starts");
            AIturn();
        }
        public void AIturn()
        {
            AIbids();
            PlayerBids();
            PlayerBidsResult();
        }
        public void PlayerStarts()
        {
            Console.WriteLine("\nYou start");
            PlayerTurn();
        }
        public void PlayerTurn()
        {
            PlayerBids();
            AIdefence();
            AIdefenseResult();
        }


        //Actions
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
        }
        public void Shuffle()
        {
            ShuffledDeck = GameDeck.OrderBy(c => Guid.NewGuid()).ToList();
        }
        public void DealHands()
        {
            while (Player.Count < 6 && AIplayer.Count < 6 && ShuffledDeck.Count > 0)
            {
                Player.Add(ShuffledDeck[0]);
                ShuffledDeck.RemoveAt(0);
                AIplayer.Add(ShuffledDeck[0]);
                ShuffledDeck.RemoveAt(0);
            }
        }
        public void ShowTrump()
        {
            Swap.Add(ShuffledDeck[0]);
            ShuffledDeck.RemoveAt(0);
            ShuffledDeck.AddRange(Swap);
            Swap.Clear();
            trump = ShuffledDeck[ShuffledDeck.Count - 1];
            Console.WriteLine($"\n{trump.Suit} are trumps for this game.");
        }
        public void AIbids()
        {
            Thread.Sleep(500);
            AIplayer.OrderBy(f => f.Face);
            aiBid = AIplayer.ElementAt(0);
            Swap.Add(AIplayer[0]);
            AIplayer.RemoveAt(0);
            Console.WriteLine($"\nAI bids a card: {aiBid.ShowCard()}");
        }
        public void AIdefence()
        {
            Thread.Sleep(500);
            aiBid = AIplayer.FirstOrDefault(c => (c.Face > playerBid.Face && c.Suit == playerBid.Suit) ||
                                        (c.Face < playerBid.Face && c.Suit == trump.Suit) ||
                                        (c.Face < playerBid.Face && c.Suit != trump.Suit));
            Console.WriteLine($"\nAI bids a card: {aiBid.ShowCard()}");
            AIplayer.Remove(aiBid);
        }
        public void PlayerBids()
        {
            Console.WriteLine($"\nBid a card. Choose by index and press <Enter> ({trump.Suit} are trumps)");
            ShowPlayerHand();
            int.TryParse(Console.ReadLine(), out int PlChoice);
            playerBid = Player.ElementAt(PlChoice);
            Swap.Add(playerBid);
            Console.WriteLine($"\nYour card is {playerBid.ShowCard()}");
            Player.RemoveAt(PlChoice);
        }
        public void AItakes()
        {
            AIplayer.AddRange(Swap);
            Swap.Clear();
            Console.WriteLine("\nAI takes the card");
            Thread.Sleep(500);
            DealHands();
        }
        public void PlayerTakes()
        {
            Player.AddRange(Swap);
            Swap.Clear();
            Console.WriteLine("\nYou take the card");
            Thread.Sleep(500);
            DealHands();
        }
        public void Discard()
        {
            Swap.Clear();
            Console.WriteLine("\nDiscarded");
            DealHands();
        }

        public void PlayerBidsResult()
        {
            if (PlayerCardHigher())
            {
                Discard();
                PlayerTurn();
            }
            else
            {
                PlayerTakes();
                AIturn();
            }
        }
        public void AIdefenseResult()
        {
            if (AIcardHigher())
            {
                Discard();
                AIturn();
            }
            else
            {
                AItakes();
                PlayerTurn();
            }
        }
        public void AskForReplay()
        {
            Console.WriteLine("Replay? (Y/N)");
            string answer = Console.ReadLine();
            answer.ToLower();
            while (answer != "y" || answer != "n")
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

        //Conditions
        public bool AIFaceLower()
        {
            bool f = (aiBid.Face < playerBid.Face);
            return f;
        }
        public bool PlayerFaceLower()
        {
            bool f = (aiBid.Face > playerBid.Face);
            return f;
        }
        public bool EqualFaces()
        {
            bool f = (aiBid.Face == playerBid.Face);
            return f;
        }
        public bool SameSuit()
        {
            bool s = (aiBid.Suit == playerBid.Suit);
            return s;
        }
        public bool DiffSuits()
        {
            bool s = (aiBid.Suit != playerBid.Suit);
            return s;
        }
        public bool AIbidIsTrump()
        {
            bool s = (aiBid.Suit == trump.Suit);
            return s;
        }
        public bool PlayerBidIsTrump()
        {
            bool s = (playerBid.Suit == trump.Suit);
            return s;
        }
        public bool AIcardHigher()
        {
            bool c = (PlayerFaceLower() && SameSuit()) || (EqualFaces() && AIbidIsTrump());
            return c;
        }
        public bool PlayerCardHigher()
        {
            bool c = (AIFaceLower() && SameSuit()) || (EqualFaces() || PlayerBidIsTrump());
            return c;
        }

        //(un)comment to (see)hide display of hands and(or) deck. don't forget to insert/remove the methods' calls!
        public void ShowPlayerHand()
        {
            Console.WriteLine("\nYour hand is:\n");
            foreach (Card pCards in Player)
            {
                Console.WriteLine($"{Player.IndexOf(pCards)} - {pCards.ShowCard()}");
            }
        }
        public void ShowAIplayerHand()
        {
            Console.WriteLine("\nAI's hand is:\n");
            foreach (Card aiCards in AIplayer)
            {
                Console.WriteLine($"{AIplayer.IndexOf(aiCards)} - {aiCards.ShowCard()}");
            }
        }
        public void ShowShuffledDeck()
        {
            Console.WriteLine("\nShuffled deck is:\n");
            foreach (Card card in ShuffledDeck)
            {
                Console.WriteLine($"{ShuffledDeck.IndexOf(card)} -  {card.ShowCard()}");
            }
        }
    }
}
