using System;
using ProjetoLPDS.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoLPDS.Helpers;

namespace ProjetoLPDS.Models
{
    public class Aluno
    {
        public string Numero { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    
        // opcional - grupo do aluno
        public string Grupo { get; set; }

        // Classificações do aluno
        public List<Classificacao> Classificacoes { get; set; } = new();
        

        // Construtor sem parâmetros
        public Aluno() { }

        // Construtor com parâmetros
        public Aluno(string nome, string numero, string email)
        {
            Nome = nome;
            Numero = numero;
            Email = email;
            
        }
        public override bool Equals(object? obj) {
            if (obj is not Aluno other)
                return false;

            return Numero == other.Numero;
        }

        public override int GetHashCode() {
            return Numero?.GetHashCode() ?? 0;
        }
    }
    
}

