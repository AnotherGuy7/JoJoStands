using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class ArtificialSoul : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Artificial Soul");
            Description.SetDefault("An artificial soul has been given to you!");
            canBeCleared = false;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] <= 2)
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + "'s artificial soul has left him."), player.statLife + 1, player.direction);
                player.GetModPlayer<MyPlayer>().revived = false;
                player.ClearBuff(Type);
            }
        }
    }
}