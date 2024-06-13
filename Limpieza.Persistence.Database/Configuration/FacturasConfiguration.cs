using Limpieza.Domain.DFacturas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limpieza.Persistence.Database.Configuration
{
    public class FacturasConfiguration
    {
        public FacturasConfiguration(EntityTypeBuilder<Factura> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.EstatusId).HasDefaultValue(1);
        }
    }
}
