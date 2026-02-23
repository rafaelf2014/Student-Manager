using ProjetoLPDS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLPDS.ViewModels
{
    public class TarefasViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService = new();
        private ObservableCollection<Tarefa> _tarefas;
        private ObservableCollection<Tarefa> _todasTarefas;
        private ObservableCollection<Tarefa> _selecionadas;

        public ObservableCollection<Tarefa> Tarefas
        {
            get => _tarefas;
            set
            {
                if (_tarefas != value)
                {

                    _tarefas = value;
                    OnPropertyChanged(nameof(Tarefas));
                }
            }

        }

        public ObservableCollection<Tarefa> TodasTarefas
        {
            get => _todasTarefas;
            set
            {
                if (_todasTarefas != value)
                {

                    _todasTarefas = value;
                    OnPropertyChanged(nameof(TodasTarefas));
                }
            }

        }

        public ObservableCollection<Tarefa> Selecionadas
        {
            get => _selecionadas;
            set
            {
                if (_selecionadas != value)
                {

                    _selecionadas = value;
                    OnPropertyChanged(nameof(Selecionadas));
                }
            }
        }

        public TarefasViewModel()
        {
            TodasTarefas = new ObservableCollection<Tarefa>(_dataService.CarregarTarefas());
            Tarefas = new ObservableCollection<Tarefa>(_todasTarefas);
        }

        public void adicionarTarefa(Tarefa novaTarefa)
        {
            TodasTarefas.Add(novaTarefa);
            Tarefas.Add(novaTarefa);

            
            // Associar a nova tarefa a todos os grupos existentes
            foreach (var grupo in App.GruposViewModel.Grupos) {
                if (!grupo.TarefasAssociadas.Any(t => t.Id == novaTarefa.Id))
                    grupo.TarefasAssociadas.Add(novaTarefa);
            }
            Guardar();
            App.GruposViewModel.Guardar();
            OnPropertyChanged(nameof(Tarefas));

            // Notificar o ClassificacoesViewModel
            App.ClassificacoesViewModel.AtualizarGrupo();
            App.ClassificacoesViewModel.OnPropertyChanged(nameof(App.ClassificacoesViewModel.TarefasAssociadas));

            
            




        }

        public void removerTarefa(Tarefa tarefa)
        {
            TodasTarefas.Remove(tarefa);
            Tarefas.Remove(tarefa);
            Guardar();
            OnPropertyChanged(nameof(Tarefas));
        }

        private string _filtroTitulo;
        public string FiltroTitulo {
            get => _filtroTitulo;
            set {
                if (_filtroTitulo != value) {
                    _filtroTitulo = value;
                    OnPropertyChanged(nameof(FiltroTitulo));
                    FiltraTarefasPorTitulo(_filtroTitulo);
                }
            }
        }

        private ObservableCollection<Tarefa> _todasTarefasBackup;

        public void FiltraTarefasPorTitulo(string termo) {
            if (_todasTarefasBackup == null)
                _todasTarefasBackup = new ObservableCollection<Tarefa>(Tarefas);

            if (string.IsNullOrWhiteSpace(termo)) {
                Tarefas = new ObservableCollection<Tarefa>(_todasTarefasBackup);
            }
            else {
                var termoLower = termo.ToLower();
                var filtradas = _todasTarefasBackup
                    .Where(t =>
                        (!string.IsNullOrEmpty(t.Titulo) && t.Titulo.ToLower().Contains(termoLower)) ||
                        (!string.IsNullOrEmpty(t.Id) && t.Id.ToLower().Contains(termoLower))
                    )
                    .ToList();

                Tarefas = new ObservableCollection<Tarefa>(filtradas);
            }
            OnPropertyChanged(nameof(Tarefas));
        }


        public void Guardar()
        {
            _dataService.GuardarTarefas(TodasTarefas.ToList());
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string nome) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
    }
}
