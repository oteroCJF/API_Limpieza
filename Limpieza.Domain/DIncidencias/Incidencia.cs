using System;

namespace Limpieza.Domain.DIncidencias
{
    public class Incidencia
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public Nullable<int> MesId { get; set; }
        public int Pregunta { get; set; }
        public Nullable<DateTime> FechaIncidencia { get; set; }
        public Nullable<int> Inasistencias { get; set; }
        public string? Observaciones { get; set; }
        public Nullable<bool> Penalizable { get; set; }
        public Nullable<decimal> MontoPenalizacion { get; set; }
        public Nullable<DateTime> FechaCreacion { get; set; }
        public Nullable<DateTime> FechaActualizacion { get; set; }
        public Nullable<DateTime> FechaEliminacion { get; set; }
    }
}
