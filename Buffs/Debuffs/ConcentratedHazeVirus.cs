using System;
using Terraria;
using Microsoft.Xna.Framework;

namespace JoJoStands.Buffs.Debuffs
{
    public class ConcentratedHazeVirus : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spin");
            // Description.SetDefault("You're being infinitely spun... There's no hope in surviving.");
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
        }
    }
}