using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using Limpieza.Persistence.Database;
using Limpieza.Domain.DCedulaEvaluacion;
using Limpieza.Domain.DFacturas;
using Limpieza.Domain.DIncidencias;
using Limpieza.Domain.DCuestionario;
using Limpieza.Service.EventHandler.Commands.CedulasEvaluacion;

namespace Limpieza.Service.EventHandler.Handlers.CedulasEvaluacion
{
    public class CedulaEvaluacionUpdateEventHandler : IRequestHandler<CedulaEvaluacionUpdateCommand, CedulaEvaluacion>
    {
        private readonly ApplicationDbContext _context;

        public CedulaEvaluacionUpdateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CedulaEvaluacion> Handle(CedulaEvaluacionUpdateCommand request, CancellationToken cancellationToken)
        {

            try
            {
                CedulaEvaluacion cedula = _context.CedulaEvaluacion.FirstOrDefault(c => c.Id == request.Id);

                if (request.Calcula)
                {
                    List<Factura> facturas = _context.Facturas
                                                                   .Where(f => f.RepositorioId == request.RepositorioId &&
                                                                               f.InmuebleId == cedula.InmuebleId && f.Tipo.Equals("Factura")
                                                                               && f.Facturacion.Equals("Mensual"))
                                                                   .ToList();

                    List<Factura> notasCredito = _context.Facturas
                                                               .Where(f => f.RepositorioId == request.RepositorioId &&
                                                                           f.InmuebleId == cedula.InmuebleId && f.Tipo.Equals("NC")
                                                                           && f.Facturacion.Equals("Mensual"))
                                                               .ToList();

                    List<CuestionarioMensual> cuestionarioMensual = _context.CuestionarioMensual
                                                                .Where(cm => cm.Anio == cedula.Anio && cm.MesId == cedula.MesId && cm.ContratoId == cedula.ContratoId)
                                                                .ToList();

                    var respuestas = await Obtienetotales(request.Id, cuestionarioMensual);
                    var calificacion = await GetCalificacionCedula(request.Id, cuestionarioMensual);

                    cedula.EstatusId = request.EstatusId;
                    if (calificacion < 10)
                    {
                        //string calif = calificacion.ToString().Substring(0, 4);
                        string calif = (Math.Round(calificacion, 1)).ToString();
                        cedula.Calificacion = Convert.ToDouble(calif);
                    }
                    else
                    {
                        cedula.Calificacion = (double)calificacion;
                    }
                    if (calificacion < Convert.ToDecimal(8))
                    {
                        //decimal aux = (Convert.ToDecimal(facturas.Sum(f => f.Subtotal)) * Convert.ToDecimal(0.01));
                        //aux = Math.Round(aux, 2);

                        //cedula.Penalizacion = aux / Math.Round(calificacion, 1);
                        //cedula.Penalizacion = (Convert.ToDecimal(facturas.Sum(f => f.Subtotal)) * Convert.ToDecimal(0.01)) / Math.Round(calificacion,1);
                        cedula.Penalizacion = (Convert.ToDecimal(facturas.Sum(f => f.Subtotal)) * Convert.ToDecimal(0.01)) / calificacion;
                        cedula.Penalizacion = Math.Round(cedula.Penalizacion, 2);

                    }
                    else
                    {
                        cedula.Penalizacion = 0;
                        cedula.EstatusId = request.EstatusId;
                        cedula.Bloqueada = false;
                    }
                    cedula.FechaActualizacion = DateTime.Now;

                    await _context.SaveChangesAsync();

                    foreach (var fac in facturas)
                    {
                        fac.EstatusId = request.EFacturaId;

                        await _context.SaveChangesAsync();
                    }

                    foreach (var fac in notasCredito)
                    {
                        fac.EstatusId = request.ENotaCreditoId;

                        await _context.SaveChangesAsync();
                    }
                    }

                    return cedula;
                }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        private async Task<List<RespuestaEvaluacion>> Obtienetotales(int cedula, List<CuestionarioMensual> cuestionario)
        {
            var incidencias = new List<Incidencia>();

            foreach (var cm in cuestionario)
            {
                incidencias = _context.Incidencias.Where(i => i.Pregunta == cm.Consecutivo && i.CedulaEvaluacionId == cedula
                                                            && !i.FechaEliminacion.HasValue).ToList();
                var respuesta = _context.Respuestas.SingleOrDefault(r => r.CedulaEvaluacionId == cedula && r.Pregunta == cm.Consecutivo);

                if (!respuesta.Detalles.Equals("N/A"))
                {
                    if (incidencias.Where(i => i.Inasistencias != 0).Count() != 0)
                    {
                        respuesta.Detalles = incidencias.Where(i => i.Inasistencias != 0).Sum(i => i.Inasistencias) + "";
                    }
                    else
                    {
                        respuesta.Detalles = incidencias.Count() + "";
                    }
                }
                respuesta.Penalizable = (incidencias.Sum(i => i.MontoPenalizacion) != 0 ? true : false);
                respuesta.MontoPenalizacion = (incidencias.Sum(i => i.MontoPenalizacion) != 0 ? incidencias.Sum(i => i.MontoPenalizacion) : 0);

                await _context.SaveChangesAsync();
            }
            var respuestas = _context.Respuestas.ToList();

            return respuestas;
        }

        private async Task<decimal> GetCalificacionCedula(int cedula, List<CuestionarioMensual> cuestionario)
        {
            decimal calificacion = 0;
            decimal ponderacion = 0;
            bool calidad = true;
            List<Incidencia> incidencias = null;

            var respuestas = _context.Respuestas.Where(r => r.CedulaEvaluacionId == cedula).ToList();
            List<decimal> ponderaciones = new List<decimal>();

            foreach (var rs in respuestas)
            {
                var cm = cuestionario.Single(c => c.Consecutivo == rs.Pregunta);
                var dtPregunta = _context.Cuestionarios.Single(c => c.Id == cm.CuestionarioId);
                if (cm.ACLRS == rs.Respuesta)
                {
                    incidencias = _context.Incidencias.Where(i => i.CedulaEvaluacionId == cedula && i.Pregunta == cm.Consecutivo && !i.FechaEliminacion.HasValue).ToList();

                    if (incidencias.Count() != 0)
                    {
                        if (dtPregunta.Concepto.Equals("Inasistencias"))
                        {
                            ponderacion = incidencias.Sum(i => i.Inasistencias) > 0 ? GetPonderacionInasistencias(cedula, (int)cm.Ponderacion, Convert.ToInt32(rs.Detalles)) : (int)cm.Ponderacion;
                        }
                        else if (dtPregunta.Concepto.Equals("MaquinariaEH") || dtPregunta.Concepto.Equals("ActividadesPO"))
                        {
                            ponderacion = Convert.ToDecimal(incidencias) * Convert.ToDecimal(0.01);
                            ponderacion = Convert.ToDecimal(cm.Ponderacion) - Convert.ToDecimal(ponderacion);
                        }
                        else
                        {
                            ponderacion = Convert.ToDecimal(cm.Ponderacion) / Convert.ToDecimal(2);
                        }
                    }

                    calificacion += ponderacion;

                    rs.Detalles = incidencias + "";
                    rs.Penalizable = true;
                    rs.MontoPenalizacion = _context.Incidencias.Where(i => i.CedulaEvaluacionId == cedula &&
                                                                      i.Pregunta == cm.Consecutivo &&
                                                                      !i.FechaEliminacion.HasValue).Sum(i => i.MontoPenalizacion);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    calificacion += Convert.ToDecimal(cm.Ponderacion);
                    rs.Penalizable = false;
                }

                ponderaciones.Add(ponderacion);
            }

            calificacion = Convert.ToDecimal(calificacion / respuestas.Count());

            if (calificacion >= Convert.ToDecimal(8))
            {
                calificacion += 1;
            }

            return calificacion;
        }

        public int GetDiasHabiles(int anio, int mes)
        {
            var primerDia = new DateTime(anio, mes, 1);
            var ultimoDia = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));
            var diasLaborados = 0;
            while (primerDia <= ultimoDia)
            {
                if (!primerDia.DayOfWeek.ToString().Equals("Sunday") && !primerDia.DayOfWeek.ToString().Equals("Domingo"))
                {
                    diasLaborados++;
                }
                primerDia = primerDia.AddDays(1);
            }

            return diasLaborados;
        }

        public decimal GetPonderacionInasistencias(int cedula, int ponderacion, int inasistencias)
        {
            var cedulaE = _context.CedulaEvaluacion.Single(c => c.Id == cedula);
            var elementos = _context.ElementosLimpieza.Single(e => e.Anio == cedulaE.Anio && e.MesId == cedulaE.MesId && e.InmuebleId == cedulaE.InmuebleId
                                                          && e.ContratoId == cedulaE.ContratoId);

            decimal pAsistencia = elementos.TotalPersonal * GetDiasHabiles(cedulaE.Anio, cedulaE.MesId);
            decimal total = 0;

            inasistencias = (int)(pAsistencia - inasistencias);

            pAsistencia = (inasistencias * 100) / pAsistencia;

            total = Convert.ToDecimal((((pAsistencia * ponderacion) / 100)));

            return total;
        }
    }
}
