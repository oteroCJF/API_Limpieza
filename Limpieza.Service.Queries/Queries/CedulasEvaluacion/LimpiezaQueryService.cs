using Limpieza.Domain.DCuestionario;
using Limpieza.Persistence.Database;
using Limpieza.Service.Queries.DTOs.CedulaEvaluacion;
using Limpieza.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Limpieza.Service.Queries.Queries.CedulasEvaluacion
{
    public interface ILimpiezaQueryService
    {
        Task<List<CedulaEvaluacionDto>> GetAllCedulasAsync();
        Task<List<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnio(int anio);
        Task<List<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnioMes(int anio, int mes);
        Task<CedulaEvaluacionDto> GetCedulaEvaluacionByInmuebleAnioMesAsync(int inmueble, int anio, int mes);
        Task<CedulaEvaluacionDto> GetCedulaById(int cedula);
    }

    public class LimpiezaQueryService : ILimpiezaQueryService
    {
        private readonly ApplicationDbContext _context;

        public LimpiezaQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CedulaEvaluacionDto>> GetAllCedulasAsync()
        {
            var collection = await _context.CedulaEvaluacion.OrderByDescending(x => x.Id).ToListAsync();

            return collection.MapTo<List<CedulaEvaluacionDto>>();
        }

        public async Task<List<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnio(int anio)
        {
            try
            {
                var cedulas = await _context.CedulaEvaluacion
                        .Where(x => x.Anio == anio && !x.FechaEliminacion.HasValue)
                        .ToListAsync();
                return cedulas.MapTo<List<CedulaEvaluacionDto>>();
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<List<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnioMes(int anio, int mes)
        {
            var cedulas = await _context.CedulaEvaluacion
                          .Where(x => x.Anio == anio && x.MesId == mes && !x.FechaEliminacion.HasValue)
                          /*.Select(c => new CedulaEvaluacionDto {
                              Id = c.Id,
                              Anio = c.Anio,
                              MesId = c.MesId,
                              EstatusId = c.EstatusId,
                              InmuebleId = c.InmuebleId,
                              Folio = c.Folio,
                              Calificacion = c.Calificacion,
                              FechaCreacion = c.FechaCreacion,
                              FechaActualizacion = c.FechaActualizacion
                          })*/
                          .ToListAsync();
            return cedulas.MapTo<List<CedulaEvaluacionDto>>();
        }

        public async Task<CedulaEvaluacionDto> GetCedulaById(int cedula)
        {
            return (await _context.CedulaEvaluacion.SingleOrDefaultAsync(x => x.Id == cedula)).MapTo<CedulaEvaluacionDto>();
        }
        
        public async Task<CedulaEvaluacionDto> GetCedulaEvaluacionByInmuebleAnioMesAsync(int inmueble, int anio, int mes)
        {
            try
            {
                var cedula = await _context.CedulaEvaluacion.SingleOrDefaultAsync(x => x.InmuebleId == inmueble && x.Anio == anio && x.MesId == mes);
                return cedula.MapTo<CedulaEvaluacionDto>();
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
