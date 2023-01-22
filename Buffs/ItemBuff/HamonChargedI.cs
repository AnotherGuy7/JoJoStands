using JoJoStands.Items.Hamon;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class HamonChargedI : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hamon Charged I");
            Description.SetDefault("The large amount of Hamon in you is increasing your physical abilities.");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
            hPlayer.hamonDamageBoosts += 0.03f;
            player.moveSpeed += 0.03f;
            player.jumpSpeedBoost += 0.03f;
            player.GetDamage(DamageClass.Generic) += 0.03f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.03f;
            player.pickSpeed += 0.03f;

            if (Main.rand.Next(0, 8 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(player.position, player.width, player.height, 169, Scale: Main.rand.NextFloat(1f, 2f + 1f));
                Main.dust[dustIndex].noGravity = true;
            }
        }
    }
}