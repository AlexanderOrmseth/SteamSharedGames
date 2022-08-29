using Steam.Models.SteamCommunity;
using SteamSharedGamesAPI.Models;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace SteamSharedGamesAPI.Services;

public class SteamApiService
{
    private readonly ILogger<SteamApiService> _logger;
    private readonly SteamWebInterfaceFactory _steamFactory;

    public SteamApiService(ILogger<SteamApiService> logger, IConfiguration config)
    {
        _logger = logger;
        _steamFactory = new SteamWebInterfaceFactory(config["Steam:SteamApiKey"]);
    }

    public async Task<CheckSharedGamesResponse> CheckSharedGames(CheckSharedGamesRequest request)
    {
        var inventories = new List<PlayerInventory>();
        var steamInterface = _steamFactory.CreateSteamWebInterface<PlayerService>(new HttpClient());


        foreach (var steamId in request.Ids)
        {
            var steamGamesResponse =
                await steamInterface.GetOwnedGamesAsync(steamId, true, request.IncludeFreeGames);
            inventories.Add(new PlayerInventory
            {
                GameCount = int.Parse(steamGamesResponse.Data.GameCount.ToString()),
                Games = steamGamesResponse.Data.OwnedGames
            });
        }


        // find shared games
        var games = inventories.SelectMany(x => x.Games).Select(MapOwnedGameModel)
            .GroupBy(x => x.Id)
            .Where(x => x.Count() == inventories.Count)
            .SelectMany(g => g).Select(e => e).DistinctBy(d => d.Id)
            .ToList();

        return new CheckSharedGamesResponse
        {
            Count = games.Count,
            Games = games
        };
    }

    private static GameModel MapOwnedGameModel(OwnedGameModel ownedGameModel)
    {
        return new GameModel
        {
            Id = (int) ownedGameModel.AppId,
            Name = ownedGameModel.Name,
            Icon =
                $"https://media.steampowered.com/steamcommunity/public/images/apps/{ownedGameModel.AppId}/{ownedGameModel.ImgIconUrl}.jpg"
        };
    }
}