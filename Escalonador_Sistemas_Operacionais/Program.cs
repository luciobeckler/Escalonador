// See https://aka.ms/new-console-template for more information


using Escalonador_Sistemas_Operacionais;

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

        List<Processo> resultFIFO = escalonador.executaFIFO(processos);
        List<Processo> resultSJF = escalonador.executaSJF(processos);
        List<Processo> resultSRT = escalonador.executaSRT(processos);
        List<Processo> resultRR = escalonador.executaRR(processos,  numQuantum);

        string nomeArquivoSemExtensao = Path.GetFileNameWithoutExtension(file);
        string numero = nomeArquivoSemExtensao.Split("-")[1];

        Console.WriteLine(numero);

        string caminhoArquivoResultado = $"{pasta}TESTE-{numero}-RESULTADO.txt";

        try
        {
            string conteudoCompleto = string.Join(Environment.NewLine, new List<string>());
            File.WriteAllText(caminhoArquivoResultado, conteudoCompleto);
            
        }
        catch (Exception error) 
        {
            Console.WriteLine($"Erro ao criar ou escrever no arquivo: {error.Message}");
        }
    }

   
}


