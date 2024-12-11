using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escalonador_Sistemas_Operacionais
{
    public static class Calculadora
    {
        public static List<float> CalculaRetornoEsperaETurnAroundMedios(List<Processo> processos)
        {
            float somaRetorno = 0;
            float somaEspera = 0;
            float somaTurnAround = 0;
            foreach (var item in processos)
            {
                somaRetorno += item.tempoRetorno;
                somaEspera += item.tempoEspera;
                somaTurnAround += item.tempoTurnAround;
            }
            int quantProcessos = processos.Count;

            List<float> tempoMedioRetornoEsperaTurnAround = new List<float> 
            { 
                somaRetorno/quantProcessos, 
                somaEspera/quantProcessos, 
                somaTurnAround/quantProcessos 
            };

            return tempoMedioRetornoEsperaTurnAround;
        }
    }
}
