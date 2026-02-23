using ProjetoLPDS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjetoLPDS.ViewModels {
    public class AlunosViewModel : INotifyPropertyChanged {

        private readonly DataService _dataService = new();
        private ObservableCollection<Aluno> _alunos;
        private ObservableCollection<Aluno> _todosAlunos;
        private ObservableCollection<Aluno> _selecionados;

        public IEnumerable<Aluno> AlunosComGrupo => Alunos.Where(a => !string.IsNullOrEmpty(a.Grupo));
        public IEnumerable<Aluno> AlunosSemGrupo => Alunos.Where(a => string.IsNullOrEmpty(a.Grupo));

        public ObservableCollection<Aluno> Alunos {
            get => _alunos;
            set {
                if (_alunos != value) {
                   
                    _alunos = value;
                    OnPropertyChanged(nameof(Alunos));
                }
            }
                
        }

        public ObservableCollection<Aluno> TodosAlunos {
            get => _todosAlunos;
            set {
                if (_todosAlunos != value) {

                    _todosAlunos = value;
                    OnPropertyChanged(nameof(TodosAlunos));
                }
            }

        }

        public ObservableCollection<Aluno> Selecionados {
            get => _selecionados;
            set {
                if ( _selecionados != value) {

                    _selecionados = value;
                    OnPropertyChanged(nameof(Selecionados));
                }
            }
        }

        private string _grupo;
        public string Grupo {
            get => _grupo;
            set {
                _grupo = value;
                OnPropertyChanged(nameof(Grupo));
            }
        }

        private string _filtroNome;
        public string FiltroNome {
            get => _filtroNome;
            set {
                if (_filtroNome != value) {
                    _filtroNome = value;
                    OnPropertyChanged(nameof(FiltroNome));
                }
            }
        }


        public AlunosViewModel() {
            _todosAlunos = new ObservableCollection<Aluno>(_dataService.CarregarAlunos());
            Alunos = new ObservableCollection<Aluno>(_todosAlunos);
            FiltroNome = string.Empty;  // limpa o filtro ao iniciar

        }

        public void CarregarDeCSV(string fileName) {
            var resultado = MessageBox.Show(
                "Esta operação irá remover todos os alunos atuais e importar os alunos do ficheiro CSV. Deseja continuar?",
                "Importar Alunos do CSV",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes) {
                RemoverTodosAlunos();

                var novosAlunos = _dataService.CarregarAlunosDeCSV(fileName);
                foreach (var aluno in novosAlunos) {
                    Alunos.Add(aluno);
                    TodosAlunos.Add(aluno);
                }

                Guardar();
                MessageBox.Show("Os alunos foram carregados do ficheiro CSV.", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void FiltrarAlunos()
        {
            if (_todosAlunos == null || _todosAlunos.Count == 0)
                return;

            if (string.IsNullOrWhiteSpace(FiltroNome))
            {
                Alunos = new ObservableCollection<Aluno>(_todosAlunos);
            }
            else
            {
                string termo = FiltroNome.ToLower();

                var filtrados = _todosAlunos.Where(a =>
                    (!string.IsNullOrEmpty(a.Nome) && a.Nome.ToLower().Contains(termo)) ||
                    a.Numero.ToString().Contains(termo) ||
                    (!string.IsNullOrEmpty(a.Email) && a.Email.ToLower().Contains(termo)) ||
                    (a.Grupo != null && a.Grupo.ToString().ToLower().Contains(termo))
                ).ToList();

                Alunos = new ObservableCollection<Aluno>(filtrados);
            }
        }



        public void AdicionarAluno(Aluno novoAluno) {

            Alunos.Add(novoAluno);
            TodosAlunos.Add(novoAluno);

            _dataService.GuardarAlunos(TodosAlunos.ToList());
            OnPropertyChanged(nameof(Alunos));
        }


        public void RecarregarAlunos() {

            _alunos.Clear();
            foreach (var aluno in _dataService.CarregarAlunosDeCSV("Data/Alunos.csv"))
                _alunos.Add(aluno);
        }

        public void Guardar()
        {
            _dataService.GuardarAlunos(TodosAlunos.ToList());
        }

        public void NotifyAll() {
            OnPropertyChanged(nameof(Alunos));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string nome) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));


        public void RemoverAlunosSelecionados(IList<Aluno> alunosSelecionados)
        {
            if (alunosSelecionados == null || alunosSelecionados.Count == 0)
            {
                MessageBox.Show("Selecione pelo menos um aluno para remover.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var resultado = MessageBox.Show(
                $"Tem a certeza que pretende remover {alunosSelecionados.Count} aluno(s)?",
                "Confirmar remoção",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                // Remover dos grupos
                foreach (var grupo in App.GruposViewModel.Grupos) {
                    foreach (var aluno in alunosSelecionados) {
                        grupo.Alunos.Remove(aluno);
                    }
                }

                // Remover classificações
                foreach (var aluno in alunosSelecionados) {
                    aluno.Classificacoes?.Clear();
                }

                // Remover das listas globais
                foreach (var aluno in alunosSelecionados) {
                    Alunos.Remove(aluno);
                    TodosAlunos.Remove(aluno);
                }

                // Atualizar interface
                OnPropertyChanged(nameof(Alunos));
                OnPropertyChanged(nameof(TodosAlunos));

                // Atualizar pautas/classificações
                App.ClassificacoesViewModel.Grupos = new ObservableCollection<Grupo>(App.GruposViewModel.Grupos);
                App.ClassificacoesViewModel.CalcularNotasFinais();
                App.ClassificacoesViewModel.OnPropertyChanged(nameof(App.ClassificacoesViewModel.Grupos));
                App.ClassificacoesViewModel.OnPropertyChanged(nameof(App.ClassificacoesViewModel.NotasFinais));
                App.ClassificacoesViewModel.OnPropertyChanged(nameof(App.ClassificacoesViewModel.ListaNotasFinais));

                Guardar();
                App.GruposViewModel.Guardar();
            }
        }

        public void RemoverTodosAlunos() {

            // Remover alunos de todos os grupos
            foreach (var grupo in App.GruposViewModel.Grupos)
                grupo.Alunos.Clear();

            // Limpar listas de alunos
            Alunos.Clear();
            TodosAlunos.Clear();

            // Atualizar interface e guardar
            OnPropertyChanged(nameof(Alunos));
            OnPropertyChanged(nameof(TodosAlunos));
            Guardar();
            App.GruposViewModel.Guardar();

            // Atualizar classificações
            App.ClassificacoesViewModel.Grupos = new ObservableCollection<Grupo>(App.GruposViewModel.Grupos);
            App.ClassificacoesViewModel.CalcularNotasFinais();
            App.ClassificacoesViewModel.OnPropertyChanged(nameof(App.ClassificacoesViewModel.Grupos));
            App.ClassificacoesViewModel.OnPropertyChanged(nameof(App.ClassificacoesViewModel.NotasFinais));
            App.ClassificacoesViewModel.OnPropertyChanged(nameof(App.ClassificacoesViewModel.ListaNotasFinais));
        }

    }
}
