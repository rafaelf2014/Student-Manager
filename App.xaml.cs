using ProjetoLPDS.ViewModels;
using ProjetoLPDS.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ProjetoLPDS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e) {
            var mainWindow = new MainWindow();

            mainWindow.Show();
        }

        public static PerfilViewModel PerfilViewModel { get; set; }
        public static AlunosViewModel AlunosViewModel { get; set; }
        public static TarefasViewModel TarefasViewModel { get; set; }

        public static GruposViewModel GruposViewModel { get; set; }

        public static ClassificacoesViewModel ClassificacoesViewModel { get; set; } 

        public App() {

            

            PerfilViewModel = new PerfilViewModel(); // Todas as janelas acessam esta instância de PerfilViewModel
            AlunosViewModel = new AlunosViewModel(); // Todas as janelas acessam esta instância de AlunosViewModel
            TarefasViewModel = new TarefasViewModel(); //Todas as janelas acessam a instância de TarefasViewModel
            GruposViewModel = new GruposViewModel();
            ClassificacoesViewModel = new ClassificacoesViewModel();
        }
    }

}
