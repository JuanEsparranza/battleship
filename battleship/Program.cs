using System;

namespace battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Welcome to Battleship!");
                Console.WriteLine("\nPress any key to start the game...");
                Console.ReadKey();
                Console.Clear();

                Game game = new Game();
                game.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("\nThanks for playing! Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}
