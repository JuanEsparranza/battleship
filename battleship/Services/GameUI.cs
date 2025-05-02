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
                CellState.Hit => 'X',
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

        public static (int, int, bool, bool) GetShipPlacement(ShipType shipType)
        {
            Console.WriteLine($"Placing {shipType}");

            int row = -1, col = -1;
            bool isHorizontal = false;
            bool isLeftToRight = false;
            bool validInput = false;

            while (!validInput)
            {
                Console.Write($"Enter starting row (0-9) for {shipType}: ");
                if (!int.TryParse(Console.ReadLine(), out row) || row < 0 || row >= Board.Size)
                {
                    Console.WriteLine("Invalid row. Please enter a number between 0 and 9.");
                    continue;
                }

                Console.Write($"Enter starting column (0-9) for {shipType}: ");
                if (!int.TryParse(Console.ReadLine(), out col) || col < 0 || col >= Board.Size)
                {
                    Console.WriteLine("Invalid column. Please enter a number between 0 and 9.");
                    continue;
                }

                // Different placement logic based on ship type
                if (shipType == ShipType.Destroyer)
                {
                    // Destroyers are 2x2 squares, so no orientation needed
                    Console.WriteLine("Destroyer is a 2x2 square (orientation not needed)");
                    validInput = true;
                }
                else if (shipType == ShipType.Submarine)
                {
                    // Submarines are 3 diagonal cells
                    Console.Write("Direction for submarine (l for left-to-right diagonal \\, r for right-to-left diagonal /): ");
                    string direction = Console.ReadLine()?.ToLower() ?? "";

                    if (direction == "l")
                    {
                        isLeftToRight = true;
                        validInput = true;
                    }
                    else if (direction == "r")
                    {
                        isLeftToRight = false;
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid direction. Please enter 'l' or 'r'.");
                    }
                }
                else if (shipType == ShipType.Cruiser)
                {
                    // Cruisers are 3 consecutive cells (horizontal or vertical)
                    Console.Write("Orientation for cruiser (h for horizontal, v for vertical): ");
                    string orientation = Console.ReadLine()?.ToLower() ?? "";

                    if (orientation == "h")
                    {
                        isHorizontal = true;
                        validInput = true;
                    }
                    else if (orientation == "v")
                    {
                        isHorizontal = false;
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid orientation. Please enter 'h' or 'v'.");
                    }
                }
            }

            return (row, col, isHorizontal, isLeftToRight);
        }

        // Method to display a legend for the game symbols and ship shapes
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

            Console.WriteLine("\n=== SHIP SHAPES ===");
            Console.WriteLine("Destroyer: 2x2 square of cells (4 cells total)");
            Console.WriteLine("Submarine: 3 diagonal cells (left-to-right \\ or right-to-left /)");
            Console.WriteLine("Cruiser: 3 consecutive cells (horizontal or vertical)");
        }
    }
}