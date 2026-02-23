using ProjetoLPDS.ViewModels;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjetoLPDS.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PerfilViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = App.PerfilViewModel;
            this.DataContext = _viewModel;

            AtualizarNomePerfil();
        }

        private void ViewModel_PerfilGuardado(object sender, EventArgs e)
        {
            AtualizarNomePerfil();
        }

        private void AtualizarNomePerfil()
        {
            string fullName = _viewModel.Nome;
            string firstName = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
            tbPerfil.Text = _viewModel.Nome;
            tbPerfil.Width = 140;
            tbPerfil.HorizontalAlignment = HorizontalAlignment.Left;
            tbPerfil.TextAlignment = TextAlignment.Right;
            
        }

        private void GestaoAlunosBtn_Click(object sender, RoutedEventArgs e)
        {
            GestaoAlunos gestaoAlunos = new GestaoAlunos();
            gestaoAlunos.Show();
            this.Close();
        }

        private void PautasBtn_Click(object sender, RoutedEventArgs e)
        {
            Pautas pautas = new Pautas();
            pautas.Show();
            this.Close();
        }

        private void GestaoTarefasBtn_Click(object sender, RoutedEventArgs e)
        {
            GestaoTarefas gestaoTarefas = new GestaoTarefas();
            gestaoTarefas.Show();
            this.Close();

        }

        private void GestaoGruposBtn_Click(object sender, RoutedEventArgs e)
        {
            GestaoGrupos gestaoGrupos = new GestaoGrupos();
            gestaoGrupos.Show();
            this.Close();
        }

        private void GestaoClassificacoesBtn_Click(object sender, RoutedEventArgs e) {

            Notas notas = new Notas();
            notas.Show();
            this.Close();
        }

        private void PerfilBtn_Click(object sender, RoutedEventArgs e)
        {
            PerfilUtilizador perfil = new PerfilUtilizador();
            perfil.Show();
            this.Close();
        }

        
    }
}