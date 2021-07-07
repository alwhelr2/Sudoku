using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Board
    {
        public byte[,] game_board;

        private Board(byte [,] new_b)
        {
            game_board = new_b;
        }

        byte[,] GetBoard()
        {
            return game_board;
        }

        /*static Board getNewBoardInst()
        {
            b = generate_board()
            return Board(b);
        }
        */
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
                    brd[x, y] = num;
                }
            }

            return brd;
        }
        private bool isValid(byte[,] brd, byte num, int x, int y)
        {
            bool valid = true;
            for(int i = 0; i < 10; i++)
            {
                if(brd[i,y] == num)
                {
                    valid = false;
                }
                if (brd[x, i] == num)
                {
                    valid = false;
                }
            }
            return valid;
        }
        bool Win()
        {
            return false;
        }
    }
}