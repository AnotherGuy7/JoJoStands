using JoJoStands.Buffs.Debuffs;
using JoJoStands.Dusts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
        private const int InitialBuffTicks = 20 * 60;
        private const int ConcentratedThresholdTicks = 10 * 60;

        private readonly Dictionary<int, ulong> _infectionStartTick = new();

        private static int EntityKey(Entity entity) =>
            entity is NPC npc ? -(npc.whoAmI + 1) : ((Player)entity).whoAmI;

        public override void AI()
        {
            int ticksElapsed = (30 * 60) - Projectile.timeLeft;
            float currentGasRange = MathHelper.Lerp(
                0, MaxGasRange,
                Math.Min((float)ticksElapsed / ExpansionTimeTicks, 1f));

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

            bool hasVirus = false;
            bool hasConcentrated = false;

            for (int i = 0; i < buffTypes.Length; i++)
            {
                if (buffTypes[i] == virusType) hasVirus = true;
                if (buffTypes[i] == concentratedType) hasConcentrated = true;
            }

            if (hasConcentrated)
            {
                _infectionStartTick.Remove(EntityKey(entity));
                return;
            }

            int key = EntityKey(entity);

            if (!hasVirus)
            {
                if (entity is NPC npcTarget)
                    npcTarget.AddBuff(virusType, InitialBuffTicks);
                else
                    ((Player)entity).AddBuff(virusType, InitialBuffTicks);
                _infectionStartTick.TryAdd(key, Main.GameUpdateCount);
                return;
            }

            if (!_infectionStartTick.TryGetValue(key, out ulong startTick))
            {
                _infectionStartTick[key] = Main.GameUpdateCount;
                return;
            }

            ulong elapsed = Main.GameUpdateCount - startTick;

            if (elapsed >= (ulong)ConcentratedThresholdTicks)
            {
                if (entity is NPC upgradeNpc)
                {
                    int idx = upgradeNpc.FindBuffIndex(virusType);
                    if (idx >= 0) upgradeNpc.DelBuff(idx);
                    upgradeNpc.AddBuff(concentratedType, 30 * 60 * 60);
                }
                else
                {
                    Player upgradePlayer = (Player)entity;
                    int idx = upgradePlayer.FindBuffIndex(virusType);
                    if (idx >= 0) upgradePlayer.DelBuff(idx);
                    upgradePlayer.AddBuff(concentratedType, 30 * 60 * 60);
                }

                _infectionStartTick.Remove(key);
            }
            else
            {
                if (entity is NPC refreshNpc)
                    refreshNpc.AddBuff(virusType, InitialBuffTicks);
                else
                    ((Player)entity).AddBuff(virusType, InitialBuffTicks);
            }
        }
    }
}