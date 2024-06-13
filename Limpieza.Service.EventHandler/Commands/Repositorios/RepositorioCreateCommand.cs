using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limpieza.Service.EventHandler.Commands.Repositorios
{
    public class RepositorioCreateCommand : IRequest<int>
    {
        public int ContratoId { get; set; }
        public string UsuarioId { get; set; }
        public int MesId { get; set; }
        public int Anio { get; set; }
        public int EstatusId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
