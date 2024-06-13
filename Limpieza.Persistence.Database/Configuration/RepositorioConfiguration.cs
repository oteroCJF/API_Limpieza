using Limpieza.Domain.DRepositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limpieza.Persistence.Database.Configuration
{
    public class FacturacionMensajeriaConfiguration
    {
        public FacturacionMensajeriaConfiguration(EntityTypeBuilder<Repositorio> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.EstatusId).HasDefaultValue(1);
        }
    }
}
