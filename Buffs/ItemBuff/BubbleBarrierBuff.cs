using JoJoStands.Buffs.Debuffs;
using Terraria;
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

        public override void UpdateBuffOnPlayer(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            player.gravity = player.gravity *= 0.8f;
            player.statDefense += 5 * mPlayer.standTier;
            player.noFallDmg = true;
            for (int i = 0; i < BuffLoader.BuffCount; i++)
            {
                if (Main.debuff[i])
                    player.buffImmune[i] = true;
            }
        }

        public override void OnBuffEnd(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            int abilityCooldownTime = 30 - (10 * (mPlayer.standTier - 3));
            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(abilityCooldownTime));
        }
    }
}