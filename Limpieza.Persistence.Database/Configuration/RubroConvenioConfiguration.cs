using Limpieza.Domain.DContratos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limpieza.Persistence.Database.Configuration
{
    public class RubroConvenioConfiguration
    {
        public RubroConvenioConfiguration(EntityTypeBuilder<RubroConvenio> entityBuilder)
        {
            entityBuilder.HasKey(x => new { x.RubroId, x.ConvenioId });
        }
    }
}
