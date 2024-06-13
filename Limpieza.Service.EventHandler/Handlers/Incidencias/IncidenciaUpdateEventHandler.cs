using Limpieza.Persistence.Database;
using Limpieza.Service.EventHandler.Commands.Incidencias;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Limpieza.Domain.DIncidencias;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Limpieza.Service.EventHandler.Handlers.Incidencias
{
    public class IncidenciaUpdateEventHandler : IRequestHandler<IncidenciaUpdateCommand, Incidencia>
    {
        private readonly ApplicationDbContext _context;

        public IncidenciaUpdateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Incidencia> Handle(IncidenciaUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                decimal montoPenalizacion = GetPenaDeductiva(request);

                var incidencia = _context.Incidencias.Single(i => i.Id == request.Id);

                incidencia.UsuarioId = request.UsuarioId;
                incidencia.CedulaEvaluacionId = request.CedulaEvaluacionId;
                incidencia.IncidenciaId = request.IncidenciaId;
                incidencia.MesId = request.MesId;
                incidencia.Pregunta = request.Pregunta;
                incidencia.FechaIncidencia = request.FechaIncidencia;
                incidencia.Inasistencias = request.Inasistencias;
                incidencia.Penalizable = (montoPenalizacion != 0 ? true : false);
                incidencia.MontoPenalizacion = montoPenalizacion;
                incidencia.Observaciones = request.Observaciones;
                incidencia.FechaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();

                return incidencia;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public decimal GetMontoFacturaAIVA(IncidenciaUpdateCommand incidencia)
        {
            var cedula = _context.CedulaEvaluacion.Single(ce => ce.Id == incidencia.CedulaEvaluacionId);
            var repositorio = _context.Repositorios.Single(r => r.Anio == cedula.Anio && r.ContratoId == cedula.ContratoId
                                                                && r.MesId == cedula.MesId);
            var facturas = _context.Facturas.Where(f => f.RepositorioId == repositorio.Id &&
                                                            cedula.InmuebleId == f.InmuebleId && f.Tipo.Equals("Factura")).Sum(f => f.Subtotal);

            return (decimal)facturas;
        }

        public decimal GetMontoFacturaDIVA(IncidenciaUpdateCommand incidencia)
        {
            var cedula = _context.CedulaEvaluacion.Single(ce => ce.Id == incidencia.CedulaEvaluacionId);
            var repositorio = _context.Repositorios.Single(r => r.Anio == cedula.Anio && r.ContratoId == cedula.ContratoId
                                                                && r.MesId == cedula.MesId);
            var facturas = _context.Facturas.Where(f => f.RepositorioId == repositorio.Id &&
                                                            cedula.InmuebleId == f.InmuebleId && f.Tipo.Equals("Factura")).Sum(f => f.Total);

            return (decimal)facturas;
        }

        public decimal GetPenaDeductiva(IncidenciaUpdateCommand incidencia)
        {
            decimal montoPenalizacion = 0;
            var cedula = _context.CedulaEvaluacion.Single(ce => ce.Id == incidencia.CedulaEvaluacionId);

            var respuesta = _context.Respuestas.Single(r => r.Pregunta == incidencia.Pregunta && r.CedulaEvaluacionId == incidencia.CedulaEvaluacionId);

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
