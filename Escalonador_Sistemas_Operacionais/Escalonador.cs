using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escalonador_Sistemas_Operacionais
{
    public class Escalonador
    {

        public Escalonador() { }

        public string FIFO()
        {
            return "FIFO";
        }
        public string SJF()
        {
            return "SJF";
        }
        public string SRT()
        {
            return "SRT";
        }
        public string RR(int numQuantum)
        {
            return "RR";
        }
    }

}
