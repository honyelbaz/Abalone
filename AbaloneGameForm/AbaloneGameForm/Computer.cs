using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace AbaloneGameForm
{
    internal class Computer
    {
        static Board board;
        static Random random = new Random();
        public static void DoStep(Board b)
        {
            board = b;
            Thread threadThink = new Thread(new ThreadStart(ThinkFun));
            threadThink.IsBackground = true;
            threadThink.Start();
        }

        private static void ThinkFun()
        {
            Thread.Sleep(100);
            Move move = GetAiMove(3, board.currentPlayer, int.MinValue, int.MaxValue, true).move;
            board.MakeMove(move);
            board.readyForClick = true;
            board.SwitchPlayer();
            if ( Board.gameForm.InvokeRequired )
            {
                    Board.gameForm.Invoke((MethodInvoker)  delegate
                   {
                       Board.gameForm.pictureBox1.Invalidate();
                   });
            }
           
        }

        private static EMove GetAiMove(int depth, int player, int a, int b, bool isMaximaizingPlayer)
        {
            int winFlag = board.CheckWin(player);
            if (depth == 0 || winFlag == 1 || winFlag == -1)
                return new EMove(Eval(player), null);

            List<Move> moves = All_Possible_Moves();
            EMove bestMove;
            bestMove = new EMove(int.MinValue, moves[(new Random()).Next(moves.Count)]);

            foreach (Move move in moves)
            {
                MoveDetails md = MakeAIMove(move);
                int opponent = board.OtherPlayer(player);

                int score = -GetAiMove(depth - 1, opponent, -b, -a, !isMaximaizingPlayer).value;

                if (score > bestMove.value)
                {
                    bestMove.value = score;
                    bestMove.move = move;
                }
                else if (score == bestMove.value)
                {
                    if (random.Next(14) == 4)
                    {
                        bestMove.value = score;
                        bestMove.move = move;
                    }
                }
                UndoMove(md);

                if (bestMove.value >= b) break;
            }
            return bestMove;

        }

        public static List<Move> All_Possible_Moves()
        {
            Player player = board.GetPlayer(board.currentPlayer);
            List<Move> lst = new List<Move>();
            Dictionary<int, Piece> pss = player.GetPieces();

            foreach (Piece piece in pss.Values)
                lst.AddRange(board.Possible_Moves_for_Index(piece.GetRow(), piece.GetCol()));

            return lst;
        }

        private static int Eval(int player)
        {
            if (board.CheckWin(player) == 1) return 1000;
            if (board.CheckWin(board.OtherPlayer(player)) == -1) return -1000;

            Player cur = board.GetPlayer(player);
            return (board.GetPlayer(player).GetCount() - board.GetPlayer(board.OtherPlayer(player)).GetCount()) * 20 + GetStrongPositionCount(cur);
        }

        private static MoveDetails MakeAIMove(Move move)
        {
            MoveDetails md;
            int startcount1 = board.player1.GetCount();
            int startcount2 = board.player2.GetCount();

            int row = move.row;
            int col = move.col;

            int pusherCount = 0;
            int pushedCount = 0;

            int color = board.GetPiece(move.row, move.col).GetColor();
            Piece piece = board.GetPiece(row, col);
            while (piece != null)
            {
                if (piece.GetColor() == color)
                    pusherCount++;
                else if (piece.GetColor() == board.OtherPlayer(color))
                    pushedCount++;


                row += move.RowDir();
                col += move.ColDir();
                piece = board.GetPiece(row, col);
            }

            board.MakeMove(move);

            if (color == board.player1.GetColor())
                md = new MoveDetails(move, color, pusherCount, pushedCount, startcount1 - board.player1.GetCount(), startcount2 - board.player2.GetCount());
            else
                md = new MoveDetails(move, color, pusherCount, pushedCount, startcount2 - board.player2.GetCount(), startcount1 - board.player1.GetCount());

            return md;
        }

        private static void UndoMove(MoveDetails md)
        {
            int color = md.color;
            Move move = md.move;

            if (md.pusherCount == 1 && md.suicieds == 1)
            {
                board.ChangeColor(move.row, move.col, color);
                return;
            }
            int row = move.row;
            int col = move.col;

            for (int i = 0; i < md.pusherCount + md.pushedCount - (md.killed != 0 || md.suicieds != 0 ? 1 : 0); i++)
            {
                row += move.RowDir();
                col += move.ColDir();
            }

            Move undoMove = new Move(row, col, new Direction(move.direction.row * -1, move.direction.col * -1));
            board.MakeMove(undoMove);
            if (md.suicieds == 1)
                board.ChangeColor(row, col, color);
            if (md.killed == 1)
                board.ChangeColor(row, col, board.OtherPlayer(color));

        }

        private static int GetStrongPositionCount(Player player)
        {
            int count = 0;
            foreach (Piece piece in player.GetPieces().Values)
            {
                int row = piece.GetRow();
                int col = piece.GetCol();
                int rowmid = 9 / 2;
                int colmid = 17 / 2;
                if (row > rowmid - 3 && row < rowmid + 3 && col > colmid - 3 && col < colmid + 3)
                    count++;
            }
            return count;
        }

        class EMove
        {
            public Move move;
            public int value;

            public EMove(int value, Move move)
            {
                this.move = move;
                this.value = value;
            }
        }
    }
}