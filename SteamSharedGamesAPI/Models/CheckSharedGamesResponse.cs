namespace SteamSharedGamesAPI.Models;

public class CheckSharedGamesResponse
{
    public int Count { get; set; }
    public IEnumerable<GameModel> Games { get; set; }
}