using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship.Models.Enums
{
    /// <summary>
    /// Represents the different types of ships in the game
    /// </summary>
    public enum ShipType
    {
        Destroyer,  // 2x2 square (4 cells)
        Submarine,  // 3 diagonal cells
        Cruiser     // 3 consecutive cells (horizontal or vertical)
    }
}
