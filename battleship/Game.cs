using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static battleship.Game;
using battleship.Models;
using battleship.Models.Enums;
using battleship.Services;
using System;
using System.Threading;

namespace battleship
{
  
        public class Game
        {
            private Board _playerBoard;
            private Board _computerBoard;
            private TrackingBoard _playerTracking;
            private TrackingBoard _computerTracking;
            private ComputerPlayer _computer;
            private int _playerShipsSunk;
            private int _computerShipsSunk;

            public Game()
            {
                _playerBoard = new Board();
                _computerBoard = new Board();
                _playerTracking = new TrackingBoard();
                _computerTracking = new TrackingBoard();
                _computer = new ComputerPlayer();
                _playerShipsSunk = 0;
                _computerShipsSunk = 0;
            }

            public void Start()
            {
                Console.WriteLine("=== BATTLESHIP GAME ===");
                PlacePlayerShips();
                PlaceComputerShips();

                bool gameOver = false;
                bool playerTurn = true;

                while (!gameOver)
                {
                    if (playerTurn)
                    {
                        Console.Clear();
                        DisplayGameStatus();

                        playerTurn = PlayerTurn();
                    }
                    else
                    {
                        Console.WriteLine("\nComputer's turn...");
                        Thread.Sleep(1000);  // Add a small delay for better UX

                        playerTurn = !ComputerTurn();

                        // Show updated player board after computer's turn
                        Console.WriteLine("\n=== Your Board (After Computer's Attack) ===");
                        GameUI.DisplayBoard(_playerBoard, false);
                    }

                    if (_playerBoard.AllShipsSunk())
                    {
                        Console.Clear();
                        Console.WriteLine("All your ships have been sunk. You lose!");
                        gameOver = true;
                    }
                    else if (_computerBoard.AllShipsSunk())
                    {
                        Console.Clear();
                        Console.WriteLine("All computer ships have been sunk. You win!");
                        gameOver = true;
                    }
                }

                // Display final boards
                DisplayGameStatus();
                Console.WriteLine("\n=== Computer's Board ===");
                GameUI.DisplayBoard(_computerBoard, false);
            }

            private void DisplayGameStatus()
            {
                Console.WriteLine($"=== GAME STATUS ===");
                Console.WriteLine($"Your ships sunk: {_computerShipsSunk}/{_computerBoard.Ships.Count}");
                Console.WriteLine($"Computer ships sunk: {_playerShipsSunk}/{_playerBoard.Ships.Count}");

                Console.WriteLine("\n=== Your Board ===");
                GameUI.DisplayBoard(_playerBoard, false);
                Console.WriteLine("\n=== Tracking Board (Computer's Ocean) ===");
                GameUI.DisplayTrackingBoard(_playerTracking);
            }

            private void PlacePlayerShips()
            {
                Console.WriteLine("Place your ships:");
                ShipType[] shipTypes = (ShipType[])Enum.GetValues(typeof(ShipType));

                foreach (var shipType in shipTypes)
                {
                    bool placed = false;
                    Ship ship = new Ship(shipType);

                    while (!placed)
                    {
                        Console.Clear();
                        Console.WriteLine("=== Your Board ===");
                        GameUI.DisplayBoard(_playerBoard, false);

                        var (row, col, isHorizontal) = GameUI.GetShipPlacement(shipType, ship.Size);
                        placed = _playerBoard.PlaceShip(ship, row, col, isHorizontal);

                        if (!placed)
                            Console.WriteLine("Invalid placement. Ship cannot be placed there.");
                    }
                }
            }

            private void PlaceComputerShips()
            {
                _computer.PlaceShipsRandomly(_computerBoard);
                Console.WriteLine("Computer has placed its ships.");
            }

            private bool PlayerTurn()
            {
                Console.WriteLine("\nYour turn to attack!");

                // Get player's target
                (int row, int col) = GameUI.GetPlayerTarget();

                // Check if it's a valid target
                if (!_playerTracking.IsValidTarget(row, col))
                {
                    Console.WriteLine("You've already attacked that position. Try again.");
                    return true;  // Player gets another turn
                }

                // Process the attack
                var (hit, ship) = _computerBoard.ReceiveAttack(row, col);

                if (hit)
                {
                    _playerTracking.MarkHit(row, col);
                    Console.WriteLine("Hit!");

                    if (ship != null && ship.IsSunk())
                    {
                        _playerShipsSunk++;
                        Console.WriteLine($"You sunk the computer's {ship.Type}!");
                    }

                    return true;  // Player gets another turn
                }
                else
                {
                    _playerTracking.MarkMiss(row, col);
                    Console.WriteLine("Miss!");
                    return false;  // Computer's turn
                }
            }

            private bool ComputerTurn()
            {
                // Computer chooses a target
                (int row, int col) = _computer.ChooseTarget(_computerTracking);
                Console.WriteLine($"Computer attacks: ({row}, {col})");

                // Process the attack
                var (hit, ship) = _playerBoard.ReceiveAttack(row, col);

                if (hit)
                {
                    // Mark hit on computer's tracking board - this is how the computer
                    // keeps track of where it has hit the player's ships
                    _computerTracking.MarkHit(row, col);
                    Console.WriteLine("Computer hit your ship!");

                    bool shipSunk = false;
                    if (ship != null && ship.IsSunk())
                    {
                        _computerShipsSunk++;
                        Console.WriteLine($"Computer sunk your {ship.Type}!");
                        shipSunk = true;
                    }

                    _computer.RecordHit(row, col, shipSunk);
                    return true;  // Computer gets another turn
                }
                else
                {
                    _computerTracking.MarkMiss(row, col);
                    Console.WriteLine("Computer missed!");
                    return false;  // Player's turn
                }
            }
        }
    }