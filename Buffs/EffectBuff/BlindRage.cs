using JoJoStands.Buffs.Debuffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Buffs.EffectBuff
{
    public class BlindRage : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blind Rage");
            Description.SetDefault("Rage fills you! You deal more damage and your attacks seal the enemy to stone! Destroy the one who insulted your stupid hairstyle! Ouch...");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.HasBuff(Type))
                mPlayer.standDamageBoosts += 0.5f;
            else
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(47));
        }
    }
}