using ProjetoLPDS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLPDS.ViewModels
{
    public delegate void PerfilGuardadoEventHandler(object sender, EventArgs e);
    public class PerfilViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService = new();
        private Perfil _perfil;

        public string Nome {
            get => _perfil.Nome;
            set {
                if(_perfil.Nome != value) {

                    _perfil.Nome = value;
                    OnPropertyChanged(nameof(Nome));
                }
            }
        }

        public string Email {
            get => _perfil.Email;
            set {
                if(_perfil.Email != value) {

                    _perfil.Email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string CaminhoFotografia {
            get => _perfil.CaminhoFotografia;
            set {
                if (_perfil.CaminhoFotografia != value) {
                    
                    _perfil.CaminhoFotografia = value;
                    OnPropertyChanged(nameof(CaminhoFotografia));
                }
            }
        }

        public event PerfilGuardadoEventHandler PerfilGuardado;
        public event PropertyChangedEventHandler PropertyChanged;

        public PerfilViewModel() {
            _perfil = _dataService.CarregarPerfil();
        }

        public bool ValidarDados() {
            return _perfil.EditarPerfil(_perfil.Nome, _perfil.Email, _perfil.CaminhoFotografia);
        }

        public bool Guardar() {

            if (!ValidarDados()) {
                return false; 
            }
            _dataService.GuardarPerfil(_perfil);
            PerfilGuardado?.Invoke(this, EventArgs.Empty);
            
            return true;
        }

        public void RecarregarDados() {
            _perfil = _dataService.CarregarPerfil();

            OnPropertyChanged(nameof(Nome));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(CaminhoFotografia));

        }

        protected void OnPropertyChanged(string nome) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
    }
}
