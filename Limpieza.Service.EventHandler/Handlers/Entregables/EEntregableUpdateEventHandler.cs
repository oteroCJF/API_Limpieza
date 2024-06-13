using Limpieza.Persistence.Database;
using Limpieza.Service.EventHandler.Commands.Entregables;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using Limpieza.Domain.DEntregables;

namespace Limpieza.Service.EventHandler.Handlers.Entregables
{
    public class EEntregableUpdateEventHandler : IRequestHandler<EEntregableUpdateCommand, Entregable>
    {
        private readonly ApplicationDbContext _context;

        public EEntregableUpdateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entregable> Handle(EEntregableUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Entregable entregable = _context.Entregables.Where(e => e.Id == request.Id && !e.FechaEliminacion.HasValue).FirstOrDefault();

                entregable.UsuarioId = request.UsuarioId;
                entregable.EstatusId = request.EstatusId;
                entregable.Observaciones = request.Observaciones;
                entregable.FechaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();

                return entregable;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
