using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_for_damn
{
    class Cell
    {

        public System.Windows.Forms.PictureBox mypic;
        public int x;
        public int y;
        public int Condition;
        public int chessCondition;
        public Cell(System.Windows.Forms.PictureBox anyPic, int newCond)
        {

            string str = anyPic.Name;

            this.mypic = anyPic;
            this.x = int.Parse(str[5].ToString());
            this.y = int.Parse(str[4].ToString());
            this.Condition = newCond;
            

        }
        public Cell() { }

    }

    class Deleted_Items
    {
        public Cell greenItem;
        public List<Cell> deletedItems;

        public Deleted_Items(Cell newGI , List<Cell> newLST)
        {
            greenItem = newGI;
            deletedItems = newLST;
        }
    }
}
