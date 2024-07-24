using JoJoStands.Buffs.Debuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class BubbleBarrierBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bubble Barrier");
            // Description.SetDefault("A protective bubble surrounds you. Increases defense, makes you immune to debuffs, and allows you to glide around!");
        }

        private readonly int[] VanillaPhysicalDebuffs = new int[18] { BuffID.OnFire, BuffID.Bleeding, BuffID.BrokenArmor, BuffID.Burning, BuffID.Chilled, BuffID.Electrified, BuffID.Frostburn, BuffID.Ichor, BuffID.Inferno, BuffID.Oiled, BuffID.OnFire3, BuffID.Poisoned, BuffID.ShadowFlame, BuffID.Stoned, BuffID.Suffocation, BuffID.Venom, BuffID.Webbed, BuffID.Wet };

        public override void UpdateBuffOnPlayer(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            player.gravity = player.gravity *= 0.8f;
            player.statDefense += 5 * mPlayer.standTier;
            player.noFallDmg = true;
            for (int i = 0; i < VanillaPhysicalDebuffs.Length; i++)
            {
                player.buffImmune[VanillaPhysicalDebuffs[i]] = true;
            }
        }

        public override void OnBuffEnd(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            int abilityCooldownTime = 30 - (5 * (mPlayer.standTier - 2));
            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(abilityCooldownTime));
        }
    }
}