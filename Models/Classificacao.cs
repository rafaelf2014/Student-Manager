using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLPDS.Models
{
    public class Classificacao
    {
        public double Nota {  get;  set; }

        public Tarefa Tarefa { get;  set; }

        public string NumeroAluno { get;  set; }

        public Classificacao() { }
        public Classificacao(string numeroAluno, Tarefa tarefa, double nota) {

            NumeroAluno = numeroAluno;
            Tarefa = tarefa;
            Nota = nota;
        }

    }
}

