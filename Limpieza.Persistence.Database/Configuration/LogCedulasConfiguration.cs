using Limpieza.Domain.DHistorial;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Limpieza.Persistence.Database.Configuration
{
    public class LogCedulasConfiguration
    {
        public LogCedulasConfiguration(EntityTypeBuilder<LogCedula> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
