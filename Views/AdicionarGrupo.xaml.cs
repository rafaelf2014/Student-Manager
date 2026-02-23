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
    /// Lógica interna para AdicionarGrupo.xaml
    /// </summary>
    public partial class AdicionarGrupo : Window
    {
        private readonly EditarGrupoViewModel _viewModel;
        public AdicionarGrupo(EditarGrupoViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e) {
            var novoGrupo = _viewModel.GrupoEditavel;

            // Validação
            if (App.GruposViewModel.Grupos.Any(g => g.Id == novoGrupo.Id)) {
                MessageBox.Show("Já existe um grupo com esse ID.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(novoGrupo.Id, out int n))
            {
                MessageBox.Show("Id tem de ser numero inteiro.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(novoGrupo.Nome)) {
                MessageBox.Show("Nome é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

    }
}
