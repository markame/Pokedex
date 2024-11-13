using System;
using System.Collections.Generic;
using System.Media;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace PokedexConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Inicialização do banco de dados
            var client = new MongoClient("mongodb+srv://marcosgsrocha:certi1234@cluster0.xagwprx.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
            var database = client.GetDatabase("marcosgsrocha");
            var collection = database.GetCollection<Pokemon>("pokedex");

            // Menu da Pokedex
            bool sair = false;
            while (!sair)
            {
                Console.WriteLine("========== Pokedex ==========");
                Console.WriteLine("1. Adicionar Pokemon");
                Console.WriteLine("2. Buscar Pokemon por Nome");
                Console.WriteLine("3. Listar todos os Pokemon");
                Console.WriteLine("4. Atualizar Pokemon");
                Console.WriteLine("5. Deletar Pokemon");
                Console.WriteLine("6. Sair");
                Console.WriteLine("=============================");
                Console.Write("Escolha uma opção: ");
                var opcao = Console.ReadLine();
                Console.WriteLine();

                switch (opcao)
                {
                    case "1":
                        await AdicionarPokemon(collection);
                        break;
                    case "2":
                        await BuscarPokemon(collection);
                        break;
                    case "3":
                        await ListarTodosPokemons(collection);
                        break;
                    case "4":
                        await AtualizarPokemon(collection);
                        break;
                    case "5":
                        await DeletarPokemon(collection);
                        break;
                    case "6":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static async Task AdicionarPokemon(IMongoCollection<Pokemon> collection)
        {
            Console.Write("Nome do Pokemon: ");
            var nome = Console.ReadLine();
            Console.Write("Peso do Pokemon: ");
            var peso = double.Parse(Console.ReadLine());
            Console.Write("Tamanho do Pokemon: ");
            var tamanho = double.Parse(Console.ReadLine());
            Console.Write("Tipo do Pokemon: ");
            var tipo = Console.ReadLine();

            List<Ataque> ataques = new List<Ataque>();
            for (int i = 0; i < 4; i++)
            {
                Console.Write($"Nome do Ataque {i + 1}: ");
                var nomeAtaque = Console.ReadLine();
                Console.Write($"Poder do Ataque {i + 1}: ");
                var poderAtaque = int.Parse(Console.ReadLine());
                ataques.Add(new Ataque { Nome = nomeAtaque, Poder = poderAtaque });
            }

            var pokemon = new Pokemon
            {
                Nome = nome,
                Peso = peso,
                Tamanho = tamanho,
                Tipo = tipo,
                Ataques = ataques
            };

            await collection.InsertOneAsync(pokemon);
            Console.WriteLine("Pokemon adicionado com sucesso.");
            PlaySound("soundcatch.wav");
        }

        static async Task BuscarPokemon(IMongoCollection<Pokemon> collection)
        {
            Console.Write("Digite o nome do Pokemon: ");
            var nome = Console.ReadLine();
            var filter = Builders<Pokemon>.Filter.Eq(p => p.Nome, nome);
            var pokemon = await collection.Find(filter).FirstOrDefaultAsync();
            if (pokemon != null)
            {
                Console.WriteLine("Pokemon encontrado:");
                Console.WriteLine($"Nome: {pokemon.Nome}");
                Console.WriteLine($"Peso: {pokemon.Peso}");
                Console.WriteLine($"Tamanho: {pokemon.Tamanho}");
                Console.WriteLine($"Tipo: {pokemon.Tipo}");
                Console.WriteLine("Ataques:");
                foreach (var ataque in pokemon.Ataques)
                {
                    Console.WriteLine($"- {ataque.Nome} (Poder: {ataque.Poder})");
                }
            }
            else
            {
                Console.WriteLine("Pokemon não encontrado.");
            }
        }

        static async Task ListarTodosPokemons(IMongoCollection<Pokemon> collection)
        {
            var pokemons = await collection.Find(_ => true).ToListAsync();
            foreach (var pokemon in pokemons)
            {
                Console.WriteLine($"Nome: {pokemon.Nome}");
                Console.WriteLine($"Peso: {pokemon.Peso}");
                Console.WriteLine($"Tamanho: {pokemon.Tamanho}");
                Console.WriteLine($"Tipo: {pokemon.Tipo}");
                Console.WriteLine("Ataques:");
                foreach (var ataque in pokemon.Ataques)
                {
                    Console.WriteLine($"- {ataque.Nome} (Poder: {ataque.Poder})");
                }
                Console.WriteLine();
            }
        }

        static async Task AtualizarPokemon(IMongoCollection<Pokemon> collection)
        {
            Console.Write("Digite o nome do Pokemon que deseja atualizar: ");
            var nome = Console.ReadLine();
            var filter = Builders<Pokemon>.Filter.Eq(p => p.Nome, nome);
            var pokemon = await collection.Find(filter).FirstOrDefaultAsync();
            if (pokemon != null)
            {
                Console.Write("Digite o novo peso: ");
                var novoPeso = double.Parse(Console.ReadLine());
                Console.Write("Digite o novo tamanho: ");
                var novoTamanho = double.Parse(Console.ReadLine());
                Console.Write("Digite o novo tipo: ");
                var novoTipo = Console.ReadLine();

                List<Ataque> novosAtaques = new List<Ataque>();
                for (int i = 0; i < 4; i++)
                {
                    Console.Write($"Nome do novo Ataque {i + 1}: ");
                    var nomeAtaque = Console.ReadLine();
                    Console.Write($"Poder do novo Ataque {i + 1}: ");
                    var poderAtaque = int.Parse(Console.ReadLine());
                    novosAtaques.Add(new Ataque { Nome = nomeAtaque, Poder = poderAtaque });
                }

                var update = Builders<Pokemon>.Update.Set(p => p.Peso, novoPeso)
                                                     .Set(p => p.Tamanho, novoTamanho)
                                                     .Set(p => p.Tipo, novoTipo)
                                                     .Set(p => p.Ataques, novosAtaques);
                await collection.UpdateOneAsync(filter, update);
                Console.WriteLine("Pokemon atualizado com sucesso.");
            }
            else
            {
                Console.WriteLine("Pokemon não encontrado.");
            }
        }

        static async Task DeletarPokemon(IMongoCollection<Pokemon> collection)
        {
            Console.Write("Digite o nome do Pokemon que deseja deletar: ");
            var nome = Console.ReadLine();
            var filter = Builders<Pokemon>.Filter.Eq(p => p.Nome, nome);
            var result = await collection.DeleteOneAsync(filter);

            if (result.DeletedCount > 0)
            {
                Console.WriteLine("Pokemon deletado com sucesso.");
            }
            else
            {
                Console.WriteLine("Pokemon não encontrado.");
            }
        }

        public static void PlaySound(string caminho)
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = caminho;
            player.Play();
        }
    }
}

public class Pokemon
{
    public ObjectId Id { get; set; }
    public string Nome { get; set; }
    public double Peso { get; set; }
    public double Tamanho { get; set; }
    public string Tipo { get; set; }
    public List<Ataque> Ataques { get; set; }
}

public class Ataque
{
    public string Nome { get; set; }
    public int Poder { get; set; }
}
