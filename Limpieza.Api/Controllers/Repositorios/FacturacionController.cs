using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Limpieza.Service.Queries.Queries.Repositorios;
using Limpieza.Service.EventHandler.Commands.Repositorios;
using Limpieza.Service.Queries.DTOs.Repositorios;

namespace Limpieza.Api.Controllers.Repositorios
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/limpieza/repositorios")]
    public class FacturacionController : ControllerBase
    {
        private readonly IRepositorioQueryService _facturacion;
        private readonly IHostingEnvironment _environment;
        private readonly IMediator _mediator;

        public FacturacionController(IRepositorioQueryService facturacion, IMediator mediator, IHostingEnvironment environment)
        {
            _facturacion = facturacion;
            _mediator = mediator;
            _environment = environment;
        }

        [HttpGet("{anio}")]
        public async Task<List<RepositorioDto>> GetFacturaciones(int anio)
        {
            return await _facturacion.GetAllRepositoriosAsync(anio);
        }

        [HttpGet("getRepositorioById/{id}")]
        public async Task<RepositorioDto> GetFacturacionById(int id)
        {
            var repositorio = await _facturacion.GetRepositorioByIdAsync(id);
            return repositorio;
        }

        [Route("createRepositorio")]
        [HttpPost]
        public async Task<IActionResult> CreateRepositorio([FromBody] RepositorioCreateCommand request)
        {
            int status = await _mediator.Send(request);
            return Ok(status);
        }

        [Route("updateRepositorio")]
        [HttpPut]
        public async Task<IActionResult> UpdateRepositorio([FromBody] RepositorioUpdateCommand request)
        {
            var status = await _mediator.Send(request);
            return Ok(status);
        }
    }
}
