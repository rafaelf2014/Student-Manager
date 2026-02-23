using System;
using ProjetoLPDS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ProjetoLPDS.ViewModels {
    public class EditarGrupoViewModel {

        public Grupo GrupoEditavel {  get; set; }
        public Grupo GrupoOriginal { get; }

        public EditarGrupoViewModel() {
            GrupoEditavel = new Grupo(); // novo grupo vazio
        }

        public EditarGrupoViewModel(Grupo grupo) {

            GrupoEditavel = new Grupo(grupo.Id, grupo.Nome);
            GrupoOriginal = grupo;
        }

       
    }
}
