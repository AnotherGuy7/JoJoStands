using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.PlayerBuffs
{
    public class TopOfTheChain : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Top of the Chain");
            Description.SetDefault("You are the beast to be feared the most, and no one will take that spot.\nIncreased stats for the duration of the battle.");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            player.moveSpeed *= 1.08f + (3f * (vPlayer.GetSkillLevel(player, VampirePlayer.TopOfTheChain) - 1));
            player.GetDamage(DamageClass.Generic) *= 1.1f + (0.05f * (vPlayer.GetSkillLevel(player, VampirePlayer.TopOfTheChain) - 1));
            player.jumpSpeedBoost *= 1.15f + (1.05f * (vPlayer.GetSkillLevel(player, VampirePlayer.TopOfTheChain) - 1));
            player.maxRunSpeed += 1f;
        }
    }
}