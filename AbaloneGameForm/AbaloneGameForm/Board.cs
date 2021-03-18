using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbaloneGameForm
{
    /// <summary>
    /// 
    /// </summary>
    class Board
    {
        enum GameType { AI_AI, AI_PLAYER, PLAYER_PLAYER };

        public static int CENTER_ROW_INDEX = 4;
        public static int COLUMNS = 17;
        public static int ROWS = 9;
        public Player player1, player2;
        int player1_StartingCount, player2_StartingCount;
        public int currentPlayer;
        public static GameForm gameForm;
        private Stack<MoveDetails> lastmoveStack;

        //sets to false when ai thinks of a move
        public bool readyForClick = true;

        GameType gametype = GameType.PLAYER_PLAYER;
        // we make a matix, look at it like paralelogram and we cut it by ranges to get hexagon
        public static Point[] ranges = {
                                    new Point(4, 12) ,
                                    new Point(3, 13) ,
                                    new Point(2, 14) ,
                                    new Point(1, 15) ,
                                    new Point(0, 16) ,
                                    new Point(1, 15) ,
                                    new Point(2, 14) ,
                                    new Point(3, 13) ,
                                    new Point(4, 12) ,
                                  };

        // The size of the matrix although some of it is cut 
        public static int size = ranges.Length;

        public static Direction[] directions =
                                            {
                                    new Direction( 0,  2),
                                    new Direction(-1,  1),
                                    new Direction(-1, -1),
                                    new Direction( 0, -2),
                                    new Direction( 1, -1),
                                    new Direction( 1,  1)
                                 };


        Point firstClicked;
        Point secondClicked;

        public Player GetPlayer(int color)
        {
            return player1.GetColor() == color ? player1 : player2;
        }

        public Board(GameForm gameform)
        {
            gameForm = gameform;
            //1 is Black
            // 2 is White

            player1 = new Player(1);
            player2 = new Player(2);
            currentPlayer = 1;

            player1_StartingCount = player1.GetCount();
            player2_StartingCount = player2.GetCount();

            if (currentPlayer == 1)
                gameForm.labelTurn.Text = "Black's Turn";
            if (currentPlayer == 2)
                gameForm.labelTurn.Text = "White's Turn";

            lastmoveStack = new Stack<MoveDetails>();

            player1.AddPiece(2, 4);
            player2.AddPiece(2, 2);
        }
        internal void Paint(Graphics graphics)
        {
            player1.Paint(graphics);
            player2.Paint(graphics);
        }

        public Piece GetPiece(int row, int col)
        {
            Piece piece = null;
            if (player1.HasPiece(row, col))
                piece = player1.GetPiece(row, col);

            else if (player2.HasPiece(row, col))
                piece = player2.GetPiece(row, col);

            return piece;
        }

        public void MovePiece(Point p1, Point p2)
        {
            Piece piece;
            piece = GetPiece(p1.X, p1.Y);
            piece.ChangePosition(p2.X, p2.Y);
        }

        public bool InRange(int row, int col)
        {
            if (row < 0 || row >= 9)
                return false;

            return (ranges[row].X <= col && ranges[row].Y >= col);
        }

        public static bool InDirections(Direction dir_to_check)
        {
            foreach (Direction dir in directions)
            {
                if (dir_to_check.row == dir.row && dir_to_check.col == dir.col)
                    return true;
            }
            return false;
        }

        public bool IsValidPoint(int row, int col)
        {
            return InRange(row, col);
        }

        internal void Click(Point location)
        {
            if (!readyForClick)
                return;
            if (gametype == GameType.PLAYER_PLAYER)
                TWOPlayersMove(location);
            else if (gametype == GameType.AI_PLAYER)
                Player_AI_Move(location);
        }

        private void Player_AI_Move( Point location)
        {

            int row = (location.Y - 40) / (Piece.PIECESIZE + 4);
            int col = (location.X - 47) / (Piece.PIECESIZE * 2 / 3);

            Possible_Moves_for_Index(row, col);

            Piece piece1 = player1.GetPiece(row, col);
            Piece piece2 = player2.GetPiece(row, col);
            gameForm.labelMessage.Text = row + ", " + col;
            Move move;
            if (!IsValidPoint(row, col))
            {
                gameForm.labelMessage.Text = "not valid click";
                return;
            }

            if (firstClicked.Y == 0 && firstClicked.X == 0)
            {
                firstClicked.X = row;
                firstClicked.Y = col;

                gameForm.labelMessage.Text = "waiting for second click";
                return;
            }
            secondClicked.X = row;
            secondClicked.Y = col;


            if (Move.IsGoodForMove(firstClicked, secondClicked))
                move = new Move(firstClicked, secondClicked);
            else
            {
                gameForm.labelMessage.Text = "not valid move";
                firstClicked.X = 0;
                firstClicked.Y = 0;
                return;
            }

            if (IsPossible(move))
            {
                MakeMove(move);
                CheckWin(currentPlayer);
                //switch player and then print it 
                SwitchPlayer();
                //switch player missing
                gameForm.labelTurn.Text = currentPlayer == 1 ? "Black's Turn" : "White's Turn";
                readyForClick = false;
                Computer.DoStep(this);
            }
            else
            {
                gameForm.labelMessage.Text = "not possible move";
                firstClicked.X = 0;
                firstClicked.Y = 0;
            }                 
        }

        private void TWOPlayersMove( Point location )
        {

            int row = (location.Y - 40) / (Piece.PIECESIZE + 4);
            int col = (location.X - 47) / (Piece.PIECESIZE * 2 / 3);

            Possible_Moves_for_Index(row, col);

            Piece piece1 = player1.GetPiece(row, col);
            Piece piece2 = player2.GetPiece(row, col);
            gameForm.labelMessage.Text = row + ", " + col;
            Move move;
            if (!IsValidPoint(row, col))
            {
                gameForm.labelMessage.Text = "not valid click";
                return;
            }

            if (firstClicked.Y == 0 && firstClicked.X == 0)
            {
                firstClicked.X = row;
                firstClicked.Y = col;

                gameForm.labelMessage.Text = "waiting for second click";
                return;
            }
            secondClicked.X = row;
            secondClicked.Y = col;


            if (Move.IsGoodForMove(firstClicked, secondClicked))
                move = new Move(firstClicked, secondClicked);
            else
            {
                gameForm.labelMessage.Text = "not valid move";

                firstClicked.X = 0;
                firstClicked.Y = 0;
                return;
            }

            if (IsPossible(move))
            {
                MakeMove(move);
                CheckWin(currentPlayer);
            }
            else
            {
                gameForm.labelMessage.Text = "not possible move";

                firstClicked.X = 0;
                firstClicked.Y = 0;
                return;
            }

            //switch player and then print it 
            SwitchPlayer();
            firstClicked.X = 0;
            firstClicked.Y = 0;

            //switch player missing

            gameForm.labelTurn.Text = currentPlayer == 1 ? "Black's Turn" : "White's Turn";
        }
        private bool IsPossible(Move move)
        {
            if (move == null)
                return false;
            if (GetPiece(move.row, move.col) == null)
                return false;
            if (GetPiece(move.row, move.col).GetColor() != currentPlayer)
                return false;
            int row = move.row;
            int col = move.col;

            int curCounter = 0;
            int otherCounter = 0;

            bool alreadySwitchedFlag = false;

            for (int i = 0; i < 6; i++)
            {
                try
                {
                    if (GetPiece(row, col).GetColor() == currentPlayer)
                    {
                        if (alreadySwitchedFlag)
                            return false;
                        curCounter++;

                        if (curCounter > 3)
                            return false;

                    }
                    else if (GetPiece(row, col).GetColor() == OtherPlayer(currentPlayer))
                    {
                        alreadySwitchedFlag = true;
                        otherCounter++;
                        if (curCounter <= otherCounter)
                            return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    return true;
                }
                catch (NullReferenceException)
                {
                    return true;
                }


                row += move.RowDir();
                col += move.ColDir();
            }


            return true;
        }

        public List<Move> Possible_Moves_for_Index(int row, int col)
        {
            List<Move> lst = new List<Move>();
            Move move = new Move(row, col, null);
            foreach (Direction dir in directions)
            {
                move.direction = dir;
                if (IsPossible(move))
                    lst.Add(new Move(row, col, new Direction(dir)));
            }
            return lst;
        }

        public int OtherPlayer(int currentPlayer)
        {
            return currentPlayer == player1.GetColor() ? player2.GetColor() : player1.GetColor();

        }

        public int CheckWin(int player)
        {
            if (player1_StartingCount - player1.GetCount() >= 6)
                return player == player1.GetColor() ? 1 : -1;
            if (player2_StartingCount - player2.GetCount() >= 6)
                return player == player2.GetColor() ? 1 : -1;
            return 0;


        }

        private void EndGame()
        {
            //throw new NotImplementedException();
        }

        public void MakeMove(Move move)
        {
            int row = move.row;
            int col = move.col;

            int rowDir = move.RowDir();
            int colDir = move.ColDir();

            int temp1 = GetPiece(row, col).GetColor();
            int temp2;
            try
            {
                temp2 = GetPiece(row + rowDir, col + colDir).GetColor();
            }
            catch (NullReferenceException)
            {
                RemovePiece(move.row, move.col);
                ChangeColor(row + rowDir, col + colDir, temp1);
                return;
            }

            while (GetPiece(row, col) != null)
            {
                try
                {
                    temp2 = GetPiece(row + rowDir, col + colDir).GetColor();
                }
                catch (NullReferenceException)
                {
                    ChangeColor(row + rowDir, col + colDir, temp1);
                    break;
                }
                row += rowDir;
                col += colDir;
                ChangeColor(row, col, temp1);
                temp1 = temp2;
            }

            RemovePiece(move.row, move.col);
        }


        private void RemovePiece(int row, int col)
        {
            Piece piece = GetPiece(row ,col);
            if (piece == null)
                return;
            if (piece.GetColor() == player1.GetColor())
                player1.RemovePiece(row, col);
            else player2.RemovePiece(row, col);
        }

        public void ChangeColor(int row, int col, int newColor)
        {
            if (!IsValidPoint(row, col))
                return;
            Piece piece = GetPiece(row, col); 
            if (piece != null)
            {
                int color = piece.GetColor();
                if (color == player1.GetColor())
                    player1.RemovePiece(row, col);
                else
                    player2.RemovePiece(row, col);
            }
            if (newColor == player1.GetColor())
                player1.AddPiece(row, col);
            else
                player2.AddPiece(row, col);

        }

        public void SwitchPlayer()
        {
            if (currentPlayer == player1.GetColor())
                currentPlayer = player2.GetColor();
            else
                currentPlayer = player1.GetColor();
            firstClicked.X = 0;
            firstClicked.Y = 0;
        }


    }

    enum PlayerColor { EMPTY, BLACK, WHITE};
    internal class MoveDetails
    {
        public Move move;
        public int color;
        public int pusherCount;
        public int pushedCount;
        public int suicieds;
        public int killed;

        public MoveDetails(Move move, int color, int pusherCount, int pushedCount, int suicieds , int killed)
        {
            this.move = move;
            this.color = color;
            this.pusherCount = pusherCount;
            this.pushedCount = pushedCount;
            this.suicieds = suicieds;
            this.killed = killed;
        }
    }
}
