﻿using Limpieza.Domain.DEntregables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limpieza.Persistence.Database.Configuration
{
    public class EntregablesConfiguration
    {
        public EntregablesConfiguration(EntityTypeBuilder<Entregable> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
