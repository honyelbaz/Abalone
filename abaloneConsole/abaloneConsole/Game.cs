using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace abaloneConsole
{
    class Game
    {
        Board brd;
        public Game()
        {
            brd = new Board();
        }

        public void Start()
        {
            int row = 0;
            int col = 0;

            bool gameRunning = true;
            while (gameRunning)
            {
                brd.DrawBoard();
                Input.ReceiveMove_ForceValidation(ref row, ref col);
                brd.GetBall(row, col).ChangeColor('B');
            }
        }


        public List<Vector2> PossibleMoves(int row, int col)
        {

            return null;
        }


    }
}
