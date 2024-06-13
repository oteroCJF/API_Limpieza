using Limpieza.Persistence.Database;
using Limpieza.Service.Queries.DTOs.Cuestionario;
using Limpieza.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Limpieza.Service.Queries.Queries.Cuestionarios
{
    public interface ICuestionariosQueryService
    {
        Task<List<CuestionarioDto>> GetAllPreguntasAsync();
        Task<List<CuestionarioMensualDto>> GetCuestionarioMensualAsync(int anio, int mes, int contrato);
        Task<CuestionarioDto> GetPreguntaByIdAsync(int pregunta);
    }

    public class CuestionarioQueryService : ICuestionariosQueryService
    {
        private readonly ApplicationDbContext _context;

        public CuestionarioQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CuestionarioDto>> GetAllPreguntasAsync()
        {
            var preguntas = await _context.Cuestionarios.ToListAsync();

            return preguntas.MapTo<List<CuestionarioDto>>();
        }

        public async Task<List<CuestionarioMensualDto>> GetCuestionarioMensualAsync(int anio, int mes, int contrato)
        {
            var preguntas = await _context.CuestionarioMensual.Where(x => x.Anio == anio && x.MesId == mes && x.ContratoId == contrato).ToListAsync();

            return preguntas.MapTo<List<CuestionarioMensualDto>>();
        }

        public async Task<CuestionarioDto> GetPreguntaByIdAsync(int pregunta)
        {
            try
            {
                return (await _context.Cuestionarios.SingleAsync(x => x.Id == pregunta)).MapTo<CuestionarioDto>();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
