using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class Exposing : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exposed Form");
            Description.SetDefault("You are more vulnurable to enemy attacks but you recover Void faster.");
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.noFallDmg = true;
            player.controlUseItem = false;
            player.controlUseTile = false;
            player.statDefense -= 12;
            player.velocity.X = -1f;
            player.velocity.Y = -1f;
            player.preventAllItemPickups = true;
            if (player.HasBuff(BuffID.Suffocation))
                player.ClearBuff(BuffID.Suffocation);
        }

    }
}