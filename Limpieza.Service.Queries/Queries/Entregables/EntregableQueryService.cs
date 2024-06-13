using Limpieza.Persistence.Database;
using Limpieza.Service.Queries.DTOs.Entregables;
using Limpieza.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Limpieza.Service.Queries.Queries.Entregables
{
    public interface IEntregableQueryService
    {
        Task<List<EntregableDto>> GetAllEntregablesAsync();
        Task<List<EntregableDto>> GetEntregablesByCedula(int cedula);
        Task<EntregableDto> GetEntregableById(int entregable);
    }

    public class EntregableQueryService : IEntregableQueryService
    {
        private readonly ApplicationDbContext _context;

        public EntregableQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EntregableDto>> GetAllEntregablesAsync()
        {
            var entregables = await _context.Entregables.ToListAsync();

            return entregables.MapTo<List<EntregableDto>>();
        }

        public async Task<List<EntregableDto>> GetEntregablesByCedula(int cedula)
        {
            try
            {
                var entregables = await _context.Entregables.Where(x => x.CedulaEvaluacionId == cedula && !x.FechaEliminacion.HasValue).ToListAsync();

                return entregables.MapTo<List<EntregableDto>>();
            }
            catch (Exception ex)
            { 
                string message = ex.Message;
                return null;
            }
        }

        public async Task<EntregableDto> GetEntregableById(int entregable)
        {
            var entregables = await _context.Entregables.SingleOrDefaultAsync(x => x.Id == entregable);

            return entregables.MapTo<EntregableDto>();
        }
    }
}
