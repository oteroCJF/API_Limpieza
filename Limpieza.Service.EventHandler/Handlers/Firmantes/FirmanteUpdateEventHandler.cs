﻿using Limpieza.Persistence.Database;
using Limpieza.Service.EventHandler.Commands.Incidencias;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Limpieza.Service.EventHandler.Commands.Firmantes;
using System.Linq;
using Limpieza.Domain.DIncidencias;
using Limpieza.Domain.DFirmantes;

namespace Limpieza.Service.EventHandler.Handlers.Firmantes
{
    public class FirmanteUpdateEventHandler : IRequestHandler<FirmantesUpdateCommand, Firmante>
    {
        private readonly ApplicationDbContext _context;

        public FirmanteUpdateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Firmante> Handle(FirmantesUpdateCommand firmantes, CancellationToken cancellationToken)
        {
            try
            {
                var firmante = _context.Firmantes.SingleOrDefault(f => f.Id == firmantes.Id);

                firmante.UsuarioId = firmantes.UsuarioId;
                firmante.Escolaridad = firmantes.Escolaridad;
                firmante.FechaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();

                return firmante;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
