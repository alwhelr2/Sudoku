using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        Dictionary<Tuple<byte, byte>, Button> buttonKeys;
        //Board gameBoard;
        Tuple<bool, byte> drawImage;
        Image[] images;

        public Form1()
        {
            InitializeComponent();

            buttonKeys = new Dictionary<Tuple<byte, byte>, Button>();
            //gameBoard = new Board();
            //setButtons(gameBoard);
            drawImage = Tuple.Create(false, (byte)0);
            images = new Image[9];
            images[0] = new Bitmap(Sudoku.Properties.Resources.s1);
            images[1] = new Bitmap(Sudoku.Properties.Resources.s2);
            images[2] = new Bitmap(Sudoku.Properties.Resources.s3);
            images[3] = new Bitmap(Sudoku.Properties.Resources.s4);
            images[4] = new Bitmap(Sudoku.Properties.Resources.s5);
            images[5] = new Bitmap(Sudoku.Properties.Resources.s6);
            images[6] = new Bitmap(Sudoku.Properties.Resources.s7);
            images[7] = new Bitmap(Sudoku.Properties.Resources.s8);
            images[8] = new Bitmap(Sudoku.Properties.Resources.s9);
        }

        private void setButtons(byte[,] board)
        {
            for (byte i = 0; i < 9; i++)
                for (byte j = 0; j < 9; j++)
                {
                    Button b = getButtonFromIndices(i, j);
                    b.Text = board[i, j].ToString();
                }
        }

        private void setButton(byte row, byte col, byte value)
        {
            buttonKeys[Tuple.Create(row, col)].Text = value.ToString();
        }

        private Button getButtonFromIndices(byte row, byte col)
        {
            return buttonKeys[Tuple.Create(row, col)];
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawImage.Item1)
                Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (drawImage.Item1)
            {
                Point local = this.PointToClient(Cursor.Position);
                e.Graphics.DrawImage(images[drawImage.Item2], local);
            }
        }

        private Button getMouseButton()
        {
            var formButtons = GetAll(this, typeof(Button));
            foreach (Button b in formButtons)
            {
                if (b.ClientRectangle.Contains(b.PointToClient(Cursor.Position)))
                {
                    return b;
                }
            }
            return null;
        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            drawImage = Tuple.Create(true, (byte)0);
            Invalidate();
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            drawImage = Tuple.Create(true, (byte)1);
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            drawImage = Tuple.Create(true, (byte)2);
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            drawImage = Tuple.Create(true, (byte)3);
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            drawImage = Tuple.Create(true, (byte)4);
        }

        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            drawImage = Tuple.Create(true, (byte)5);
        }

        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            drawImage = Tuple.Create(true, (byte)6);
        }

        private void pictureBox8_MouseDown(object sender, MouseEventArgs e)
        {
            drawImage = Tuple.Create(true, (byte)7);
        }

        private void pictureBox9_MouseDown(object sender, MouseEventArgs e)
        {
            drawImage = Tuple.Create(true, (byte)8);
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Button button = getMouseButton();
            if (button != null)
            {
                button.Text = (drawImage.Item2 + 1).ToString();

            }

            Console.WriteLine("STOP DRAWING");
            drawImage = Tuple.Create(false, (byte)0);
            Invalidate();
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawImage.Item1)
            {
                Invalidate();
            }
        }
    }
}
