using System;
using System.Collections.Generic;
using System.Media;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using NAudio.Wave;

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

            // Reprodução da música
           

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
           
        }

        static async Task BuscarPokemon(IMongoCollection<Pokemon> collection)
        {
            
        }

        static async Task ListarTodosPokemons(IMongoCollection<Pokemon> collection)
        {
            
        }

        static async Task AtualizarPokemon(IMongoCollection<Pokemon> collection)
        {
            
        }

        static async Task DeletarPokemon(IMongoCollection<Pokemon> collection)
        {
            
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

