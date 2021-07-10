using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;




namespace Sudoku
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr dc);

        Dictionary<Tuple<byte, byte>, Button> buttonKeys;
        Dictionary<Button, Tuple<byte, byte>> buttonIndices;
        Dictionary<PictureBox, byte> pictureBoxKeys;
        Board board;
        Tuple<bool, byte> drawImage;
        Image[] images;
        Graphics screenGraphics;
        Button currB = null;

        public Form1()
        {
            InitializeComponent();

            buttonKeys = new Dictionary<Tuple<byte, byte>, Button>();
            buttonIndices = new Dictionary<Button, Tuple<byte, byte>>();
            board = Board.getNewBoardInst();
            drawImage = Tuple.Create(false, (byte)0);
            pictureBoxKeys = new Dictionary<PictureBox, byte>();
            for (byte i = 1; i < 10; i++)
            {
                string name = "pictureBox" + i;
                pictureBoxKeys[(PictureBox)Controls.Find(name, true)[0]] = i;
            }
            images = new Image[10] {
                null,
                new Bitmap(Sudoku.Properties.Resources.s1),
                new Bitmap(Sudoku.Properties.Resources.s2),
                new Bitmap(Sudoku.Properties.Resources.s3),
                new Bitmap(Sudoku.Properties.Resources.s4),
                new Bitmap(Sudoku.Properties.Resources.s5),
                new Bitmap(Sudoku.Properties.Resources.s6),
                new Bitmap(Sudoku.Properties.Resources.s7),
                new Bitmap(Sudoku.Properties.Resources.s8),
                new Bitmap(Sudoku.Properties.Resources.s9),
            };
            setButtons(board.Game_Board);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            screenGraphics = Graphics.FromHdc(GetDC(IntPtr.Zero));
        }

        private void setButtons(byte[,] board)
        {
            byte counter = 1;
            for (byte i = 0; i < 9; i++)
            {
                for (byte j = 0; j < 9; j++)
                {
                    Button b = getButtonFromID(counter);
                    byte id = board[i, j];
                    setButton(b, images[id], i, j);
                    counter++;
                }
            }
        }

        private void setButton(Button b, Image i, byte row, byte col)
        {
            var t = Tuple.Create(row, col);
            buttonKeys[t] = b;
            buttonIndices[b] = t;
            setButtonImage(b, i);
        }

        private void setButtonImage(Button b, Image i)
        {
            b.Image = i;
            b.Text = "";
        }

        private Button getButtonFromID(byte id)
        {
            return (Button)Controls.Find("button" + id.ToString(), true)[0];
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

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            byte id = (byte)pictureBoxKeys[box];
            drawImage = Tuple.Create(true, id);

            timer1.Start();
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Button button = getMouseButton();
            if (button != null)
            {
                setButtonImage(button, ((PictureBox)sender).Image);
            }

            drawImage = Tuple.Create(false, (byte)0);
            timer1.Stop();
            Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            screenGraphics.DrawImage(images[drawImage.Item2], Cursor.Position);
            Button button = getMouseButton();
            if (button != null)
            {
                var t = buttonIndices[button];
                if (board.isValidMove(drawImage.Item2, t.Item1, t.Item2))
                    button.BackColor = Color.Green;

                else
                    button.BackColor = Color.Red;
            }
        }

        private void button81_MouseMove(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;
            if (drawImage.Item1)
            {

                Console.WriteLine("Moved: " + b.Name);
                var t = buttonIndices[b];
                if (board.isValidMove(drawImage.Item2, t.Item1, t.Item2))
                    b.BackColor = Color.Green;

                else
                    b.BackColor = Color.Red;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.BackColor == Color.Red)
                button1.BackColor = Control.DefaultBackColor;
            else
                button1.BackColor = Color.Red;
        }
    }
}
