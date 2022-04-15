using SEG.Domain.Contracts.Clients.Aplicacao;
using SEG.Domain.Contracts.Clients.Autenticacao;
using SEG.Domain.Contracts.Clients.Seguranca;
using SEG.Domain.Contracts.UnitOfWorks;
using System;

namespace SEG.Client
{
    public class UnitOfWork : IUnitOfWork
    {
        public IEventoClient Eventos { get; }
        public IFormularioClient Formularios { get; }
        public IFuncaoClient Funcoes { get; }
        public IModuloClient Modulos { get; }
        public IPerfilClient Perfis { get; }
        public IUsuarioClient Usuarios { get; }
        public ISegurancaClient Seguranca { get; }
        public IAutenticacaoClient Autenticacao { get; }
        public UnitOfWork(IEventoClient eventoClient
            , IFormularioClient formularioClient
            , IFuncaoClient funcaoClient
            , IModuloClient moduloClient
            , IPerfilClient perfilClient
            , IUsuarioClient usuarioClient
            , ISegurancaClient segurancaClient
            , IAutenticacaoClient autenticacaoClient)
        {
            Eventos = eventoClient;
            Formularios = formularioClient;
            Funcoes = funcaoClient;
            Modulos = moduloClient;
            Perfis = perfilClient;
            Usuarios = usuarioClient;
            Seguranca = segurancaClient;
            Autenticacao = autenticacaoClient;
        }
    }
}
