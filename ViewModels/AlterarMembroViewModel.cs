using ProjetoLPDS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLPDS.ViewModels {
    public class AlterarMembroViewModel {
        private readonly GruposViewModel _gruposViewModel;
        public Grupo GrupoSelecionado { get; set; }

        public Aluno AlunoSelecionado { get; set; }


        public AlterarMembroViewModel(Grupo grupo) {

            GrupoSelecionado = grupo;
            _gruposViewModel = App.GruposViewModel;
        }


        public void AdicionarAlunosAoGrupo(IEnumerable<Aluno> alunosSelecionados) {
            
            foreach (var aluno in alunosSelecionados) {
                GrupoSelecionado.AdicionarAluno(aluno);
                aluno.Grupo = GrupoSelecionado.Nome;
                
            }

            
        }

        public void RemoverAlunoAoGrupo(Aluno aluno) {

            var grupo = _gruposViewModel.Grupos.FirstOrDefault(g => g.Nome == aluno.Grupo);

            if (grupo == null)
                return;

            grupo.RemoverAluno(aluno);
            aluno.Grupo = null; // remover associacao
            
            
        }
    }
}
