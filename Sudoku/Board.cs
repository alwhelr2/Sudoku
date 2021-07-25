using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;

namespace Sudoku
{
    class Board
    {
        public byte[,] Game_Board
        {
            get;
            private set;
        }

        private static byte[,] emptyBoard = { { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
        private static Dictionary<byte, ArrayList> ninthButtons;

        private Random rand
        {
            get;
            set;
        }

        static Board()
        {
            ninthButtons = new Dictionary<byte, ArrayList>();
            for (byte b = 0; b < 9; b++)
                ninthButtons[b] = new ArrayList();
            for (byte i = 0; i < 9; i++)
            {
                for (byte j = 0; j < 9; j++)
                {
                    //byte ninth = (byte)(GetSection(i) * 3 + GetSection(j));
                    ninthButtons[GetNinth(i, j)].Add(Tuple.Create(i, j));
                }
            }
        }

        private Board(bool empty = true)
        {
            rand = new Random();
            if (empty)
                Game_Board = emptyBoard;
            else
                Game_Board = GenerateBoard();
            
        }

        public void clearButton(byte x, byte y)
        {
            Game_Board[x, y] = 0;
        }

        //Note: Maybe the static generation is unnecessary, and just make the constructor public?
        public static Board getNewBoardInst(bool empty = true)
        {
            return new Board(empty);
        }
        
        //Generate randomized sudoku board (Maybe parse pre-existing boards available online?)
        /*private byte[,] GenerateBoard()
        {
            Random rnd = new Random();

            byte[,] brd = new byte[9, 9];

            for(int i = 0; i < 10; i++)
            {
                byte num = (byte)rnd.Next(1,10);
                int x = rnd.Next(1, 10);
                int y = rnd.Next(1, 10);

                if (isValid(brd, num, x, y))
                {
                    brd[x, y] = (byte)(-num);
                }
            }

            return brd;
        }*/
        private byte[,] GenerateBoard()
        {
            byte[,] board = new byte[9, 9];

            //Fill diagonal
            for (byte i = 0; i < 9; i+= 3)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        byte num = 0;
                        do
                        {
                            num = (byte)(rand.Next(9) + 1);
                        } while (!isUnusedInBox(board, GetNinth(i, i), num));
                        board[i + x, i + y] = num;
                    }
                }
            }

            //Fill remaining
            fillRemaining(board, 0, 3);

            //Randomly remove digits
            removeKDigits(board);
            
            return board;
        }

        private bool fillRemaining(byte[,] board, byte i, byte j)
        {
            if (j >= 9 && ++i < 8)
                j = 0;
            if (i >= 9 && j >= 9)
                return true;

            if (i < 3)
            {
                if (j < 3)
                    j = 3;
            }
            else if (i < 6)
            {
                if (j == (int)(i / 3) * 3)
                {
                    j += 3;
                }    
            }
            else
            {
                if (j == 6)
                {
                    i++;
                    j = 0;
                    if (i >= 9)
                        return true;
                }
            }
            for (byte num = 1; num <= 9; num++)
            {
                if (isValidMove(board, num, i, j))
                {
                    if (fillRemaining(board, i, (byte)(j + 1)))
                        return true;

                    board[i, j] = 0;
                }
            }
            return false;
        }

        private void removeKDigits(byte[,] board)
        {
            int count = rand.Next(45, 55);
            while (count != 0)
            {
                //int cellId = (int)Math.Floor((double)(rand.Next(81) + 1)) - 1;
                int cellId = (int)(Math.Floor(rand.NextDouble() * 81 + 1)) - 1;
                int i = (cellId / 9);
                int j = cellId % 9;

                if (board[i, j] != 0)
                {
                    count--;
                    board[i, j] = 0;
                }
            }   
        }

        private static byte GetNinth(byte row, byte col)
        {
            return (byte)(GetSection(row) * 3 + GetSection(col));
        }


        //This method takes a row or col value and gives you the corresponding third of the board
        //0-2 will return 0, 1-3 will return 1 and 4-6 will return 2
        private static int GetSection(int pos)
        {
            return (int)Math.Floor((float)(pos/3));
        }

        //This method checks to see if a move the player makes is valid
        //Input: byte num = potential move, x = row, y = col in Game_Board (this checks quadrants and rows + cols)
        //Dictionary may be irrelevant if you can create a function that will return the corresponding byte values in the Game_Board array for a specified ninth (0-9, with 0-3 being top 3 buttons and so on)
        public bool isValidMove(byte[,] board, byte num, byte x, byte y, bool addButton = true)
        {
            byte q = GetNinth(x, y);
            if (!isUnusedInBox(board, q, num) || !isUnusedInRow(board, x, num) || !isUnusedInCol(board, y, num))
                return false;
            if (addButton)
                board[x, y] = num;

            return true;
        }

        private bool isUnusedInBox(byte[,] board, byte box, byte num)
        {
            ArrayList tuples = ninthButtons[box];
            for (int i = 0; i < tuples.Count; i++)
            {
                Tuple<byte, byte> t = (Tuple<byte, byte>)tuples[i];
                if (board[t.Item1, t.Item2] == num)
                    return false;
            }
            return true;
        }

        private bool isUnusedInRow(byte[,] board, byte i, byte num)
        {
            for (byte j = 0; j < 9; j++)
                if (board[i, j] == num)
                    return false;
            return true;
        }

        private bool isUnusedInCol(byte[,] board, byte j, byte num)
        {
            for (byte i = 0; i < 9; i++)
                if (board[i, j] == num)
                    return false;
            return true;
        }


        //This method is used to check if a number is valid for board creation
        //Why are you passing the board to the method if the method has access to the game_board instance variable?'
        //Rework this method to include the isValidMove method I wrote
        /*
        private bool isValid(byte[,] brd, byte num, int x, int y)
        {
            bool valid = true;
            int x_sector = GetSection(x);
            int y_sector = GetSection(y);

            if(brd[x,y] < 0)
            {
                return false;
            }

            for(int i = x_sector; i < x_sector+3; i++) // Check section
            {
                for(int j = y_sector; j < y_sector+3; j++)
                {
                    if(Math.Abs(brd[i,j]) == num)
                    {
                        return false;
                    }
                }
            }
            for(int i = 0; i < 10; i++) // Check Column+Row
            {
                if(Math.Abs(brd[i,y]) == num)
                {
                    return false;
                }
                if(Math.Abs(brd[x, i]) == num)
                {
                    return false;
                }
            }

            //Check that other section have a spot open for num



            return true;
        }
        void PlaceNum(byte num, int x, int y)
        {
            if(isValid(Game_Board, num, x, y))
            {
                Game_Board[x,y] = num;
            }
        }*/

        public bool Win()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if(Game_Board[i,j] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}