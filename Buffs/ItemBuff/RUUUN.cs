using Terraria;

namespace JoJoStands.Buffs.ItemBuff
{
    public class RUUUN : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("RUUUN!");
            // Description.SetDefault("The Secret Joestar Technique- there's no shame!");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.moveSpeed *= 2f;
        }
    }
}