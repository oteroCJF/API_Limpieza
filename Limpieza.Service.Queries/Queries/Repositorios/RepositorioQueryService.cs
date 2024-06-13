using Limpieza.Persistence.Database;
using Limpieza.Service.Queries.DTOs.Repositorios;
using Limpieza.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Limpieza.Service.Queries.Queries.Repositorios
{
    public interface IRepositorioQueryService
    {
        Task<List<RepositorioDto>> GetAllRepositoriosAsync(int anio);
        Task<RepositorioDto> GetRepositorioByIdAsync(int id);
    }

    public class RepositorioQueryService : IRepositorioQueryService
    {
        private readonly ApplicationDbContext _context;

        public RepositorioQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RepositorioDto>> GetAllRepositoriosAsync(int anio)
        {
            try
            {
                var collection = await _context.Repositorios.Where(x => x.Anio == anio).OrderBy(x => x.MesId).ToListAsync();

                return collection.MapTo<List<RepositorioDto>>();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
        public async Task<RepositorioDto> GetRepositorioByIdAsync(int id)
        {
            try
            {
                var collection = await _context.Repositorios.SingleOrDefaultAsync(x => x.Id == id);

                return collection.MapTo<RepositorioDto>();
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

    }
}
