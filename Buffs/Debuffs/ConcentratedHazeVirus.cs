using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class ConcentratedHazeVirus : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            int tileX = (int)(player.Center.X / 16);
            int tileY = (int)(player.Center.Y / 16);
            Color lightColor = Lighting.GetColor(tileX, tileY);
            float light = (lightColor.R + lightColor.G + lightColor.B) / (255f * 3f);
            float lightFactor = MathHelper.Clamp(light / 1f, 0f, 1f);
            float effectMultiplier = 1f - (lightFactor * 0.75f);
            int maxDuration = light > 0.1f ? 10 * 60 : 20 * 60;
            int buffIndex = player.FindBuffIndex(Type);
            if (buffIndex >= 0 && player.buffTime[buffIndex] > maxDuration)
                player.buffTime[buffIndex] = maxDuration;
            bool hasFilter = player.GetModPlayer<MyPlayer>().hazeVirusFilter;
            float filterMultiplier = hasFilter ? 0.2f : 1f;
            player.lifeRegen = (int)(-60 * effectMultiplier * filterMultiplier);
            player.moveSpeed /= 1f + (1f * effectMultiplier);
        }

        private const int spreadRange = 12 * 16;
        private const int spreadInterval = 2 * 60;

        public override void UpdateBuffOnNPC(NPC npc)
        {
            int tileX = (int)(npc.Center.X / 16);
            int tileY = (int)(npc.Center.Y / 16);
            Color lightColor = Lighting.GetColor(tileX, tileY);
            float light = (lightColor.R + lightColor.G + lightColor.B) / (255f * 3f);
            float lightFactor = MathHelper.Clamp(light / 1f, 0f, 1f);
            float effectMultiplier = 1f - (lightFactor * 0.75f);
            int maxDuration = light > 0.1f ? 10 * 60 : 20 * 60;
            int buffIndex = npc.FindBuffIndex(Type);
            if (buffIndex >= 0 && npc.buffTime[buffIndex] > maxDuration)
                npc.buffTime[buffIndex] = maxDuration;
            int healthASecond = 6 + GetDebuffOwnerModPlayer(npc).standTier;
            npc.lifeRegen -= (int)(healthASecond * 2 * 120 * effectMultiplier);
            if (Math.Abs(npc.velocity.X) > 1f)
                npc.velocity.X *= 1f - (0.5f * effectMultiplier);
            if (Main.GameUpdateCount % spreadInterval == 0)
            {
                int hazeVirusType = ModContent.BuffType<HazeVirus>();
                int concentratedType = ModContent.BuffType<ConcentratedHazeVirus>();
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC other = Main.npc[n];
                    if (!other.active || other.whoAmI == npc.whoAmI)
                        continue;
                    if (npc.Distance(other.Center) < spreadRange
                        && other.FindBuffIndex(hazeVirusType) < 0
                        && other.FindBuffIndex(concentratedType) < 0)
                    {
                        other.AddBuff(hazeVirusType, 10 * 60);
                    }
                }
            }
        }
    }
}