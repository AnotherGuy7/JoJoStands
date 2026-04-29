using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class RainBarrier : ModProjectile
    {
        public override string Texture => "JoJoStands/Projectiles/RainBarrier";

        private const float BLOCK_X = 52f;
        private const float BLOCK_Y = 85f;

        private const float OUTER_X = 140f;
        private const float OUTER_Y = 220f;

        private const int OUTER_INTERVAL = 2;
        private const float OUTER_SLOW = 0.04f;

        private const float DEFLECT_X = 64f;
        private const float DEFLECT_Y = 100f;

        private const float KNOCKBACK_FORCE = 4f;

        private int visualTimer = 0;
        private Dictionary<int, int> outerTimers = new Dictionary<int, int>();
        private Vector2[] prevNPCPos = new Vector2[Main.maxNPCs];

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 900;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead) { Projectile.Kill(); return; }
            Projectile.Center = player.Center;
            BarrierVisuals();
            DeflectProjectiles();
            HandleNPCs(player);
            Lighting.AddLight(Projectile.Center, 0.15f, 0.35f, 0.7f);
        }

        private bool InBlock(Vector2 p)
        {
            float nx = (p.X - Projectile.Center.X) / BLOCK_X;
            float ny = (p.Y - Projectile.Center.Y) / BLOCK_Y;
            return nx * nx + ny * ny < 1f;
        }

        private bool InOuter(Vector2 p)
        {
            float nx = (p.X - Projectile.Center.X) / OUTER_X;
            float ny = (p.Y - Projectile.Center.Y) / OUTER_Y;
            return nx * nx + ny * ny < 1f;
        }

        private bool InDeflect(Vector2 p)
        {
            float nx = (p.X - Projectile.Center.X) / DEFLECT_X;
            float ny = (p.Y - Projectile.Center.Y) / DEFLECT_Y;
            return nx * nx + ny * ny < 1f;
        }

        private float EllipseRadius(Vector2 outDir)
        {
            float denom = (outDir.X * outDir.X) / (BLOCK_X * BLOCK_X)
                        + (outDir.Y * outDir.Y) / (BLOCK_Y * BLOCK_Y);
            if (denom < 0.00001f) return BLOCK_X;
            return (float)Math.Sqrt(1.0 / denom);
        }

        private Vector2 GetOutDir(Vector2 npcCenter)
        {
            Vector2 d = npcCenter - Projectile.Center;
            float len = d.Length();
            if (len < 1f) d = Vector2.UnitX;
            else d /= len;
            return d;
        }

        private void PushOut(NPC npc)
        {
            Vector2 outDir = GetOutDir(npc.Center);
            float escapeR = EllipseRadius(outDir);
            npc.Center = Projectile.Center + outDir * (escapeR * 1.08f + npc.width * 0.5f);
            float dot = Vector2.Dot(npc.velocity, -outDir);
            if (dot > 0f) npc.velocity += outDir * dot;
            bool isBrainFight = (npc.type == NPCID.BrainofCthulhu || npc.type == NPCID.Creeper);
            if (!npc.boss || isBrainFight)
                npc.velocity += outDir * KNOCKBACK_FORCE;
        }

        private bool NPCCrossedBoundary(NPC npc, int i)
        {
            if (prevNPCPos[i] == Vector2.Zero) return false;
            return !InBlock(prevNPCPos[i]) && InBlock(npc.Center);
        }

        private void BarrierVisuals()
        {
            visualTimer++;
            if (visualTimer < 1) return;
            visualTimer = 0;
            Vector2 c = Projectile.Center;

            for (int i = 0; i < 18; i++)
            {
                float rx = Main.rand.NextFloat(-OUTER_X, OUTER_X);
                float maxRY = OUTER_Y * (float)Math.Sqrt(Math.Max(0, 1.0 - (rx * rx) / (OUTER_X * OUTER_X)));
                float ry = Main.rand.NextFloat(-maxRY, maxRY);
                int d = Dust.NewDust(new Vector2(c.X + rx, c.Y + ry), 2, 2, DustID.Water,
                    Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.NextFloat(18f, 30f),
                    60, default, Main.rand.NextFloat(0.8f, 1.3f));
                Main.dust[d].noGravity = true;
            }

            for (int i = 0; i < 10; i++)
            {
                float angle = Main.rand.NextFloat(0f, MathHelper.TwoPi);
                float rx2 = OUTER_X * (0.85f + Main.rand.NextFloat(0f, 0.15f));
                float ry2 = OUTER_Y * (0.85f + Main.rand.NextFloat(0f, 0.15f));
                int d = Dust.NewDust(new Vector2(c.X + (float)Math.Cos(angle) * rx2, c.Y + (float)Math.Sin(angle) * ry2),
                    3, 18, DustID.Water, Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(16f, 24f),
                    40, default, Main.rand.NextFloat(1.0f, 1.4f));
                Main.dust[d].noGravity = true;
            }
        }

        private void DeflectProjectiles()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (!proj.active || !proj.hostile || proj.whoAmI == Projectile.whoAmI) continue;
                if (InDeflect(proj.Center))
                {
                    Vector2 diff = proj.Center - Projectile.Center;
                    if (diff == Vector2.Zero) diff = Vector2.UnitX;
                    diff.Normalize();
                    proj.velocity = diff * Math.Max(proj.velocity.Length(), 4f) * 0.9f;
                    proj.friendly = true;
                    proj.hostile = false;
                    for (int d = 0; d < 5; d++)
                        Dust.NewDust(proj.Center, 4, 4, DustID.Water,
                            Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 1f), 0, default, 1f);
                }
            }
        }

        private void HandleNPCs(Player player)
        {
            if (Projectile.owner != Main.myPlayer) return;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.dontTakeDamage)
                {
                    prevNPCPos[i] = Vector2.Zero;
                    continue;
                }

                bool inBlock = InBlock(npc.Center)
                    || InBlock(new Vector2(npc.position.X, npc.position.Y))
                    || InBlock(new Vector2(npc.position.X + npc.width, npc.position.Y))
                    || InBlock(new Vector2(npc.position.X, npc.position.Y + npc.height))
                    || InBlock(new Vector2(npc.position.X + npc.width, npc.position.Y + npc.height));

                bool crossed = NPCCrossedBoundary(npc, i);

                if (inBlock || crossed)
                    PushOut(npc);

                prevNPCPos[i] = npc.Center;

                if (InOuter(npc.Center) && !InBlock(npc.Center))
                {
                    npc.velocity *= OUTER_SLOW;
                    if (!outerTimers.ContainsKey(i)) outerTimers[i] = 0;
                    outerTimers[i]++;
                    if (outerTimers[i] >= OUTER_INTERVAL)
                    {
                        outerTimers[i] = 0;
                        bool crit = Main.rand.Next(100) < player.GetTotalCritChance<MeleeDamageClass>();
                        npc.SimpleStrikeNPC(Projectile.damage, Projectile.direction, crit: crit, knockBack: 0f);
                        for (int d = 0; d < 3; d++)
                            Dust.NewDust(npc.position, npc.width, npc.height,
                                DustID.Water, Main.rand.NextFloat(-2f, 2f), -1.5f, 0, default, 1f);
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (outerTimers.ContainsKey(i)) outerTimers.Remove(i);
                }
            }
        }
    }
}
