using Escalonador_Sistemas_Operacionais;
using System.Text;
using System.Text.RegularExpressions;

string pasta = "C:\\Users\\lucio\\Documents\\ParkIF\\Escalonador_Sistemas_Operacionais\\Escalonador_Sistemas_Operacionais\\ArquivosTestes\\";

if (Directory.Exists(pasta))
{
    string[] files = Directory.GetFiles(pasta);

    foreach (string file in files)
    {
        
            // Inicializa a lista de processos
            List<Processo> processos = new List<Processo>();
            Escalonador escalonador = new Escalonador();

            // Lê o conteúdo do arquivo
            string content = File.ReadAllText(file);
            List<string> processosString = content.Split(Environment.NewLine).ToList();

            // Extrai o quantum e remove a primeira linha
             int numQuantum = int.Parse(processosString[0]);
            processosString.RemoveAt(0);

            Console.WriteLine($"Processando arquivo: {file}");

            // Converte as linhas restantes em objetos Processo
            foreach (var item in processosString)
            {
                if (string.IsNullOrWhiteSpace(item)) continue; // Ignora linhas vazias

                string[] partes = Regex.Replace(item, @"\s+", " ").Split(" "); // Normaliza os tipos de espaços, necessário pois podem haver espaços inquebráveis que devem ser substituidos por espaços normais.

                int chegada = int.Parse(partes[0]);
                int duracao = int.Parse(partes[1]);

                processos.Add(new Processo(chegada, duracao));
            }

            // Executa os algoritmos de escalonamento
            List<Processo> processosOriginais = processos.Select(p => p.Clone()).ToList();
            List<Processo> resultadoFIFO = escalonador.executaFIFO(processos);
            
            processos = processosOriginais.Select(p => p.Clone()).ToList();
            List<Processo> resultadoSJF = escalonador.executaSJF(processos);
            
            processos = processosOriginais.Select(p => p.Clone()).ToList();
            List<Processo> resultadoSRT = escalonador.executaSRT(processos);
            
            processos = processosOriginais.Select(p => p.Clone()).ToList();
            List<Processo> resultadoRR = escalonador.executaRR(processos, numQuantum);

            // Calcula as métricas médias
            List<float> mediasFIFO = Calculadora.CalculaRetornoEsperaETurnAroundMedios(resultadoFIFO);
            List<float> mediasSJF = Calculadora.CalculaRetornoEsperaETurnAroundMedios(resultadoSJF);
            List<float> mediasSRT = Calculadora.CalculaRetornoEsperaETurnAroundMedios(resultadoSRT);
            List<float> mediasRR = Calculadora.CalculaRetornoEsperaETurnAroundMedios(resultadoRR);

            // Gera o nome do arquivo de saída
            string nomeArquivoSemExtensao = Path.GetFileNameWithoutExtension(file);
            string numero = nomeArquivoSemExtensao.Split("-")[1];
            string caminhoArquivoResultado = $"{pasta}TESTE-{numero}-RESULTADO.txt";

            // Cria o conteúdo do arquivo de saída
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{mediasFIFO[0]:F3} {mediasFIFO[1]:F3} {mediasFIFO[2]:F3}");
            sb.AppendLine($"{mediasSJF[0]:F3} {mediasSJF[1]:F3} {mediasSJF[2]:F3}");
            sb.AppendLine($"{mediasSRT[0]:F3} {mediasSRT[1]:F3} {mediasSRT[2]:F3}");
            sb.AppendLine($"{mediasRR[0]:F3} {mediasRR[1]:F3} {mediasRR[2]:F3}");
            
            // Grava o arquivo de resultados
            File.WriteAllText(caminhoArquivoResultado, sb.ToString());

            Console.WriteLine($"Resultados salvos em: {caminhoArquivoResultado}");
        
    }
}
