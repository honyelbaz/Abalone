using System;
using System.Drawing;
using System.Windows.Forms;

namespace AbaloneGameForm
{
    internal class Piece
    {
        public static int PIECESIZE = 35;
        int row, col;
        int color;
        public Piece(int color, int row, int col)
        {
            this.color = color;
            this.row = row;
            this.col = col;
        }

        // draw a piece
        public void Paint(Graphics graphics)
        {
            Image image = null; 

            switch (color)
            {
                case 1: //                   BLACK_PLAYER:
                    image = Properties.Resources.black_ball;
                    break;
                case 2:  //                    WHITE_PLAYER:
                    image = Properties.Resources.white_ball;
                    break;
            }
            int x = 47 + col * (PIECESIZE*2/3);
            int y = 40 + row*(PIECESIZE + 4);
            graphics.DrawImage(image, x, y, PIECESIZE, PIECESIZE);
        }

        public void ChangePosition(int row, int col)
        {
            this.row = row;
            this.col = col; 
        }

        public int GetColor()
        {
            return color;
        }

        public int GetRow()
        {
            return row;
        }
        public int GetCol()
        {
            return col;
        }
    }
}