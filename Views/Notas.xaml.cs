using ProjetoLPDS.Models;
using ProjetoLPDS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjetoLPDS.Views {
    /// <summary>
    /// Lógica interna para Notas.xaml
    /// </summary>
    public partial class Notas : Window {
        private readonly ClassificacoesViewModel _viewModel;
        public Notas() {

            InitializeComponent();
            _viewModel = App.ClassificacoesViewModel;
            this.DataContext = _viewModel;
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }



        private void btnPublicar_Click(object sender, RoutedEventArgs e) {

            _viewModel.GuardarNotas();
            _viewModel.CalcularNotasFinais();
            MessageBox.Show("Notas guardadas com sucesso!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);


        }

        private void tbNotaGrupo_KeyDown(object sender, KeyEventArgs e) {

            if (e.Key == Key.Enter) {
                if (sender is TextBox textBox) {
                    if (double.TryParse(textBox.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double grade)) {
                        
                        if (grade >= 0 && grade <= 20) {
                            AplicarNotaGrupo(grade);
                            tbNotaGrupo.Clear();
                        }
                        else {
                            MessageBox.Show("Insira uma nota entre 0 e 20 valores.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else {
                        MessageBox.Show("Insira uma nota válida (número).", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }

        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e) {

            if (double.TryParse(tbNotaGrupo.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double grade)) {

                if (grade >= 0 && grade <= 20) {
                    AplicarNotaGrupo(grade);
                    tbNotaGrupo.Clear();
                } else {
                    MessageBox.Show("Insira uma nota entre 0 e 20 valores.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else {
                MessageBox.Show("Insira uma nota válida (número).", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void AplicarNotaGrupo(double grade) {

            foreach (var aluno in _viewModel.GrupoSelecionado.Alunos) {
                if (aluno.Classificacoes == null)
                    aluno.Classificacoes = new List<Classificacao>();

                var classificacao = aluno.Classificacoes
                    .FirstOrDefault(c => c.Tarefa != null && c.Tarefa.Id == _viewModel.TarefaSelecionada.Id);

                if (classificacao != null) {
                    classificacao.Nota = grade;
                }
                else {
                    aluno.Classificacoes.Add(new Classificacao {
                        NumeroAluno = aluno.Numero,
                        Tarefa = _viewModel.TarefaSelecionada,
                        Nota = grade
                    });
                }
            }

            _viewModel.AtualizarGrupo();
            dataGridAlunos.Items.Refresh();
        }

    }
}