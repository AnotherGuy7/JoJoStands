using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class ProtectiveFilmBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Protective Film");
            Description.SetDefault("The mud and dirt all over you offers some protection against the sunlight!");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            vPlayer.sunburnRegenTimeMultiplier += 1.8f;
            vPlayer.sunburnDamageMultiplier *= 0.7f;
            vPlayer.sunburnMoveSpeedMultiplier += 0.5f;
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            player.buffTime[buffIndex] += time;
            return true;
        }
    }
}