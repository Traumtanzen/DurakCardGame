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
            //Console.WriteLine("\nGot deck is:\n");
            //foreach (Card card in GameDeck)
            //{
            //    Console.WriteLine($"{GameDeck.IndexOf(card)} - {card.ShowCard()}");
            //}
        }

        public void Shuffle()
        {
            ShuffledDeck = GameDeck.OrderBy(c => Guid.NewGuid()).ToList();
            //ShowShuffledDeck(); //(un)comment to (see)hide shuffled deck display
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
        public void Play()
        {

            Console.WriteLine("\nLet's start the game. Checking for the lowest trump or value in hands...");
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
                        //player turn
                        PlayerTurn();
                        AIdefence();
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
                    else
                    {
                        //comp turn
                        AIturn();
                        PlayerTurn();
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
                }
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
        //turns
        public void AIturn()
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
            aiBid = AIplayer.FirstOrDefault(c => (PlayerFaceLower() && SameSuit()) ||
                                                 (AIFaceLower() && AIbidIsTrump()) ||
                                                 (AIFaceLower() && DiffSuits()));
            Console.WriteLine($"\nAI bids a card: {aiBid.ShowCard()}");
            AIplayer.Remove(aiBid);
        }
        public void PlayerTurn()
        {
            Console.WriteLine($"\nBid a card. Choose by index and press <Enter> ({trump.Suit} are trumps)");
            ShowPlayerHand();
            int.TryParse(Console.ReadLine(), out int PlChoice);
            playerBid = Player.ElementAt(PlChoice);
            Swap.Add(playerBid);
            Console.WriteLine($"\nYour card is {playerBid.ShowCard()}");
            Player.RemoveAt(PlChoice);
        }
        //conditions
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
        //actions
        public void AItakes()
        {
            AIplayer.AddRange(Swap);
            Swap.Clear();
            Console.WriteLine("AI takes the card");
            Thread.Sleep(500);
            DealHands();
        }
        public void PlayerTakes()
        {
            Player.AddRange(Swap);
            Swap.Clear();
            Console.WriteLine("You take the card");
            Thread.Sleep(500);
            DealHands();
        }
        public void Discard()
        {
            Swap.Clear();
            DealHands();
        }
        public void AIstarts()
        {
            Console.WriteLine("\nAI starts");
            AIturn();
            PlayerTurn();
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
        public void PlayerStarts()
        {
            Console.WriteLine("\nYou start");
            PlayerTurn();
            AIdefence();
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
