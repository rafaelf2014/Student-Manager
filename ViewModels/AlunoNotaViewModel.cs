using ProjetoLPDS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLPDS.ViewModels {
    public class AlunoNotaViewModel : INotifyPropertyChanged {
        public Aluno Aluno { get; }
        private double _nota;
        public double Nota {
            get => _nota;
            set {
                if (_nota != value) {
                    _nota = value;
                    OnPropertyChanged(nameof(Nota));
                }
            }
        }

        public AlunoNotaViewModel(Aluno aluno, double nota) {
            Aluno = aluno;
            _nota = nota;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string nome) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
    }

}
