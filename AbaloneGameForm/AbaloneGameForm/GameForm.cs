using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbaloneGameForm
{
    public partial class GameForm : Form
    {
        //int BOARDSIZE = 660;
        Board board;
        public GameForm()
        {
            InitializeComponent();
            pictureBox1.Bounds = new Rectangle( 10,10,Board.ROWS * (Piece.PIECESIZE +10)+100, Board.ROWS*(Piece.PIECESIZE+4)+80);
            this.Size = new Size(pictureBox1.Width + 200, pictureBox1.Height + 100);
            labelMessage.Location = new Point(pictureBox1.Right + 40, pictureBox1.Top);
            labelTurn.Location = new Point(pictureBox1.Right + 40, pictureBox1.Top+50);
            board = new Board(this);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            board.Paint(e.Graphics);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            board.Click(e.Location);
            pictureBox1.Invalidate();
        }


    }
}
