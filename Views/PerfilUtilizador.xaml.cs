using Microsoft.Win32;
using ProjetoLPDS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica interna para PerfilUtilizador.xaml
    /// </summary>
    public partial class PerfilUtilizador : Window
    {
        private readonly PerfilViewModel _viewModel;
        public PerfilUtilizador()
        {
            InitializeComponent();
            _viewModel = App.PerfilViewModel;
            this.DataContext = _viewModel;

            LoadProfileImage();


            this.Closing += PerfilUtilizador_Closing;
        }

        private void PerfilUtilizador_Closing(object sender, System.ComponentModel.CancelEventArgs e) {

            Application.Current.Shutdown();

        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void GravarButton_Click(object sender, RoutedEventArgs e) {

            if (_viewModel.Guardar()) {
                MessageBox.Show("Perfil guardado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);

                
            }
            else {
                MessageBox.Show("Erro ao guardar perfil. Verifique se o nome não está vazio e se o email é válido.",
                               "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadProfileImage() {

            if(File.Exists(_viewModel.CaminhoFotografia)) {

                var image = new BitmapImage(new Uri(_viewModel.CaminhoFotografia, UriKind.RelativeOrAbsolute));
                UserPhoto.Source = image;
            }
        }

        private void AlterarFotoButton_Click(object sender, RoutedEventArgs e) {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imagens (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

            if(openFileDialog.ShowDialog() == true) {

                _viewModel.CaminhoFotografia = openFileDialog.FileName;
                LoadProfileImage();
            }
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e) {

            _viewModel.RecarregarDados();

            LoadProfileImage();
        }
    }
}
