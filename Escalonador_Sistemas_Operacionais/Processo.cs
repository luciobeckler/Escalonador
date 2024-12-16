using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escalonador_Sistemas_Operacionais
{
    public class Processo
    {
        public int chegada { get;}
        public int duracao { get; set; }
        private int duracaoOriginal { get; set; } //Criado pois "duracao" é alterado durante o fluxo de execução e este é usado para o turnAround
        public float tempoEspera { get; set; } = 0;  //Tempo de espera total
        public float tempoRetorno { get; set; } = 0; //Tempo que o processo espera para começar a ser executado
        public float tempoTurnAround {                
            get 
            { 
                return tempoEspera + duracaoOriginal;
            }
        }
        public bool isEsteveEmExecucao { get; set; }

        public Processo(int chegada, int duracao)
        {
            this.chegada = chegada;
            this.duracao = duracao;
            this.duracaoOriginal = duracao;
        }

        public Processo Clone()
        {
            return new Processo(this.chegada, this.duracao)
            {
                tempoRetorno = this.tempoRetorno,
                tempoEspera = this.tempoEspera,
            };
        }

        public bool isFinalizado()
        {
            return this.duracao == 0;
        }
    }
}
