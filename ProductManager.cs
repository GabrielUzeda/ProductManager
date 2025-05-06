using System;                      // Permite acesso a Console, tipos básicos e utilitários
using System.Collections.Generic;  // Disponibiliza List<T> para armazenar coleções de objetos
using System.Globalization;        // Fornece CultureInfo para parsing e formatação de números
using System.Linq;                 // Habilita operações de consulta (LINQ) sobre coleções

// Representa um produto com propriedades essenciais e formatação customizada
class Produto
{
    public int Id { get; set; }           // Identificador único, gerado sequencialmente
    public string Codigo { get; set; }    // Código textual exclusivo para localizar o produto
    public string Nome { get; set; }      // Nome curto que identifica o produto
    public string Descricao { get; set; } // Texto explicativo com detalhes adicionais
    public decimal Preco { get; set; }    // Valor monetário, tipo decimal para precisão
    public bool Ativo { get; set; }       // Flag indicando se o produto está disponível

    // Retorna uma string formatada com todos os dados do produto
    public override string ToString()
    {
        return $"ID: {Id} | Código: {Codigo} | Nome: {Nome} | Preço: R${Preco:F2} | Descrição: {Descricao} | Ativo: {(Ativo ? "Sim" : "Não")}";
    }
}

// Classe principal que controla o fluxo do programa
class Program
{
    private static readonly CultureInfo cultureInfo = CultureInfo.InvariantCulture;
    // cultureInfo fixa a formatação para ponto como separador decimal, sem depender da cultura do sistema

    private static List<Produto> produtos = new List<Produto>();
    // Lista em memória que armazena todos os produtos adicionados

    private static int proximoId = 1;
    // Variável auxiliar que gera IDs únicos para cada novo produto

    // Método de entrada: exibe o menu e chama as funcionalidades até o usuário optar por sair
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\n=== SISTEMA DE GERENCIAMENTO DE PRODUTOS ===");
            Console.WriteLine("1. Cadastrar/Atualizar produto por Código");
            Console.WriteLine("2. Editar produto por ID");
            Console.WriteLine("3. Desativar produto por ID");
            Console.WriteLine("4. Pesquisar produto (por Código ou Nome)");
            Console.WriteLine("5. Listar produtos ativos");
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");

