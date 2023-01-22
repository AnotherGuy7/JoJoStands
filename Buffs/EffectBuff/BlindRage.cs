using JoJoStands.Buffs.Debuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class BlindRage : JoJoBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Rage;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blind Rage");
            Description.SetDefault("Rage fills you! You deal more damage and your attacks seal the enemy to stone! Destroy the one who insulted your stupid hairstyle! Ouch...");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.5f;
        }

        public override void OnBuffEnd(Player player)
        {
            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), player.GetModPlayer<MyPlayer>().AbilityCooldownTime(30));
        }
    }
}