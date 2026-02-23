using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLPDS.Models
{
    public class Tarefa
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataTermino { get; set; }
        public double Peso { get; set; }

        // Construtor sem parametros
        public Tarefa() {

        }

        // Construtor com parametros
        public Tarefa(string id, string titulo, string descricao = "", DateTime? dataInicio = null, DateTime? dataTermino = null, double peso = 1.0) {
            Id = id;
            Titulo = titulo;
            Descricao = descricao;
            DataInicio = dataInicio ?? DateTime.Now;
            DataTermino = dataTermino ?? DateTime.Now.AddDays(7);
            Peso = peso;
        }

        public override bool Equals(object? obj) {
            return obj is Tarefa tarefa && Id == tarefa.Id;
        }

        public override int GetHashCode() {
            return Id != null ? Id.GetHashCode() : 0;
        }
    }
}
