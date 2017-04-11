using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace chess_for_damn
{

    public partial class Form1 : Form
    {
        Cell[,] pole = new Cell[8, 8];
        Cell buf_pic;
        Cell otkuda_jum;

        int flag_for_timer = 1;

        Cell[,] buf = new Cell[8, 8]; // AI cup



        // for Network
        //
        TcpClient Client;
        int port = 8005;
        //</for Network>


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

        int AI_CON = 2;
        int NOT_AI = 1;

        int otkuda_x;
        int otkuda_y;
        int canGetMove = 0;
        int flagOnStep = 1;//
        //
        //Condition:
        //1 - white
        //2 - black


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Ход Белых";

            System.Windows.Forms.PictureBox[,] mass = new System.Windows.Forms.PictureBox[8, 8]; // инициализируем массив пикчербокса

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    
                    int f = 0;                                              //
                    string nam = "";                                        //
                    nam = "PBox" + i + j;                                   // создаем новый пикчербокс
                    mass[i, j] = new System.Windows.Forms.PictureBox();     // и задаем ему нужные Св-Ва
                    
                    if ((i + 1) % 2 == 1)
                    {
                        if ((j + 1) % 2 == 1)
                        {
                            mass[i, j].Image = white.Image;
                        }
                        else
                        {
                            if (i < 3)
                            {
                                f = 1;
                                mass[i, j].Image = white_chess.Image;
                            }
                            else if (i > 4)
                            {
                                f = 2;
                                mass[i, j].Image = black_chess.Image;
                            }
                            else
                            {
                                if (i < 3)
                                {
                                    f = 1;
                                    mass[i, j].Image = white_chess.Image;
                                }
                                else if (i > 4)
                                {
                                    f = 2;
                                    mass[i, j].Image = black_chess.Image;
                                }
                                else
                                    mass[i, j].Image = black.Image;
                            }
                        }
                    }
                    else
                    {
                        if ((j + 2) % 2 == 0)
                        {
                            if (i < 3)
                            {
                                f = 1;
                                mass[i, j].Image = white_chess.Image;
                            }
                            else if (i > 4)
                            {
                                f = 2;
                                mass[i, j].Image = black_chess.Image;
                            }
                            else
                                mass[i, j].Image = black.Image;
                        }
                        else
                        {

                            mass[i, j].Image = white.Image;
                        }
                    }
                    mass[i, j].Name = nam;
                    mass[i, j].Location = new System.Drawing.Point(j * 70, i * 70);
                    mass[i, j].Size = new System.Drawing.Size(70, 70);
                    mass[i, j].TabIndex = 0;
                    mass[i, j].TabStop = false;
                    mass[i, j].Click += new System.EventHandler(this.Any_Click);
                    this.Controls.Add(mass[i, j]);
                    
                    pole[i, j] = new Cell(mass[i, j], f);       // f - Condition
                                                                // Condition:
                                                                // 1 - White
                                                                // 2 - Black          
                }
            }




            Board b = new Board(SaveToString());
            List<Board> lst1 = new List<Board>();
            lst1 = b.getPossibleBoards(1);
            //draw("0101010110101010000101010000002001000000202020000202020220202020\rr");

            //if (AI_CON == 1)
            //{
            //    makeChoose();
            //}
        }



        /*
        // Logic Block
        //
        //
        //
        */

        

        private void makeChoose(int  depth)
        {
            string currentBoard = SaveToString();
            List<logicStruct> rating = new List<logicStruct>();



            Board first_step = new Board(currentBoard);
            List<Board> pos_mov = first_step.getPossibleBoards(AI_CON);

            for (int i =0; i < pos_mov.Count; i++)
            {
                rating.Add(new logicStruct(pos_mov[i].getCurrentBoard(), 0));
            }
            int max = -10000;
            string max_br = "";
            for(int i = 0; i < rating.Count; i++)
            {//"0101010110101010000101012000000000000002200020000202020220202020\r  "0101010110101010000101010020000000000002002020000202020220202020\r
                rating[i].fx = max_f(rating[i].brd,NOT_AI,4) ;/* rateFunc(NOT_AI, rating[i].brd, 5);*/
                max = max > rating[i].fx ? max : rating[i].fx;
                max_br = max > rating[i].fx ? max_br : rating[i].brd ;
            }
            draw(max_br);
            flagOnStep = flagOnStep % 2 + 1;
            int k = 10;
        }

        public int heuristic(string brd, int condition)
        {
            Board tmp = new Board(brd);
            if(condition == 1)
            {
                return (tmp.white_queens * 3 + tmp.white_checkers) - (tmp.black_queens * 3 + tmp.black_checkers);
            }
            else if (condition == 2)
            {
                return (tmp.black_queens * 3 + tmp.black_checkers) - (tmp.white_queens * 3 + tmp.white_checkers); 
            }
            else
            {
                return 0;
            }
        }

        public int isTerminal(string brd, int con)
        {
            Board tmp = new Board(brd);
            if (tmp.getPossibleBoards(con).Count == 0)
                return 0;
            else if (tmp.white_checkers + tmp.white_queens == 0)
                return 100;
            else if (tmp.black_checkers + tmp.black_queens == 0)
                return -100;
            return 1;
        }

        public int min_f(string brd, int condition, int depth)
        {
            int score = 100000;
            if(depth <= 0 )
            {
                return heuristic(brd, AI_CON);
            }
            if(isTerminal(brd,condition) == 0)
            {
                return 0;
            }else if(isTerminal(brd,condition) == 100)
            {
                if(AI_CON == 2)
                {
                    return 1000;
                }
                else
                {
                    return -1000;
                }
            } else if(isTerminal(brd,condition) == -100)
            {
                if(AI_CON == 1)
                {
                    return 1000;
                }else
                {
                    return -1000;
                }
            }

            Board tmp = new Board(brd);
            List<Board> pos_movings = new List<Board>();
            pos_movings = tmp.getPossibleBoards(condition);
            for(int i =0; i < pos_movings.Count;i++)
            {
                int buf = max_f(pos_movings[i].getCurrentBoard(), condition % 2 + 1, depth - 1);
                if(score > buf)
                {
                    score = buf;
                }
            }


            return score;
        }

        public int max_f(string brd, int condition, int depth)
        {
            int score = -100000;
            if (depth <= 0)
            {
                return heuristic(brd, AI_CON);
            }
            if (isTerminal(brd, condition) == 0)
            {
                return 0;
            }
            else if (isTerminal(brd, condition) == 100)
            {
                if (AI_CON == 2)
                {
                    return 1000;
                }
                else
                {
                    return -1000;
                }
            }
            else if (isTerminal(brd, condition) == -100)
            {
                if (AI_CON == 1)
                {
                    return 1000;
                }
                else
                {
                    return -1000;
                }
            }



            Board tmp = new Board(brd);
            List<Board> pos_movings = new List<Board>();
            pos_movings = tmp.getPossibleBoards(condition);
            for (int i = 0; i < pos_movings.Count; i++)
            {
                int buf = min_f(pos_movings[i].getCurrentBoard(), condition % 2 + 1, depth - 1);
                if (score < buf)
                {
                    score = buf;
                }
            }





            return score;
        }







        public int rateFunc( int condition , string toRateBoard, int depth )
        {
            
            if (--depth < 0)
            {
                return 0;
            }
            
            int rat = 0;
            Board tmp = new Board(toRateBoard);
            List<Board> brdList = new List<Board>();
            brdList = tmp.getPossibleBoards(condition);

            if (condition == AI_CON)
            {
                
                for (int i =0; i < brdList.Count; i++)
                {
                    //rat = 0;
                    if (AI_CON == 1) // если белый - AI
                    {
                        int buf = 0;
                        buf -= (tmp.white_queens * 3 + tmp.white_checkers) - (brdList[i].white_queens * 3 + brdList[i].white_checkers);
                        buf += (tmp.black_queens * 3 + tmp.black_checkers) - (brdList[i].black_queens * 3 + brdList[i].black_checkers);
                        buf += rateFunc(NOT_AI, brdList[i].getCurrentBoard(), depth);
                        rat = rat >= buf ? rat : buf; 
                    }
                    else // если черный - меняем знаки;
                    {
                        int buf = 0;
                        buf += (tmp.white_queens * 3 + tmp.white_checkers) - (brdList[i].white_queens * 3 + brdList[i].white_checkers);
                        buf -= (tmp.black_queens * 3 + tmp.black_checkers) - (brdList[i].black_queens * 3 + brdList[i].black_checkers);
                        buf += rateFunc(NOT_AI, brdList[i].getCurrentBoard(), depth);
                        rat = rat >= buf ? rat : buf;
                    }
                    
                    
                }
                
            }

            else if (condition == NOT_AI)
            {

                for (int i = 0; i < brdList.Count; i++)
                {
                    if (NOT_AI == 1) // если белый -NOT_AI
                    {
                        int buf = 0;
                        buf -= (tmp.white_queens * 3 + tmp.white_checkers) - (brdList[i].white_queens * 3 + brdList[i].white_checkers);
                        buf += (tmp.black_queens * 3 + tmp.black_checkers) - (brdList[i].black_queens * 3 + brdList[i].black_checkers);
                        buf *= -1;
                        buf += rateFunc(AI_CON, brdList[i].getCurrentBoard(), depth) ;
                        
                        rat = rat <= buf ? rat : buf;
                    }
                    else // если черный - меняем знаки;
                    {
                        int buf = 0;
                        buf += (tmp.white_queens * 3 + tmp.white_checkers) - (brdList[i].white_queens * 3 + brdList[i].white_checkers);
                        buf -= (tmp.black_queens * 3 + tmp.black_checkers) - (brdList[i].black_queens * 3 + brdList[i].black_checkers);
                        buf *= -1;
                        buf += rateFunc(AI_CON, brdList[i].getCurrentBoard(), depth) ;
                        rat = rat <= buf ? rat : buf;
                    }
                    
                }

            }


            return rat;
        }
       


        private int checkToWinAI()
        {
            int black_win_ai = 0;
            int white_win_ai = 0;
            int Ans = 1;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (pole[i, j].Condition == AI_CON)
                        black_win_ai = 1;

                    if (pole[i, j].Condition == NOT_AI)
                        white_win_ai = 1;
                }
            }
            if (white_win_ai == 0 || black_win_ai == 0)
                return 1;
            else
                return 0;
        }

        /*
        // The End Of Logic Block
        //
        //4-1 5-6
        //
         * 
        */


        private void checkToWin()
        {
            
            int white_win = 1;
            int black_win = 1;
            for(int i =0; i < 8; i ++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (pole[i, j].Condition == 1)
                        black_win = 0;

                    if (pole[i, j].Condition == 2)
                        white_win = 0;
                }
            }

            if (white_win == 1)
                Win("White");
            if (black_win == 1)
                Win("Black");
        }

        private void Win(string who)
        {
            Clear();
            //MessageBox.Show(who+" IS WIN!!!");              // Вывод сообщения о победе
            label1.Text = "NICE JOB, YOU MAY EXIT";         // И еще мы меняем лейблик
        }



        private void Any_Click(object sender, EventArgs e)
        {
            int x;
            int y;

            x = (Cursor.Position.X - this.DesktopLocation.X);
            y = (Cursor.Position.Y - this.DesktopLocation.Y);

            x = (x / 73) == 8 ? 7 : (x / 73);
            y = (y / 73) == 8 ? 7 : (y / 73);
            
            buf_pic = pole[y, x];

            if (canGetMove == 0 && buf_pic.Condition % 3 != flagOnStep) return; // что то нужное


            if (canGetMove == 0 && ( buf_pic.mypic.Image == black_chess.Image || buf_pic.mypic.Image == white_chess.Image || buf_pic.mypic.Image == white_queen.Image || buf_pic.mypic.Image == black_queen.Image))     // при первом клике на пикчу
            {
                ClearDelItems();                                                                                    //очищаем массив delItems - 
                otkuda_y = y;                                                                                       // парсим координаты
                otkuda_x = x;                                                                                       
                otkuda_jum = pole[y, x];                                                                            
                List<Cell> null_Iter = new List<Cell>();                                                            // нулевой List потому что нужен указатель на пустой List

                if( flagOnStep ==AI_CON)
                {
                    makeChoose(10);
                }

                if (buf_pic.mypic.Image == white_queen.Image || buf_pic.mypic.Image == black_queen.Image )
                {
                                                                                                            // Если это королева
                    canMoveQueen(buf_pic, 1, null_Iter, buf_pic.Condition);                                 //
                }
                else
                {
                                                                                                            // Все остальные холопы
                    canMove(buf_pic, 1, null_Iter, buf_pic.Condition);                                      //
                }

                canGetMove = 1;                                                                             // флаг на то что мы нажали и подсветили

            }

            else if (canGetMove == 1 && check(buf_pic) == false)
            {
                canGetMove = 0;                                                                             // Если мы подсветили но нажали не туда
                Clear();                                                                                    // Тогда убираем флажок и "Чистим подсветку"
                
            }

            else if (canGetMove == 1 && check(buf_pic) == true)
            {
                Clear();                                                                            // Если нажали Туда, то тоже чистим подсветку за собой 
                canGetMove = 0;                                                                     //Go                                         
                MoveIt(buf_pic);                                                                    //двигаем

                flagOnStep = flagOnStep == 1 ? 2 : 1;                                               // меняем шаг на противоположный
                label1.Text = flagOnStep == 1 ? "Ход Белых" : "Ход Черных";

                checkToWin();                                                                       // Проверяем на победку
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
                    if (pole[i, j].mypic.Name == trs.mypic.Name)
                    {
                        x = j;
                        y = i;
                    }
                }
            }

            ClearLongItems(pole[y, x]);     //очищаем ячейки, которые удаляются при переходе в ячейку pole[y,x]
            ClearDelItems();                //

            if (pole[otkuda_y, otkuda_x].Condition == 1)        //if-ы для королевы
            {
                if (y == 7)
                {
                    pole[y, x].mypic.Image = white_queen.Image;
                }
                else
                {
                    pole[y, x].mypic.Image = pole[otkuda_y, otkuda_x].mypic.Image;
                }
            }
            else if (pole[otkuda_y, otkuda_x].Condition == 2)
            {
                if(y==0)
                {
                    pole[y, x].mypic.Image = black_queen.Image;
                }
                else
                {
                    pole[y, x].mypic.Image = pole[otkuda_y, otkuda_x].mypic.Image;
                }
            }

                                                                                        // непосредственно свап ячеек
            pole[otkuda_y, otkuda_x].mypic.Image = pole[y, x].mypic.Image;              //
            move_queve.Remove(trs);                                                     //
                                                                                        //
            pole[otkuda_y,otkuda_x].mypic.Image = black.Image;                          //
                                                                                        //
            pole[y, x].Condition = pole[otkuda_y, otkuda_x].Condition;                  //
            pole[otkuda_y, otkuda_x].Condition = 0;                                     //

            parseTo(otkuda_y, otkuda_x, y, x);                      // выводим ход, который мы сделали
            
        }

        private string getBUKVA(int n)
        {
            //switch для букв по номеру
            string Ans = "";
            switch (n)
            {
                case 0:
                    Ans += "a";
                    break;
                case 1:
                    Ans += "b";
                    break;
                case 2:
                    Ans += "c";
                    break;
                case 3:
                    Ans += "d";
                    break;
                case 4:
                    Ans += "e";
                    break;
                case 5:
                    Ans += "f";
                    break;
                case 6:
                    Ans += "g";
                    break;
                case 7:
                    Ans += "h";
                    break;
            }
            return Ans;

        }

        private void parseTo(int y0, int x0, int y1, int x1)
        {
            // парсинг ходов
            string additiveString = "";
            additiveString += getBUKVA(x0);
            additiveString += " " + (y0+1).ToString() + " --> ";
            additiveString += getBUKVA(x1);
            additiveString += " " + (y1 + 1).ToString() + "\n";

            richTextBox1.Text += additiveString;
                
        }

        private bool check(Cell trash)
        {
            //
            //проверяем, можно ли походить в клетку
            //т.е. лежит ли она в move_queve
            int len = move_queve.Count;
            bool flag = false;
            for (int i = 0; i < len; i++)
            {
                if (trash.mypic.Name == move_queve[i].mypic.Name)
                    flag = true;
            }
            return flag;
        }

        private void ClearLongItems(Cell trs)
        {
            //
            //очистка delItems ячеек, соответствующих ячейке в которую мы походили
            //
            int len = delItems.Count;

            for(int i = 0; i < len; i++)
            {
                if(delItems[i].greenItem.mypic.Name == trs.mypic.Name)
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

                pole[y, x].mypic.Image = black.Image;
                pole[y, x].Condition = 0;

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

                pole[y, x].mypic.Image = black.Image;
                pole[y, x].Condition = 0;
                //
                trs.Remove(trs[0]);
            }
        }

        private void canMove(Cell pic, int firstReqv, List<Cell> deletedCells, int currentCond)
        {
            int x;
            int y;

            Deleted_Items newDeletedItem;

            x = pic.x;
            y = pic.y;

            // возможные ходы для Черной в 1 ход
            if (y > 0 && x>0 &&pole[y - 1, x-1].Condition == 0 && currentCond == 2 && firstReqv == 1)
            {
                pole[y - 1, x-1].mypic.Image = possible_step.Image;
                move_queve.Add(pole[y - 1, x - 1]);
            }
            if (y > 0 && x < 7 && pole[y - 1, x + 1].Condition == 0 && currentCond == 2 && firstReqv == 1)
            {
                pole[y - 1, x + 1].mypic.Image = possible_step.Image;
                move_queve.Add(pole[y - 1, x + 1]);
            }

            // возможные ходы для Белой в 1 ход
            if (y < 7 && x > 0 && pole[y + 1, x - 1].Condition == 0 && currentCond == 1 && firstReqv == 1)
            {
                pole[y + 1, x - 1].mypic.Image = possible_step.Image;
                move_queve.Add(pole[y + 1, x - 1]);
            }
            if (y < 7 && x < 7 && pole[y + 1, x + 1].Condition == 0 && currentCond == 1 && firstReqv == 1)
            {
                pole[y + 1, x + 1].mypic.Image = possible_step.Image;
                move_queve.Add(pole[y + 1, x + 1]);
            }

            //
            //
            //Алгоритм кушания (рекурсивный)
            if(y > 1 && x > 1 && pole[y-1,x-1].Condition != 0 && pole[y-1,x-1].Condition != currentCond && pole[y-2,x-2].Condition == 0 && pole[y - 2, x - 2].mypic.Image != possible_step.Image)
            {
                pole[y - 2, x - 2].mypic.Image = possible_step.Image;

                List<Cell> newList = new List<Cell>();
                for (int i = 0; i < deletedCells.Count; i++) { newList.Add(deletedCells[i]); }

                newList.Add(pole[y - 1, x - 1]);

                newDeletedItem = new Deleted_Items(pole[y - 2, x - 2], newList);
                delItems.Add(newDeletedItem);

                move_queve.Add(pole[y - 2, x - 2]);
                canMove(pole[y - 2, x - 2],0, newList, currentCond);
            }
            if(y > 1 && x < 6 && pole[y-1,x+1].Condition != 0 && pole[y - 1, x + 1].Condition != currentCond && pole[y-2,x+2].Condition == 0 && pole[y-2,x+2].mypic.Image != possible_step.Image)//&& (currentCond == 2 || (currentCond == 0 && firstReqv == 0)))
            {
                pole[y - 2, x + 2].mypic.Image = possible_step.Image;

                List<Cell> newList = new List<Cell>();
                for (int i = 0; i < deletedCells.Count; i++) { newList.Add(deletedCells[i]); }

                newList.Add(pole[y - 1, x + 1]);

                newDeletedItem = new Deleted_Items(pole[y - 2, x +2], newList);
                delItems.Add(newDeletedItem);

                move_queve.Add(pole[y - 2, x + 2]);

                canMove(pole[y - 2, x + 2], 0, newList, currentCond);
            }
            if(y < 6 && x > 1 && pole[y+1,x-1].Condition != 0 && pole[y + 1, x - 1].Condition != currentCond && pole[y + 2, x - 2].Condition == 0 && pole[y + 2, x - 2].mypic.Image != possible_step.Image)
            {
                pole[y + 2, x - 2].mypic.Image = possible_step.Image;

                List<Cell> newList = new List<Cell>();
                for (int i = 0; i < deletedCells.Count; i++) { newList.Add(deletedCells[i]); }

                newList.Add(pole[y + 1, x - 1]);

                newDeletedItem = new Deleted_Items(pole[y + 2, x - 2], newList);
                delItems.Add(newDeletedItem);

                move_queve.Add(pole[y + 2, x - 2]);
                
                canMove(pole[y + 2, x - 2], 0, newList, currentCond);
            }
            if(y <6 && x < 6 && pole[y + 1, x + 1].Condition != 0 && pole[y + 1, x + 1].Condition != currentCond && pole[y + 2, x + 2].Condition == 0 && pole[y + 2, x + 2].mypic.Image != possible_step.Image)
            {
                pole[y + 2, x + 2].mypic.Image = possible_step.Image;

                List<Cell> newList = new List<Cell>();
                for (int i = 0; i < deletedCells.Count; i++) { newList.Add(deletedCells[i]); }

                newList.Add(pole[y + 1, x + 1]);

                newDeletedItem = new Deleted_Items(pole[y + 2, x + 2], newList);
                delItems.Add(newDeletedItem);

                move_queve.Add(pole[y + 2, x + 2]);

                canMove(pole[y + 2, x + 2], 0, newList, currentCond);
            }
        }

        private void canMoveQueen(Cell pic, int firstReqv, List<Cell> deletedCells, int currentCond)
        {
            int x;
            int y;

            string str = pic.mypic.Name.ToString();

            x = int.Parse(str[5].ToString());
            y = int.Parse(str[4].ToString());

            int i = y;                                      //
            int j = x;                                      //обнуление переменных и List-а
            List<Cell> newList = new List<Cell>();          //

            while ( true )// вправо вверх
            {
                Deleted_Items buf_del;
                for (int m = 0; m < deletedCells.Count; m++){ newList.Add(deletedCells[m]); }

                if (i <= -1 || j >= 8 || i >= 8 || j <= -1) break;
                if (i > 0 && j < 7 && pole[i - 1, j + 1].Condition == currentCond) break;
                if (i > 1 && j < 6 && (pole[i - 1, j + 1].Condition != 0 && pole[i - 1, j + 1].Condition != currentCond) && (pole[i - 2, j + 2].Condition != 0 && pole[i - 2, j + 2].Condition != currentCond)) break;

                if (i > 0 && j < 7 && pole[i - 1, j + 1].Condition == 0 && check(pole[i - 1, j + 1]) == false)
                {
                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    pole[i - 1, j + 1].mypic.Image = possible_step.Image;

                    buf_del = new Deleted_Items(pole[i - 1, j + 1], bufL);
                    delItems.Add(buf_del);

                    move_queve.Add(pole[i - 1, j + 1]);
                }
                if(i > 1 && j<6 && pole[i-1,j+1].Condition != 0 && pole[i - 1, j + 1].Condition != currentCond && pole[i-2,j+2].Condition == 0)
                {
                    newList.Add(pole[i - 1, j + 1]);

                    List<Cell> bufL = new List<Cell>();
                    for(int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    pole[i - 2, j + 2].mypic.Image = possible_step.Image;
                    buf_del = new Deleted_Items(pole[i - 2, j + 2], bufL);
                    delItems.Add(buf_del);
                    canMove(pole[i - 2, j + 2], 0, bufL, currentCond);

                    move_queve.Add(pole[i - 2, j + 2]);
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
                if (i < 7 && j > 0 && pole[i + 1, j - 1].Condition == currentCond) break;
                if (i < 6 && j > 1 && (pole[i + 1, j - 1].Condition != 0 && pole[i + 1, j - 1].Condition != currentCond) && (pole[i + 2, j - 2].Condition != 0 && pole[i + 2, j - 2].Condition != currentCond)) break;

                if (i < 7 && j > 0 && pole[i + 1, j - 1].Condition == 0 && check(pole[i + 1, j - 1]) == false)
                {
                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    pole[i + 1, j - 1].mypic.Image = possible_step.Image;

                    buf_del = new Deleted_Items(pole[i + 1, j - 1], bufL);
                    delItems.Add(buf_del);
                    
                    move_queve.Add(pole[i + 1, j - 1]);
                }
                if (i < 6 && j > 1 && pole[i + 1, j - 1].Condition != 0 && pole[i + 1, j - 1].Condition != currentCond && pole[i + 2, j - 2].Condition == 0)
                {
                    newList.Add(pole[i + 1, j - 1]);

                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    pole[i + 2, j - 2].mypic.Image = possible_step.Image;
                    buf_del = new Deleted_Items(pole[i + 2, j - 2], bufL);
                    delItems.Add(buf_del);
                    canMove(pole[i + 2, j - 2], 0, bufL, currentCond);
                    move_queve.Add(pole[i + 2, j - 2]);
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
                if (i < 7 && j < 7 && pole[i + 1, j + 1].Condition == currentCond) break;
                if (i < 6 && j < 6 && (pole[i + 1, j + 1].Condition != 0 && pole[i + 1, j + 1].Condition != currentCond) && (pole[i + 2, j + 2].Condition != 0 && pole[i + 2, j + 2].Condition != currentCond)) break;


                if (i < 7 && j < 7 && pole[i + 1, j + 1].Condition == 0 && check(pole[i + 1, j + 1]) == false)
                {
                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    pole[i + 1, j + 1].mypic.Image = possible_step.Image;

                    buf_del = new Deleted_Items(pole[i + 1, j + 1], bufL);
                    delItems.Add(buf_del);

                    move_queve.Add(pole[i + 1, j + 1]);
                }

                if (i < 6 && j < 6 && pole[i + 1, j + 1].Condition != 0 && pole[i + 1, j + 1].Condition != currentCond && pole[i + 2, j + 2].Condition == 0)
                {
                    newList.Add(pole[i + 1, j + 1]);

                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    pole[i + 2, j + 2].mypic.Image = possible_step.Image;
                    buf_del = new Deleted_Items(pole[i + 2, j + 2], bufL);
                    delItems.Add(buf_del);
                    canMove(pole[i + 2, j + 2], 0, bufL, currentCond);
                    move_queve.Add(pole[i + 2, j + 2]);
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
                if (i > 0 && j > 0 && pole[i - 1, j - 1].Condition == currentCond) break;
                if (i > 1 && j > 1 && (pole[i - 1, j - 1].Condition != 0 && pole[i - 1, j - 1].Condition != currentCond) && (pole[i - 2, j - 2].Condition != 0 && pole[i - 2, j - 2].Condition != currentCond)) break;

                if (i > 0 && j > 0 && pole[i - 1, j - 1].Condition == 0 && check(pole[i - 1, j - 1]) == false)
                {
                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    pole[i - 1, j - 1].mypic.Image = possible_step.Image;

                    buf_del = new Deleted_Items(pole[i - 1, j - 1], bufL);
                    delItems.Add(buf_del);

                    move_queve.Add(pole[i - 1, j - 1]);
                }
                if (i > 1 && j > 1 && pole[i - 1, j - 1].Condition != 0 && pole[i - 1, j - 1].Condition != currentCond && pole[i - 2, j - 2].Condition == 0)
                {
                    newList.Add(pole[i - 1, j - 1]);

                    List<Cell> bufL = new List<Cell>();
                    for (int m = 0; m < newList.Count; m++) { bufL.Add(newList[m]); }

                    pole[i - 2, j - 2].mypic.Image = possible_step.Image;
                    buf_del = new Deleted_Items(pole[i - 2, j - 2], bufL);
                    delItems.Add(buf_del);
                    canMove(pole[i - 2, j - 2], 0, bufL, currentCond);
                    move_queve.Add(pole[i - 2, j - 2]);
                }
                i--;
                j--;
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form1 FORMA = new Form1();
            FORMA.Show();
            this.Hide();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Connect();
        }


        private void Connect()
        {
            
            try { 
                Client = new TcpClient("127.0.0.1", port);
            } catch(Exception e) {
                //Client.Close();
                richTextBox1.Text += "\nError\n";
                Connect();
                return;
            }

            string color = AI_CON == 1 ? "black\r\n\r\n" : "black\r\n\r\n";
            NetworkStream cin = Client.GetStream();

            try
            {
                byte[] data = new byte[1024];
                data = Encoding.ASCII.GetBytes(color);

                cin.Write(data, 0, data.Length);

                string Request = "";
                // Буфер для хранения принятых от клиента данных
                byte[] Buffer = new byte[1024];
                // Переменная для хранения количества байт, принятых от клиента
                int Count;
                // Читаем из потока клиента до тех пор, пока от него поступают данные
                while ((Count = Client.GetStream().Read(Buffer, 0, Buffer.Length)) > 0)
                {
                    // Преобразуем эти данные в строку и добавим ее к переменной Request
                    Request += Encoding.ASCII.GetString(Buffer, 0, Count);
                    // Запрос должен обрываться последовательностью \r\n\r\n
                    // Либо обрываем прием данных сами, если длина строки Request превышает 4 килобайта

                    if (Request.IndexOf("\r\n\r\n") >= 0 || Request.Length > 4096)
                    {
                        break;
                    }
                }

                if (Request == "BAD\r\n\r\n")
                {
                    cin.Flush();
                    cin.Close();
                    Client.Close();
                }
                else
                {
                    draw(Request);
                    makeChoose(10);
                    byte[] data2 = new byte[1024];
                    data2 = Encoding.ASCII.GetBytes(SaveToString());
                    cin.Write(data2, 0, data2.Length);

                }
            }
            catch (Exception e)
            {
                cin.Flush();
                cin.Close();
                Client.Close();
            }

        }


        private void button3_Click(object sender, EventArgs e)
        {
            
            timer1.Enabled = true;
        }

        private void draw(String s)
        {
            int k = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (s[k] == '1')
                    {
                        pole[i, j].mypic.Image = white_chess.Image;
                        pole[i, j].Condition = 1;
                    }
                    else if (s[k] == '2')
                    {
                        pole[i, j].mypic.Image = black_chess.Image;
                        pole[i, j].Condition = 2;
                    }
                    else if (s[k] == '3')
                    {
                        pole[i, j].mypic.Image = white_queen.Image;
                        pole[i, j].Condition = 1;
                    }
                    else if (s[k] == '4')
                    {
                        pole[i, j].mypic.Image = black_queen.Image;
                        pole[i, j].Condition = 2;
                    }
                    else
                    {
                        if ((i + 1) % 2 == 1)
                        {
                            if ((j + 1) % 2 == 1)
                            {
                                pole[i, j].mypic.Image = white.Image;
                                pole[i, j].Condition = 0;
                            }
                            else { 
                                pole[i, j].mypic.Image = black.Image;
                                pole[i, j].Condition = 0;
                            }
                        }
                        else
                        {
                            if ((j + 1) % 2 == 1)
                            {
                                pole[i, j].mypic.Image = black.Image;
                                pole[i, j].Condition = 0;
                            }
                            else
                            {
                                pole[i, j].mypic.Image = white.Image;
                                pole[i, j].Condition = 0;
                            }
                        }
                    }
                    k++;
                }

            }
        }

        private string SaveToString()
        {
            string buf = "";

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (pole[i, j].mypic.Image == white.Image || pole[i, j].mypic.Image == black.Image)
                    {
                        buf = buf + "0";
                    }
                    else if (pole[i, j].mypic.Image == white_chess.Image)
                    {
                        buf = buf + "1";
                    }
                    else if (pole[i, j].mypic.Image == white_queen.Image)
                    {
                        buf = buf + "3";
                    }
                    else if (pole[i, j].mypic.Image == black_chess.Image)
                    {
                        buf = buf + "2";
                    }
                    else if (pole[i, j].mypic.Image == black_queen.Image)
                    {
                        buf = buf + "4";
                    }
                }
            }

            return buf+"\r\n\r\n";
        }

    }
}