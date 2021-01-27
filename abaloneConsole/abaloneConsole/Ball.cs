using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace abaloneConsole
{
    class Ball
    {
        int row;
        int col;

        // 'R' - Red  ,  'B' - Black  ,  '.' - Empty
        char color;

        public Ball(int row, int col, char color)
        {
            this.row = row;
            this.col = col;
            this.color = color;
        }

        //
        public static void DrawBall(Ball ballToDraw, int row, int col)
        {
            if (col == 0)
            {
                Console.Write(row + " ");
                for (int rus = 0; rus < row; rus++)
                {
                    Console.Write(" ");
                }
            }
            if (ballToDraw == null)
            Console.Write("  ");
            else
                Console.Write("{0} ", ballToDraw.color);
            if (col == Board.RangeY(row) - 1)
            {
                if (row >= 5)
                {
                    Console.Write(8 - row + 5);
                }

            }

            if (col == 8)
            {

                Console.Write("\n");
                if (row == 8)
                {
                    Console.Write("           ");
                    for (int i = 0; i < row - 3; i++)
                    {
                        Console.Write(i + " ");
                    }
                    Console.WriteLine();
                }
            }
            
        }

        //
        public void ChangeColor(char color)
        {
            this.color = color;
        }
    }
}
