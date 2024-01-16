using Microsoft.AspNetCore.Mvc;
using ResourceLoader.Services;
using ResourceLoader.Models;

namespace ResourceLoader.Controllers;

[Route("api/resources")]
public class ResourcesController: ControllerBase{
    
    private readonly ILogger<ResourcesController> _logger;
    private readonly IStatsService _statsService;

    public ResourcesController(ILogger<ResourcesController> logger, IStatsService statsService)
    {
        _logger = logger;
        _statsService = statsService;
    }
    [HttpPost]
    [Route("network")]
    public async Task<string> NetworkLoad([FromBody]dynamic json){
        int bytes = System.Text.ASCIIEncoding.UTF8.GetByteCount(json.ToString());
        _statsService.LogRequest((uint)bytes);

        return await Task.FromResult(new {input = json.ToString(), size = bytes}.ToString());
    }

    [HttpGet]
    [Route("stats")]
    public async Task<Stats> GetStats(){
        var stats = _statsService.GetLastStats();

        return stats;
    }
}