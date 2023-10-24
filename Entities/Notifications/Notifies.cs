using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Notifications
{
    public class Notifies
    {
        public Notifies()
        {
            Notifications = new List<Notifies>();
        }

        [NotMapped]
        public string NomePropriedade { get; set; }

        [NotMapped]
        public string mensagem { get; set; }

        [NotMapped]
        public List<Notifies> Notifications { get; set; }

        public bool ValidaDescricao(string valor, string nomePropriedade, int numeroCaracteresPermitido)
        {
            if (string.IsNullOrWhiteSpace(valor) || string.IsNullOrWhiteSpace(nomePropriedade))
            {
                Notifications.Add(new Notifies
                {
                    mensagem = "Campo obrigatório.",
                    NomePropriedade = nomePropriedade
                });
                return false;
            }
            else if (valor.Length > numeroCaracteresPermitido)
            {
                Notifications.Add(new Notifies()
                {
                    mensagem = "Nome do genero musical deve conter no máximo 20 caracteres."
                });
                return false;
            }
            return true;
        }

        public bool ValidaAnoLancamento(int valor, string nomePropriedade, int numeroCaracteresPermitido)
        {
            if (valor < 1 || string.IsNullOrWhiteSpace(nomePropriedade))
            {
                Notifications.Add(new Notifies
                {
                    mensagem = "Campo obrigatório.",
                    NomePropriedade = nomePropriedade
                });
                return false;
            }
            else if (valor.ToString().Length > numeroCaracteresPermitido)
            {
                Notifications.Add(new Notifies()
                {
                    mensagem = "Ano de lançamento deve conter 4 dígitos."
                });
                return false;
            }
            return true;
        }
    }
}
