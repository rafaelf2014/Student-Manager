using ProjetoLPDS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLPDS.ViewModels {
    public class EditarAlunoViewModel {
        public Aluno AlunoEditavel { get; set; }
        public Aluno AlunoOriginal { get; }

        public EditarAlunoViewModel() {
            AlunoEditavel = new Aluno(); // novo aluno vazio
        }
        public EditarAlunoViewModel(Aluno aluno) {
            // Clonar para não editar o original
            AlunoEditavel = new Aluno(aluno.Nome, aluno.Numero, aluno.Email);
            AlunoOriginal = aluno;
        }

        
    }
}
