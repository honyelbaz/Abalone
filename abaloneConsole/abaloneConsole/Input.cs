using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace abaloneConsole
{
    class Input
    {
        public static Vector2 ReceiveMove()
        {
            try
            {

                int row = int.Parse(Console.ReadLine());
                int col = int.Parse(Console.ReadLine());

                return new Vector2(row, col);

            }
            catch (FormatException)
            {
                Console.WriteLine("insert a number please");
                return ReceiveMove();
            }
        }

        public static void ReceiveMove(ref int row, ref int col)
        {
            Vector2 move = ReceiveMove();
            row = (int)move.X;
            col = (int)move.Y;
        }

        public static void ReceiveMove_ForceValidation(ref int row, ref int col)
        {
            ReceiveMove(ref row,ref col);
            if (!Board.IsValid_Index(row, col))
            {
                Console.WriteLine("invalid index, try again");
                ReceiveMove_ForceValidation(ref row, ref col);
            }
        }
    }


}
