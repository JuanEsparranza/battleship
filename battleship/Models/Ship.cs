using battleship.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship.Models
{
    public class Ship
    {
        public ShipType Type { get; private set; }
        public int Size { get; private set; }
        public int HitsReceived { get; private set; }
        public List<(int, int)> Coordinates { get; private set; }
        public bool IsSubmarine { get; private set; }
        public bool IsLeftToRight { get; private set; } // For submarine diagonal direction

        public Ship(ShipType type)
        {
            Type = type;
            Size = GetShipSize(type);
            HitsReceived = 0;
            Coordinates = new List<(int, int)>();
            IsSubmarine = type == ShipType.Submarine;
            IsLeftToRight = false; // Default direction, will be set during placement
        }

        private int GetShipSize(ShipType type)
        {
            return type switch
            {
                ShipType.Destroyer => 4, // 2x2 square
                ShipType.Submarine => 3, // 3 diagonal cells
                ShipType.Cruiser => 3,   // 3 consecutive cells
                _ => throw new ArgumentException("Invalid ship type")
            };
        }

        public void AddCoordinate(int row, int col)
        {
            Coordinates.Add((row, col));
        }

        public bool Hit()
        {
            HitsReceived++;
            return IsSunk();
        }

        public bool IsSunk()
        {
            return HitsReceived >= Size;
        }

        public bool Contains(int row, int col)
        {
            return Coordinates.Contains((row, col));
        }

        public void SetSubmarineDirection(bool isLeftToRight)
        {
            if (Type == ShipType.Submarine)
            {
                IsLeftToRight = isLeftToRight;
            }
        }
    }
}
