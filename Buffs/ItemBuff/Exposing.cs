using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class Exposing : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Exposed Form");
            Description.SetDefault("You’re vulnerable more vulnurable to enemy attacks but you recover Void faster.");
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.noFallDmg = true;
            player.controlUseItem = false;
            player.controlUseTile = false;
            player.statDefense -= 6;
            if (player.HasBuff(BuffID.Suffocation))
            {
                player.ClearBuff(BuffID.Suffocation);
            }
        }

    }
}