using Limpieza.Persistence.Database;
using Limpieza.Service.Queries.DTOs.LogCedulas;
using Limpieza.Service.Queries.DTOs.LogEntregables;
using Limpieza.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limpieza.Service.Queries.Queries.LogEntregables
{
    public interface ILogEntregablesQueryService
    {
        Task<List<LogEntregableDto>> GetHistorialEntregablesByCedula(int cedula);
    }

    public class LogEntregableQueryService : ILogEntregablesQueryService
    {
        private readonly ApplicationDbContext _context;

        public LogEntregableQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LogEntregableDto>> GetHistorialEntregablesByCedula(int cedula)
        {
            var historial = await _context.LogEntregables.Where(h => h.CedulaEvaluacionId == cedula).OrderByDescending(h => h.FechaCreacion).ToListAsync();

            return historial.MapTo<List<LogEntregableDto>>();
        }
    }
}
