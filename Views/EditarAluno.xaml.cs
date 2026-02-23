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

namespace ProjetoLPDS.Views
{
    /// <summary>
    /// Lógica interna para EditarAluno.xaml
    /// </summary>
    public partial class EditarAluno : Window
    {

        private readonly EditarAlunoViewModel _viewModel;

        public EditarAluno(EditarAlunoViewModel viewModel) {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e) {
            var editado = _viewModel.AlunoEditavel;

            // Verificação de unicidade do Numero (exceto o próprio aluno)
            if (App.AlunosViewModel.Alunos.Any(a => a.Numero == editado.Numero && a != _viewModel.AlunoOriginal)) {
                MessageBox.Show("Já existe um aluno com este número.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Numero obrigatório, diferente de 0, só dígitos e tamanho máximo
            if (string.IsNullOrWhiteSpace(editado.Numero) || editado.Numero == "0") {
                MessageBox.Show("Numero é obrigatório e precisa ser diferente de 0.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!editado.Numero.All(char.IsDigit)) {
                MessageBox.Show("O número do aluno deve conter apenas dígitos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (editado.Numero.Length > 10) {
                MessageBox.Show("O número do aluno deve ter no máximo 10 dígitos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Nome obrigatório, sem números, tamanho mínimo e máximo
            if (string.IsNullOrWhiteSpace(editado.Nome)) {
                MessageBox.Show("Nome é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (editado.Nome.Any(char.IsDigit)) {
                MessageBox.Show("O nome não pode conter números.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (editado.Nome.Length < 3 || editado.Nome.Length > 100) {
                MessageBox.Show("O nome deve ter entre 3 e 100 caracteres.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Email obrigatório, formato básico, tamanho máximo
            if (string.IsNullOrWhiteSpace(editado.Email)) {
                MessageBox.Show("Email é obrigatório.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!editado.Email.Contains("@") || !editado.Email.Contains(".")) {
                MessageBox.Show("Formato de email incorreto.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (editado.Email.Length > 100) {
                MessageBox.Show("O email deve ter no máximo 100 caracteres.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (editado.Email.Any(c => char.IsWhiteSpace(c))) {
                MessageBox.Show("O email não pode conter espaços.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Copia valores para o original
            _viewModel.AlunoOriginal.Nome = editado.Nome;
            _viewModel.AlunoOriginal.Email = editado.Email;
            _viewModel.AlunoOriginal.Numero = editado.Numero;

            this.DialogResult = true;
            this.Close();
        }


        private void btnCancelar_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

    }
}
