using Limpieza.Domain.DFlujos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limpieza.Persistence.Database.Configuration
{
    public class FlujoCedulaConfiguration
    {
        public FlujoCedulaConfiguration(EntityTypeBuilder<FlujoCedula> entityBuilder)
        {
            entityBuilder.HasKey(x => new { x.EstatusCedulaId, x.EstatusId, x.Flujo });
        }
    }

}
