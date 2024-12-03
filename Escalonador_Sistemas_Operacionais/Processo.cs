using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escalonador_Sistemas_Operacionais
{
    public class Processo
    {
        public int chegada { get; set; }
        public int duracao { get; set; }

        public Processo(int chegada, int duracao)
        {
            this.chegada = chegada;
            this.duracao = duracao;
        }
    }
}
