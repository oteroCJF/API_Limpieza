using MediatR;
using Limpieza.Domain.DIncidencias;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Limpieza.Service.EventHandler.Commands.CedulasEvaluacion;

namespace Limpieza.Service.EventHandler.Commands.Incidencias
{
    public class IncidenciaUpdateCommand : IRequest<Incidencia>
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
        public DateTime FechaActualizacion { get; set; }

        public virtual List< ServicioContratoDto> Penalizacion { get; set; } = new List<ServicioContratoDto>();
    }
}
