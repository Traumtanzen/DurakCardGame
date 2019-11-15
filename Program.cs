using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak_Card_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Gameplay game = new Gameplay();

            game.GetGameDeck(); // comment the marked code in method before build
            game.Shuffle(); // comment the marked code in method before build
            game.DealHands();
            //game.ShowPlayerHand(); // comment the line before build 
            //game.ShowAIplayerHand(); // comment the line before build 
            game.ShowTrump();
            //game.ShowShuffledDeck(); //comment the line before build
            game.Play();

            Console.ReadLine();

        }
    }
}
