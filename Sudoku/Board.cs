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


        /*Maybe unnecessary?
        //? after the data type means we can use store null to the variable (byte is otherwise a non-nullabe type)
        public byte? this[int row, int col]
        {
            get
            {
                if (row < 9 && col < 9)
                {
                    return Game_Board[row, col];
                }
                else
                {
                    //TODO: Indices not value, throw exception?
                    return null;
                }
            }
            set
            {
                if (row < 9 && col < 9 && value >= 1 && value <= 9)
                {
                    if (value != null)
                        Game_Board[row, col] = (byte)value;

                }
                else
                {
                    //TODO: Throw exception?
                }
            }
        }*/

        public byte[,] Game_Board
        {
            get;
            private set;
        }

        public void setBoardValue(byte num, byte x, byte y)
        {
            if (x >= 0 && y < 9 && y >= 0 && x < 9 && num >= 0 && num <= 9)
                Game_Board[x, y] = num;
            else
            {
                //Throw exception?
            }
        }

        private Board()
        {
            //Generate empty board for now
            Game_Board = new byte[,] { { 1, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 2, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 3, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 4, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 5, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 6, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 7, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 8, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 9 } }; 
        }

        /*
        public byte[,] GetBoard()
        {
            return game_board;
        }*/

        /*
        static Board getNewBoardInst()
        {
            b = generate_board();
            return Board(b);
        }*/

        //Note: Maybe the static generation is unnecessary, and just make the constructor public?
        public static Board getNewBoardInst()
        {
            return new Board();
        }
        
        //Generate randomized sudoku board (Maybe parse pre-existing boards available online?)
        private byte[,] GenerateBoard()
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
        }


        //This method takes a row or col value and gives you the corresponding third of the board
        //0-2 will return 0, 1-3 will return 1 and 4-6 will return 2
        private int GetSection(int pos)
        {
            return (int)Math.Floor((float)(pos/3));
        }

        //This method checks to see if a move the player makes is valid
        //Input: byte num = potential move, x = row, y = col in Game_Board (this checks quadrants and rows + cols)
        //Dictionary may be irrelevant if you can create a function that will return the corresponding byte values in the Game_Board array for a specified ninth (0-9, with 0-3 being top 3 buttons and so on)
        public bool isValidMove(byte num, byte x, byte y, Dictionary<byte, ArrayList> getNinthButtons, bool addButton = true)
        {
            //Check quadrants
            ArrayList buttonsInQuadrant = getNinthButtons[(byte)((GetSection(x) * 3) + GetSection(y))];
            
            for (int i = 0; i < buttonsInQuadrant.Count; i++)
            {
                Button button = (Button)buttonsInQuadrant[i];

                //Check for matching nums in the same ninth
                if ((byte)button.Tag == num)
                    return false;
            }
            //Check rows+cols
            for (int i = 0; i < Game_Board.GetLength(0); i++)
                if (Game_Board[i, y] == num)
                    return false;
            for (int i = 0; i < Game_Board.GetLength(1); i++)
                if (Game_Board[x, i] == num)
                    return false;

            return true;
        }


        //This method is used to check if a number is valid for board creation
        //Why are you passing the board to the method if the method has access to the game_board instance variable?'
        //Rework this method to include the isValidMove method I wrote
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
        }

        bool Win()
        {
            return false;
        }
    }
}