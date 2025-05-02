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

        public bool PlaceShip(Ship ship, int row, int col, bool isHorizontal, bool isLeftToRight = false)
        {
            // For submarines, we need to know if it's left-to-right or right-to-left diagonal
            if (ship.Type == ShipType.Submarine)
            {
                return PlaceSubmarine(ship, row, col, isLeftToRight);
            }
            // For destroyers (2x2 square), orientation doesn't matter
            else if (ship.Type == ShipType.Destroyer)
            {
                return PlaceDestroyer(ship, row, col);
            }
            // For cruisers (3 consecutive cells)
            else if (ship.Type == ShipType.Cruiser)
            {
                return PlaceCruiser(ship, row, col, isHorizontal);
            }

            return false;
        }

        private bool PlaceSubmarine(Ship ship, int row, int col, bool isLeftToRight)
        {
            ship.SetSubmarineDirection(isLeftToRight);

            // Check if submarine fits on board (3 diagonal cells)
            if (isLeftToRight)
            {
                // Left to right diagonal (\)
                if (row + 2 >= Size || col + 2 >= Size)
                    return false;
            }
            else
            {
                // Right to left diagonal (/)
                if (row + 2 >= Size || col - 2 < 0)
                    return false;
            }

            // Check if submarine overlaps with another ship
            for (int i = 0; i < 3; i++)
            {
                int currentRow = row + i;
                int currentCol = isLeftToRight ? col + i : col - i;

                if (Grid[currentRow, currentCol] == CellState.Ship)
                    return false;
            }

            // Place submarine (3 diagonal cells)
            for (int i = 0; i < 3; i++)
            {
                int currentRow = row + i;
                int currentCol = isLeftToRight ? col + i : col - i;

                Grid[currentRow, currentCol] = CellState.Ship;
                ship.AddCoordinate(currentRow, currentCol);
            }

            Ships.Add(ship);
            return true;
        }

        private bool PlaceDestroyer(Ship ship, int row, int col)
        {
            // Check if destroyer fits on board (2x2 square)
            if (row + 1 >= Size || col + 1 >= Size)
                return false;

            // Check if destroyer overlaps with another ship
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    int currentRow = row + r;
                    int currentCol = col + c;

                    if (Grid[currentRow, currentCol] == CellState.Ship)
                        return false;
                }
            }

            // Place destroyer (2x2 square)
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    int currentRow = row + r;
                    int currentCol = col + c;

                    Grid[currentRow, currentCol] = CellState.Ship;
                    ship.AddCoordinate(currentRow, currentCol);
                }
            }

            Ships.Add(ship);
            return true;
        }

        private bool PlaceCruiser(Ship ship, int row, int col, bool isHorizontal)
        {
            // Check if cruiser fits on board (3 consecutive cells)
            if (isHorizontal)
            {
                if (col + 2 >= Size)
                    return false;
            }
            else
            {
                if (row + 2 >= Size)
                    return false;
            }

            // Check if cruiser overlaps with another ship
            for (int i = 0; i < 3; i++)
            {
                int currentRow = isHorizontal ? row : row + i;
                int currentCol = isHorizontal ? col + i : col;

                if (Grid[currentRow, currentCol] == CellState.Ship)
                    return false;
            }

            // Place cruiser (3 consecutive cells)
            for (int i = 0; i < 3; i++)
            {
                int currentRow = isHorizontal ? row : row + i;
                int currentCol = isHorizontal ? col + i : col;

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
