using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_for_damn
{
    class Board
    {

        public int white_checkers;
        public int white_queens;
        public int black_checkers;
        public int black_queens;


        Cell buf_pic;
        Cell otkuda_jum;
        //
        // move_queve - массив для значений подсвеченных ячеек
        //
        List<Cell> move_queve = new List<Cell>();
        //
        //
        //
        // delItems - что-то типо мапа где одному значению ячейки из move_queve 
        // соответствует массив Cell-ов которые удалятся при ходе в эту клетку
        //
        //
        List<Deleted_Items> delItems = new List<Deleted_Items>();

        int otkuda_x;
        int otkuda_y;
        int canGetMove = 0;
        int flagOnStep = 1;//
        //
        public Cell[,] board;

        #region <Constructor>

        public void Clear_Board()
        {
            board = new Cell[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Cell buf = new Cell();
                    buf.y = i;
                    buf.x = j;
                    buf.Condition = 0;
                    buf.chessCondition = 0;
                    board[i, j] = buf;
                }
            }
        }

        public Board()
        {
            Clear_Board();
        }

        public Board(string s)
        {
            Clear_Board();
            draw(s);
        }

        #endregion

        public String getCurrentBoard()
        {
            string ans = "";

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j].chessCondition == 0 )
                    {
                        ans = ans + "0";
                    }
                    else if (board[i, j].chessCondition == 1)
                    {
                        ans = ans + "1";
                    }
                    else if (board[i, j].chessCondition == 3)
                    {
                        ans = ans + "3";
                    }
                    else if (board[i, j].chessCondition == 2)
                    {
                        ans = ans + "2";
                    }
                    else if (board[i, j].chessCondition == 4)
                    {
                        ans = ans + "4";
                    }
                }
            }

            return ans + "\r\n\r\n";
        }

        public void draw(string boardCondition)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int k = i * 8 + j;

                    if (boardCondition[k] == '0')
                    {
                        board[i, j].Condition = 0;
                        board[i, j].chessCondition = 0;
                    }
                    else if(boardCondition[k] == '1')
                    {
                        board[i, j].Condition = 1;
                        board[i, j].chessCondition = 1;
                    }
                    else if(boardCondition[k] == '2')
                    {
                        board[i, j].Condition = 2;
                        board[i, j].chessCondition = 2;
                    }
                    else if(boardCondition[k] == '3')
                    {
                        board[i, j].Condition = 1;
                        board[i, j].chessCondition = 3;
                    }
                    else if (boardCondition[k] == '4')
                    {
                        board[i, j].Condition = 2;
                        board[i, j].chessCondition = 4;
                    }

                }
            }
            updateStatistic();
        }
        
        private bool check(Cell trash)
        {
            //
            //проверяем, можно ли походить в клетку
            //т.е. лежит ли она в move_queve
            int len = move_queve.Count;
            for (int i = 0; i < len; i++)
            {
                if (trash.x == move_queve[i].x && trash.y == move_queve[i].y)
                    return true;
            }
            return false;
        }

        #region <Clears>

        private void ClearLongItems(Cell trs)
        {
            //
            //очистка delItems ячеек, соответствующих ячейке в которую мы походили
            //
            int len = delItems.Count;

            for(int i = 0; i < len; i++)
            {
                if(delItems[i].greenItem.x == trs.x && delItems[i].greenItem.y == trs.y)
                {
                    Clear(delItems[i].deletedItems);
                }
            }
        }

        private void ClearDelItems()
        {
            // очистка для delItems
            for (int i = 0; i < delItems.Count; i++)
            {
                delItems.Remove(delItems[0]);
            }
        }

        private void Clear()
        {
            //
            //просто чистим массив move_queve
            //
            int len = move_queve.Count;

            for (int i = 0; i < len; i++)
            {

                int y = move_queve[0].y;
                int x = move_queve[0].x;

                board[y, x].chessCondition = 0;
                board[y, x].Condition = 0;

                move_queve.Remove(move_queve[0]);
            }
        }

        private void Clear(List<Cell> trs)
        {
            //
            //перегрузка чтобы "чистить" все возможные List-ы
            //
            int len = trs.Count;

            for (int i = 0; i < len; i++)
            {

                int y = trs[0].y;
                int x = trs[0].x;

                board[y, x].chessCondition = 0;
                board[y, x].Condition = 0;
                //
                trs.Remove(trs[0]);
            }
        }
        #endregion


        #region <Moves>
        public void canMove(Cell pic, int firstReqv, List<Cell> deletedCells, int currentCond)
        {
            int x;
            int y;

            Deleted_Items newDeletedItem;

            
            x = pic.x;
            y = pic.y;

            // возможные ходы для Черной в 1 ход
            if (y > 0 && x > 0 && board[y - 1, x - 1].Condition == 0 && currentCond == 2 && firstReqv == 1)
            {
                move_queve.Add(board[y - 1, x - 1]);
            }
            if (y > 0 && x < 7 && board[y - 1, x + 1].Condition == 0 && currentCond == 2 && firstReqv == 1)
            {
                move_queve.Add(board[y - 1, x + 1]);
            }

            // возможные ходы для Белой в 1 ход
            if (y < 7 && x > 0 && board[y + 1, x - 1].Condition == 0 && currentCond == 1 && firstReqv == 1)
            {
                move_queve.Add(board[y + 1, x - 1]);
            }
            if (y < 7 && x < 7 && board[y + 1, x + 1].Condition == 0 && currentCond == 1 && firstReqv == 1)
            {
                move_queve.Add(board[y + 1, x + 1]);
            }

            //
            //
            //Алгоритм кушания (рекурсивный)
            if (y > 1 && x > 1 && board[y - 1, x - 1].Condition != 0 && board[y - 1, x - 1].Condition != currentCond && board[y - 2, x - 2].Condition == 0 && check( board[y - 2, x - 2]) == false)
            {
                List<Cell> newList = new List<Cell>();
                for (int i = 0; i < deletedCells.Count; i++) { newList.Add(deletedCells[i]); }

                newList.Add(board[y - 1, x - 1]);

                newDeletedItem = new Deleted_Items(board[y - 2, x - 2], newList);
                delItems.Add(newDeletedItem);

                move_queve.Add(board[y - 2, x - 2]);
                canMove(board[y - 2, x - 2], 0, newList, currentCond);
            }
            if (y > 1 && x < 6 && board[y - 1, x + 1].Condition != 0 && board[y - 1, x + 1].Condition != currentCond && board[y - 2, x + 2].Condition == 0 && check( board[y - 2, x + 2]) == false)
            {
                List<Cell> newList = new List<Cell>();
                for (int i = 0; i < deletedCells.Count; i++) { newList.Add(deletedCells[i]); }

                newList.Add(board[y - 1, x + 1]);

                newDeletedItem = new Deleted_Items(board[y - 2, x + 2], newList);
                delItems.Add(newDeletedItem);

                move_queve.Add(board[y - 2, x + 2]);

                canMove(board[y - 2, x + 2], 0, newList, currentCond);
            }
            if (y < 6 && x > 1 && board[y + 1, x - 1].Condition != 0 && board[y + 1, x - 1].Condition != currentCond && board[y + 2, x - 2].Condition == 0 && check( board[y + 2, x - 2]) == false)
            {
                List<Cell> newList = new List<Cell>();
                for (int i = 0; i < deletedCells.Count; i++) { newList.Add(deletedCells[i]); }

                newList.Add(board[y + 1, x - 1]);

                newDeletedItem = new Deleted_Items(board[y + 2, x - 2], newList);
                delItems.Add(newDeletedItem);

                move_queve.Add(board[y + 2, x - 2]);

                canMove(board[y + 2, x - 2], 0, newList, currentCond);
            }
            if (y < 6 && x < 6 && board[y + 1, x + 1].Condition != 0 && board[y + 1, x + 1].Condition != currentCond && board[y + 2, x + 2].Condition == 0 && check( board[y + 2, x + 2]) == false)
            {
                List<Cell> newList = new List<Cell>();
                for (int i = 0; i < deletedCells.Count; i++) { newList.Add(deletedCells[i]); }

                newList.Add(board[y + 1, x + 1]);

                newDeletedItem = new Deleted_Items(board[y + 2, x + 2], newList);
                delItems.Add(newDeletedItem);

                move_queve.Add(board[y + 2, x + 2]);

                canMove(board[y + 2, x + 2], 0, newList, currentCond);
            }
        }

        private void canMoveQueen(Cell pic, int firstReqv, List<Cell> deletedCells, int currentCond)
        {
            int x;
            int y;

            x = pic.x;
            y = pic.y;

            int i = y;                                      //
            int j = x;                                      //обнуление переменных и List-а
            List<Cell> newList = new List<Cell>();          //

            while (true)// вправо вверх
            {
                Deleted_Items buf_del;
                for (int m = 0; m < deletedCells.Count; m++) { newList.Add(deletedCells[m]); }

                if (i <= -1 || j >= 8 || i >= 8 || j <= -1) break;
                if (i > 0 && j < 7 && board[i - 1, j + 1].Condition == currentCond) break;
                if (i > 1 && j < 6 && (board[i - 1, j + 1].Condition != 0 && board[i - 1, j + 1].Condition != currentCond) && (board[i - 2, j + 2].Condition != 0 && board[i - 2, j + 2].Condition != currentCond)) break;

                if (i > 0 && j < 7 && board[i - 1, j + 1].Condition == 0 && check(board[i - 1, j + 1]) == false)
                {
                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    buf_del = new Deleted_Items(board[i - 1, j + 1], bufL);
                    delItems.Add(buf_del);

                    move_queve.Add(board[i - 1, j + 1]);
                }
                if (i > 1 && j < 6 && board[i - 1, j + 1].Condition != 0 && board[i - 1, j + 1].Condition != currentCond && board[i - 2, j + 2].Condition == 0)
                {
                    newList.Add(board[i - 1, j + 1]);

                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }


                    buf_del = new Deleted_Items(board[i - 2, j + 2], bufL);
                    delItems.Add(buf_del);
                    canMove(board[i - 2, j + 2], 0, bufL, currentCond);

                    move_queve.Add(board[i - 2, j + 2]);
                }
                i--;
                j++;
            }

            newList.Clear();                        //
            i = y;                                  //обнуление переменных и List-а
            j = x;                                  //
            while (true)// влево вниз
            {
                Deleted_Items buf_del;
                for (int m = 0; m < deletedCells.Count; m++) { newList.Add(deletedCells[m]); }

                if (i <= -1 || j >= 8 || i >= 8 || j <= -1) break;
                if (i < 7 && j > 0 && board[i + 1, j - 1].Condition == currentCond) break;
                if (i < 6 && j > 1 && (board[i + 1, j - 1].Condition != 0 && board[i + 1, j - 1].Condition != currentCond) && (board[i + 2, j - 2].Condition != 0 && board[i + 2, j - 2].Condition != currentCond)) break;

                if (i < 7 && j > 0 && board[i + 1, j - 1].Condition == 0 && check(board[i + 1, j - 1]) == false)
                {
                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    buf_del = new Deleted_Items(board[i + 1, j - 1], bufL);
                    delItems.Add(buf_del);

                    move_queve.Add(board[i + 1, j - 1]);
                }
                if (i < 6 && j > 1 && board[i + 1, j - 1].Condition != 0 && board[i + 1, j - 1].Condition != currentCond && board[i + 2, j - 2].Condition == 0)
                {
                    newList.Add(board[i + 1, j - 1]);

                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    buf_del = new Deleted_Items(board[i + 2, j - 2], bufL);
                    delItems.Add(buf_del);
                    canMove(board[i + 2, j - 2], 0, bufL, currentCond);
                    move_queve.Add(board[i + 2, j - 2]);
                }
                i++;
                j--;
            }



            newList.Clear();
            i = y;
            j = x;
            while (true)// вниз вправо
            {
                Deleted_Items buf_del;
                for (int m = 0; m < deletedCells.Count; m++) { newList.Add(deletedCells[m]); }

                if (i <= -1 || j >= 8 || i >= 8 || j <= -1) break;
                if (i < 7 && j < 7 && board[i + 1, j + 1].Condition == currentCond) break;
                if (i < 6 && j < 6 && (board[i + 1, j + 1].Condition != 0 && board[i + 1, j + 1].Condition != currentCond) && (board[i + 2, j + 2].Condition != 0 && board[i + 2, j + 2].Condition != currentCond)) break;


                if (i < 7 && j < 7 && board[i + 1, j + 1].Condition == 0 && check(board[i + 1, j + 1]) == false)
                {
                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    buf_del = new Deleted_Items(board[i + 1, j + 1], bufL);
                    delItems.Add(buf_del);

                    move_queve.Add(board[i + 1, j + 1]);
                }

                if (i < 6 && j < 6 && board[i + 1, j + 1].Condition != 0 && board[i + 1, j + 1].Condition != currentCond && board[i + 2, j + 2].Condition == 0)
                {
                    newList.Add(board[i + 1, j + 1]);

                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    buf_del = new Deleted_Items(board[i + 2, j + 2], bufL);
                    delItems.Add(buf_del);
                    canMove(board[i + 2, j + 2], 0, bufL, currentCond);
                    move_queve.Add(board[i + 2, j + 2]);
                }
                i++;
                j++;
            }


            newList.Clear();
            i = y;
            j = x;
            while (true)// вверх влево
            {
                Deleted_Items buf_del;
                for (int m = 0; m < deletedCells.Count; m++) { newList.Add(deletedCells[m]); }

                if (i <= -1 || j >= 8 || i >= 8 || j <= -1) break;
                if (i > 0 && j > 0 && board[i - 1, j - 1].Condition == currentCond) break;
                if (i > 1 && j > 1 && (board[i - 1, j - 1].Condition != 0 && board[i - 1, j - 1].Condition != currentCond) && (board[i - 2, j - 2].Condition != 0 && board[i - 2, j - 2].Condition != currentCond)) break;

                if (i > 0 && j > 0 && board[i - 1, j - 1].Condition == 0 && check(board[i - 1, j - 1]) == false)
                {
                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    buf_del = new Deleted_Items(board[i - 1, j - 1], bufL);
                    delItems.Add(buf_del);

                    move_queve.Add(board[i - 1, j - 1]);
                }
                if (i > 1 && j > 1 && board[i - 1, j - 1].Condition != 0 && board[i - 1, j - 1].Condition != currentCond && board[i - 2, j - 2].Condition == 0)
                {
                    newList.Add(board[i - 1, j - 1]);

                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    buf_del = new Deleted_Items(board[i - 2, j - 2], bufL);
                    delItems.Add(buf_del);
                    canMove(board[i - 2, j - 2], 0, bufL, currentCond);
                    move_queve.Add(board[i - 2, j - 2]);
                }
                i--;
                j--;
            }

        }

        private void MoveIt(Cell trs)
        {
            // "двигаем" фишку, свапая значения ячеек
            int x = 0;
            int y = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j].x == trs.x && board[i, j].y == trs.y )
                    {
                        x = j;
                        y = i;
                    }
                }
            }

            ClearLongItems(board[y, x]);     //очищаем ячейки, которые удаляются при переходе в ячейку pole[y,x]
            ClearDelItems();                //

            if (board[otkuda_y, otkuda_x].Condition == 1)        //if-ы для королевы
            {
                if (y == 7)
                {
                    board[y, x].chessCondition = 3;
                }
                else
                {
                    board[y, x].chessCondition = board[otkuda_y, otkuda_x].chessCondition;
                }
            }
            else if (board[otkuda_y, otkuda_x].Condition == 2)
            {
                if (y == 0)
                {
                    board[y, x].chessCondition = 4;
                }
                else
                {
                    board[y, x].chessCondition= board[otkuda_y, otkuda_x].chessCondition;
                }
            }

            // непосредственно свап ячеек
            board[otkuda_y, otkuda_x].chessCondition = 0;              //
            move_queve.Remove(trs);                                                     //
                                                                                        //                          //
                                                                                        //
            board[y, x].Condition = board[otkuda_y, otkuda_x].Condition;                  //
            board[otkuda_y, otkuda_x].Condition = 0;                                     //
            board[otkuda_y, otkuda_x].chessCondition = 0;


        }

        #endregion

        public List<Board> getPossibleBoards(int curCondition)
        {
            List<Board> possibleBoards = new List<Board>();
            string current = getCurrentBoard();

            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    draw(current); // откатываемся
                    ClearDelItems();
                    Clear();
                    if (board[i,j].Condition == curCondition)
                    {
                        if (board[i,j].chessCondition == 1 || board[i, j].chessCondition == 2 ) // если простая шашка
                        {
                            
                            otkuda_jum = board[i, j];
                            otkuda_x = board[i, j].x;
                            otkuda_y = board[i, j].y;

                            canMove(board[i, j], 1, new List<Cell>(), curCondition);

                            List<Cell> bufListOfMoves = new List<Cell>(); // буферные вычисления
                            for (int z = 0; z < move_queve.Count; z++) { bufListOfMoves.Add(move_queve[z]); }

                            for(int z = 0; z < bufListOfMoves.Count; z++) // проходимся по возможным ходам и делаем каждый из них
                            {
                                ClearDelItems();
                                Clear();
                                draw(current);
                                canMove(board[i, j], 1, new List<Cell>(), curCondition);
                                MoveIt(bufListOfMoves[z]);
                                string s = getCurrentBoard();
                                if (current == s) { 
                                    int jz = 10 - 2;
                                }
                                possibleBoards.Add( new Board( getCurrentBoard() ) );
                            }
                        }
                    }
                }
            }


            return possibleBoards;
        }

        public void updateStatistic()
        {
            white_queens ^= white_queens;           // это как white_queens = 0 только с выпендрежем
            white_checkers ^= white_checkers;       // потому что могу :3
            black_checkers ^= black_checkers;       //
            black_queens ^= black_queens;           //

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j].chessCondition == 1)
                        white_checkers++;
                    else if (board[i, j].chessCondition == 2)
                        black_checkers++;
                    else if (board[i, j].chessCondition == 3)
                        white_queens++;
                    else if (board[i, j].chessCondition == 4)
                        black_queens++;
                }
            }
        }

    }
}
