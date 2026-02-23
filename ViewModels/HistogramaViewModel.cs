using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ProjetoLPDS.Models;
using System.ComponentModel;

namespace ProjetoLPDS.ViewModels {
    public class HistogramaViewModel : INotifyPropertyChanged {
        public ISeries[] Series { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }        

        public HistogramaViewModel() {


            int[] data = App.ClassificacoesViewModel?.ListaNotasFinais?.ToArray() ?? Array.Empty<int>();
            // Dados de exemplo - notas dos alunos (valores inteiros de 0 a 20)
            //double[] data = new double[] {
            //    // Notas muito baixas (0-4) - 5 alunos
            //    0, 2, 3, 3, 4,

            //    // Notas baixas (5-9) - 25 alunos
            //    5, 5, 5, 6, 6, 6, 6, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9, 9, 9, 9,

            //    // Notas médias baixas (10-11) - 25 alunos
            //    10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11,

            //    // Notas médias (12-13) - 30 alunos
            //    12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
            //    13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13,

            //    // Notas médias altas (14-15) - 33 alunos
            //    14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
            //    15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15,

            //    // Notas boas (16-17) - 20 alunos
            //    16, 16, 16, 16, 16, 16, 16, 16, 16, 16,
            //    17, 17, 17, 17, 17, 17, 17, 17, 17, 17,

            //    // Notas muito boas (18-19) - 10 alunos
            //    18, 18, 18, 18, 18, 19, 19, 19, 19, 19,

            //    // Notas excelentes (20) - 5 alunos
            //    20, 20, 20, 20, 20
            //};

            // Configurar o histograma para mostrar a frequência de cada nota inteira de 0 a 20
            int minNota = 0;
            int maxNota = 20;
            int totalNotas = maxNota - minNota + 1; // 21 possíveis notas (0 a 20)

            // Criar um array para contar a frequência de cada nota
            var frequencias = new double[totalNotas];

            // Contar a frequência de cada nota no conjunto de dados
            foreach (double nota in data) {
                int indice = (int)nota - minNota;
                if (indice >= 0 && indice < totalNotas) {
                    frequencias[indice]++;
                }
            }

            // Criar labels para cada nota inteira
            var notasLabels = new string[totalNotas];
            for (int i = 0; i < totalNotas; i++) {
                notasLabels[i] = (i + minNota).ToString();
            }

            // Criar a série do Histograma
            Series = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = frequencias,
                    Name = "Frequência",
                    Fill = new SolidColorPaint(SKColors.DodgerBlue),
                    Stroke = new SolidColorPaint(SKColors.DarkBlue) { StrokeThickness = 2 },
                    Padding = 0.5 // Pequeno espaçamento entre as colunas para melhor visualização
                }
            };

            // Configurar eixos
            XAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Notas",
                    Labels = notasLabels,
                    ForceStepToMin = true,
                    // Mostrar apenas algumas labels para evitar congestionamento
                    UnitWidth = 1,
                    MinStep = 2, // Mostrar labels de 2 em 2 notas
                    TextSize = 10,
                    
                    // Configurações para texto em negrito
                    NamePaint = new SolidColorPaint
                    {
                        Color = SKColors.Black,
                        SKTypeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright)
                    },
                    LabelsPaint = new SolidColorPaint
                    {
                        Color = SKColors.Black,
                        SKTypeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright)
                    }
                }
            };

            YAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Frequência",
                    MinLimit = 0, // Iniciar eixo Y em zero
                    // Formato de números inteiros para a frequência
                    Labeler = value => value.ToString("F0"),
                    ShowSeparatorLines = true, // mostrar linhas horizontais
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray),

                    // Configurações para texto em negrito
                    NamePaint = new SolidColorPaint
                    {
                        Color = SKColors.Black,
                        SKTypeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright)
                    },
                    LabelsPaint = new SolidColorPaint
                    {
                        Color = SKColors.Black,
                        SKTypeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright)
                    }
                }
            };
        }

        // Método para atualizar o histograma com novos dados
        public void AtualizarDados(IEnumerable<double> novasNotas) {
            int minNota = 0;
            int maxNota = 20;
            int totalNotas = maxNota - minNota + 1;

            // Resetar as frequências
            var frequencias = new double[totalNotas];

            // Contar a frequência de cada nota no novo conjunto de dados
            foreach (double nota in novasNotas) {
                int indice = (int)Math.Round(nota) - minNota;
                if (indice >= 0 && indice < totalNotas) {
                    frequencias[indice]++;
                }
            }

            // Atualizar a série com os novos dados
            if (Series.Length > 0 && Series[0] is ColumnSeries<double> columnSeries) {
                columnSeries.Values = frequencias;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string nome) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
    }
}
