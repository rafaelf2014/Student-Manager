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
    /// Lógica interna para AdicionarTarefa.xaml
    /// </summary>
    public partial class AdicionarTarefa : Window
    {
        private readonly EditarTarefaViewModel _viewModel;

        public AdicionarTarefa(EditarTarefaViewModel viewModel) {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            var novaTarefa = _viewModel.TarefaEditavel;

            // Validação
            if (App.TarefasViewModel.TodasTarefas.Any(t => t.Id == novaTarefa.Id)) {
                MessageBox.Show("Já existe uma tarefa com esse ID.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (novaTarefa.DataInicio > novaTarefa.DataTermino) {

                MessageBox.Show("As datas introduzidas são inválidas. Data de inicio precisa ser anterior a data de término", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if(novaTarefa.Peso < 1 || novaTarefa.Peso > 100) {

                MessageBox.Show("O peso de cada tarefa deve estar compreedido entre 1 e 100", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            double somaPesos = App.TarefasViewModel.TodasTarefas.Sum(t => t.Peso) + novaTarefa.Peso;
            if (somaPesos > 100) {
                MessageBox.Show($"A soma dos pesos das tarefas não pode ultrapassar 100. Soma atual com esta tarefa: {somaPesos}", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(novaTarefa.Id, out int n))
            {
                MessageBox.Show("Id tem que ser um numero inteiro.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
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
