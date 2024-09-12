using DesafioFundamentos.Models;

// Coloca o encoding para UTF8 para exibir acentuação
Console.OutputEncoding = System.Text.Encoding.UTF8;

// Carregar configuração do arquivo config.json
var config = Estacionamento.CarregarConfig();
Console.WriteLine($"Configuração Carregada: Preço Inicial = R${config.PrecoInicial}, Preço por Hora = R${config.PrecoPorHora}, Vagas = {config.Vagas}");
decimal precoInicial = config.PrecoInicial;
decimal precoPorHora = config.PrecoPorHora;
int vagas = config.Vagas;

Console.WriteLine("Seja bem vindo ao sistema de estacionamento!");
Console.WriteLine("Pressione uma tecla para continuar");
Console.ReadLine();

// Instancia a classe Estacionamento, já com os valores obtidos anteriormente
Estacionamento es = new Estacionamento(precoInicial, precoPorHora, vagas);

string opcao = string.Empty;
bool exibirMenu = true;

// Realiza o loop do menu
while (exibirMenu)
{
    //Console.Clear();
    Console.WriteLine("Digite a sua opção:");
    Console.WriteLine("1 - Cadastrar veículo");
    Console.WriteLine("2 - Remover veículo");
    Console.WriteLine("3 - Listar veículos");
    Console.WriteLine("4 - Editar configurações");
    Console.WriteLine("5 - Encerrar");

    switch (Console.ReadLine())
    {
        case "1":
            es.AdicionarVeiculo();
            break;

        case "2":
            es.RemoverVeiculo();
            break;

        case "3":
            es.ListarVeiculos();
            break;

        case "4":
            es.EditarConfiguracoes();
            break;

        case "5":
            exibirMenu = false;
            break;

        default:
            Console.WriteLine("Opção inválida");
            break;
    }

    Console.WriteLine("Pressione uma tecla para continuar");
    Console.ReadLine();
}

Console.WriteLine("O programa se encerrou");