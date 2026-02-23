using ProjetoLPDS.Models;
using ProjetoLPDS.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace ProjetoLPDS.Views
{
    /// <summary>
    /// Lógica interna para Pautas.xaml
    /// </summary>
    public partial class Pautas : Window
    {
        private readonly PautasViewModel _viewModel;
        
        public Pautas()
        {
            InitializeComponent();
            _viewModel = new PautasViewModel();
            DataContext = _viewModel;

            
        }

        

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }

        private void btnHistograma_Click(object sender, RoutedEventArgs e) {

            Grafico grafico = new Grafico();
            grafico.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConstruirColunasDataGrid();
        }

        private void ConstruirColunasDataGrid()
        {
            var vm = DataContext as PautasViewModel;
            if (vm == null) return;

            dataGridPautas.Columns.Clear();

            // Coluna Número
            dataGridPautas.Columns.Add(new DataGridTextColumn
            {
                Header = "Número",
                Binding = new Binding("Aluno.Numero"),
                Width = DataGridLength.SizeToCells,
                MinWidth = 55
            });

            // Coluna Nome
            dataGridPautas.Columns.Add(new DataGridTextColumn
            {
                Header = "Nome",
                Binding = new Binding("Aluno.Nome"),
                Width = DataGridLength.SizeToCells,
                MinWidth = 300
            });

            // Colunas dinâmicas para cada tarefa
            foreach (var tarefa in vm.TodasTarefas)
            {
                dataGridPautas.Columns.Add(new DataGridTextColumn
                {
                    Header = tarefa,
                    Binding = new Binding($"NotasPorTarefa[{tarefa}]") { StringFormat = "F2" },
                    Width = DataGridLength.SizeToHeader,
                    MinWidth = 50
                });
            }

            // Coluna Média
            dataGridPautas.Columns.Add(new DataGridTextColumn
            {
                Header = "Média",
                Binding = new Binding("Media") { StringFormat = "F2" },
                Width = DataGridLength.SizeToHeader,
                MinWidth = 50
            });

            // Coluna Nota Final
            dataGridPautas.Columns.Add(new DataGridTextColumn 
            {
                Header = "Nota Final",
                Binding = new Binding("NotaFinal"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                MinWidth = 70
            });
        }

        private void tbPesquisar_TextChanged(object sender, TextChangedEventArgs e) {
            var vm = DataContext as PautasViewModel;
            if (vm != null) {
                vm.Filtro = tbPesquisar.Text;
            }
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e) {
            var vm = DataContext as PautasViewModel;
            if (vm != null) {
                vm.Filtro = tbPesquisar.Text;
            }
        }


    }
}

