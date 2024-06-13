namespace Limpieza.Service.Queries.DTOs.Incidencias
{
    public class ConfiguracionIncidenciaDto
    {
        public int Id { get; set; }
        public int Pregunta { get; set; }
        public bool Respuesta { get; set; }
        public bool Obligatorio { get; set; }
        public bool Detalles { get; set; }
        public bool FechaIncidencia { get; set; }
        public bool Inasistencias { get; set; }
        public bool MesSua { get; set; }
        public bool Observaciones { get; set; }
        public string? Ayuda { get; set; }
        public string? RespuestaCedula { get; set; }

    }
}
