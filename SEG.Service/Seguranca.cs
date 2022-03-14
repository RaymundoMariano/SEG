using SEG.Domain.Models.Aplicacao;
using System.Collections.Generic;
using System.Linq;

namespace SEG.Service
{
    public class Seguranca
    {
        public UsuarioModel Usuario { get; private set; }
        public ModuloModel Modulo { get; private set; }
        public PerfilModel Perfil { get; private set; }
        private List<FormularioModel> Formularios { get; set; } = new List<FormularioModel>();
        private string Mensagem { get; set; }

        #region Construtor
        public Seguranca(UsuarioModel usuario, ModuloModel modulo, PerfilModel perfil) 
        {
            Usuario = usuario;
            Modulo = modulo;
            Perfil = perfil;
            
            if (Usuario == null)
            {
                Mensagem = $"{ Usuario.Nome }! Voçê não está cadastrado no segurança";
                return;
            }

            if (Modulo == null)
            {
                Mensagem = $"{ Usuario.Nome }! O modulo { Modulo.Nome } não está cadastrado no segurança";
                return;
            }

            foreach (var mf in Modulo.ModulosFormulario)
            {
                Formularios.Add(mf.Formulario);
            }

            var pu = Usuario.PerfisUsuario.FirstOrDefault(pu =>
                pu.UsuarioId == Usuario.UsuarioId && pu.ModuloId == Modulo.ModuloId);
            if (pu == null || pu.Perfil.Nome == "Sem Perfil")
            {
                Mensagem = $"{ Usuario.Nome }! Você não tem perfil definido para usar este modulo { Modulo.Nome }";
            }
        }
        #endregion

        #region TemPermissao
        /// <summary>
        /// Verifica permissões de perfil e usuário
        /// </summary>
        /// <returns></returns>
        public string TemPermissao()
        {
            if (Mensagem != null && Mensagem != string.Empty) return Mensagem;
            if (TemPermissaoPerfil() != null) return Mensagem;
            if (TemPermissaoUsuario() != null) return Mensagem;
            return null;
        }

        /// <summary>
        /// Verifica permissões de formulário
        /// </summary>
        /// <param name="formularioNome"></param>
        /// <returns></returns>
        public string TemPermissao(string formularioNome)
        {
            var mf = Modulo.ModulosFormulario.FirstOrDefault(mf =>
                mf.ModuloId == Modulo.ModuloId && mf.Formulario.Nome == formularioNome);
            if (mf == null)
            {
                Mensagem = $"{ Usuario.Nome }! não foi encontrado o formulário { formularioNome } para este modulo { Modulo.Nome }";
                return Mensagem;
            }

            //Mensagem = null;
            if (TemPermissao() != null) return Mensagem;
            if (TemPermissaoPerfilFormulario(formularioNome) != null) return Mensagem;
            if (TemPermissaoUsuarioFormulario(formularioNome) != null) return Mensagem;
            return null;
        }

        /// <summary>
        /// Verifica permissões de evento
        /// </summary>
        /// <param name="formularioNome"></param>
        /// <param name="eventoNome"></param>
        /// <returns></returns>
        public string TemPermissao(string formularioNome, string eventoNome)
        {
            if (TemPermissao(formularioNome) != null) return Mensagem;

            var mf = Modulo.ModulosFormulario.FirstOrDefault(mf =>
                mf.ModuloId == Modulo.ModuloId && mf.Formulario.Nome == formularioNome);

            bool existeEvento = false;
            foreach (var fe in mf.Formulario.FormulariosEvento)
            {
                if (fe.Evento.Nome == eventoNome)
                {
                    existeEvento = true;
                    break;
                }
            }

            if (!existeEvento)
            {
                return $"{ Usuario.Nome }! não foi encontrado o evento { eventoNome } para este formulário { formularioNome }";
            }

            if (TemPermissaoPerfilEvento(formularioNome, eventoNome) != null) return Mensagem;
            if (TemPermissaoUsuarioEvento(formularioNome, eventoNome) != null) return Mensagem;
            return null;
        }
        #endregion

