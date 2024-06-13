using MediatR;
using Limpieza.Domain.DCedulaEvaluacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limpieza.Service.EventHandler.Commands.CedulasEvaluacion
{
    public class CedulaSRUpdateCommand: IRequest<CedulaEvaluacion>
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public string Estatus { get; set; }
        public bool Bloqueada { get; set; }
        public bool Aprobada { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string Observaciones { get; set; }
    }
}
