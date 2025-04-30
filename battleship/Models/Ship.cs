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

        public Ship(ShipType type)
        {
            Type = type;
            Size = GetShipSize(type);
            HitsReceived = 0;
            Coordinates = new List<(int, int)>();
        }

        private int GetShipSize(ShipType type)
        {
            return type switch
            {
                ShipType.Carrier => 5,
                ShipType.Battleship => 4,
                ShipType.Cruiser => 3,
                ShipType.Submarine => 3,
                ShipType.Destroyer => 2,
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
    }
}
