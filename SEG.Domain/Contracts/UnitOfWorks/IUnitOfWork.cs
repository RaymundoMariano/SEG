using SEG.Domain.Contracts.Clients.Aplicacao;
using SEG.Domain.Contracts.Clients.Autenticacao;
using SEG.Domain.Contracts.Clients.Seguranca;
using System;

namespace SEG.Domain.Contracts.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IEventoClient Eventos { get; }
        IFormularioClient Formularios { get; }
        IFuncaoClient Funcoes { get; }
        IModuloClient Modulos { get; }
        IPerfilClient Perfis { get; }
        IUsuarioClient Usuarios { get; }
        ISegurancaClient Seguranca { get; }
        IAutenticacaoClient Autenticacao { get; }
    }
}
