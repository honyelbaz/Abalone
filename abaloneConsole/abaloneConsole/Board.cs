using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace abaloneConsole
{
    class Board
    {
        // we make a matix, look at it like paralelogram and we cut it by ranges to get hexagon
        public static Vector2[] ranges = {
                                    new Vector2(4, 9) ,
                                    new Vector2(3, 9) ,
                                    new Vector2(2, 9) ,
                                    new Vector2(1, 9) ,
                                    new Vector2(0, 9) ,
                                    new Vector2(0, 8) ,
                                    new Vector2(0, 7) ,
                                    new Vector2(0, 6) ,
                                    new Vector2(0, 5)
                                  };

        // The size of the matrix although some of it is cut 
        public static int size = ranges.Length;

        public static Vector2[] directions =
                                            {
                                    new Vector2(-1, 0),
                                    new Vector2(-1, 1),
                                    new Vector2(0,  1),
                                    new Vector2(1,  0),
                                    new Vector2(0,  -1),
                                    new Vector2(1, -1)
                                 };


        // 
        Ball[,] mat;

        public Board()
        {
            this.mat = new Ball[size, size];
            initBoard();
        }

        //give the board values where needs to be and nulls where not
        private void initBoard()
        {

            // insert empty balls the get the structure
            for (int indexOfRange = 0; indexOfRange < ranges.Length; indexOfRange++)
            {
                for (int indexByRange = RangeX(indexOfRange); indexByRange < RangeY(indexOfRange); indexByRange++)
                {
                    mat[indexOfRange, indexByRange] = new Ball(indexOfRange, indexByRange, '.');
                }
            }

            //insert the actual balls in 
            for (int indexOfRange = 0; indexOfRange < 2; indexOfRange++)
            {
                for (int indexByRange = RangeX(indexOfRange); indexByRange < RangeY(indexOfRange); indexByRange++)
                {
                    mat[indexOfRange, indexByRange].ChangeColor('B');
                }

                for (int indexByRange = RangeX(size - 1 - indexOfRange); indexByRange < RangeY(size - 1 - indexOfRange); indexByRange++)
                {
                    mat[size - 1 - indexOfRange, indexByRange].ChangeColor('W');
                }
            }
            mat[2, (int)((RangeX(2) + RangeY(2)) / 2.0)].ChangeColor('B');
            mat[2, 1 + (int)((RangeX(2) + RangeY(2)) / 2.0)].ChangeColor('B');
            mat[2, -1 + (int)((RangeX(2) + RangeY(2)) / 2.0)].ChangeColor('B');

            mat[size - 1 - 2, (int)((RangeX(size - 1 - 2) + RangeY(size - 1 - 2)) / 2.0)].ChangeColor('W');
            mat[size - 1 - 2, 1 + (int)((RangeX(size - 1 - 2) + RangeY(size - 1 - 2)) / 2.0)].ChangeColor('W');
            mat[size - 1 - 2, -1 + (int)((RangeX(size - 1 - 2) + RangeY(size - 1 - 2)) / 2.0)].ChangeColor('W');


        }

        //draw each ball in the board
        public void DrawBoard()
        {
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    Ball.DrawBall(mat[row, col], row, col);
                }
            }

        }

        //if the index is in the hexagon
        public static bool IsValid_Index(int row, int col)
        {
            if (row >= size || row < 0)
                return false;
            return col >= ranges[row].X && col < ranges[row].Y;
        }

        //
        public Ball GetBall(int row, int col)
        {
            return mat[row, col];
        }

        public static int RangeX(int indexOfRange)
        {
            return (int)ranges[indexOfRange].X;
        }

        public static int RangeY(int indexOfRange)
        {
            return (int)ranges[indexOfRange].Y;
        }

    }
}
