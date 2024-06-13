﻿using MediatR;
using Limpieza.Domain.DOficios;
using Limpieza.Persistence.Database;
using Limpieza.Service.EventHandler.Commands.Oficios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Limpieza.Service.EventHandler.Handlers.Oficios
{
    public class DetalleOficioDeleteEventHandler : IRequestHandler<DetalleOficioDeleteCommand, DetalleOficio>
    {
        private readonly ApplicationDbContext _context;

        public DetalleOficioDeleteEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DetalleOficio> Handle(DetalleOficioDeleteCommand request, CancellationToken cancellationToken)
        {
            var factura = _context.DetalleOficios.Single(dt => dt.FacturaId == request.FacturaId && 
                                                               dt.CedulaId == request.CedulaId && 
                                                               dt.OficioId == request.OficioId);

            _context.DetalleOficios.Remove(factura);

            await _context.SaveChangesAsync();

            return factura;
        }
    }
}
