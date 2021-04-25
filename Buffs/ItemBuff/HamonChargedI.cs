using JoJoStands.Items.Hamon;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class HamonChargedI : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Hamon Charged I");
            Description.SetDefault("The large amount of Hamon in you is increasing your physical abilities.");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
            hPlayer.hamonDamageBoosts += 0.03f;
            player.moveSpeed += 0.03f;
            player.jumpSpeedBoost += 0.03f;
            player.allDamage += 0.03f;
            player.meleeSpeed += 0.03f;
            player.pickSpeed += 0.03f;
        }
    }
}