using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AbaloneGameForm
{
    class Move
    {
        public int row;
        public int col;
        public Direction direction;

        public Move(int row, int col, Direction direction)
        {
            this.row = row;
            this.col = col;
            this.direction = direction;
        }

        public Move(Point from, Point to)
        {
            this.row = from.X;
            this.col = from.Y;
            this.direction = new Direction(to.X - from.X, to.Y - from.Y);
        }


        public int RowDir()
        {
            return direction.row;
        }
        public int ColDir()
        {
            return direction.col;
        }

        public static bool IsGoodForMove(Point from, Point to)
        {
            Direction dir = new Direction(to.X - from.X, to.Y - from.Y);
            return Board.InDirections(dir);
        }
    }
}
