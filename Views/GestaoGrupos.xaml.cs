using ProjetoLPDS.Models;
using ProjetoLPDS.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
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
    /// Lógica interna para GestaoGrupos.xaml
    /// </summary>
    public partial class GestaoGrupos : Window
    {
        private readonly GruposViewModel _viewModel;

        public GestaoGrupos()
        {
            InitializeComponent();
            _viewModel = App.GruposViewModel;
            this.DataContext = _viewModel;

        }

        

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
            App.AlunosViewModel.Guardar();
            _viewModel.Guardar();
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e) {

            var vm = new EditarGrupoViewModel();
            var janela = new AdicionarGrupo(vm);

            if(janela.ShowDialog() == true) {

                var novoGrupo = vm.GrupoEditavel;
                _viewModel.AdicionarGrupo(novoGrupo);

                // Atualizar grupos no ClassificacoesViewModel
                App.ClassificacoesViewModel.Grupos = new ObservableCollection<Grupo>(_viewModel.Grupos);
            }
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e) {

            var selecionado = dataGridGrupos.SelectedItem as Grupo;

            if(selecionado == null) {
                MessageBox.Show("Selecione um grupo para editar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if(dataGridGrupos.SelectedItems.Count > 1) {
                MessageBox.Show("Selecione no máximo um grupo para editar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var editarVM = new EditarGrupoViewModel(selecionado);
            var editarWindow = new EditarGrupo(editarVM);

            if (editarWindow.ShowDialog() == true) {
                // Os dados já foram copiados para o objeto original na janela de edição
                dataGridGrupos.Items.Refresh();
                _viewModel.Guardar();

                // Atualizar grupos no ClassificacoesViewModel
                App.ClassificacoesViewModel.Grupos = new ObservableCollection<Grupo>(_viewModel.Grupos);
            }
        }

        

        private void btnRemover_Click(object sender, RoutedEventArgs e) {

            var selecionados = dataGridGrupos.SelectedItems.Cast<Grupo>().ToList();

            if(selecionados.Count == 0) {

                MessageBox.Show("Selecione pelo menos um grupo para remover.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var resultado = MessageBox.Show($"Tem a certeza que pretende remover {selecionados.Count} grupo(s)?", "Confirmar remoção",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes) {
                foreach (var grupo in selecionados) {
                    // Desvincular alunos globalmente
                    foreach (var aluno in App.AlunosViewModel.Alunos) {
                        if (aluno.Grupo == grupo.Nome) {
                            aluno.Grupo = null;
                        }
                    }

                    // Remover o grupo
                    _viewModel.Grupos.Remove(grupo);
                    _viewModel.TodosGrupos.Remove(grupo);
                }

                // Atualizar UI e gravar
                App.AlunosViewModel.NotifyAll();
                App.AlunosViewModel.Guardar();
                _viewModel.Guardar();

                // Atualizar grupos no ClassificacoesViewModel
                App.ClassificacoesViewModel.Grupos = new ObservableCollection<Grupo>(_viewModel.Grupos);
                App.ClassificacoesViewModel.OnPropertyChanged(nameof(App.ClassificacoesViewModel.Grupos));




            }
        }

        

        private void btnAdicionarMembro_Click(object sender, RoutedEventArgs e) {

            var selecionado = dataGridGrupos.SelectedItem as Grupo;

            if (selecionado == null) {
                return;
            }
            

            var adicionarVM = new AlterarMembroViewModel(selecionado);
            var adicionarWindow = new AdicionarMembro(adicionarVM);

            if(adicionarWindow.ShowDialog() == true) {

                // Notificar Interface
                dataGridGrupos.Items.Refresh();
                App.AlunosViewModel.NotifyAll();
                App.AlunosViewModel.Guardar();

                _viewModel.Guardar();

                
                // Atualizar grupos no ClassificacoesViewModel
                App.ClassificacoesViewModel.Grupos = new ObservableCollection<Grupo>(_viewModel.Grupos);
                App.ClassificacoesViewModel.OnPropertyChanged(nameof(App.ClassificacoesViewModel.Grupos));
                App.ClassificacoesViewModel.AtualizarGrupo();


            }




        }

        private void btnRemoverAluno_Click(object sender, RoutedEventArgs e) {

            var botao = sender as Button;
            var alunoSelecionado = botao?.Tag as Aluno;

            if (alunoSelecionado == null)
                return;

            // Obter o grupo ao qual o aluno pertence
            var grupoDoAluno = _viewModel.Grupos.FirstOrDefault(g => g.Nome == alunoSelecionado.Grupo);
            if (grupoDoAluno == null)
                return;

            var removerVM = new AlterarMembroViewModel(grupoDoAluno);
            removerVM.RemoverAlunoAoGrupo(alunoSelecionado);

            dataGridGrupos.Items.Refresh();
            App.AlunosViewModel.NotifyAll(); 
            App.AlunosViewModel.Guardar();
            _viewModel.Guardar();

            // Atualizar grupos no ClassificacoesViewModel
            App.ClassificacoesViewModel.Grupos = new ObservableCollection<Grupo>(_viewModel.Grupos);
            App.ClassificacoesViewModel.OnPropertyChanged(nameof(App.ClassificacoesViewModel.Grupos));
            App.ClassificacoesViewModel.AtualizarGrupo();





        }

        private void btnAssociar_Click(object sender, RoutedEventArgs e) {

            var grupoSelecionado = dataGridGrupos.SelectedItem as Grupo;

            if (grupoSelecionado == null) {

                MessageBox.Show("Selecione um grupo para gerir as suas tarefas associadas.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (dataGridGrupos.SelectedItems.Count > 1) {

                MessageBox.Show("Selecione no máximo um grupo para gerir as suas tarefas associadas.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var associarWindow = new AssociarGrupoTarefa(grupoSelecionado);

            if (associarWindow.ShowDialog() == true)

                _viewModel.Guardar();
                return;
        }

        private void tbPesquisar_TextChanged(object sender, TextChangedEventArgs e) {
            _viewModel.FiltroNome = tbPesquisar.Text;
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e) {
            _viewModel.FiltroNome = tbPesquisar.Text;
        }

    }
}
