using System;
using System.Collections.Generic;
using System.Drawing;

namespace AbaloneGameForm
{
    internal class Player
    {
        Dictionary<int, Piece> pieces = new Dictionary<int, Piece>();
        private int color;

        public Player(int color)
        {
            this.color = color;
            int row = color == 1 ? 0 : Board.ROWS - 1;
            int factor = color == 1 ? 1 :  -1;
            int [] count = { 5, 6, 3 };
            int[] col_start = { 4, 3, 6 };
            for (int i = 0; i < 3; i++,row+=factor)
            {
                for (int j = col_start[i]; j < col_start[i] +2*count[i]; j+=2)
                {
                    pieces.Add(TranslateToKey(row, j), new Piece(color, row, j)) ;
                }
            }
           
        }
        public int GetColor()
        {
            return color;
        }

        internal void Paint(Graphics graphics)
        {
            foreach (Piece piece in pieces.Values)
                piece.Paint(graphics);
        }
        public int GetCount()
        {
            return pieces.Count;
        }

        internal Piece GetPiece(int row, int col)
        {
            int key = row * Board.COLUMNS + col;
            return pieces.ContainsKey(key) ? pieces[key] : null;
        }
        internal Piece GetPiece(Point p)
        {
            int key = p.X * Board.COLUMNS +p.Y;
            return pieces.ContainsKey(key) ? pieces[key] : null;
        }

        public static int TranslateToKey(int row, int col)
        {
            return row * Board.COLUMNS + col;
        }

        public void MovePiece(Point p1, Point p2)
        {
            Piece piece = GetPiece(p1);
            piece.ChangePosition(p2.X, p2.Y);
        }

        public bool HasPiece(int row, int col)
        {
            return pieces.ContainsKey(TranslateToKey(row, col));
        }

        public void RemovePiece(int row, int col)
        {
            pieces.Remove(TranslateToKey(row, col));
        }
        public void AddPiece(int row, int col)
        {
            pieces.Add(TranslateToKey(row, col), new Piece(color, row, col));
        }

        public Dictionary<int, Piece> GetPieces()
        {
            return pieces;
        }
    }
}