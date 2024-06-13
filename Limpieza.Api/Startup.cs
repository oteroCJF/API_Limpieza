using Limpieza.Persistence.Database;
using Limpieza.Service.Queries.Queries.Contratos;
using Limpieza.Service.Queries.Queries.Convenios;
using Limpieza.Service.Queries.Queries.Cuestionarios;
using Limpieza.Service.Queries.Queries.Entregables;
using Limpieza.Service.Queries.Queries.EntregablesContratacion;
using Limpieza.Service.Queries.Queries.Repositorios;
using Limpieza.Service.Queries.Queries.Facturas;
using Limpieza.Service.Queries.Queries.Firmantes;
using Limpieza.Service.Queries.Queries.Incidencias;
using Limpieza.Service.Queries.Queries.LogCedulas;
using Limpieza.Service.Queries.Queries.LogEntregables;
using Limpieza.Service.Queries.Queries.ServiciosContrato;
using Limpieza.Service.Queries.Queries.Variables;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Limpieza.Service.Queries.Queries.Flujo;
using Limpieza.Service.Queries.Queries.CedulasEvaluacion;
using Microsoft.Extensions.Options;
using Limpieza.Service.Queries.Queries.Oficios;

namespace Mensajeria.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddDbContext<ApplicationDbContext>(opts => {
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
               x => x.MigrationsHistoryTable("__EFMigrationHistory", "Mensajeria")
               );
            });

            services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; options.JsonSerializerOptions.PropertyNameCaseInsensitive = false; });

            services.AddMediatR(Assembly.Load("Limpieza.Service.EventHandler"));

            services.AddTransient<ICuestionariosQueryService, CuestionarioQueryService>();
            services.AddTransient<IVariablesQueryService, VariableQueryService>();
            services.AddTransient<IFlujoCedulaQueryService, FlujoCedulaQueryService>();
            services.AddTransient<IFirmantesQueryService, FirmanteQueryService>();
            services.AddTransient<IRepositorioQueryService, RepositorioQueryService>();
            services.AddTransient<IFacturasQueryService, FacturaQueryService>();
            services.AddTransient<ILimpiezaQueryService, LimpiezaQueryService>();
            services.AddTransient<IRespuestasQueryService, RespuestaQueryService>();
            services.AddTransient<IContratosQueryService, ContratoQueryService>();
            services.AddTransient<IConvenioQueryService, ConvenioQueryService>();
            services.AddTransient<IServicioContratoQueryService, ServicioContratoQueryService>();
            services.AddTransient<IIncidenciasQueryService, IncidenciaQueryService>();
            services.AddTransient<IEntregableQueryService, EntregableQueryService>();
            services.AddTransient<IEntregableContratacionQueryService, EntregableContratacionQueryService>();
            services.AddTransient<ILogCedulasQueryService, LogCedulaQueryService>();
            services.AddTransient<ILogEntregablesQueryService, LogEntregableQueryService>();
            services.AddTransient<IOficioQueryService, OficioQueryService>();

            services.AddControllers();

            var secretKey = Encoding.ASCII.GetBytes(
               Configuration.GetValue<string>("SecretKey")
           );

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
