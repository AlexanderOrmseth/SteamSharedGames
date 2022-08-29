using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SteamSharedGamesAPI.Models;
using SteamSharedGamesAPI.Services;

namespace SteamSharedGamesAPI.Controllers;

[AllowAnonymous]
public class SteamController : BaseController
{
    private readonly SteamApiService _steamApiService;

    public SteamController(SteamApiService steamApiService)
    {
        _steamApiService = steamApiService;
    }

    /// <summary>
    ///     Checks each users game inventory and returns a list of shared games.
    /// </summary>
    /// <param name="request">list of steamId (min 2), and if user wants to include free games</param>
    /// <returns>Returns a list of games all users owns</returns>
    [HttpPost("compare")]
    public async Task<ActionResult<CheckSharedGamesResponse>> CheckSharedGames(
        [FromBody] CheckSharedGamesRequest request)
    {
        if (!request.Ids.Any() || request.Ids.Count() < 2)
            return BadRequest(new ProblemDetails {Title = "Please provide at least two Id's!"});
        var response = await _steamApiService.CheckSharedGames(request);
        return Ok(response);
    }
}