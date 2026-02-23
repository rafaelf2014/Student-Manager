using ProjetoLPDS.Models;
using ProjetoLPDS.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ProjetoLPDS.Views {
    /// <summary>
    /// Lógica interna para AssociarGrupoTarefa.xaml
    /// </summary>
    public partial class AssociarGrupoTarefa : Window {

        private readonly AssociarGrupoTarefaViewModel _viewModel;

        public AssociarGrupoTarefa(Grupo grupo) {
            InitializeComponent();
            _viewModel = new AssociarGrupoTarefaViewModel(grupo);
            DataContext = _viewModel;
        }
        
        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
            
        }

        private void btnSair_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
            this.Close();
        }
    }
}
