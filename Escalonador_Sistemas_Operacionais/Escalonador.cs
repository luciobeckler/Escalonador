using System;
using System.Collections;
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

            int tempoAtual = 0; 
            while (queue.Count > 0)
            {
                Processo processoEmExecucao = queue.Dequeue();
                processoEmExecucao.tempoEspera = tempoAtual - processoEmExecucao.chegada;
                processoEmExecucao.tempoRetorno = processoEmExecucao.tempoEspera;
                tempoAtual += processoEmExecucao.duracao;

                retorno.Add(processoEmExecucao);
            }

            return retorno;
        }
        public List<Processo> executaSJF(List<Processo> processos)
        {
            List<Processo> retorno = new List<Processo>();
            int tempoAtual = 0;

            while (processos.Count > 0)
            {
                Processo processoEmExecucao = processos
                    .OrderBy(p => p.duracao)
                    .Where(p => tempoAtual >= p.chegada)
                    .FirstOrDefault();

                if(processoEmExecucao is not null)
                {
                    processoEmExecucao.tempoEspera = tempoAtual - processoEmExecucao.chegada;
                    processoEmExecucao.tempoRetorno = processoEmExecucao.tempoEspera;
                    tempoAtual += processoEmExecucao.duracao;

                    retorno.Add(processoEmExecucao);
                    processos.Remove(processoEmExecucao);

                    var processoComMenorDuracaoENoTempoAtual = processos
                        .OrderBy(x => x.duracao)
                        .Where(x => tempoAtual >= x.chegada)
                        .FirstOrDefault();

                    if (processoComMenorDuracaoENoTempoAtual != null)
                    {
                        processos.Remove(processoComMenorDuracaoENoTempoAtual);
                        processos.Insert(0, processoComMenorDuracaoENoTempoAtual);
                    }
                }
                else
                {
                    tempoAtual++;
                }           
            }
            return retorno;
        }
        public List<Processo> executaSRT(List<Processo> processos)
        {
            List<Processo> retorno = new List<Processo>();
            List<Processo> listaExecução = new List<Processo>();

            int tempoAtual = 0; 
            while (processos.Count > 0)
            {
                List<Processo> processosQueJaChegaramOrdenadosPorDuracao = processos
                        .Where(p => tempoAtual >= p.chegada)
                        .OrderBy(p => p.duracao)
                        .ToList();
                Processo processoEmExecucao = processosQueJaChegaramOrdenadosPorDuracao.FirstOrDefault();
                
                if(processoEmExecucao is not null)
                {
                    processoEmExecucao.duracao--;
                    processoEmExecucao.isEsteveEmExecucao = true;

                    if (processoEmExecucao.isFinalizado())
                        retorno.Add(processoEmExecucao);

                    processosQueJaChegaramOrdenadosPorDuracao.RemoveAt(0);

                    foreach (var item in processosQueJaChegaramOrdenadosPorDuracao)
                    {
                        if (item.isEsteveEmExecucao)
                            item.tempoEspera++;
                        else
                            item.tempoRetorno++;
                    }

                    processos.RemoveAll(p => p.isFinalizado());
                    tempoAtual++;
                }
                else
                {
                    tempoAtual++;
                }
            }

            foreach (var item in retorno)
            {
                item.tempoEspera += item.tempoRetorno;
            }

            return retorno;
        }

        public List<Processo> executaRR(List<Processo> processos, int numQuantum)
        {
            int tempoAtual = 0;
            List<Processo> processosQueJaChegaram = new List<Processo>();
            int tempoNaCPU = numQuantum;
            List<Processo> retorno = new List<Processo>();
            Queue<Processo> queue = new Queue<Processo>();
            
            while (processos.Count > 0 || queue.Count > 0)
            {
                tempoNaCPU = numQuantum;
                EnfileraProcessosQueJaChegaramEExcluiProcesso(ref processos, ref queue, tempoAtual);
                
                if(queue.Count > 0)
                {
                    Processo processoEmExecucao = queue.Dequeue();

                    while (tempoNaCPU > 0)
                    {
                        EnfileraProcessosQueJaChegaramEExcluiProcesso(ref processos, ref queue, tempoAtual);
                        processoEmExecucao.duracao--;
                        processoEmExecucao.isEsteveEmExecucao = true;

                        foreach (var item in queue)
                        {
                            if (item.isEsteveEmExecucao)
                                item.tempoEspera++;
                            else
                                item.tempoRetorno++;
                        }

                        tempoNaCPU--;
                        tempoAtual++;

                        if (processoEmExecucao.isFinalizado())
                        {
                            retorno.Add(processoEmExecucao);
                            tempoNaCPU = 0; //Sai do loop
                        }
                        
                    }

                    if (!processoEmExecucao.isFinalizado() && tempoNaCPU == 0)
                        processos.Add(processoEmExecucao);
                }
                else
                {
                    tempoAtual++;
                }
            }

            foreach (var item in retorno)
            {
                item.tempoEspera += item.tempoRetorno;
            }

            return retorno;
        }

        private Queue<Processo> EnfileraProcessosQueJaChegaramEExcluiProcesso(ref List<Processo> processos, ref Queue<Processo> queue, int tempoAtual)
        {
            List<int> indicesParaRemover = new List<int>();

            for (int i = 0; i < processos.Count; i++)
            {
                if (tempoAtual >= processos[i].chegada)
                {
                    queue.Enqueue(processos[i]);
                    indicesParaRemover.Add(i);
                }
            }

            for (int i = indicesParaRemover.Count - 1; i >= 0; i--)
            {
                processos.RemoveAt(indicesParaRemover[i]);
            }

            return queue;
        }


        private List<Processo> AtualizaTempoRetornoEEsperaFilaExecucao(List<Processo> list)
        {
            foreach (var item in list)
            {
                item.tempoEspera++;

                if (item.isEsteveEmExecucao)
                    item.tempoRetorno++;
            }

            return list;
        }
    }
}
