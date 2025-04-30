using battleship.Models.Enums;
using battleship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship.Services
{
    public class ComputerPlayer
    {
        private Random _random;
        private List<(int, int)> _potentialTargets;
        private List<(int, int)> _lastHits;

        public ComputerPlayer()
        {
            _random = new Random();
            _potentialTargets = new List<(int, int)>();
            _lastHits = new List<(int, int)>();
        }

        public (int, int) ChooseTarget(TrackingBoard trackingBoard)
        {
            // If we have potential targets (due to previous hits), prioritize those
            if (_potentialTargets.Count > 0)
            {
                int index = _random.Next(_potentialTargets.Count);
                var target = _potentialTargets[index];
                _potentialTargets.RemoveAt(index);

                if (trackingBoard.IsValidTarget(target.Item1, target.Item2))
                    return target;
            }

            // Otherwise, choose a random valid target
            int row, col;
            do
            {
                row = _random.Next(Board.Size);
                col = _random.Next(Board.Size);
            } while (!trackingBoard.IsValidTarget(row, col));

            return (row, col);
        }

        public void RecordHit(int row, int col, bool shipSunk)
        {
            _lastHits.Add((row, col));

            if (!shipSunk)
            {
                // Add adjacent cells as potential targets
                AddPotentialTarget(row - 1, col);  // Up
                AddPotentialTarget(row + 1, col);  // Down
                AddPotentialTarget(row, col - 1);  // Left
                AddPotentialTarget(row, col + 1);  // Right
            }
            else
            {
                // If ship is sunk, clear last hits and potential targets
                _lastHits.Clear();
                _potentialTargets.Clear();
            }
        }

        private void AddPotentialTarget(int row, int col)
        {
            if (row >= 0 && row < Board.Size && col >= 0 && col < Board.Size &&
                !_potentialTargets.Contains((row, col)) && !_lastHits.Contains((row, col)))
            {
                _potentialTargets.Add((row, col));
            }
        }

        public void PlaceShipsRandomly(Board board)
        {
            ShipType[] shipTypes = (ShipType[])Enum.GetValues(typeof(ShipType));

            foreach (var shipType in shipTypes)
            {
                bool placed = false;
                Ship ship = new Ship(shipType);

                while (!placed)
                {
                    int row = _random.Next(Board.Size);
                    int col = _random.Next(Board.Size);
                    bool isHorizontal = _random.Next(2) == 0;

                    placed = board.PlaceShip(ship, row, col, isHorizontal);
                }
            }
        }
    }
}
