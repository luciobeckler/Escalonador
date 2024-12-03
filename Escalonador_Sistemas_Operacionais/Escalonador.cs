using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escalonador_Sistemas_Operacionais
{
    public class Escalonador
    {

        public Escalonador() { }

        public List<Processo> executaFIFO(List<Processo> processos)
        {
            Queue<Processo> queue = new Queue<Processo>();
            List<Processo> retorno = new List<Processo>();
            foreach (var item in processos)
                queue.Enqueue(item);

            int tempoAtual = 1;
            while (queue.Count > 0)
            {
                Processo processoEmExecucao = queue.Dequeue();
                processoEmExecucao.tempoRetorno = 0;
                processoEmExecucao.tempoEspera = tempoAtual - processoEmExecucao.chegada;
                tempoAtual += processoEmExecucao.duracao;

                retorno.Add(processoEmExecucao);
            }

            return retorno;
        }
        public List<Processo> executaSJF(List<Processo> processos)
        {
            List<Processo> retorno = new List<Processo>();
            int tempoAtual = 1;

            while(processos.Count > 0)
            {
                Processo processoEmExecucao = processos[0];
                processoEmExecucao.tempoRetorno = 0;
                processoEmExecucao.tempoEspera = tempoAtual - processoEmExecucao.chegada;
                tempoAtual += processoEmExecucao.duracao;

                retorno.Add(processoEmExecucao);
                processos.Remove(processoEmExecucao);

                var processoComMenorDuracaoENoTempoAtual = processos
                    .OrderBy(x => x.duracao)
                    .Where(x => tempoAtual >= x.chegada)
                    .FirstOrDefault();

                if(processoComMenorDuracaoENoTempoAtual != null)
                {
                    processos.Remove(processoComMenorDuracaoENoTempoAtual);
                    processos.Insert(0, processoComMenorDuracaoENoTempoAtual);
                }

            }
            return retorno;
        }
        public List<Processo> executaSRT(List<Processo> processos)
        {
            return new List<Processo>();
        }
        public List<Processo> executaRR(List<Processo> processos, int numQuantum)
        {
            return new List<Processo>();
        }
    }

}
