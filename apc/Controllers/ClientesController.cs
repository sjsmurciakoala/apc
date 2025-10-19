using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIAD.Core.DTOs.Clientes;
using SIAD.Services.Clientes;

namespace apc.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClientesService _clientesService;

    public ClientesController(IClientesService clientesService)
    {
        _clientesService = clientesService;
    }
    //obtnener tarifas de un cliente
    [HttpGet("{id:int}/tarifas")]
    public async Task<IActionResult> GetTarifas(int id, CancellationToken cancellationToken)
    {
        var tarifas = await _clientesService.GetTarifasAsync(id, cancellationToken);
        return Ok(tarifas);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] ClienteFilterDto filtro, CancellationToken cancellationToken)
    {
        var clientes = await _clientesService.SearchClientesAsync(filtro, cancellationToken);
        return Ok(clientes);
    }


    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var clientes = await _clientesService.GetClientesAsync(cancellationToken);
        return Ok(clientes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var cliente = await _clientesService.GetClienteAsync(id, cancellationToken);
        return cliente is null ? NotFound() : Ok(cliente);
    }

}
