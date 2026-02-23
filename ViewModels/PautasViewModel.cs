using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoLPDS.ViewModels;
using ProjetoLPDS.Models;
using ProjetoLPDS.Helpers;
using System.ComponentModel;


namespace ProjetoLPDS.ViewModels
{
    public class PautasViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<NotaAlunoPauta> _notas;

        public ObservableCollection<string> _todastarefas;
               
        public ObservableCollection<NotaAlunoPauta> Notas
        {
            get => _notas;
            set
            {
                if (_notas != value)
                {
                    _notas = value;
                    OnPropertyChanged(nameof(Notas));
                }
            }
        }

        public ObservableCollection<string> TodasTarefas
        {
            get => _todastarefas;
            set
            {
                if (_todastarefas != value)
                {
                    _todastarefas = value;
                    OnPropertyChanged(nameof(TodasTarefas));
                }
            }
        }

        public double _media;
        public double Media
        {
            get => _media;
            set
            {
                if (_media != value)
                {
                    _media = value;
                    OnPropertyChanged(nameof(Media));
                }
            }
        }

        private string _filtro;
        public string Filtro {
            get => _filtro;
            set {
                if (_filtro != value) {
                    _filtro = value;
                    OnPropertyChanged(nameof(Filtro));
                    FiltrarNotas();
                }
            }
        }

        private ObservableCollection<NotaAlunoPauta> _todasNotas;

        public void FiltrarNotas() {
            if (_todasNotas == null)
                _todasNotas = new ObservableCollection<NotaAlunoPauta>(Notas);

            if (string.IsNullOrWhiteSpace(Filtro)) {
                Notas = new ObservableCollection<NotaAlunoPauta>(_todasNotas);
            }
            else {
                var termo = Filtro.ToLower();
                var filtradas = _todasNotas.Where(n =>
                    (n.Aluno.Nome != null && n.Aluno.Nome.ToLower().Contains(termo)) ||
                    (n.Aluno.Numero != null && n.Aluno.Numero.ToLower().Contains(termo))
                ).ToList();

                Notas = new ObservableCollection<NotaAlunoPauta>(filtradas);
            }
            OnPropertyChanged(nameof(Notas));
        }



        public PautasViewModel()
        {
            var dataService = new DataService();

            var classificacoesVM = App.ClassificacoesViewModel;
            var notasFinais = classificacoesVM.NotasFinais;

            // Carregar alunos e tarefas do sistema
            var alunos = App.AlunosViewModel.Alunos;
            var tarefas = App.TarefasViewModel.Tarefas;

            // Descobrir todas as tarefas distintas (pelo nome)
            TodasTarefas = new ObservableCollection<string>(classificacoesVM.Tarefas.Select(t => t.Titulo));
    

            // Construir as linhas da pauta
            Notas = new ObservableCollection<NotaAlunoPauta>(
                    classificacoesVM.Grupos
                       .SelectMany(g => g.Alunos)
                       .Distinct()
                       .Select(aluno => new NotaAlunoPauta {
                           Aluno = aluno,
                           NotasPorTarefa = classificacoesVM.Tarefas.ToDictionary(
                               t => t.Titulo,
                               t => aluno.Classificacoes?.FirstOrDefault(c => c.Tarefa?.Id == t.Id)?.Nota ?? 0.0
                           ),
                           Media = CalcularMediaPonderada(aluno, classificacoesVM.Tarefas),
                           NotaFinal = notasFinais.TryGetValue(aluno, out var nota) ? nota : 0
                        })
                );
        }

        private double CalcularMediaPonderada(Aluno aluno, ObservableCollection<Tarefa> tarefasValidas)
        {
            if (aluno.Classificacoes == null || aluno.Classificacoes.Count == 0)
                return 0.0;

            var classificacoesValidas = aluno.Classificacoes
                .Where(c => c.Tarefa != null && tarefasValidas.Any(t => t.Id == c.Tarefa.Id))
                .ToList();

            if (classificacoesValidas.Count == 0)
                return 0.0;

            double somaPesos = classificacoesValidas.Sum(c => c.Tarefa.Peso);
            double somaNotasPonderadas = classificacoesValidas.Sum(c => c.Nota * c.Tarefa.Peso);

            return somaPesos > 0 ? somaNotasPonderadas / somaPesos : 0.0;
        }


        public class NotaAlunoPauta : INotifyPropertyChanged
        {
            public Aluno Aluno { get; set; }
            public Dictionary<string, double> NotasPorTarefa { get; set; } = new();
            public double Media { get; set; }
            public int NotaFinal { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
            protected void OnPropertyChanged(string nome) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string nome) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
    }
}
