using ProjetoLPDS.Models;
using ProjetoLPDS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;
using ProjetoLPDS.ViewModels;
using ProjetoLPDS.Helpers;
using System.Diagnostics;

namespace ProjetoLPDS.ViewModels
{
    public class ClassificacoesViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService = new();


        public ObservableCollection<Tarefa> _tarefas;
        public ObservableCollection<Tarefa> _tarefasAssociadas;
        public ObservableCollection<Grupo> _grupos;
        public ObservableCollection<Aluno> _alunosGrupoSelecionado;
        public ObservableDictionary<Aluno, int> _notasFinais;
        public ObservableCollection<int> _listaNotasfinais;

        public ObservableCollection<NotaAluno> NotasAlunos { get; set; }



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

        public ObservableCollection<Tarefa> TarefasAssociadas {

            get => _tarefasAssociadas;
            set {
                if (_tarefasAssociadas != value) {
                    _tarefasAssociadas = value;
                    OnPropertyChanged(nameof(TarefasAssociadas));
                }
            }
        }

        public ObservableCollection<Grupo> Grupos
        {
            get => _grupos;
            set
            {
                if (_grupos != value)
                {
                    _grupos = value;
                    OnPropertyChanged(nameof(Grupos));
                }
            }
        }

        public ObservableCollection<Aluno> AlunosGrupoSelecionado {
            get => _alunosGrupoSelecionado;
            set {
                if (_alunosGrupoSelecionado != value) {
                    _alunosGrupoSelecionado = value;
                    OnPropertyChanged(nameof(AlunosGrupoSelecionado));
                }
            }
        }


        public ObservableDictionary<Aluno, int> NotasFinais {
            get => _notasFinais;
            set {
                if (_notasFinais != value) {
                    _notasFinais = value;
                    OnPropertyChanged(nameof(NotasFinais));
                }
            }
        }

        public ObservableCollection<int> ListaNotasFinais {
            get => _listaNotasfinais;
            set {
                if (_listaNotasfinais != value) {
                    _listaNotasfinais = value;
                    OnPropertyChanged(nameof(ListaNotasFinais));
                }
            }
        }

        public Tarefa _tarefaSelecionada;
        public Tarefa TarefaSelecionada {
            get => _tarefaSelecionada;
            set {
                if (_tarefaSelecionada != value) {
                    _tarefaSelecionada = value;
                    OnPropertyChanged(nameof(TarefaSelecionada));
                    AtualizarGrupo();
                }
            }
        }

        public Grupo _grupoSelecionado;
        public Grupo GrupoSelecionado {
            get => _grupoSelecionado;
            set {
                if (_grupoSelecionado != value) {

                    _grupoSelecionado = value;
                    OnPropertyChanged(nameof(GrupoSelecionado));
                    OnPropertyChanged(nameof(NotasFinais));
                    AtualizarGrupo();
                }
            }
        }


        public ClassificacoesViewModel() {

            Grupos = App.GruposViewModel.Grupos;

            var alunos = _dataService.CarregarAlunos();
            // Sincronização das classificações dos alunos nos grupos
            foreach (var grupo in Grupos) {
                foreach (var aluno in grupo.Alunos) {
                    var alunoPersistido = alunos.FirstOrDefault(a => a.Numero == aluno.Numero);
                    if (alunoPersistido != null && alunoPersistido.Classificacoes != null)
                        aluno.Classificacoes = alunoPersistido.Classificacoes;
                }
            }

            Tarefas = App.TarefasViewModel.Tarefas;

            


            NotasAlunos = new ObservableCollection<NotaAluno>();

            NotasFinais = new ObservableDictionary<Aluno, int>();

            CalcularNotasFinais();

        }

