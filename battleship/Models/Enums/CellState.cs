using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship.Models.Enums
{
    /// <summary>
    /// Represents the possible states of a cell on the game board
    /// </summary>
    public enum CellState
    {
        Empty,  // Water, no ship or attack
        Ship,   // Contains a ship, not hit
        Hit,    // Contains a ship that was hit
        Miss    // Was attacked but no ship present
    }
}
