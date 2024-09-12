# DIO - Trilha .NET - Fundamentos
www.dio.me

## Desafio de projeto
Para este desafio, você precisará usar seus conhecimentos adquiridos no módulo de fundamentos, da trilha .NET da DIO.

## Contexto
Você foi contratado para construir um sistema para um estacionamento, que será usado para gerenciar os veículos estacionados e realizar suas operações, como por exemplo adicionar um veículo, remover um veículo (e exibir o valor cobrado durante o período) e listar os veículos.

## Proposta
Você precisará construir uma classe chamada "Estacionamento", conforme o diagrama abaixo:
![Diagrama de classe estacionamento](diagrama_classe_estacionamento.png)

A classe contém três variáveis, sendo:

**precoInicial**: Tipo decimal. É o preço cobrado para deixar seu veículo estacionado.

**precoPorHora**: Tipo decimal. É o preço por hora que o veículo permanecer estacionado.

**veiculos**: É uma lista de string, representando uma coleção de veículos estacionados. Contém apenas a placa do veículo.

A classe contém três métodos, sendo:

**AdicionarVeiculo**: Método responsável por receber uma placa digitada pelo usuário e guardar na variável **veiculos**.

**RemoverVeiculo**: Método responsável por verificar se um determinado veículo está estacionado, e caso positivo, irá pedir a quantidade de horas que ele permaneceu no estacionamento. Após isso, realiza o seguinte cálculo: **precoInicial** * **precoPorHora**, exibindo para o usuário.

**ListarVeiculos**: Lista todos os veículos presentes atualmente no estacionamento. Caso não haja nenhum, exibir a mensagem "Não há veículos estacionados".

Por último, deverá ser feito um menu interativo com as seguintes ações implementadas:
1. Cadastrar veículo
2. Remover veículo
3. Listar veículos
4. Encerrar


## Solução
O código está pela metade, e você deverá dar continuidade obedecendo as regras descritas acima, para que no final, tenhamos um programa funcional. Procure pela palavra comentada "TODO" no código, em seguida, implemente conforme as regras acima.


## Implatações

1. Sistema de Log
2. Sistema de salvamento (Veiculos e Configurações)
3. Controle de vagas

## Sistema de logs
Para garantir que o sistema não pare devido a um erro e para melhor controle das operações, implementei um sistema de logs. Todos os erros são passadoas para o arquivo log.txt. Além disso, incluí confirmações em todas nas etapas registro e remoção possibilitando o cancelamento e evitando possíveis erros do usuário.
Exemplo: 12/09/2024 18:21:49 - Veículo com placa XXX-0000 adicionado ao estacionamento.

## Sistema de salvamento
Sistema que salva informações em um JSON, permitindo consultar, alterar e criar registros. Isso assegura que todos os veículos sejam mantidos como valores registrados.(arquivos gerados automatico)

## Sistema de vagas
Sistema desenvolvido para controlar as vagas livres no estacionamento com base nos veículos estacionados, evitando a aceitação de clientes quando não há vagas disponíveis.

## Melhoria futura
Regex para validar as placas de veiculos exemplos:
1. Modelo novo Mercosul: ( ^[A-Z]{3}[0-9][A-Z][0-9]{2}$ ).
2. Modelo antigo: ( ^[A-Z]{3}-[0-9]{4}$ ).
