using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLPDS.Models
{
    public class Grupo
    {
        public string Id { get; set; }
        public string Nome { get; set; }

        public ObservableCollection<Tarefa> TarefasAssociadas { get; set; }
        public ObservableCollection<Aluno> Alunos { get; set; }

        // construtor vazio
        public Grupo() {
            TarefasAssociadas = new ObservableCollection<Tarefa>();
            Alunos = new ObservableCollection<Aluno>();    
        }
        // Construtor com parâmetros
        public Grupo(string id, string nome)
        {
            Id = id;
            Nome = nome;
            TarefasAssociadas = new ObservableCollection<Tarefa>();
            Alunos = new ObservableCollection<Aluno>();  // Inicializa a lista de alunos
        }

        // Método para adicionar aluno ao grupo
        public void AdicionarAluno(Aluno aluno) {

            if(!Alunos.Contains(aluno)) {
                Alunos.Add(aluno);
                aluno.Grupo = this.Id;
            }
        }
        // Método para remover aluno do grupo
        public void RemoverAluno(Aluno aluno)
        {
            // Busca pelo aluno com mesmo número
            Aluno? alunoParaRemover = null;
            foreach (var a in Alunos)
            {
                if (a.Numero == aluno.Numero)
                {
                    alunoParaRemover = a;
                    break;
                }
            }

            // Remove o aluno se encontrado
            if (alunoParaRemover != null)
            {
                Alunos.Remove(alunoParaRemover);
            }
        }

        // Método para representar o grupo como string
        public override string ToString()
        {
            return $"{Id} - {Nome}";
        }
    }
}

