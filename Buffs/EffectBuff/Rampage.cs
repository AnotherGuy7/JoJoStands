using JoJoStands.Buffs.Debuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class Rampage : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rampage");
            Description.SetDefault("What happened, did someone make fun of your hair? You're filled with rage!\nDamage is doubled and punch speed is increased, but range and movement speed is halved!");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.5f;
            player.GetModPlayer<MyPlayer>().standSpeedBoosts += 2;
            player.moveSpeed *= 0.5f;
        }

        public override void OnBuffEnd(Player player)
        {
            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), player.GetModPlayer<MyPlayer>().AbilityCooldownTime(10));
        }
    }
}