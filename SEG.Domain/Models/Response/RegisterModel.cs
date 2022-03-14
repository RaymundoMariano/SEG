using SEG.Domain.Models.Aplicacao;
using System.Collections.Generic;

namespace SEG.Domain.Models.Response
{
    public class RegisterModel
    {
        public SegurancaModel Seguranca { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool Authenticated { get; set; }
        public int ObjectResult { get; set; }
        public List<string> Errors { get; set; }
    }
}
