using Steam.Models.SteamCommunity;

namespace SteamSharedGamesAPI.Models;

public class PlayerInventory
{
    public int GameCount { get; set; }
    public IEnumerable<OwnedGameModel> Games { get; set; }
}