            // Tenta converter input em inteiro; se falhar, repete o menu
            if (!int.TryParse(Console.ReadLine(), out int opcao))
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
                continue;
            }

            switch (opcao)
            {
                case 1:
                    CadastrarOuAtualizarProduto();
                    break;
                case 2:
                    EditarProdutoPorId();
                    break;
                case 3:
                    DesativarProduto();
                    break;
                case 4:
                    PesquisarProdutoUnificado();
                    break;
                case 5:
                    ListarProdutosAtivos();
                    break;
                case 0:
                    Console.WriteLine("Saindo do sistema...");
                    return; // Encerra o programa
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    // Adiciona um produto novo ou atualiza um existente com base no código informado
    static void CadastrarOuAtualizarProduto()
    {
        Console.WriteLine("\n--- CADASTRO/ATUALIZAÇÃO DE PRODUTO POR CÓDIGO ---");
        Console.Write("Código do produto: ");
        string codigo = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(codigo))
        {
            Console.WriteLine("Código não pode ser vazio. Operação cancelada.");
            return;
        }

        // Pesquisa por código, ignorando diferenças entre maiúsculas e minúsculas
        Produto produtoExistente = produtos
            .FirstOrDefault(p => p.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));

        if (produtoExistente != null)
        {
            // Atualiza produto já existente
            Console.WriteLine($"Produto com código '{codigo}' já existe (ID: {produtoExistente.Id}). Atualizando...");
            Console.WriteLine($"Status atual: {(produtoExistente.Ativo ? "Ativo" : "Inativo")}");

            Console.Write($"Novo Nome ({produtoExistente.Nome}): ");
            string nome = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nome)) { Console.WriteLine("Nome é obrigatório."); return; }

            Console.Write($"Nova Descrição ({produtoExistente.Descricao}): ");
            string descricao = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(descricao)) { Console.WriteLine("Descrição é obrigatória."); return; }

            Console.Write($"Novo Preço ({produtoExistente.Preco:F2}): ");
            if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, cultureInfo, out decimal preco) || preco < 0)
            {
                Console.WriteLine("Preço inválido. Atualização cancelada.");
                return;
            }

            bool ativo = SolicitarStatusAtivo($"Ativo? (Atual: {(produtoExistente.Ativo ? "S" : "N")}) (S/N): ");

            produtoExistente.Nome = nome;
            produtoExistente.Descricao = descricao;
            produtoExistente.Preco = preco;
            produtoExistente.Ativo = ativo;

            Console.WriteLine("Produto atualizado com sucesso!");
        }
        else
        {
            // Cria um novo produto
            Console.WriteLine($"Criando novo produto com código '{codigo}'...");
            var novoProduto = new Produto { Codigo = codigo };

            Console.Write("Nome do produto: ");
            string nome = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nome)) { Console.WriteLine("Nome é obrigatório. Cadastro cancelado."); return; }

            Console.Write("Descrição do produto: ");
            string descricao = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(descricao)) { Console.WriteLine("Descrição é obrigatória. Cadastro cancelado."); return; }

            Console.Write("Preço: ");
            if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, cultureInfo, out decimal preco) || preco < 0)
            {
                Console.WriteLine("Preço inválido. Cadastro cancelado.");
                return;
            }

            bool ativo = SolicitarStatusAtivo("Produto está ativo? (S/N): ");

            novoProduto.Nome = nome;
            novoProduto.Descricao = descricao;
            novoProduto.Preco = preco;
            novoProduto.Ativo = ativo;
            novoProduto.Id = proximoId++;
            produtos.Add(novoProduto);

            Console.WriteLine("Produto cadastrado com sucesso!");
        }
    }

    // Permite editar todas as propriedades de um produto existente pelo seu ID
    static void EditarProdutoPorId()
    {
        Console.WriteLine("\n--- EDIÇÃO DE PRODUTO POR ID ---");
        Console.Write("Digite o ID do produto que deseja editar: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var produto = produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null)
        {
            Console.WriteLine("Produto não encontrado.");
            return;
        }

        Console.WriteLine($"Editando produto: {produto}");

        Console.Write($"Novo Código ({produto.Codigo}): ");
        string novoCodigo = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(novoCodigo)) { Console.WriteLine("Código é obrigatório."); return; }
        if (!produto.Codigo.Equals(novoCodigo, StringComparison.OrdinalIgnoreCase) &&
            produtos.Any(p => p.Id != produto.Id && p.Codigo.Equals(novoCodigo, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine($"Erro: Já existe outro produto com o código '{novoCodigo}'. Edição cancelada.");
            return;
        }

        Console.Write($"Novo Nome ({produto.Nome}): ");
        string novoNome = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(novoNome)) { Console.WriteLine("Nome é obrigatório."); return; }

        Console.Write($"Nova Descrição ({produto.Descricao}): ");
        string novaDescricao = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(novaDescricao)) { Console.WriteLine("Descrição é obrigatória."); return; }

        Console.Write($"Novo Preço ({produto.Preco:F2}): ");
        if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, cultureInfo, out decimal novoPreco) || novoPreco < 0)
        {
            Console.WriteLine("Preço inválido. Edição cancelada.");
            return;
        }

        bool novoAtivo = SolicitarStatusAtivo($"Ativo? (Atual: {(produto.Ativo ? "S" : "N")}) (S/N): ");

        produto.Codigo = novoCodigo;
        produto.Nome = novoNome;
        produto.Descricao = novaDescricao;
        produto.Preco = novoPreco;
        produto.Ativo = novoAtivo;

        Console.WriteLine("Produto atualizado com sucesso!");
    }

    // Marca um produto como inativo (remoção lógica), sem excluí-lo definitivamente
    static void DesativarProduto()
    {
        Console.WriteLine("\n--- DESATIVAÇÃO DE PRODUTO POR ID ---");
        Console.Write("Digite o ID do produto que deseja desativar: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var produto = produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null)
        {
            Console.WriteLine("Produto não encontrado.");
            return;
        }

        if (!produto.Ativo)
        {
            Console.WriteLine($"Produto (ID: {id}, Código: {produto.Codigo}) já está inativo.");
            return;
        }

        Console.WriteLine($"Tem certeza que deseja DESATIVAR o produto: {produto}? (S/N)");
        var confirmacao = Console.ReadLine()?.ToUpper();

        if (confirmacao == "S")
        {
            produto.Ativo = false;
            Console.WriteLine("Produto desativado com sucesso!");
        }
        else
        {
            Console.WriteLine("Operação cancelada.");
        }
    }

    // Busca produtos por código ou nome, retornando ativos e inativos
    static void PesquisarProdutoUnificado()
    {
        Console.WriteLine("\n--- PESQUISA DE PRODUTO (POR CÓDIGO OU NOME) ---");
        Console.Write("Digite o termo para pesquisar: ");
        var termo = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(termo))
        {
            Console.WriteLine("Termo de pesquisa não pode ser vazio.");
            return;
        }

        var resultados = produtos
            .Where(p =>
                p.Codigo.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                p.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (resultados.Count == 0)
        {
            Console.WriteLine("Nenhum produto encontrado com o termo informado.");
        }
        else
        {
            Console.WriteLine($"\n{resultados.Count} produto(s) encontrado(s) (inclui ativos e inativos):");
            foreach (var produto in resultados)
            {
                Console.WriteLine(produto);
            }
        }
    }

    // Lê repetidamente 'S' ou 'N' até obter resposta válida, retornando true para 'S'
    static bool SolicitarStatusAtivo(string prompt)
    {
        string ativoInput;
        while (true)
        {
            Console.Write(prompt);
            ativoInput = Console.ReadLine()?.ToUpper();
            if (ativoInput == "S" || ativoInput == "N")
                break;
            Console.WriteLine("Entrada inválida. Digite S para Sim ou N para Não.");
        }
        return ativoInput == "S";
    }

    // Exibe apenas produtos com Ativo = true, ordenados alfabeticamente pelo nome
    static void ListarProdutosAtivos()
    {
        Console.WriteLine("\n--- LISTA DE PRODUTOS ATIVOS ---");
        var produtosAtivos = produtos
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToList();

        if (produtosAtivos.Count == 0)
        {
            Console.WriteLine("Nenhum produto ativo cadastrado.");
            return;
        }

        foreach (var produto in produtosAtivos)
        {
            Console.WriteLine(produto);
        }

        Console.WriteLine($"\n({produtos.Count - produtosAtivos.Count} produto(s) inativo(s) não listado(s))");
    }
}
