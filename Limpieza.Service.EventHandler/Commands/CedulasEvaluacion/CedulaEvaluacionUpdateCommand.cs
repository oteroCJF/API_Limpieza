using MediatR;
using Limpieza.Domain.DCedulaEvaluacion;
using System.Collections.Generic;

namespace Limpieza.Service.EventHandler.Commands.CedulasEvaluacion
{
    public class CedulaEvaluacionUpdateCommand : IRequest<CedulaEvaluacion>
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public int RepositorioId { get; set; }
        public int EFacturaId { get; set; }
        public int ENotaCreditoId { get; set; }
        public bool Calcula { get; set; }
        public string Estatus { get; set; }
        public string Observaciones { get; set; }
    }
}
