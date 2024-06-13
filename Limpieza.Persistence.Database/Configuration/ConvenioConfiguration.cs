using Limpieza.Domain.DContratos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limpieza.Persistence.Database.Configuration
{
    public class ConvenioConfiguration
    {
        public ConvenioConfiguration(EntityTypeBuilder<Convenio> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
