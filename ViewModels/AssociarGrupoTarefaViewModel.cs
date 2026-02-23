using ProjetoLPDS.Models;
using ProjetoLPDS.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLPDS.ViewModels {
    public class AssociarGrupoTarefaViewModel {

        public Grupo GrupoSelecionado { set;  get; }
        public ObservableCollection<Tarefa> TodasTarefas { get; }
        public ObservableCollection<Tarefa> TarefasSelecionadas { get; set; }
        public ObservableCollection<TarefaAssociavel> TarefasComCheckbox { get; }




        public AssociarGrupoTarefaViewModel(Grupo grupo) {
            GrupoSelecionado = grupo;
            TodasTarefas = App.TarefasViewModel.TodasTarefas;
            TarefasSelecionadas = new ObservableCollection<Tarefa>(grupo.TarefasAssociadas);
            TarefasComCheckbox = new ObservableCollection<TarefaAssociavel>(
                TodasTarefas.Select(t => new TarefaAssociavel(t, grupo.TarefasAssociadas.Contains(t)))
            );
        }

    }
}
