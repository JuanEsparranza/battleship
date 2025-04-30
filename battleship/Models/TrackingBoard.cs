using battleship.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship.Models
{
    public class TrackingBoard
    {
        public const int Size = 10;
        public CellState[,] Grid { get; private set; }

        public TrackingBoard()
        {
            Grid = new CellState[Size, Size];
        }

        public void MarkHit(int row, int col)
        {
            Grid[row, col] = CellState.Hit;
        }

        public void MarkMiss(int row, int col)
        {
            Grid[row, col] = CellState.Miss;
        }

        public bool IsValidTarget(int row, int col)
        {
            return row >= 0 && row < Size && col >= 0 && col < Size &&
                  Grid[row, col] != CellState.Hit && Grid[row, col] != CellState.Miss;
        }
    }
}
