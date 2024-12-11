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
            List<Processo> retorno = new List<Processo>();
            List<Processo> listaExecução = new List<Processo>();

            int tempoAtual = 1;
            while (processos.Count > 0)
            {
                List<Processo> processosQueJaChegaramOrdenadosPorDuracao = processos
                        .Where(p => tempoAtual >= p.chegada )
                        .OrderBy(p => p.duracao)
                        .ToList();

                Processo processoEmExecucao = processosQueJaChegaramOrdenadosPorDuracao.FirstOrDefault();
                processoEmExecucao.duracao--;
                processoEmExecucao.isEsteveEmExecucao = true;

                if (processoEmExecucao.duracao == 0)
                    retorno.Add(processoEmExecucao);
                
                processosQueJaChegaramOrdenadosPorDuracao.RemoveAt(0);

                foreach (var item in processosQueJaChegaramOrdenadosPorDuracao)
                {
                    if(!item.isEsteveEmExecucao)
                        item.tempoEspera++;

                    if (item.isEsteveEmExecucao)
                        item.tempoRetorno++;
                }

                

                processos.RemoveAll(p => p.duracao == 0);

                tempoAtual++;
            }

            return retorno;
        }
        public List<Processo> executaRR(List<Processo> processos, int numQuantum)
        {
            return new List<Processo>();
        }
    }

}
