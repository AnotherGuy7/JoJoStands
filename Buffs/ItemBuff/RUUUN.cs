using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class RUUUN : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("RUUUN!");
            Description.SetDefault("The Secret Joestar Technique- there's no shame!");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 2f;
        }
    }
}