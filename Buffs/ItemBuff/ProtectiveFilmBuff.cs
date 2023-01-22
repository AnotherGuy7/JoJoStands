using JoJoStands.Items.Vampire;
using Terraria;

namespace JoJoStands.Buffs.ItemBuff
{
    public class ProtectiveFilmBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Protective Film");
            Description.SetDefault("The mud and dirt all over you offers some protection against the sunlight!");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
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