using ProjetoLPDS.Models;
using ProjetoLPDS.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLPDS.ViewModels {
    public class GruposViewModel : INotifyPropertyChanged {
        private readonly DataService _dataService = new();

        private ObservableCollection<Grupo> _grupos;
        private ObservableCollection<Grupo> _todosGrupos;
        private ObservableCollection<Grupo> _selecionados;

        


        public ObservableCollection<Grupo> Grupos {
            get => _grupos;
            set {
                if(_grupos != value) {

                    _grupos = value;
                    OnPropertyChanged(nameof(Grupos));
                }
            }
        }

        public ObservableCollection<Grupo> TodosGrupos {
            get => _todosGrupos;
            set {
                if (_todosGrupos != value) {

                    _todosGrupos = value;
                    OnPropertyChanged(nameof(TodosGrupos));
                }
            }
        }

        public ObservableCollection<Grupo> Selecionados {
            get => _selecionados;
            set {
                if (_selecionados != value) {

                    _selecionados = value;
                    OnPropertyChanged(nameof(Selecionados));
                }
            }
        }

        private string _filtroNome;
        public string FiltroNome {
            get => _filtroNome;
            set {
                if (_filtroNome != value) {
                    _filtroNome = value;
                    OnPropertyChanged(nameof(FiltroNome));
                    FiltrarGrupos();
                }
            }
        }

        private ObservableCollection<Grupo> _todosGruposBackup;

        public void FiltrarGrupos() {
            if (_todosGruposBackup == null)
                _todosGruposBackup = new ObservableCollection<Grupo>(Grupos);

            if (string.IsNullOrWhiteSpace(FiltroNome)) {
                Grupos = new ObservableCollection<Grupo>(_todosGruposBackup);
            }
            else {
                var termo = FiltroNome.ToLower();
                var filtrados = _todosGruposBackup
                    .Where(g =>
                        (!string.IsNullOrEmpty(g.Nome) && g.Nome.ToLower().Contains(termo)) ||
                        (!string.IsNullOrEmpty(g.Id) && g.Id.ToLower().Contains(termo))
                    )
                    .ToList();

                Grupos = new ObservableCollection<Grupo>(filtrados);
            }
            OnPropertyChanged(nameof(Grupos));
        }


        public GruposViewModel() {
            TodosGrupos = new ObservableCollection<Grupo>(_dataService.CarregarGrupos());
            Grupos = new ObservableCollection<Grupo>(_todosGrupos);
        }

        

        public void AdicionarGrupo(Grupo novoGrupo) {

            if (novoGrupo.TarefasAssociadas == null)
                novoGrupo.TarefasAssociadas = new ObservableCollection<Tarefa>();

            // Associar todas as tarefas existentes
            foreach (var tarefa in App.TarefasViewModel.Tarefas) {
                if (!novoGrupo.TarefasAssociadas.Any(t => t.Id == tarefa.Id))
                    novoGrupo.TarefasAssociadas.Add(tarefa);
            }

            Grupos.Add(novoGrupo);
            TodosGrupos.Add(novoGrupo);

            Guardar();
            OnPropertyChanged(nameof(Grupos));
        }

        public void Guardar() {

            _dataService.GuardarGrupos(Grupos.ToList());
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string nome) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
    }
}
