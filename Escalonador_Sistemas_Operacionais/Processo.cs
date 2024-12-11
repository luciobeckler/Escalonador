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
        public float tempoEspera { get; set; } = 0;  //Tempo que o processo aguarda para ser executado
        public float tempoRetorno { get; set; } = 0; //Tempo que o processo espera para voltar a ser executado
        public float tempoTurnAround {                
            get 
            { 
                return tempoEspera + duracao;
            }
        }

        public Processo(int chegada, int duracao)
        {
            this.chegada = chegada;
            this.duracao = duracao;
        }

        public Processo Clone()
        {
            return new Processo(this.chegada, this.duracao)
            {
                tempoRetorno = this.tempoRetorno,
                tempoEspera = this.tempoEspera,
            };
        }
    }
}
