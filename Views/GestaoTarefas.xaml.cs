using ProjetoLPDS.Models;
using ProjetoLPDS.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Formats.Tar;
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
    /// Lógica interna para GestaoTarefas.xaml
    /// </summary>
    public partial class GestaoTarefas : Window
    {
        private readonly TarefasViewModel _viewModel;
        public GestaoTarefas()
        {
            InitializeComponent();
            _viewModel = App.TarefasViewModel;
            this.DataContext = _viewModel;

            
        }

        

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
            _viewModel.Guardar();
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            var vm = new EditarTarefaViewModel();
            var janela = new AdicionarTarefa(vm);

            if (janela.ShowDialog() == true) {

                var novaTarefa = vm.TarefaEditavel;
                _viewModel.adicionarTarefa(novaTarefa);

                // Atualizar tarefas no ClassificacoesViewModel
                App.ClassificacoesViewModel.Tarefas = new ObservableCollection<Tarefa>(_viewModel.Tarefas);
                App.ClassificacoesViewModel.OnPropertyChanged(nameof(App.ClassificacoesViewModel.Tarefas));

                // Atualizar tarefas associadas do grupo selecionado (se aplicável)
                App.ClassificacoesViewModel.AtualizarGrupo();
                App.ClassificacoesViewModel.OnPropertyChanged(nameof(App.ClassificacoesViewModel.TarefasAssociadas));

                _viewModel.Guardar();
                App.ClassificacoesViewModel.AtualizarGrupo();
                App.GruposViewModel.Guardar();
            }

        }

        private void btnEditar_Click(object sender, RoutedEventArgs e) {
            var selecionado = dataGridTarefas.SelectedItem as Tarefa;

            if (selecionado == null) {
                MessageBox.Show("Selecione uma tarefa para editar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if (dataGridTarefas.SelectedItems.Count > 1) {
                MessageBox.Show("Selecione no máximo uma tarefa para editar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var editarVM = new EditarTarefaViewModel(selecionado);
            var editarWindow = new EditarTarefa(editarVM);

            if (editarWindow.ShowDialog() == true) {
                // Os dados já foram copiados para o objeto original na janela de edição
                dataGridTarefas.Items.Refresh();
                _viewModel.Guardar();

                App.ClassificacoesViewModel.Tarefas = new ObservableCollection<Tarefa>(_viewModel.Tarefas);
            }
        }



        private void btnRemover_Click(object sender, RoutedEventArgs e)
        {
            var selecionadas = dataGridTarefas.SelectedItems.Cast<Tarefa>().ToList();

            if (selecionadas.Count == 0)
            {
                MessageBox.Show("Selecione pelo menos uma tarefa para remover.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var resultado = MessageBox.Show($"Tem a certeza que pretende remover {selecionadas.Count} tarefa(s)?", "Confirmar remoção",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                foreach (var tarefa in selecionadas)
                {
                    _viewModel.Tarefas.Remove(tarefa);
                    _viewModel.TodasTarefas.Remove(tarefa);

                    // Remover a tarefa de todos os grupos
                    foreach (var grupo in App.GruposViewModel.Grupos)
                    {
                        var tarefaAssoc = grupo.TarefasAssociadas.FirstOrDefault(t => t.Id == tarefa.Id);
                        if (tarefaAssoc != null)
                            grupo.TarefasAssociadas.Remove(tarefaAssoc);
                    }

                    // Remover classificações da tarefa de todos os alunos
                    foreach (var aluno in App.AlunosViewModel.Alunos)
                    {
                        if (aluno.Classificacoes != null)
                        {
                            aluno.Classificacoes.RemoveAll(c => c.Tarefa != null && c.Tarefa.Id == tarefa.Id);
                        }
                    }
                }

                // Guardar alterações nos alunos
                App.AlunosViewModel.Guardar();

                // Atualizar tarefas no ClassificacoesViewModel
                App.ClassificacoesViewModel.Tarefas = new ObservableCollection<Tarefa>(_viewModel.Tarefas);
                App.ClassificacoesViewModel.CalcularNotasFinais();
            }
        }



        private void tbPesquisar_TextChanged(object sender, TextChangedEventArgs e) {
            _viewModel.FiltraTarefasPorTitulo(tbPesquisar.Text);
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e) {
            _viewModel.FiltraTarefasPorTitulo(tbPesquisar.Text);
        }
    }
}
