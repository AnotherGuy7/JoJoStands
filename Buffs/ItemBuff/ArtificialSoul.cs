using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace JoJoStands.Buffs.ItemBuff
{
    public class ArtificialSoul : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Artificial Soul");
            // Description.SetDefault("An artificial soul has been given to you!");
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void OnBuffEnd(Player player)
        {
            player.KillMe(PlayerDeathReason.ByCustomReason(player.name + "'s artificial soul has left them."), player.statLife + 1, player.direction);
            player.GetModPlayer<MyPlayer>().revived = false;
        }
    }
}