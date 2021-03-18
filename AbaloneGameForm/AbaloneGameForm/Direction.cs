using System;
using System.Collections.Generic;
using System.Text;

namespace AbaloneGameForm
{
    class Direction
    {
        public int row { get; set; }
        public int col { get; set; }

        public Direction(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public Direction(Direction other)
        {
            this.col = other.col;
            this.row = other.row;
        }
    }
}
