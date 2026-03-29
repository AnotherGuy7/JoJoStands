using JoJoStands.Buffs.Debuffs;
using JoJoStands.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class HazeVirusCloud : ModProjectile
    {
        public override string Texture => Mod.Name + "/Projectiles/ControllableNail";

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 30 * 60;
        }

        private const float MaxGasRange = 15 * 16;
        private const int ExpansionTimeTicks = 60;
        private const int ConcentratedThresholdSeconds = 5;
        private const int InitialBuffTicks = 30 * 60 * 60;
        private const int ConcentratedThresholdTicks = ConcentratedThresholdSeconds * 60;

        public override void AI()
        {
            int ticksElapsed = (30 * 60) - Projectile.timeLeft;

            float currentGasRange = MathHelper.Lerp(0, MaxGasRange, Math.Min((float)ticksElapsed / ExpansionTimeTicks, 1f));

            if (Projectile.localAI[0] == 0f)
            {
                HazeCloudBurst.Spawn(Projectile.Center, count: 8);
                Projectile.localAI[0] = 1f;
            }

            for (int i = 0; i < 3; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float dist = Main.rand.NextFloat(currentGasRange);
                Vector2 dustPos = Projectile.Center + angle.ToRotationVector2() * dist;
                Dust.NewDust(dustPos, 1, 1, ModContent.DustType<GratefulDeadCloud>());
            }

            int hazeVirusType = ModContent.BuffType<HazeVirus>();
            int concentratedType = ModContent.BuffType<ConcentratedHazeVirus>();

            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (!npc.active || npc.friendly || Projectile.Distance(npc.Center) >= currentGasRange)
                    continue;

                ApplyVirus(npc, hazeVirusType, concentratedType);
            }

            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player = Main.player[p];
                if (!player.active || player.dead || Projectile.Distance(player.Center) >= currentGasRange)
                    continue;

                ApplyVirus(player, hazeVirusType, concentratedType);
            }
        }

        private void ApplyVirus(Entity entity, int virusType, int concentratedType)
        {
            int[] buffTypes = entity is NPC n ? n.buffType : ((Player)entity).buffType;
            int[] buffTimes = entity is NPC npc ? npc.buffTime : ((Player)entity).buffTime;

            int buffIndex = -1;
            for (int i = 0; i < 10; i++)
            {
                if (buffTypes[i] == virusType) { buffIndex = i; break; }
            }

            if (buffIndex >= 0)
            {
                int ticksRemaining = buffTimes[buffIndex];
                int elapsed = InitialBuffTicks - ticksRemaining;
                if (elapsed >= ConcentratedThresholdTicks)
                {
                    if (entity is NPC targetNpc) targetNpc.AddBuff(concentratedType, 30 * 60 * 60);
                    else ((Player)entity).AddBuff(concentratedType, 30 * 60 * 60);

                    if (entity is NPC targetN) targetN.DelBuff(buffIndex);
                    else ((Player)entity).DelBuff(buffIndex);
                }
            }
            else
            {
                bool hasConcentrated = false;
                for (int i = 0; i < 10; i++) if (buffTypes[i] == concentratedType) hasConcentrated = true;

                if (!hasConcentrated)
                {
                    if (entity is NPC targetNpc) targetNpc.AddBuff(virusType, InitialBuffTicks);
                    else ((Player)entity).AddBuff(virusType, InitialBuffTicks);
                }
            }
        }
    }
}