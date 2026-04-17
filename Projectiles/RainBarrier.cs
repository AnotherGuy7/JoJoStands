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

        private const float OUTER_X = 95f;
        private const float OUTER_Y = 140f;
        private const float DAMAGE_X = 65f;
        private const float DAMAGE_Y = 100f;
        private const float PUSH_X = 98f;
        private const float PUSH_Y = 145f;
        private const float DEFLECT_X = 100f;
        private const float DEFLECT_Y = 148f;
        private const int DAMAGE_INTERVAL = 8;
        private const float NORMAL_KB = 5f;

        private int damageTimer = 0;
        private int visualTimer = 0;
        private Dictionary<int, int> npcDamageTimers = new Dictionary<int, int>();

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
            BarrierDamage(player);

            Lighting.AddLight(Projectile.Center, 0.15f, 0.35f, 0.7f);
        }

        private bool InsideOuter(Vector2 center)
        {
            float nx = (center.X - Projectile.Center.X) / PUSH_X;
            float ny = (center.Y - Projectile.Center.Y) / PUSH_Y;
            return nx * nx + ny * ny < 1f;
        }

        private bool InsideDamage(Vector2 center)
        {
            float nx = (center.X - Projectile.Center.X) / DAMAGE_X;
            float ny = (center.Y - Projectile.Center.Y) / DAMAGE_Y;
            return nx * nx + ny * ny < 1f;
        }

        private bool InsideDeflect(Vector2 center)
        {
            float nx = (center.X - Projectile.Center.X) / DEFLECT_X;
            float ny = (center.Y - Projectile.Center.Y) / DEFLECT_Y;
            return nx * nx + ny * ny < 1f;
        }

        private void BarrierVisuals()
        {
            visualTimer++;
            if (visualTimer < 1) return;
            visualTimer = 0;

            Vector2 c = Projectile.Center;
            for (int i = 0; i < 12; i++)
            {
                float angle = Main.rand.NextFloat(0f, MathHelper.TwoPi);
                float rx = OUTER_X * (0.75f + Main.rand.NextFloat(0f, 0.25f));
                float ry = OUTER_Y * (0.75f + Main.rand.NextFloat(0f, 0.25f));
                Vector2 pos = new Vector2(c.X + (float)Math.Cos(angle) * rx, c.Y + (float)Math.Sin(angle) * ry - OUTER_Y * 0.25f);
                int d = Dust.NewDust(pos, 3, 16, DustID.Water,
                    Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(16f, 24f),
                    60, default, Main.rand.NextFloat(1.0f, 1.5f));
                Main.dust[d].noGravity = true;
            }
            for (int i = 0; i < 6; i++)
            {
                float ox = Main.rand.NextFloat(-OUTER_X * 0.4f, OUTER_X * 0.4f);
                float oy = Main.rand.NextFloat(-OUTER_Y * 0.7f, OUTER_Y * 0.4f);
                int d = Dust.NewDust(new Vector2(c.X + ox, c.Y + oy), 2, 12, DustID.Water,
                    0f, Main.rand.NextFloat(18f, 26f), 50, default, Main.rand.NextFloat(1.1f, 1.6f));
                Main.dust[d].noGravity = true;
            }
        }

        private void DeflectProjectiles()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (!proj.active || !proj.hostile || proj.whoAmI == Projectile.whoAmI) continue;

                if (InsideDeflect(proj.Center))
                {
                    Vector2 diff = proj.Center - Projectile.Center;
                    if (diff == Vector2.Zero) diff = Vector2.UnitX;
                    diff.Normalize();
                    proj.velocity = diff * proj.velocity.Length() * 0.85f;
                    proj.friendly = true;
                    proj.hostile = false;
                    for (int d = 0; d < 5; d++)
                        Dust.NewDust(proj.Center, 4, 4, DustID.Water,
                            Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 1f), 0, default, 1f);
                }
            }
        }

        private void BarrierDamage(Player player)
        {
            if (Projectile.owner != Main.myPlayer) return;

            damageTimer++;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.dontTakeDamage) continue;

                bool inDamage = InsideDamage(npc.Center);
                bool inOuter = InsideOuter(npc.Center);

                if (inDamage)
                {
                    if (!npcDamageTimers.ContainsKey(i)) npcDamageTimers[i] = 0;
                    npcDamageTimers[i]++;

                    if (npcDamageTimers[i] >= DAMAGE_INTERVAL)
                    {
                        npcDamageTimers[i] = 0;
                        bool crit = Main.rand.Next(100) < player.GetTotalCritChance<MeleeDamageClass>();
                        npc.SimpleStrikeNPC(Projectile.damage, Projectile.direction, crit: crit, knockBack: 0f);

                        for (int d = 0; d < 5; d++)
                            Dust.NewDust(npc.position, npc.width, npc.height,
                                DustID.Water, Main.rand.NextFloat(-3f, 3f), -2f, 0, default, 1.1f);
                        Projectile.netUpdate = true;
                    }
                }
                else if (inOuter && !inDamage)
                {
                    Vector2 diff = npc.Center - Projectile.Center;
                    if (diff == Vector2.Zero) diff = Vector2.UnitX;
                    diff.Normalize();
                    if (!npc.boss) npc.velocity += diff * NORMAL_KB;
                    if (npcDamageTimers.ContainsKey(i)) npcDamageTimers.Remove(i);
                }
                else
                {
                    if (npcDamageTimers.ContainsKey(i)) npcDamageTimers.Remove(i);
                }
            }
        }
    }
}
