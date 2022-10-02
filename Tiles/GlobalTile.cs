using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Tiles
{
    public class JoJoGlobalTile : GlobalTile
    {

        public override void FloorVisuals(int type, Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.collideY = true;
        }
    }
}