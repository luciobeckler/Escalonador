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

                    if (processoEmExecucao.duracao == 0)
                        retorno.Add(processoEmExecucao);

                    processosQueJaChegaramOrdenadosPorDuracao.RemoveAt(0);

                    foreach (var item in processosQueJaChegaramOrdenadosPorDuracao)
                    {
                        if (!item.isEsteveEmExecucao)
                            item.tempoRetorno++;

                        if (item.isEsteveEmExecucao)
                            item.tempoEspera++;
                    }

                    processos.RemoveAll(p => p.duracao == 0);

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
            List<Processo> retorno = new List<Processo>();

            while(processos.Count > 0)
            {
                Processo processoNaoFinalizado = null;

                foreach (var item in processos.ToList())
                {
                    if (tempoAtual >= item.chegada)
                    {
                        processosQueJaChegaram.Add(item);
                        processos.Remove(item);
                    }
                }

                if (processoNaoFinalizado is not null)
                    processosQueJaChegaram.Add(processoNaoFinalizado);


                bool isProcessoFinalizou = false;
                int quantumBackup = numQuantum;
                Processo processoEmExecucao = new Processo(0, 0);

                if (processosQueJaChegaram.Count > 0)
                {
                    processoEmExecucao = processosQueJaChegaram[0];
                    processosQueJaChegaram.RemoveAt(0);

                    while (quantumBackup > 0 && !isProcessoFinalizou)
                    {
                        processoEmExecucao.isEsteveEmExecucao = true;
                        processoEmExecucao.duracao--;
                        quantumBackup--;

                        isProcessoFinalizou = processoEmExecucao.duracao == 0;

                        bool isTempoFinalizouSemFinalizarProcesso =
                            (numQuantum == 0) &&
                            (!isProcessoFinalizou);

                        if (isProcessoFinalizou)
                        {
                            retorno.Add(processoEmExecucao);
                            processos.Remove(processoEmExecucao);
                        }
                        else if (isTempoFinalizouSemFinalizarProcesso)
                        {
                            processosQueJaChegaram.Add(processoEmExecucao);
                        }

                        processosQueJaChegaram = this.AtualizaTempoRetornoEEsperaFilaExecucao(processosQueJaChegaram);
                        tempoAtual++;
                    }
                    if (processoEmExecucao.duracao > 0)
                        processoNaoFinalizado = processoEmExecucao;


                }
                else
                {
                    tempoAtual++;
                }
                              
            }          

            return retorno;
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
