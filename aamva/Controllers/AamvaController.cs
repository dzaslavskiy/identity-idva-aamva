using Aamva.Models;
using Aamva.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aamva.Controllers;

[ApiController]
[Route("[controller]")]
public class AamvaController : ControllerBase
{
    private readonly ILogger<AamvaController> _logger;
    private readonly IDldvService _service;

    public AamvaController(ILogger<AamvaController> logger, IDldvService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    public async Task<MatchResponse> Post(ValidationRequest request)
    {
        _logger.LogDebug("Post");

        var result = await _service.CallDldvService(request);
        return result;
    }

    [Route("/error")]
    public IActionResult HandleError() =>
        Problem();
}