        #region TemPermissaoPerfil
        private string TemPermissaoPerfil()
        {
            var rp = Perfil.RestricoesPerfil.FirstOrDefault(rp =>
                rp.PerfilId == Perfil.PerfilId &&
                rp.ModuloId == Modulo.ModuloId &&
                rp.FormularioId == null &&
                rp.EventoId == null);
            
            if (rp == null) return null;

            return $"{ Usuario.Nome }! O seu perfil de { Perfil.Nome } não tem permissão de acesso a este modulo { Modulo.Nome }";
        }
        #endregion

        #region TemPermissaoUsuario
        private string TemPermissaoUsuario()
        {
            var ru = Usuario.RestricoesUsuario.FirstOrDefault(ru =>
                ru.UsuarioId == Usuario.UsuarioId &&
                ru.ModuloId == Modulo.ModuloId &&
                ru.FormularioId == null &&
                ru.EventoId == null);
            
            if (ru == null) return null;

            return $"{ Usuario.Nome }! Voçê não tem permissão de acesso a este modulo { Modulo.Nome }";
        }
        #endregion

        #region TemPermissaoPerfilFormulario
        private string TemPermissaoPerfilFormulario(string formularioNome)
        {
            var rp = Perfil.RestricoesPerfil.FirstOrDefault(rp =>
                rp.PerfilId == Perfil.PerfilId &&
                rp.ModuloId == Modulo.ModuloId &&
                rp.FormularioId == Formularios.Find(f => f.Nome == formularioNome).FormularioId &&
                rp.EventoId == null);
            
            if (rp == null) return null;

            return $"{ Usuario.Nome }! O seu perfil de { Perfil.Nome } não tem permissão de acesso a este formulário { formularioNome }";
        }
        #endregion

        #region TemPermissaoUsuarioFormulario
        private string TemPermissaoUsuarioFormulario(string formularioNome)
        {
            var ru = Usuario.RestricoesUsuario.FirstOrDefault(ru =>
                ru.UsuarioId == Usuario.UsuarioId &&
                ru.ModuloId == Modulo.ModuloId &&
                ru.FormularioId == Formularios.Find(f => f.Nome == formularioNome).FormularioId &&
                ru.EventoId == null);

            if (ru == null) return null;

            return $"{ Usuario.Nome }! Voçê não tem permissão de acesso a este formulário { formularioNome }";
        }
        #endregion

        #region TemPermissaoPerfilEvento
        private string TemPermissaoPerfilEvento(string formularioNome, string eventoNome)
        {
            var eventos = ObterEventos(formularioNome);

            var rp = Perfil.RestricoesPerfil.FirstOrDefault(rp =>
                rp.PerfilId == Perfil.PerfilId &&
                rp.ModuloId == Modulo.ModuloId &&
                rp.FormularioId == Formularios.Find(f => f.Nome == formularioNome).FormularioId &&
                rp.EventoId == eventos.Find(e => e.Nome == eventoNome).EventoId);
            
            if (rp == null) return null;

            return $"{ Usuario.Nome }! O seu perfil de { Perfil.Nome } não tem permissão de acesso a este evento { eventoNome }";
        }
        #endregion

        #region TemPermissaoUsuarioEvento
        private string TemPermissaoUsuarioEvento(string formularioNome, string eventoNome)
        {
            var eventos = ObterEventos(formularioNome);

            var ru = Usuario.RestricoesUsuario.FirstOrDefault(ru =>
                ru.UsuarioId == Usuario.UsuarioId &&
                ru.ModuloId == Modulo.ModuloId &&
                ru.FormularioId == Formularios.Find(f => f.Nome == formularioNome).FormularioId &&
                ru.EventoId == eventos.Find(e => e.Nome == eventoNome).EventoId);
            
            if (ru == null) return null;

            return $"{ Usuario }! Voçê não tem permissão de acesso a este evento { eventoNome }";
        }
        #endregion

        #region ObterEventos
        private List<EventoModel> ObterEventos(string formularioNome)
        {
            var eventos = new List<EventoModel>();

            var mf = Modulo.ModulosFormulario.FirstOrDefault(mf =>
                mf.ModuloId == Modulo.ModuloId &&
                mf.FormularioId == Formularios.Find(f => f.Nome == formularioNome).FormularioId);
            foreach (var fe in mf.Formulario.FormulariosEvento)
            {
                eventos.Add(fe.Evento);
            }
            return eventos;
        }
        #endregion
    }
}
