using Limpieza.Domain.DElementos;
using Limpieza.Domain.DIncidencias;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limpieza.Persistence.Database.Configuration
{
    public class ElementosLimpiezaConfiguration
    {
        public ElementosLimpiezaConfiguration(EntityTypeBuilder<ElementosLimpieza> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
