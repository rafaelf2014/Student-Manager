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
using ProjetoLPDS.ViewModels;

namespace ProjetoLPDS.Views
{
    /// <summary>
    /// Lógica interna para Grafico.xaml
    /// </summary>
    public partial class Grafico : Window {
        public Grafico() {
            InitializeComponent();
            DataContext = new HistogramaViewModel();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            Pautas pautas = new Pautas();
            pautas.Show();
            this.Close();
        }
    }

    
    
}
