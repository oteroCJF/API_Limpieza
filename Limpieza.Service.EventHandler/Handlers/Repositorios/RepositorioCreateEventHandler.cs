using Limpieza.Persistence.Database;
using Limpieza.Domain.DRepositorios;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Limpieza.Service.EventHandler.Commands.Repositorios;

namespace Limpieza.Service.EventHandler.Handlers.HFacturacion
{
    public class RepositorioCreateEventHandler : IRequestHandler<RepositorioCreateCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public RepositorioCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(RepositorioCreateCommand request, CancellationToken cancellationToken)
        {
            var repositorio = new Repositorio
            {
                ContratoId = request.ContratoId,
                UsuarioId = request.UsuarioId,
                MesId = request.MesId,
                Anio = request.Anio,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now
            };

            try
            {
                await _context.AddAsync(repositorio);
                await _context.SaveChangesAsync();
                return 201;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return 500;
            }
        }
    }
}
