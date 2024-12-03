// See https://aka.ms/new-console-template for more information


using Escalonador_Sistemas_Operacionais;
using System.Text;

string pasta = "C:\\Users\\lucio\\Documents\\ParkIF\\Escalonador_Sistemas_Operacionais\\Escalonador_Sistemas_Operacionais\\ArquivosTestes\\";

if (Directory.Exists(pasta))
{
    string[] files = Directory.GetFiles(pasta);

    foreach (string file in files)
    {
        
        List<Processo> processos = new List<Processo>();
        Escalonador escalonador = new Escalonador();

        string content = File.ReadAllText(file);

        List<string> processosString = content.Split("\n").ToList();
        int numQuantum = int.Parse(processosString[0]);
        processosString.RemoveRange(0, 1);



        Console.WriteLine(content);

        foreach (var item in processosString)
        {
            string[] partes = item.Split(" ");

            int chegada = int.Parse(partes[0]);
            int duracao = int.Parse(partes[1].Substring(0));

            processos.Add(new Processo(chegada, duracao));
        }

        List<List<Processo>> result = new List<List<Processo>>();

        result.Add(escalonador.executaFIFO(processos));
        result.Add(escalonador.executaSJF(processos));
        result.Add(escalonador.executaSRT(processos));
        result.Add(escalonador.executaRR(processos,  numQuantum));

        float[,] medias = new float[4,3];
        int i = 0;

        foreach (var tipoDeEscalonador in result)
        {
            float somaTempoRetorno = 0;
            float somaTempoEspera = 0;
            float somaTempoTurnaround = 0;

            int totalProcessos = tipoDeEscalonador.Count;

            foreach (var processo in tipoDeEscalonador)
            {
                somaTempoRetorno += processo.tempoRetorno;
                somaTempoEspera += processo.tempoEspera;
                somaTempoTurnaround += processo.tempoTurnAround;

                totalProcessos++;
            }

            medias[i, 0] = somaTempoRetorno / totalProcessos;
            medias[i, 1] = somaTempoEspera / totalProcessos;
            medias[i, 2] = somaTempoTurnaround / totalProcessos;

            i++;
        }

        string nomeArquivoSemExtensao = Path.GetFileNameWithoutExtension(file);
        string numero = nomeArquivoSemExtensao.Split("-")[1];

        Console.WriteLine(numero);

        string caminhoArquivoResultado = $"{pasta}TESTE-{numero}-RESULTADO.txt";

        try
        {
            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < medias.GetLength(0); j++)
            { 
                string tempoRetornoMedio = medias[j, 0].ToString();
                string tempoEsperaMedio = medias[j, 1].ToString();
                string tempoTurnaroundMedio = medias[j, 2].ToString();

                sb.AppendLine($"{tempoRetornoMedio}\t{tempoEsperaMedio}\t{tempoTurnaroundMedio}");
            }

            string conteudoCompleto = string.Join(Environment.NewLine, sb);
            File.WriteAllText(caminhoArquivoResultado, conteudoCompleto);
            
        }
        catch (Exception error) 
        {
            Console.WriteLine($"Erro ao criar ou escrever no arquivo: {error.Message}");
        }
    }

   
}


