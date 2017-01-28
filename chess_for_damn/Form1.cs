using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace chess_for_damn
{

    public partial class Form1 : Form
    {
        Cell[,] pole = new Cell[8, 8];
        Cell buf_pic;
        Cell otkuda_jum;

        int flag_for_timer = 1;

        Cell[,] buf = new Cell[8, 8]; // AI cup


        //
        // move_queve - массив для значений подсвеченных ячеек
        //
        List<Cell> move_queve = new List<Cell>();
        //
        //
        //
        // delItems - что-то типо мапа где одному значению ячейки из move_queve 
        // соответствует массив Cell-ов которые удалятся при ходе в эту клетку
        List<Deleted_Items> delItems = new List<Deleted_Items>();

        int AI_CON = 1;
        int NOT_AI = 2;

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

            if (AI_CON == 1)
            {
                makeChoose();
            }
        }



        /*
        // Logic Block
        //
        //
        //
        */

        private void AI_moving(List<logicStruct> rat) // 
        {
            List<logicStruct> anyOther = new List<logicStruct>();
            if (rat.Count == 0 && flag_for_timer == 0)
            {
                return;
            }
            if (rat.Count == 0)
            {
                flag_for_timer = 0;
                int flag_to_win_ai = checkToWinAI();
                if (flag_to_win_ai == 0)
                {
                    this.timer1.Enabled = false;
                    MessageBox.Show("ПАТ !!!");
                }
                
                return;
            }
            int max = rat[0].fx;
            int idx=0;
            for (int i = 0; i < rat.Count; i++)
            {
                if (rat[i].fx >= max)
                {
                    max = rat[i].fx;
                    idx = i;
                }
            }

            for (int i = 0; i < rat.Count; i++)
            {
                if (rat[i].fx == max)
                {
                    anyOther.Add(rat[i]);
                }

            }

            Random rnd = new Random();
            idx = rnd.Next(anyOther.Count);

            //
            List<Cell> null_Iter = new List<Cell>();
            buf_pic = anyOther[idx].otkuda;
            if (buf_pic.mypic.Image == white_queen.Image || buf_pic.mypic.Image == black_queen.Image)
            {
                // Если это королева
                canMoveQueen(buf_pic, 1, null_Iter, AI_CON);                                 //
            }
            else
            {
                // Все остальные холопы
                canMove(buf_pic, 1, null_Iter, AI_CON);                                      //
            }
            //
            this.otkuda_x = anyOther[idx].otkuda.x;
            this.otkuda_y = anyOther[idx].otkuda.y;

                                                                                       // Если нажали Туда, то тоже чистим подсветку за собой 
            canGetMove = 0;                                                                     //Go                                         
            MoveIt(anyOther[idx].kuda);                                                                    //двигаем

            flagOnStep = flagOnStep == 1 ? 2 : 1;                                               // меняем шаг на противоположный
            label1.Text = flagOnStep == 1 ? "Ход Белых" : "Ход Черных";

            checkToWin();
            Clear();



        }

        private void makeChoose()
        {
            List<logicStruct> rating = new List<logicStruct>();
            for(int i=0; i < 8; i++)
            {
                for(int j=0; j < 8; j++)
                {
                    if (pole[i, j].Condition != AI_CON) continue;
                    ClearDelItems();
                    


                    List<Cell> null_Iter = new List<Cell>();
                    Cell buf_p = pole[i, j];
                    if (buf_p.mypic.Image == white_queen.Image || buf_p.mypic.Image == black_queen.Image)
                    {
                        ClearDelItems();
                        canMoveQueen(buf_p, 1, null_Iter, buf_p.Condition);
                    }
                    else
                    {
                        ClearDelItems();
                        canMove(buf_p, 1, null_Iter, buf_p.Condition); 
                    }

                    for(int k =0; k<move_queve.Count;k++)
                    {
                        int rat = funcChoose(i, j, move_queve[k].y, move_queve[k].x);

                        rating.Add(new logicStruct(pole[i, j], move_queve[k], rat));
                        
                    }
                    Clear();
                    //canGetMove = 1;
                }
            }
            //return rating;
            AI_moving(rating);

        }


        private int funcChoose(int fromY, int fromX, int toY, int toX)
        {
            int answer = 0;
            int Ans = 0;
            int count=0;
            
            for (int i =0; i < 8; i++)
            {
                for(int j =0; j<8; j++)
                {
                    buf[i, j] = new Cell(pole[i, j].mypic, pole[i, j].Condition);

                    if (pole[i, j].Condition == NOT_AI) count+=10;

                    buf[i, j].Condition = pole[i, j].Condition;
                    buf[i, j].x = pole[i, j].x;
                    buf[i, j].y = pole[i, j].y;
                    buf[i, j].mypic = pole[i, j].mypic;
                    buf[i, j].mypic.Name = pole[i, j].mypic.Name;
                }
            }


            // checker
            int check_before = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (buf[i, j].Condition == AI_CON)
                    {
                        check_before += checker(buf, i, j);
                    }
                }

            }
            //</checker>

            int otkuda_x = fromX;
            int otkuda_y = fromY;

            buf[otkuda_y, otkuda_x].Condition = 0;


            int y = toY;
            int x = toX;

            if (delItems.Count > 0)
            {
                ClearLongItemsAI(buf[y, x]);
            }
            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (buf[i, j].Condition == NOT_AI) Ans += 15;
                }
            }

            // checker
            int check_after = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (buf[i, j].Condition == AI_CON)
                    {
                        check_after += checker(buf, i, j);
                    }
                }

            }

            //</checker>
            int check_sum = check_after - check_before; // тут - по умолчанию на выходе, правда хз насколько все хреново

            answer = count - Ans;
            answer += checker(buf, toY, toX);
            return answer + check_sum;
        }



        private void MoveItAI(Cell trs, Cell[,] bufer, int fromY, int fromX, int toY, int toX)
        {
            // "двигаем" фишку, свапая значения ячеек
            int x = fromX;
            int y = fromY;

            ClearLongItemsAI(bufer[y, x]);

            ClearDelItems();

            //
            bufer[y, x].Condition = bufer[otkuda_y, otkuda_x].Condition;                  //
            bufer[otkuda_y, otkuda_x].Condition = 0;                                     //

        }


        private void ClearLongItemsAI(Cell trs)
        {
            //
            //очистка delItems ячеек, соответствующих ячейке в которую мы походили
            //
            int len = delItems.Count;

            for (int i = 0; i < len; i++)
            {
                if (delItems[i].greenItem.mypic.Name == trs.mypic.Name)
                {
                    ClearAI(delItems[i].deletedItems);
                }
            }
        }



        private void ClearAI(List<Cell> trs)
        {
            //
            //перегрузка чтобы "чистить" все возможные List-ы
            //
            int len = trs.Count;

            for (int i = 0; i < len; i++)
            {

                int y = trs[0].y;
                int x = trs[0].x;

                //buffers[y, x].mypic.Image = black.Image;
                buf[y, x].Condition = 0;
                //
                trs.Remove(trs[0]);
            }
        }

        private int checker(Cell[,] buf, int y, int x)
        {
            int Ans = 0;

            if ((x > 0 && y < 7 && buf[y + 1, x - 1].Condition == NOT_AI) && (y > 0 && x < 7 && buf[y - 1, x + 1].Condition == 0))
            {
                Ans-=5;
            }
            if ((x < 7 && y > 0 && buf[y - 1, x + 1].Condition == NOT_AI) && (x > 0 && y < 7 && buf[y + 1, x - 1].Condition == 0))
            {
                Ans -= 5;
            }
            if ((x > 0 && y > 0 && buf[y - 1, x - 1].Condition == NOT_AI) && (x < 7 && y < 7 && buf[y + 1, x + 1].Condition == 0))
            {
                Ans -= 5;
            }
            if ((x < 7 && y < 7 && buf[y + 1, x + 1].Condition == NOT_AI) && (x > 0 && y > 0 && buf[y - 1, x - 1].Condition == 0))
            {
                Ans -= 5;
            }

            if (Ans < 0)
                return Ans;
            return Ans;
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
            MessageBox.Show(who+" IS WIN!!!");              // Вывод сообщения о победе
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
                    makeChoose();
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

                pole[y,x].mypic.Image = black.Image;
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

            string str = pic.mypic.Name.ToString();
            x = int.Parse(str[5].ToString());
            y = int.Parse(str[4].ToString());

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
            if (flagOnStep == AI_CON && flag_for_timer == 1)
            {
                makeChoose();
            }
        }
    }
}