using System.Text.Json;
using System.Globalization;

namespace DesafioFundamentos.Models
{
    public class Estacionamento
    {
        private decimal precoInicial;
        private decimal precoPorHora;
        private int vagasDisponiveis;
        private bool debug;
        private DateTime debugTime;
        private List<Veiculo> veiculos;
        private string pathVeiculosJSON = "veiculos.json";
        private string caminhoLog = "log.txt";

        public Estacionamento(decimal precoInicial, decimal precoPorHora, int vagas)
        {
            this.precoInicial = precoInicial;
            this.precoPorHora = precoPorHora;
            this.vagasDisponiveis = vagas;
            var config = CarregarConfig();
            this.debug = config.Debug;
            this.debugTime = config.DebugTime;
            this.veiculos = CarregarVeiculos();
        }

        public void AdicionarVeiculo()
        {
            try
            {

                // Verifica se há vagas disponíveis
                if (veiculos.Count >= vagasDisponiveis)
                {
                    Console.WriteLine("Não há vagas disponíveis no estacionamento.");
                    return;
                }

                Console.WriteLine("Digite a placa do veículo para estacionar:");
                string placa = Console.ReadLine();
                var dataHoraEntrada = ObterDataHoraAtual();

                if (!string.IsNullOrEmpty(placa))
                {
                    // Exibe as informações do veículo para o usuário confirmar
                    Console.WriteLine($"Você deseja adicionar o veículo com placa {placa} (Entrada: {dataHoraEntrada})? (Pressione qualquer tecla para confirmar ou ESC para cancelar)");
                    var tecla = Console.ReadKey(intercept: true).Key;

                    if (tecla == ConsoleKey.Escape)
                    {
                        Console.WriteLine("Operação de adição cancelada.");
                        return;
                    }

                    // Verifica se o veículo já está estacionado
                    if (veiculos.Any(v => v.Placa.ToUpper() == placa.ToUpper()))
                    {
                        Log("Erro: Veículo com essa placa já está estacionado.");
                        Console.WriteLine("Veículo com essa placa já está estacionado.");
                    }
                    else
                    {
                        // Adiciona o veículo à lista
                        veiculos.Add(new Veiculo { Placa = placa, DataHoraEntrada = dataHoraEntrada });
                        SalvarVeiculos();
                        Log($"Veículo com placa {placa} adicionado ao estacionamento.");
                        Console.WriteLine($"Veículo com placa {placa} adicionado ao estacionamento.");
                    }
                }
                else
                {
                    Log("Erro: Placa inválida.");
                    Console.WriteLine("Placa inválida. Tente novamente.");
                }
            }
            catch (Exception ex)
            {
                Log($"Erro ao adicionar veículo: {ex.Message}");
                Console.WriteLine("Ocorreu um erro ao adicionar o veículo. Verifique o log para mais detalhes.");
            }
        }

        public void RemoverVeiculo()
        {
            try
            {
                Console.WriteLine("Digite o número do veículo que deseja remover:");
                ListarVeiculos(true); // true para mostrar o indice
                if (!int.TryParse(Console.ReadLine(), out int indice) || indice <= 0 || indice > veiculos.Count)
                {
                    Log("Erro: Índice inválido para a remoção do veículo.");
                    Console.WriteLine("Índice inválido. Verifique a lista e tente novamente.");
                    return;
                }

                var veiculo = veiculos[indice - 1]; // Obtém o veículo selecionado
                Console.WriteLine($"Você selecionou o veículo {veiculo.Placa} - Entrada: {veiculo.DataHoraEntrada}");

                Console.WriteLine("Deseja continuar com a remoção deste veículo? (Pressione qualquer tecla para continuar ou ESC para cancelar)");
                var tecla = Console.ReadKey(intercept: true).Key;

                if (tecla == ConsoleKey.Escape)
                {
                    Console.WriteLine("Operação de remoção cancelada.");
                    return;
                }

                Console.WriteLine("Digite a quantidade de horas que o veículo permaneceu estacionado:");
                if (int.TryParse(Console.ReadLine(), out int horas))
                {
                    var valorTotal = precoInicial + precoPorHora * horas;

                    veiculos.Remove(veiculo);
                    SalvarVeiculos();
                    Log($"Veículo {veiculo.Placa} removido. Valor total: R$ {valorTotal:F2}. Data e hora de entrada: {veiculo.DataHoraEntrada}, Data e hora de remoção: {ObterDataHoraAtual()}.");
                    Console.WriteLine($"O veículo {veiculo.Placa} foi removido e o preço total foi de: R$ {valorTotal:F2}");
                }
                else
                {
                    Log("Erro: Entrada inválida para a quantidade de horas.");
                    Console.WriteLine("Entrada inválida para a quantidade de horas.");
                }
            }
            catch (Exception ex)
            {
                Log($"Erro ao remover veículo: {ex.Message}");
                Console.WriteLine("Ocorreu um erro ao remover o veículo. Verifique o log para mais detalhes.");
            }
        }