        public void CalcularNotasFinais()
        {
            var notasFinais = new ObservableDictionary<Aluno, int>();

            // Obter IDs das tarefas válidas (ainda existentes)
            var tarefasValidas = new HashSet<string>(Tarefas.Select(t => t.Id));

            foreach (var grupo in Grupos)
            {
                foreach (var aluno in grupo.Alunos)
                {
                    if (aluno.Classificacoes != null && aluno.Classificacoes.Count > 0)
                    {
                        // Filtrar classificações apenas para tarefas válidas
                        var classificacoesValidas = aluno.Classificacoes
                            .Where(c => c.Tarefa != null && tarefasValidas.Contains(c.Tarefa.Id))
                            .ToList();

                        if (classificacoesValidas.Count > 0)
                        {
                            // Média ponderada
                            double somaPesos = classificacoesValidas.Sum(c => c.Tarefa.Peso);
                            double somaNotasPonderadas = classificacoesValidas.Sum(c => c.Nota * c.Tarefa.Peso);

                            double mediaPonderada = somaPesos > 0 ? somaNotasPonderadas / somaPesos : 0;
                            int mediaArredondada = (int)Math.Round(mediaPonderada, MidpointRounding.AwayFromZero);
                            notasFinais[aluno] = mediaArredondada;
                        }
                        else
                        {
                            notasFinais[aluno] = 0;
                        }
                    }
                    else
                    {
                        notasFinais[aluno] = 0;
                    }
                }
            }

            NotasFinais = notasFinais;
            ListaNotasFinais = new ObservableCollection<int>(NotasFinais.Values);
        }


        public void AtualizarGrupo() {

            TarefasAssociadas = new ObservableCollection<Tarefa>(
                Tarefas.Where(t => GrupoSelecionado?.TarefasAssociadas.Any(ta => ta.Id == t.Id) == true)
            );
            NotasAlunos.Clear();

            if (GrupoSelecionado == null || TarefaSelecionada == null)
                return;

            foreach (var aluno in GrupoSelecionado.Alunos) {
                double nota = 0;
                if (aluno.Classificacoes == null)
                    aluno.Classificacoes = new List<Classificacao>();

                var classificacao = aluno.Classificacoes.FirstOrDefault(c => c.Tarefa != null && c.Tarefa.Id == TarefaSelecionada.Id);
                if (classificacao != null)
                    nota = classificacao.Nota;

                NotasAlunos.Add(new NotaAluno {
                    Aluno = aluno,
                    Nota = nota
                });
            }

        }

        public void GuardarNotas() {
            foreach (var notaAluno in NotasAlunos) {
                var aluno = notaAluno.Aluno;
                if (aluno.Classificacoes == null)
                    aluno.Classificacoes = new List<Classificacao>();

                var classificacao = aluno.Classificacoes.FirstOrDefault(c => c.Tarefa != null && c.Tarefa.Id == TarefaSelecionada.Id);
                if (classificacao != null) {
                    classificacao.Nota = notaAluno.Nota;
                }
                else {
                    aluno.Classificacoes.Add(new Classificacao {
                        NumeroAluno = aluno.Numero,
                        Tarefa = TarefaSelecionada,
                        Nota = notaAluno.Nota
                    });
                }
            }

            // Sincronizar classificações dos alunos dos grupos com a lista global
            foreach (var grupo in Grupos) {
                foreach (var alunoGrupo in grupo.Alunos) {
                    var alunoGlobal = App.AlunosViewModel.Alunos.FirstOrDefault(a => a.Numero == alunoGrupo.Numero);
                    if (alunoGlobal != null) {
                        alunoGlobal.Classificacoes = alunoGrupo.Classificacoes;
                    }
                }
            }

            _dataService.GuardarAlunos(App.AlunosViewModel.Alunos.ToList());
            _dataService.GuardarGrupos(Grupos.ToList());
        }





        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string nome) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
    }

    public class NotaAluno : INotifyPropertyChanged {
        public Aluno Aluno { get; set; }
        private double _nota;
        public double Nota {
            get => _nota;
            set {
                if (_nota != value) {
                    _nota = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nota)));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
