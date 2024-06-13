using Limpieza.Persistence.Database;
using Limpieza.Service.Queries.DTOs.Contratos;
using Limpieza.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Limpieza.Service.Queries.Queries.Contratos
{
    public interface IContratosQueryService
    {
        Task<List<ContratoDto>> GetAllContratosAsync();
        Task<ContratoDto> GetContratoByIdAsync(int id);
    }

    public class ContratoQueryService : IContratosQueryService
    {
        private readonly ApplicationDbContext _context;

        public ContratoQueryService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContratoDto>> GetAllContratosAsync()
        {
            var collection = await _context.Contratos.OrderBy(x => x.Id).ToListAsync();

            return collection.MapTo<List<ContratoDto>>();
        }

        public async Task<ContratoDto> GetContratoByIdAsync(int contrato)
        {
            return (await _context.Contratos.SingleAsync(x => x.Id == contrato)).MapTo<ContratoDto>();
        }
    }
}
