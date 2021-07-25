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
using System.Collections;
using System.Drawing.Imaging;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        Dictionary<Button, Tuple<byte, byte>> buttonIndices;
        //TODO: Get rid of using the names of the pictureBoxes
        Dictionary<PictureBox, byte> pictureBoxKeys;
        Board board;
        Tuple<bool, byte> drawImage;
        Bitmap[] images;
        Button currB = null;
        PopupWindow drawWindow;
        bool timeToGame;
        int timerTicks = 0;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Sudoku";
            this.AllowDrop = true;

            buttonIndices = new Dictionary<Button, Tuple<byte, byte>>();
            board = Board.getNewBoardInst(false);
            drawImage = Tuple.Create(false, (byte)0);
            pictureBoxKeys = new Dictionary<PictureBox, byte>();
            for (byte i = 1; i < 10; i++)
            {
                string name = "pictureBox" + i;
                PictureBox box = (PictureBox)Controls.Find(name, true)[0];
                box.AllowDrop = true;
                pictureBoxKeys[box] = i;
            }
            images = new Bitmap[10] {
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
            bool tG = true;
            timeToGame = tG;
            timer2.Start();
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

                    setButton(b, id, i, j);
                    
                    //setButtonImage(b, id, i, j);
                    if (id != 0)
                    {
                        b.Enabled = false;
                        setButtonImage(b, id, i, j, true);
                    }
                    else
                        setButtonImage(b, id, i, j);
                    counter++;
                }
            }
        }

        

        private void setButton(Button b, byte id, byte row, byte col)
        {
            var t = Tuple.Create(row, col);
            buttonIndices[b] = t;
        }

        private void setButtonImage(Button b, byte id, byte row, byte col, bool tint = false)
        {
            b.Image = (tint) ? images[id].ColorTint(0, 0, 0) : images[id];
            b.Text = "";
            b.UseVisualStyleBackColor = true;
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

            PictureBox clonedBox = new PictureBox();
            clonedBox.Image = box.Image;
            clonedBox.Size = new Size(60, 60);
            drawWindow = new PopupWindow(clonedBox);
            drawWindow.Show(Cursor.Position);
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Button button = getMouseButton();

            timer1.Stop();
            drawWindow.Close();
            //Reset the previous button to default state once the user lets go of the mouse button
            if (currB != null)
            {
                currB.UseVisualStyleBackColor = true;
                currB = null;
            }
            if (button == null)
                return;

            var bIndices = buttonIndices[button];

            if (board.isValidMove(board.Game_Board, drawImage.Item2, (byte)bIndices.Item1, (byte)bIndices.Item2))
            {
                
                setButtonImage(button, drawImage.Item2, (byte)bIndices.Item1, (byte)bIndices.Item2);
                button.UseVisualStyleBackColor = true;
                if(board.Win())
                {
                    MessageBox.Show("You Win", "WINNER", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    timer2.Stop();
                }
            }
            else
            {
                MessageBox.Show("Invalid move!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            drawImage = Tuple.Create(false, (byte)0);

        }

        //This timer runs whenever the user is dragging a selection to the button, and ends when the user releases the mouse button
        private void timer1_Tick(object sender, EventArgs e)
        {
            //screenGraphics.DrawImage(images[drawImage.Item2], Cursor.Position);
            drawWindow.Show(Cursor.Position);
            Button button = getMouseButton();
            if (button != null)
            {
                //Case:  we are entering a new button and it's different from the old one
                if (currB != button)
                {
                    var b1 = buttonIndices[button];
                    if (board.isValidMove(board.Game_Board, drawImage.Item2, (byte)b1.Item1, (byte)b1.Item2, false))
                        button.BackColor = Color.Green;
                    else
                        button.BackColor = Color.Red;

                    button.Invalidate();
                    if (currB != null)
                        currB.UseVisualStyleBackColor = true;
                    
                    currB = button;
                }
            }
            else if (currB != null)
            {
                currB.UseVisualStyleBackColor = true;
                currB = null;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timerTicks = timerTicks + 1;
            int sec = timerTicks % 60;
            int min = timerTicks / 60;
            this.Text = "Sudoku Time " + min.ToString() + ":" + sec.ToString();
            if(timerTicks >= 10)
            {
                timer2.Stop();
                MessageBox.Show("Time's Up", "LOSER", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button81_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            Tuple<byte, byte> t = (Tuple<byte, byte>)buttonIndices[b];
            board.clearButton(t.Item1, t.Item2);
            b.Enabled = true;
            b.Invalidate();
            setButtonImage(b, 0, t.Item1, t.Item2);
        }
    }
}
