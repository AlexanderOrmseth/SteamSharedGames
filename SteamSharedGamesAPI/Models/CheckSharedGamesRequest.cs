namespace SteamSharedGamesAPI.Models;

public class CheckSharedGamesRequest
{
    public IEnumerable<ulong> Ids { get; set; }
    public bool IncludeFreeGames { get; set; } = true;
}