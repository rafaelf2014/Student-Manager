using ProjetoLPDS.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using ProjetoLPDS.ViewModels;

namespace ProjetoLPDS.Models
{
    // Classe para gerir a persistência de dados
    public class DataService
    {
        private readonly string pastaDados;

        // Opcoes globais de JSON para imprimir com indentação
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping

        };

        // Construtor
        public DataService()
        {
            // Define o caminho para gravar os arquivos
            string pastaPerfil = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            pastaDados = Path.Combine(pastaPerfil, "GestaoAvaliacoes");
            
            // Cria a pasta se não existir
            if (!Directory.Exists(pastaDados))
            {
                Directory.CreateDirectory(pastaDados);
            }
        }
        // Métodos para perfil
        public Perfil CarregarPerfil() {

            try {
                string caminhoArquivo = Path.Combine(pastaDados, "perfil.json");
                if(File.Exists(caminhoArquivo)) {
                    string json = File.ReadAllText(caminhoArquivo, Encoding.UTF8);
                    return JsonSerializer.Deserialize<Perfil>(json);
                }
            }
            catch (Exception ex) {
                 Console.WriteLine("Erro ao carregar perfil: " + ex.Message);
            }
            // Retorna perfil padrão se não existir ou falhar
            return new Perfil {
                Nome = Environment.UserName,
                Email = "default@example.com",
                CaminhoFotografia = "pack://application:,,,/Images/defaultUser.png"
            };
        }

        public void GuardarPerfil(Perfil perfil) {

            try {
                string caminhoArquivo = Path.Combine(pastaDados, "perfil.json");
                string json = JsonSerializer.Serialize(perfil, jsonOptions);
                File.WriteAllText(caminhoArquivo, json, Encoding.UTF8);
            }
            catch (Exception ex) {
                Console.WriteLine("Erro ao guardar perfil: " + ex.Message);            
            }
        }

        // Métodos para alunos
        public List<Aluno> CarregarAlunos()
        {
            try
            {
                string caminhoArquivo = Path.Combine(pastaDados, "alunos.json");
                if (File.Exists(caminhoArquivo))
                {
                    string json = File.ReadAllText(caminhoArquivo, Encoding.UTF8);
                    return JsonSerializer.Deserialize<List<Aluno>>(json);
                }
                return new List<Aluno>();
            }
            catch
            {
                return new List<Aluno>();
            }
        }

        public void GuardarAlunos(List<Aluno> alunos)
        {
            try
            {
                string caminhoArquivo = Path.Combine(pastaDados, "alunos.json");
                string json = JsonSerializer.Serialize(alunos, jsonOptions);
                File.WriteAllText(caminhoArquivo, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar alunos: " + ex.Message);
            }
        }

        // Métodos para tarefas
        public List<Tarefa> CarregarTarefas()
        {
            try
            {
                string caminhoArquivo = Path.Combine(pastaDados, "tarefas.json");
                if (File.Exists(caminhoArquivo))
                {
                    string json = File.ReadAllText(caminhoArquivo);
                    return JsonSerializer.Deserialize<List<Tarefa>>(json);
                }
                return new List<Tarefa>();
            }
            catch
            {
                return new List<Tarefa>();
            }
        }

        public void GuardarTarefas(List<Tarefa> tarefas)
        {
            try
            {
                string caminhoArquivo = Path.Combine(pastaDados, "tarefas.json");
                string json = JsonSerializer.Serialize(tarefas, jsonOptions);
                File.WriteAllText(caminhoArquivo, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar tarefas: " + ex.Message);
            }
        }

        // Métodos para grupos
        public List<Grupo> CarregarGrupos()
        {
            try
            {
                string caminhoArquivo = Path.Combine(pastaDados, "grupos.json");
                if (File.Exists(caminhoArquivo))
                {
                    string json = File.ReadAllText(caminhoArquivo);
                    return JsonSerializer.Deserialize<List<Grupo>>(json);
                }
                return new List<Grupo>();
            }
            catch
            {
                return new List<Grupo>();
            }
        }

        public void GuardarGrupos(List<Grupo> grupos)
        {
            try
            {
                string caminhoArquivo = Path.Combine(pastaDados, "grupos.json");
                string json = JsonSerializer.Serialize(grupos, jsonOptions);
                File.WriteAllText(caminhoArquivo, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao guardar grupos: " + ex.Message);
            }
        }

        

        // Método para carregar alunos de um arquivo CSV
        public List<Aluno> CarregarAlunosDeCSV(string caminhoArquivo)
        {
            List<Aluno> alunos = new List<Aluno>();
            
            try
            {
                // Verifica se arquivo existe
                if (!File.Exists(caminhoArquivo)) {
                    Console.WriteLine($"Arquivo não encontrado: {caminhoArquivo}");
                    return alunos;
                }

                // Lê o arquivo linha por linha
                var linhas = File.ReadAllLines(caminhoArquivo);
                foreach(var linha in linhas) {

                    var colunas = linha.Split(',');

                    if (colunas.Length >= 3) {

                        string nome = colunas[0];
                        string numero = colunas[1];
                        string email = colunas[2];

                        alunos.Add(new Aluno(nome, numero, email));
                    } 
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao carregar CSV: " + ex.Message);
            }
            
            return alunos;
        }
    }
}

