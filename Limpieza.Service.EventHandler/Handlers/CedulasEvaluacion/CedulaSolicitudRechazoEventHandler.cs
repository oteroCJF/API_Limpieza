using MediatR;
using Limpieza.Domain.DCedulaEvaluacion;
using Limpieza.Persistence.Database;
using Limpieza.Service.EventHandler.Commands.CedulasEvaluacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Limpieza.Service.EventHandler.Handlers.CedulasEvaluacion
{
    public class CedulaSolicitudRechazoEventHandler : IRequestHandler<CedulaSRUpdateCommand, CedulaEvaluacion>
    {
        private readonly ApplicationDbContext _context;

        public CedulaSolicitudRechazoEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CedulaEvaluacion> Handle(CedulaSRUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cedula = _context.CedulaEvaluacion.Single(c => c.Id == request.Id);

                cedula.EstatusId = request.EstatusId;
                cedula.Bloqueada = request.Bloqueada;
                cedula.FechaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();

                return cedula;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
