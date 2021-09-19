using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Game_Rock_Paper_Scissors
{
    public class Game
    {   
        public string CreateRandomKey()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] b = new byte[16];
            rng.GetBytes(b);
            return Convert.ToBase64String(b).ToUpper();
        }
        
        public string CreateHMAC(string message, string key)
        {
            var _randomKey = Encoding.UTF8.GetBytes(key);
            var _message = Encoding.UTF8.GetBytes(message);
            HMACSHA256 hmac = new HMACSHA256(_randomKey);
            var hash = hmac.ComputeHash(_message);
            return string.Concat(Array.ConvertAll(hash, b => b.ToString("X2"))).ToUpper();      
        }    
    }

    public class Winner
    {
        public int GetResult(string[] args, string resComp, string resUser)
        {
            int avarage = Convert.ToInt32(resUser) + ((args.Length-1) / 2);
            int _resComp = Convert.ToInt32(resComp);
            int _resUser = Convert.ToInt32(resUser);
            if (_resComp > _resUser && _resComp <= avarage)
            {
                return -1;
            }
            else if(_resUser == _resComp)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }

    public class Help
    {
        public void GetHelp(int user, int nowinner, int comp)
        {
            Console.WriteLine(new string('-', 25));
            string s = string.Format("| User score:     {0}\t|\n" +
                                     "| Computer score: {1}\t|\n" +
                                     "| No winner:      {2}\t|",user,comp,nowinner);
            Console.WriteLine(s);
            Console.WriteLine(new string('-', 25));
        }
    }
    class Program
    {
        static string[] AddElement(ref string[] args, string value, int index)
        {
            string[] newArgs = new string[args.Length + 1];
            newArgs[index] = value;
            for (int i = 0; i < index; i++)
                newArgs[i] = args[i];
            for (int i = index; i < args.Length; i++)
                newArgs[i+1] = args[i];
            return newArgs;
        }
        static void Main(string[] args)
        {
            Game _rockPaperScissors = new Game();
            Random rnd = new Random();
            Winner winner = new Winner();
            Help help = new Help();
            string _move = "";
            int totalUser = 0;
            int totalNoWinner = 0;
            int totalComp = 0;
            if (args.Length < 3 || args.Length % 2 == 0)
            {
                Console.WriteLine("Error! There must be at least 3 arguments...");
                Console.WriteLine("Arguments not be the same....");
                Console.WriteLine("The number of arguments must be odd...");
                Console.WriteLine("Enter! 'rock' 'paper' 'scissors'...");
            }
            else
            {
                args = AddElement(ref args, "", 0);
                while (_move != "0")
                {                   
                    string key = _rockPaperScissors.CreateRandomKey();
                    string someNumber = rnd.Next(1, args.Length).ToString();
                    Console.WriteLine($"HMAC: {_rockPaperScissors.CreateHMAC(someNumber,key)}");
                    Console.WriteLine("Available moves: \n");
                    for (int i = 1; i < args.Length; i++)
                    {
                        Console.WriteLine($"{i} -> {args[i]}");
                    }
                    Console.WriteLine($"0 -> EXIT");
                    Console.WriteLine($"? -> Help\n");
                    Console.Write("Enter your move: ");
                    _move = Console.ReadLine();
                    if(_move != "0" && _move != "?")
                    {
                        Console.WriteLine($"Your move: {args[Convert.ToInt32(_move)]}");
                        Console.WriteLine($"Computer move: {args[Convert.ToInt32(someNumber)]}");
                        int a = winner.GetResult(args, someNumber, _move);
                        if(a == 1) { Console.WriteLine("You win!"); totalUser++;}
                        else if (a == 0) { Console.WriteLine("No winner!"); totalNoWinner++; }
                        else if (a == -1) { Console.WriteLine("Computer win!"); totalComp++; }
                        Console.WriteLine($"HMAC key: {key}");
                    }
                    else if(_move == "?")
                    {
                        help.GetHelp(totalUser,totalNoWinner,totalComp);
                    }
                    else
                    {
                        Console.WriteLine("Good luck!");
                    }
                }            
            }
        }


    }
}
