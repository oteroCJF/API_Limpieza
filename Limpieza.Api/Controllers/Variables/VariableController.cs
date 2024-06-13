using Limpieza.Service.Queries.DTOs.Incidencias;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Limpieza.Service.Queries.DTOs.Variables;
using Limpieza.Service.Queries.Queries.Variables;
using Limpieza.Service.Queries.Mapping;

namespace Mensajeria.Api.Controllers.Variables
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/limpieza/variables")]
    public class VariableController : ControllerBase
    {
        private readonly IVariablesQueryService _variables;

        public VariableController(IVariablesQueryService variables)
        {
            _variables = variables;
        }

        [HttpGet]
        public async Task<List<VariableDto>> GetAllVariables()
        {
            var variables = await _variables.GetAllVariablesAsync();

            return variables;
        }

        [Route("getVariablesTipoIncidencia")]
        [HttpGet]
        public async Task<List<VariableDto>> GetVariablesTipoIncidencia()
        {
            var tiposIncidencia = await _variables.GetVariablesTipoIncidencia();

            return tiposIncidencia;
        }

        [Route("getIdByVariables/{abreviacion}/{valor}")]
        [HttpGet]
        public async Task<int> GetIdByVariables(string abreviacion, string valor)
        {
            var id = await _variables.GetVariableIdByTipoIncidencia(abreviacion, valor);

            return id;
        }

        [Route("getIdByIncidencia/{abreviacion}")]
        [HttpGet]
        public async Task<int> GetIdByIncidencia(string abreviacion)
        {
            var id = await _variables.GetIdByIncidencia(abreviacion);

            return id;
        }

        [Route("getVariableById/{variable}")]
        [HttpGet]
        public async Task<VariableDto> GetVariableById(int variable)
        {
            var id = await _variables.GetVariableById(variable);

            return id;
        }

    }
}