        public void ListarVeiculos(bool numerar = false)
        {
            try
            {
                int vagasLivres = vagasDisponiveis - veiculos.Count;

                if (veiculos.Any())
                {
                    Console.WriteLine($"Temos {veiculos.Count} veículos estacionados e {vagasLivres} vagas livres:");
                    for (int i = 0; i < veiculos.Count; i++)
                    {
                        if (numerar)
                        {
                            Console.WriteLine($"[{i + 1}] {veiculos[i].Placa} - Entrada: {veiculos[i].DataHoraEntrada}");
                        }
                        else
                        {
                            Console.WriteLine($"{veiculos[i].Placa} - Entrada: {veiculos[i].DataHoraEntrada}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Não há veículos estacionados. Temos {vagasLivres} vagas livres.");
                }
            }
            catch (Exception ex)
            {
                Log($"Erro ao listar veículos: {ex.Message}");
                Console.WriteLine("Ocorreu um erro ao listar os veículos. Verifique o log para mais detalhes.");
            }
        }

        private void SalvarVeiculos()
        {
            try
            {
                var json = JsonSerializer.Serialize(veiculos, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(pathVeiculosJSON, json);
            }
            catch (Exception ex)
            {
                Log($"Erro ao salvar veículos: {ex.Message}");
            }
        }

        private List<Veiculo> CarregarVeiculos()
        {
            try
            {
                if (File.Exists(pathVeiculosJSON))
                {
                    var json = File.ReadAllText(pathVeiculosJSON);
                    return JsonSerializer.Deserialize<List<Veiculo>>(json) ?? new List<Veiculo>();
                }
            }
            catch (Exception ex)
            {
                Log($"Erro ao carregar veículos: {ex.Message}");
            }
            return new List<Veiculo>();
        }

        public static Config CarregarConfig()
        {
            try
            {
                if (File.Exists("config.json"))
                {
                    var json = File.ReadAllText("config.json");
                    return JsonSerializer.Deserialize<Config>(json) ?? new Config();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar configuração: {ex.Message}");
            }
            return new Config(); // Retorna uma nova instância com valores padrão
        }
        
        public void EditarConfiguracoes()
        {
            try
            {
                var config = CarregarConfig();
                bool alterar = true;

                while (alterar)
                {
                    Console.WriteLine("Editar configurações:");
                    Console.WriteLine($"1 - Preço Inicial (Atual: {FormatarValorMonetario(config.PrecoInicial)})");
                    Console.WriteLine($"2 - Preço por Hora (Atual: {FormatarValorMonetario(config.PrecoPorHora)})");
                    Console.WriteLine($"3 - Vagas (Atual: {config.Vagas})");
                    Console.WriteLine("4 - Salvar e sair");
                    Console.WriteLine("5 - Cancelar");

                    var opcao = Console.ReadLine();

                    switch (opcao)
                    {
                        case "1":
                            Console.WriteLine("Digite o novo valor para o Preço Inicial:");
                            if (decimal.TryParse(Console.ReadLine(), out decimal novoPrecoInicial))
                            {
                                novoPrecoInicial = FormatarValorMonetarioEntrada(novoPrecoInicial);
                                Console.WriteLine($"Preço Inicial alterado para: {FormatarValorMonetario(novoPrecoInicial)}. (Pressione qualquer tecla para confirmar ou ESC para cancelar)");
                                var tecla = Console.ReadKey(intercept: true).Key;

                                if (tecla == ConsoleKey.Escape)
                                {
                                    Console.WriteLine("Alteração cancelada.");
                                    break;
                                }

                                config.PrecoInicial = novoPrecoInicial;
                                Console.WriteLine("Alteração confirmada.");
                            }
                            else
                            {
                                Console.WriteLine("Valor inválido.");
                            }
                            break;

                        case "2":
                            Console.WriteLine("Digite o novo valor para o Preço por Hora:");
                            if (decimal.TryParse(Console.ReadLine(), out decimal novoPrecoPorHora))
                            {
                                novoPrecoPorHora = FormatarValorMonetarioEntrada(novoPrecoPorHora);
                                Console.WriteLine($"Preço por Hora alterado para: {FormatarValorMonetario(novoPrecoPorHora)}. (Pressione qualquer tecla para confirmar ou ESC para cancelar)");
                                var tecla = Console.ReadKey(intercept: true).Key;

                                if (tecla == ConsoleKey.Escape)
                                {
                                    Console.WriteLine("Alteração cancelada.");
                                    break;
                                }

                                config.PrecoPorHora = novoPrecoPorHora;
                                Console.WriteLine("Alteração confirmada.");
                            }
                            else
                            {
                                Console.WriteLine("Valor inválido.");
                            }
                            break;

                        case "3":
                            Console.WriteLine("Digite o novo número de Vagas:");
                            if (int.TryParse(Console.ReadLine(), out int novasVagas))
                            {
                                Console.WriteLine($"Vagas alterada para: {novasVagas}. (Pressione qualquer tecla para confirmar ou ESC para cancelar)");
                                var tecla = Console.ReadKey(intercept: true).Key;

                                if (tecla == ConsoleKey.Escape)
                                {
                                    Console.WriteLine("Alteração cancelada.");
                                    break;
                                }

                                config.Vagas = novasVagas;
                                Console.WriteLine("Alteração confirmada.");
                            }
                            else
                            {
                                Console.WriteLine("Valor inválido.");
                            }
                            break;

                        case "4":
                            SalvarConfiguracoes(config);
                            Console.WriteLine("Configurações salvas com sucesso.");
                            alterar = false; // Sai do loop após salvar
                            break;

                        case "5":
                            Console.WriteLine("Edição de configurações cancelada.");
                            alterar = false; // Sai do loop sem salvar
                            break;

                        default:
                            Console.WriteLine("Opção inválida.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Erro ao editar configurações: {ex.Message}");
                Console.WriteLine("Ocorreu um erro ao editar as configurações. Verifique o log para mais detalhes.");
            }
        }

        private string FormatarValorMonetario(decimal valor)
        {
            return valor.ToString("C", new CultureInfo("pt-BR"));
        }

        private decimal FormatarValorMonetarioEntrada(decimal valor)
        {
            return Math.Round(valor, 2);
        }

        private void SalvarConfiguracoes(Config config)
        {
            try
            {
                var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("config.json", json);
            }
            catch (Exception ex)
            {
                Log($"Erro ao salvar configurações: {ex.Message}");
            }
        }

        private DateTime ObterDataHoraAtual()
        {
            return debug ? debugTime : DateTime.Now;
        }

        private void Log(string mensagem)
        {
            try
            {
                using (var sw = new StreamWriter(caminhoLog, true))
                {
                    sw.WriteLine($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - {mensagem}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gravar no log: {ex.Message}");
            }
        }

    }



    public class Veiculo
    {
        public string Placa { get; set; }
        public DateTime DataHoraEntrada { get; set; }
    }

    public class Config
    {
        public decimal PrecoInicial { get; set; } = 0;
        public decimal PrecoPorHora { get; set; } = 0;
        public int Vagas { get; set; } = 0;
        public bool Debug { get; set; } = false;
        public DateTime DebugTime { get; set; } = DateTime.Now;
    }
}
