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

namespace ProjetoLPDS.Views {
    /// <summary>
    /// Lógica interna para AdicionarMembro.xaml
    /// </summary>
    public partial class AdicionarMembro : Window {

        private readonly AlunosViewModel _alunosViewModel;
        private readonly AlterarMembroViewModel _adicionarMembroViewModel;
        public AdicionarMembro(AlterarMembroViewModel _viewModel) {
            InitializeComponent();
            _alunosViewModel = App.AlunosViewModel;
            this.DataContext = _alunosViewModel;
            _adicionarMembroViewModel = _viewModel;


        }

        

        private void btnAdicionar_Click(object sender, RoutedEventArgs e) {

            var selecionados = dataGridAlunos.SelectedItems.Cast<Aluno>().ToList();

            if (selecionados.Count < 1 || selecionados == null) 
                return;
            
            _adicionarMembroViewModel.AdicionarAlunosAoGrupo(selecionados);
            
        }

        private void btnSair_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
            this.Close();
        }
    }
}
