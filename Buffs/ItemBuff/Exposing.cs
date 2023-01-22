using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.ItemBuff
{
    public class Exposing : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exposed Form");
            Description.SetDefault("You are more vulnurable to enemy attacks but you recover Void faster.");
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.noFallDmg = true;
            player.controlUseItem = false;
            player.controlUseTile = false;
            player.controlLeft = false;
            player.controlJump = false;
            player.controlRight = false;
            player.controlDown = false;
            player.controlRight = false;
            player.controlUp = false;
            player.controlMount = false;
            player.gravControl = false;
            player.gravControl2 = false;
            player.controlTorch = false;
            player.velocity.X = -1f;
            player.velocity.Y = -1f;
            player.preventAllItemPickups = true;
            if (player.HasBuff(BuffID.Suffocation))
                player.ClearBuff(BuffID.Suffocation);
        }

    }
}