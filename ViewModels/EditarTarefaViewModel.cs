using ProjetoLPDS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLPDS.ViewModels
{
    public class EditarTarefaViewModel : INotifyPropertyChanged
    {

        public Tarefa TarefaEditavel { get; set; }

        public Tarefa TarefaOriginal { get; }


        public EditarTarefaViewModel()
        {
            TarefaEditavel = new Tarefa {
                DataInicio = DateTime.Today,
                DataTermino = DateTime.Today.AddDays(1)
            };
        }

        public EditarTarefaViewModel(Tarefa tarefa)
        {
            TarefaEditavel = new Tarefa(tarefa.Id, tarefa.Titulo, tarefa.Descricao, tarefa.DataInicio, tarefa.DataTermino, tarefa.Peso);
            TarefaOriginal = tarefa;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string Id) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Id));
    }
    

}
