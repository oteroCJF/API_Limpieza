using Limpieza.Persistence.Database;
using Limpieza.Service.EventHandler.Commands.Incidencias;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Limpieza.Domain.DIncidencias;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using Limpieza.Domain.DFacturas;
using Microsoft.EntityFrameworkCore;

namespace Limpieza.Service.EventHandler.Handlers.Incidencias
{
    public class IncidenciaCreateEventHandler : IRequestHandler<IncidenciaCreateCommand, Incidencia>
    {
        private readonly ApplicationDbContext _context;

        public IncidenciaCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Incidencia> Handle(IncidenciaCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                decimal montoPenalizacion = await GetPenaDeductiva(request);
                var incidencia = new Incidencia
                {
                    UsuarioId = request.UsuarioId,
                    CedulaEvaluacionId = request.CedulaEvaluacionId,
                    IncidenciaId = request.IncidenciaId,
                    MesId = request.MesId,
                    Pregunta = request.Pregunta,
                    FechaIncidencia = request.FechaIncidencia,
                    Inasistencias = request.Inasistencias,
                    Observaciones = request.Observaciones,
                    Penalizable = (montoPenalizacion != 0 ? true : false),
                    MontoPenalizacion = montoPenalizacion,
                    FechaCreacion = DateTime.Now
                };

                await _context.AddAsync(incidencia);
                await _context.SaveChangesAsync();

                return incidencia;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public decimal GetMontoFacturaAIVA(IncidenciaCreateCommand incidencia)
        {
            var cedula = _context.CedulaEvaluacion.Single(ce => ce.Id == incidencia.CedulaEvaluacionId);
            var repositorio = _context.Repositorios.Single(r => r.Anio == cedula.Anio && r.ContratoId == cedula.ContratoId
                                                                && r.MesId == cedula.MesId);
            var facturas = _context.Facturas.Where(f => f.RepositorioId == repositorio.Id &&
                                                            cedula.InmuebleId == f.InmuebleId && f.Tipo.Equals("Factura")).Sum(f => f.Subtotal);

            return (decimal)facturas;
        }

        public decimal GetMontoFacturaDIVA(IncidenciaCreateCommand incidencia)
        {
            var cedula = _context.CedulaEvaluacion.Single(ce => ce.Id == incidencia.CedulaEvaluacionId);
            var repositorio = _context.Repositorios.Single(r => r.Anio == cedula.Anio && r.ContratoId == cedula.ContratoId
                                                                && r.MesId == cedula.MesId);
            var facturas = _context.Facturas.Where(f => f.RepositorioId == repositorio.Id &&
                                                            cedula.InmuebleId == f.InmuebleId && f.Tipo.Equals("Factura")).Sum(f => f.Total);

            return (decimal)facturas;
        }

        public async Task<decimal> GetPenaDeductiva(IncidenciaCreateCommand incidencia)
        {
            decimal montoPenalizacion = 0;
            var cedula = _context.CedulaEvaluacion.Single(ce => ce.Id == incidencia.CedulaEvaluacionId);

            var respuesta = await _context.Respuestas.SingleAsync(r => r.Pregunta == incidencia.Pregunta && r.CedulaEvaluacionId == incidencia.CedulaEvaluacionId);

            if (incidencia.Inasistencias != 0)
            {
                respuesta.Respuesta = true;

                await _context.SaveChangesAsync();
            }

            var cuestionario = _context.CuestionarioMensual.Single(cm => cm.Consecutivo == incidencia.Pregunta &&
                                                                         cm.Anio == cedula.Anio &&
                                                                         cm.MesId == cedula.MesId &&
                                                                         cm.ContratoId == cedula.ContratoId);

            decimal costoE = incidencia.Penalizacion.SingleOrDefault(sc => sc.Servicio.Abreviacion.Contains("Elementos")).PrecioUnitario;

            if (respuesta.Respuesta == cuestionario.ACLRS)
            {
                if (cuestionario.Formula.Contains("CFMI"))
                {
                    montoPenalizacion = GetMontoFacturaAIVA(incidencia) * cuestionario.Porcentaje;
                }
                else if (cuestionario.Formula.Contains("CFMA"))
                {
                    montoPenalizacion = GetMontoFacturaAIVA(incidencia) * cuestionario.Porcentaje;
                }
                else if (cuestionario.Formula.Contains("ELEMENTOS"))
                {
                    var ce = (Convert.ToDecimal(costoE) / Convert.ToDecimal(30.4)).ToString("#.##");
                    montoPenalizacion = Convert.ToDecimal(ce) * Convert.ToDecimal(incidencia.Inasistencias);
                }
            }

            return montoPenalizacion;
        }
    }
}
