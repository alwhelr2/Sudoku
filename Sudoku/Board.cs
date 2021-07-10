using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sudoku
{
    class Board
    {
        public byte[,] Game_Board
        {
            get;
        }

        private Board()
        {
            //Generate empty board for now
            Game_Board = new byte[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
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

        public static Board getNewBoardInst()
        {
            return new Board();
        }
        
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

        private int GetSection(int pos)
        {
            return (int)Math.Floor((float)(pos/3))*3;
        }

        //TODO: isvalidmove
        public bool isValidMove(byte num, byte x, byte y)
        {
            return true;
        }

        //Why are you passing the board to the method if the method has access to the game_board instance variable?
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