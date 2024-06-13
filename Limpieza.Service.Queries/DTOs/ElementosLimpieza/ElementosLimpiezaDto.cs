using System;
using System.Collections.Generic;
using System.Text;

namespace Limpieza.Service.Queries.DTOs.ElementosLimpieza
{
    public class ElementosLimpiezaDto
    {
        public int Id { get; set; }
        public int Anio { get; set; }
        public int MesId { get; set; }
        public int InmuebleId { get; set; }
        public int ContratoId { get; set; }
        public int TotalPersonal { get; set; }
    }
}
