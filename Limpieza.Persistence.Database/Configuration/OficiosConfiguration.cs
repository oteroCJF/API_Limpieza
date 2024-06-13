using Limpieza.Domain.DOficios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limpieza.Persistence.Database.Configuration
{
    public class OficiosConfiguration
    {
        public OficiosConfiguration(EntityTypeBuilder<Oficio> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.EstatusId).HasDefaultValue(1);
        }
    }
}
