using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Durak_Card_Game
{
    public class DurakGameplay
    {
        public List<DurakCard> GameDeck { get; set; }
        public List<DurakCard> ShuffledDeck { get; set; }
        public static List<DurakCard> Player = new List<DurakCard>();
        public List<DurakCard> AIplayer = new List<DurakCard>();
        public List<DurakCard> Swap = new List<DurakCard>();
        public DurakCard trump = new DurakCard();
        public DurakCard aiBid = new DurakCard();
        public DurakCard playerBid = new DurakCard();
        public static bool GameResult;


        public void Play()
        {
            Console.WriteLine("To beat the monster you should win a party in Durak game. Let's start!");
            GetGameDeck();
            Shuffle();
            DealHands();
            ShowTrump();
            TrumpCheckAndStart();
        }

        //Turns
        public void AIstarts()
        {
            Console.WriteLine("\nAI starts");
            AIturn();
        }
        public void AIturn()
        {
            if ((Player.Count > 0) && (ShuffledDeck.Count > 0))
            {
                AIbids();
                PlayerBids();
                PlayerBidsResult();
            }
            else
            {
                Victory();
            }
        }
        public void PlayerStarts()
        {
            Console.WriteLine("\nYou start");
            PlayerTurn();
        }
        public void PlayerTurn()
        {
            if ((AIplayer.Count > 0) && (ShuffledDeck.Count > 0))
            {
                PlayerBids();
                AIdefence();
                AIdefenseResult();
            }
            else
            {
                Defeat();
            }
        }
        public bool Victory()
        {
            Console.WriteLine("\nYou won!");
            //AskForReplay();
            return GameResult = true;
        }
        public bool Defeat()
        {
            Console.WriteLine("\nYou lost the game.");
            //AskForReplay();
            return GameResult = false;
        }

        //Actions
        public void GetGameDeck()
        {
            GameDeck = new List<DurakCard>();
            foreach (Suits suit in Enum.GetValues(typeof(Suits)))
            {
                foreach (Faces face in Enum.GetValues(typeof(Faces)))
                {
                    GameDeck.Add(new DurakCard { Suit = suit, Face = face });
                }
            }
        }
        public void Shuffle()
        {
            ShuffledDeck = GameDeck.OrderBy(c => Guid.NewGuid()).ToList();
        }
        public void DealHands()
        {
            Thread.Sleep(500);
            if (ShuffledDeck.Count > 0)
            {
                while (Player.Count < 6)
                {
                    Player.Add(ShuffledDeck[0]);
                    ShuffledDeck.RemoveAt(0);
                }
                while (AIplayer.Count < 6)
                {
                    AIplayer.Add(ShuffledDeck[0]);
                    ShuffledDeck.RemoveAt(0);
                }
            }
            Console.WriteLine($"\nDealing hands. {ShuffledDeck.Count.ToString()} cards left in the deck");
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
                                        (c.Face <= playerBid.Face && c.Suit == trump.Suit) ||
                                        (c.Face > playerBid.Face && c.Suit == trump.Suit) ||
                                        (c.Face <= playerBid.Face && c.Suit != trump.Suit));
            Console.WriteLine($"\nAI bids a card back: {aiBid.ShowCard()}");
            Swap.Add(aiBid);
            AIplayer.Remove(aiBid);
        }
        public void PlayerBids()
        {
            Console.WriteLine($"\nBid a card. Choose by index and press <Enter> ({trump.Suit} are trumps)");
            ShowPlayerHand();
            MakingBid();
        }
        public void MakingBid()
        {
            int.TryParse(Console.ReadLine(), out int PlChoice); //not good if enter not numbers & more than hand has
            if ((PlChoice <= Player.Count) && (PlChoice > 0))
            {
                playerBid = Player.ElementAt(PlChoice);
                Swap.Add(playerBid);
                Console.WriteLine($"\nYour card is {playerBid.ShowCard()}");
                Player.RemoveAt(PlChoice);
            }
            else
            {
                Console.WriteLine("You should chose a card from hand by its number");
                MakingBid();
            }
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
            Console.WriteLine("\n---Discarded---");
            Thread.Sleep(500);
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
            bool c = (PlayerFaceLower() && SameSuit()) ||
                (PlayerFaceLower() && AIbidIsTrump()) ||
                (EqualFaces() && AIbidIsTrump()) ||
                (AIFaceLower() && AIbidIsTrump());
            return c;
        }
        public bool PlayerCardHigher()
        {
            bool c = (AIFaceLower() && SameSuit()) ||
                (AIFaceLower() && PlayerBidIsTrump()) ||
                (EqualFaces() && PlayerBidIsTrump()) ||
                (PlayerFaceLower() && PlayerBidIsTrump());
            return c;
        }

        //(un)comment to (see)hide display of hands and(or) deck. don't forget to insert/remove the methods' calls!
        public void ShowPlayerHand()
        {
            Console.WriteLine("\nYour hand is:\n");
            foreach (DurakCard pCards in Player)
            {
                Console.WriteLine($"{Player.IndexOf(pCards)} - {pCards.ShowCard()}");
            }
        }
        public void ShowAIplayerHand()
        {
            Console.WriteLine("\nAI's hand is:\n");
            foreach (DurakCard aiCards in AIplayer)
            {
                Console.WriteLine($"{AIplayer.IndexOf(aiCards)} - {aiCards.ShowCard()}");
            }
        }
        public void ShowShuffledDeck()
        {
            Console.WriteLine("\nShuffled deck is:\n");
            foreach (DurakCard card in ShuffledDeck)
            {
                Console.WriteLine($"{ShuffledDeck.IndexOf(card)} -  {card.ShowCard()}");
            }
        }
    }
}
