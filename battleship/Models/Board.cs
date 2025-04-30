using battleship.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship.Models
{
    public class Board
    {
        public const int Size = 10;
        public CellState[,] Grid { get; private set; }
        public List<Ship> Ships { get; private set; }

        public Board()
        {
            Grid = new CellState[Size, Size];
            Ships = new List<Ship>();
        }

        public bool PlaceShip(Ship ship, int row, int col, bool isHorizontal)
        {
            // Check if the ship fits on the board
            if (isHorizontal)
            {
                if (col + ship.Size > Size)
                    return false;
            }
            else
            {
                if (row + ship.Size > Size)
                    return false;
            }

            // Check if the ship overlaps with another ship
            for (int i = 0; i < ship.Size; i++)
            {
                int currentRow = row + (isHorizontal ? 0 : i);
                int currentCol = col + (isHorizontal ? i : 0);

                if (Grid[currentRow, currentCol] == CellState.Ship)
                    return false;
            }

            // Place the ship
            for (int i = 0; i < ship.Size; i++)
            {
                int currentRow = row + (isHorizontal ? 0 : i);
                int currentCol = col + (isHorizontal ? i : 0);

                Grid[currentRow, currentCol] = CellState.Ship;
                ship.AddCoordinate(currentRow, currentCol);
            }

            Ships.Add(ship);
            return true;
        }

        public (bool hit, Ship? ship) ReceiveAttack(int row, int col)
        {
            if (Grid[row, col] == CellState.Ship)
            {
                Grid[row, col] = CellState.Hit;

                // Find which ship was hit
                Ship? hitShip = Ships.FirstOrDefault(ship => ship.Contains(row, col));
                if (hitShip != null)
                {
                    hitShip.Hit();
                    return (true, hitShip);
                }

                return (true, null);
            }
            else
            {
                Grid[row, col] = CellState.Miss;
                return (false, null);
            }
        }

        public bool AllShipsSunk()
        {
            return Ships.All(ship => ship.IsSunk());
        }
    }
}
