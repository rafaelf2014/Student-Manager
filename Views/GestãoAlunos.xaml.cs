using ProjetoLPDS.ViewModels;
using ProjetoLPDS.Models;
using System;
using System.Collections.Generic;
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
using Microsoft.Win32;

namespace ProjetoLPDS.Views
{
    /// <summary>
    /// Lógica interna para GestãoAlunos.xaml
    /// </summary>


    public partial class GestaoAlunos : Window
    {
        private readonly AlunosViewModel _viewModel;
        public GestaoAlunos()
        {
            InitializeComponent();
            _viewModel = App.AlunosViewModel;
            this.DataContext = _viewModel;

        }

        

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
            _viewModel.Guardar();
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e) {

            var vm = new EditarAlunoViewModel();
            var janela = new AdicionarAluno(vm);

            if(janela.ShowDialog() == true) {

                var novoAluno = vm.AlunoEditavel;
                _viewModel.AdicionarAluno(novoAluno);
            }
            
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e) {

            _viewModel.FiltrarAlunos();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e) {

            var selecionado = dataGridAlunos.SelectedItem as Aluno;

            if (selecionado == null) {
                MessageBox.Show("Selecione um aluno para editar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            if(dataGridAlunos.SelectedItems.Count > 1) {
                MessageBox.Show("Selecione no máximo um aluno para editar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Passa o aluno selecionado para o ViewModel de edição
            var editarVM = new EditarAlunoViewModel(selecionado);
            var editarWindow = new EditarAluno(editarVM);

            if (editarWindow.ShowDialog() == true) {
                // Os dados já foram copiados para o objeto original na janela de edição
                dataGridAlunos.Items.Refresh();
                _viewModel.Guardar();
            }

        }

        private void btnRemover_Click(object sender, RoutedEventArgs e) {

            var selecionados = dataGridAlunos.SelectedItems.Cast<Aluno>().ToList();
            _viewModel.RemoverAlunosSelecionados(selecionados);
        }
           

        private void tbPesquisar_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.FiltrarAlunos();
        }


        private void btnImportarCSV_Click(object sender, RoutedEventArgs e) {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV (*.csv;)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                _viewModel.CarregarDeCSV(openFileDialog.FileName);
            }
        }
    }
}

