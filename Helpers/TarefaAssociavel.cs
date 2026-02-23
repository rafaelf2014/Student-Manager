using ProjetoLPDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ProjetoLPDS.Helpers {

    public class TarefaAssociavel : INotifyPropertyChanged {
        public Tarefa Tarefa { get; }
        private bool _associada;
        public bool Associada {
            get => _associada;
            set {
                if (_associada != value) {
                    _associada = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Associada)));
                }
            }
        }

        public TarefaAssociavel(Tarefa tarefa, bool associada) {
            Tarefa = tarefa;
            _associada = associada;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
