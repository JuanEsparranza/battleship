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
        Carrier,     // 5 cells
        Battleship,  // 4 cells
        Cruiser,     // 3 cells
        Submarine,   // 3 cells
        Destroyer    // 2 cells
    }
}
