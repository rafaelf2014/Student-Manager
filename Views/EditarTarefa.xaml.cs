using ProjetoLPDS.ViewModels;
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

namespace ProjetoLPDS.Views
{
    /// <summary>
    /// Lógica interna para EditarTarefa.xaml
    /// </summary>
    public partial class EditarTarefa : Window
    {
        private readonly EditarTarefaViewModel _viewModel;
        public EditarTarefa(EditarTarefaViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e) {
            var editado = _viewModel.TarefaEditavel;

            // Validação de unicidade do Id (exceto a própria tarefa)
            if (App.TarefasViewModel.Tarefas.Any(t => t.Id == editado.Id && t != _viewModel.TarefaOriginal)) {
                MessageBox.Show("Já existe uma tarefa com este ID.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Id obrigatório
            if (string.IsNullOrWhiteSpace(editado.Id)) {
                MessageBox.Show("O ID da tarefa é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (editado.Id.Length > 20) {
                MessageBox.Show("O ID da tarefa deve ter no máximo 20 caracteres.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Título obrigatório
            if (string.IsNullOrWhiteSpace(editado.Titulo)) {
                MessageBox.Show("O título da tarefa é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (editado.Titulo.Length < 3 || editado.Titulo.Length > 100) {
                MessageBox.Show("O título deve ter entre 3 e 100 caracteres.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Datas válidas
            if (editado.DataInicio > editado.DataTermino) {
                MessageBox.Show("As datas introduzidas são inválidas. Data de início precisa ser anterior à data de término.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Peso entre 1 e 100
            if (editado.Peso < 1 || editado.Peso > 100) {
                MessageBox.Show("O peso de cada tarefa deve estar compreendido entre 1 e 100.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Copia de valores para o original
            _viewModel.TarefaOriginal.Id = editado.Id;
            _viewModel.TarefaOriginal.Titulo = editado.Titulo;
            _viewModel.TarefaOriginal.Descricao = editado.Descricao;
            _viewModel.TarefaOriginal.DataInicio = editado.DataInicio;
            _viewModel.TarefaOriginal.DataTermino = editado.DataTermino;
            _viewModel.TarefaOriginal.Peso = editado.Peso;

            this.DialogResult = true;
            this.Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
