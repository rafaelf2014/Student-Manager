using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Net.Http.Headers;

namespace ProjetoLPDS.Models
{
    public class Perfil
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get;  set; } = string.Empty;
        public string CaminhoFotografia { get;  set; } = string.Empty;

        public Perfil() {}

        public bool EditarPerfil(string nome, string email, string caminhoFotografia) {

            if (string.IsNullOrWhiteSpace(nome)) {
                return false; 
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains(".")) {
                return false; 
            }
            
            // Se for válido, então atualiza
            Nome = nome;
            Email = email;

            if (!string.IsNullOrWhiteSpace(caminhoFotografia))
                CaminhoFotografia = caminhoFotografia;

            return true;
        }


        
    }
}