using MediatR;
using Limpieza.Domain.DIncidencias;
using System;

namespace Limpieza.Service.EventHandler.Commands.Incidencias
{
    public class IncidenciaDeleteCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int MesId { get; set; }
        public int Pregunta { get; set; }
        public DateTime? FechaIncidencia { get; set; }
        public int? Inasistencias { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
