using battleship.Models.Enums;
using battleship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship.Services
{
    public class GameUI
    {
        public static void DisplayBoard(Board board, bool hideShips)
        {
            Console.WriteLine("  0 1 2 3 4 5 6 7 8 9");

            for (int row = 0; row < Board.Size; row++)
            {
                Console.Write($"{row} ");

                for (int col = 0; col < Board.Size; col++)
                {
                    char symbol = GetSymbol(board.Grid[row, col], hideShips);

                    // Add color for better visualization
                    switch (board.Grid[row, col])
                    {
                        case CellState.Hit:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case CellState.Miss:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case CellState.Ship:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                    }

                    Console.Write($"{symbol} ");
                    Console.ResetColor();
                }

                Console.WriteLine();
            }
        }

        public static void DisplayTrackingBoard(TrackingBoard trackingBoard)
        {
            Console.WriteLine("  0 1 2 3 4 5 6 7 8 9");

            for (int row = 0; row < TrackingBoard.Size; row++)
            {
                Console.Write($"{row} ");

                for (int col = 0; col < TrackingBoard.Size; col++)
                {
                    char symbol = GetSymbol(trackingBoard.Grid[row, col], false);

                    // Add color for better visualization
                    switch (trackingBoard.Grid[row, col])
                    {
                        case CellState.Hit:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case CellState.Miss:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                    }

                    Console.Write($"{symbol} ");
                    Console.ResetColor();
                }

                Console.WriteLine();
            }
        }

        private static char GetSymbol(CellState state, bool hideShips)
        {
            return state switch
            {
                CellState.Empty => '~',
                CellState.Ship => hideShips ? '~' : 'S',
                CellState.Hit => 'X',    // Both ship hits and empty hits show as X
                CellState.Miss => 'O',
                _ => '?'
            };
        }

        public static (int, int) GetPlayerTarget()
        {
            int row = -1, col = -1;
            bool validInput = false;

            while (!validInput)
            {
                Console.Write("Enter target row (0-9): ");
                if (!int.TryParse(Console.ReadLine(), out row) || row < 0 || row >= Board.Size)
                {
                    Console.WriteLine("Invalid row. Please enter a number between 0 and 9.");
                    continue;
                }

                Console.Write("Enter target column (0-9): ");
                if (!int.TryParse(Console.ReadLine(), out col) || col < 0 || col >= Board.Size)
                {
                    Console.WriteLine("Invalid column. Please enter a number between 0 and 9.");
                    continue;
                }

                validInput = true;
            }

            return (row, col);
        }

        public static (int, int, bool) GetShipPlacement(ShipType shipType, int shipSize)
        {
            Console.WriteLine($"Placing {shipType} (size: {shipSize})");

            int row = -1, col = -1;
            bool isHorizontal = false;
            bool validInput = false;

            while (!validInput)
            {
                Console.Write("Enter starting row (0-9): ");
                if (!int.TryParse(Console.ReadLine(), out row) || row < 0 || row >= Board.Size)
                {
                    Console.WriteLine("Invalid row. Please enter a number between 0 and 9.");
                    continue;
                }

                Console.Write("Enter starting column (0-9): ");
                if (!int.TryParse(Console.ReadLine(), out col) || col < 0 || col >= Board.Size)
                {
                    Console.WriteLine("Invalid column. Please enter a number between 0 and 9.");
                    continue;
                }

                Console.Write("Orientation (h for horizontal, v for vertical): ");
                string orientation = Console.ReadLine()?.ToLower() ?? "";

                if (orientation == "h")
                    isHorizontal = true;
                else if (orientation == "v")
                    isHorizontal = false;
                else
                {
                    Console.WriteLine("Invalid orientation. Please enter 'h' or 'v'.");
                    continue;
                }

                validInput = true;
            }

            return (row, col, isHorizontal);
        }

        // Method to display a legend for the game symbols
        public static void DisplayLegend()
        {
            Console.WriteLine("\n=== LEGEND ===");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("~ ");
            Console.ResetColor();
            Console.WriteLine("= Empty water");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("S ");
            Console.ResetColor();
            Console.WriteLine("= Your ship");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("X ");
            Console.ResetColor();
            Console.WriteLine("= Hit");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("O ");
            Console.ResetColor();
            Console.WriteLine("= Miss");
        }
    }
}