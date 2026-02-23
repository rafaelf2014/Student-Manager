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
    /// Lógica interna para AdicionarAluno.xaml
    /// </summary>
    public partial class AdicionarAluno : Window
    {
        private readonly EditarAlunoViewModel _viewModel;

        public AdicionarAluno(EditarAlunoViewModel viewModel) {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e) {
            var novoAluno = _viewModel.AlunoEditavel;

            // Validação de unicidade do Numero
            if (App.AlunosViewModel.Alunos.Any(a => a.Numero == novoAluno.Numero)) {
                MessageBox.Show("Já existe um aluno com este número.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Numero obrigatório, diferente de 0, só dígitos e tamanho máximo
            if (string.IsNullOrWhiteSpace(novoAluno.Numero) || novoAluno.Numero == "0") {
                MessageBox.Show("Numero é obrigatório e precisa ser diferente de 0.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!novoAluno.Numero.All(char.IsDigit)) {
                MessageBox.Show("O número do aluno deve conter apenas dígitos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (novoAluno.Numero.Length > 10) {
                MessageBox.Show("O número do aluno deve ter no máximo 10 dígitos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Nome obrigatório, sem números, tamanho mínimo e máximo
            if (string.IsNullOrWhiteSpace(novoAluno.Nome)) {
                MessageBox.Show("Nome é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (novoAluno.Nome.Any(char.IsDigit)) {
                MessageBox.Show("O nome não pode conter números.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (novoAluno.Nome.Length < 3 || novoAluno.Nome.Length > 100) {
                MessageBox.Show("O nome deve ter entre 3 e 100 caracteres.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Email obrigatório, formato básico, tamanho máximo
            if (string.IsNullOrWhiteSpace(novoAluno.Email)) {
                MessageBox.Show("Email é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!novoAluno.Email.Contains("@") || !novoAluno.Email.Contains(".")) {
                MessageBox.Show("Formato de email incorreto.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (novoAluno.Email.Length > 100) {
                MessageBox.Show("O email deve ter no máximo 100 caracteres.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (novoAluno.Email.Any(c => char.IsWhiteSpace(c))) {
                MessageBox.Show("O email não pode conter espaços.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
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
