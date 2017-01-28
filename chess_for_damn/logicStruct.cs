using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_for_damn
{
    class logicStruct
    {
        public Cell otkuda;
        public Cell kuda;
        public int fx;
        public logicStruct(Cell newOtkuda, Cell newKuda, int newFx)
        {
            this.otkuda = newOtkuda;
            this.kuda = newKuda;
            this.fx = newFx;
        }
    }
}
