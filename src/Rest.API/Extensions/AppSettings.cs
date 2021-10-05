using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest.API.Extensions
{
    public class AppSettings
    {
        /// <summary>
        /// chave de criptografia do token
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Tempo válido em horas do token
        /// </summary>
        public int ExpiracaoHoras { get; set; }

        /// <summary>
        /// Emissor do token
        /// </summary>
        public string Emissor { get; set; }

        /// <summary>
        /// Indica em quais urls o token é valido 
        /// </summary>
        public string ValidoEm { get; set; }
    }
}
