using System.Text.Json;

namespace DesafioFundamentos.Models
{
    public class Estacionamento
    {
        private decimal precoInicial;
        private decimal precoPorHora;
        private List<string> veiculos;
        //############## IMPLEMENTAÇÃO ##############
        private string caminhoArquivoJson = "veiculos.json"; // Deixar arquivo veiculos.json no path raiz

        public Estacionamento(decimal precoInicial, decimal precoPorHora)
        {
            this.precoInicial = precoInicial;
            this.precoPorHora = precoPorHora;
            //############## IMPLEMENTAÇÃO ##############
            this.veiculos = CarregarVeiculos(); // Carrega os veículos ao inicializar
        }

        public void AdicionarVeiculo()
        {
            Console.WriteLine("Digite a placa do veículo para estacionar:");

            //############## IMPLEMENTAÇÃO ##############
            string placa = Console.ReadLine();

            if (!string.IsNullOrEmpty(placa))
            {
                if (veiculos.Any(x => x.ToUpper() == placa.ToUpper()))
                {
                    Console.WriteLine("Veículo com essa placa já está estacionado.");
                }
                else
                {
                    veiculos.Add(placa);
                    Console.WriteLine($"Veículo com placa {placa} adicionado ao estacionamento.");
                    SalvarVeiculos(); // Salva na lista de veículos após adicionar
                }
            }
            else
            {
                Console.WriteLine("Placa inválida. Tente novamente.");
            }
        }

        public void RemoverVeiculo()
        {
            Console.WriteLine("Digite a placa do veículo para remover:");

            //############## IMPLEMENTAÇÃO ##############
            string placa = Console.ReadLine();

            if (veiculos.Any(x => x.ToUpper() == placa.ToUpper()))
            {
                Console.WriteLine("Digite a quantidade de horas que o veículo permaneceu estacionado:");
                if (int.TryParse(Console.ReadLine(), out int horas))
                {
                    decimal valorTotal = precoInicial + precoPorHora * horas;

                    veiculos.Remove(placa);
                    Console.WriteLine($"O veículo {placa} foi removido e o preço total foi de: R$ {valorTotal:F2}");
                    SalvarVeiculos(); // Salva a lista de veículos após remover
                }
                else
                {
                    Console.WriteLine("Entrada inválida para a quantidade de horas.");
                }
            }
            else
            {
                Console.WriteLine("Desculpe, esse veículo não está estacionado aqui. Confira se digitou a placa corretamente.");
            }
        }

        public void ListarVeiculos()
        {
            if (veiculos.Any())
            {
                Console.WriteLine("Os veículos estacionados são:");
                foreach (var veiculo in veiculos)
                {
                    Console.WriteLine(veiculo);
                }
            }
            else
            {
                Console.WriteLine("Não há veículos estacionados.");
            }
        }

        private void SalvarVeiculos()
        {   
            //############## IMPLEMENTAÇÃO ##############
            // Serializa a lista de veículos para JSON
            var json = JsonSerializer.Serialize(veiculos);
            File.WriteAllText(caminhoArquivoJson, json);
        }

        private List<string> CarregarVeiculos()
        {   
            //############## IMPLEMENTAÇÃO ##############
            // Verifica se o arquivo JSON existe
            if (File.Exists(caminhoArquivoJson))
            {
                // Lê o JSON do arquivo e desserializa para a lista de veículos
                var json = File.ReadAllText(caminhoArquivoJson);
                return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
            return new List<string>();
        }
    }
}
