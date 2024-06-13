using MediatR;
using Limpieza.Domain.DOficios;
using System;

namespace Limpieza.Service.EventHandler.Commands.Oficios
{
    public class EDGPPTOficioCommand : IRequest<Oficio>
    {
        public int Id { get; set; }
        public int ESucesivoId { get; set; }
        public int EFacturaId { get; set; }
        public int ECedulaId { get; set; }
        public string UsuarioId { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
