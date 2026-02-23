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
    /// Lógica interna para EditarGrupo.xaml
    /// </summary>
    public partial class EditarGrupo : Window
    {
        private readonly EditarGrupoViewModel _viewModel;
        public EditarGrupo(EditarGrupoViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e) {
            var editado = _viewModel.GrupoEditavel;

            // Validação de unicidade do Id (exceto o próprio grupo)
            if (App.GruposViewModel.Grupos.Any(g => g.Id == editado.Id && g != _viewModel.GrupoOriginal)) {
                MessageBox.Show("Já existe um grupo com este ID.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Id obrigatório e tamanho máximo
            if (string.IsNullOrWhiteSpace(editado.Id)) {
                MessageBox.Show("Id é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (editado.Id.Length > 20) {
                MessageBox.Show("O Id do grupo deve ter no máximo 20 caracteres.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Nome obrigatório, tamanho mínimo e máximo
            if (string.IsNullOrWhiteSpace(editado.Nome)) {
                MessageBox.Show("Nome é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (editado.Nome.Length < 3 || editado.Nome.Length > 100) {
                MessageBox.Show("O nome do grupo deve ter entre 3 e 100 caracteres.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Copia valores para o original
            _viewModel.GrupoOriginal.Id = editado.Id;
            _viewModel.GrupoOriginal.Nome = editado.Nome;

            this.DialogResult = true;
            this.Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
