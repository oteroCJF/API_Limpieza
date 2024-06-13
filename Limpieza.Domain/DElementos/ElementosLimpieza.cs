using System;
using System.Collections.Generic;
using System.Text;

namespace Limpieza.Domain.DElementos
{
    public class ElementosLimpieza
    {
        public int Id { get; set; }
        public int Anio { get; set; }
        public int MesId { get; set; }
        public int InmuebleId { get; set; }
        public int ContratoId { get; set; }
        public int TotalPersonal { get; set; }
    }
}
