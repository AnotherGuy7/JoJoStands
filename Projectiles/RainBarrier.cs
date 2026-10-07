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

        private const float DAMAGE_X = 62f;
        private const float DAMAGE_Y = 135f;

        private const float PUSH_X  = 40f;
        private const float PUSH_Y  = 52f;

        private const float DEFLECT_X = 62f;
        private const float DEFLECT_Y = 135f;

        private const float SHAPE_EXP = 4f;

        private const int   DAMAGE_INTERVAL = 4;
        private const float DAMAGE_SLOW     = 0.015f;
        private const float KNOCKBACK_FORCE = 2.5f;
        private const float CENTER_Y_OFFSET = 10f;
        private const float BARRIER_FORWARD_X = 15f;
        private const float BARRIER_WORLD_X_OFFSET = -5f;

        private int visualTimer = 0;
        private Dictionary<int, int> damageTimers = new Dictionary<int, int>();
        private Vector2[] prevNPCPos = new Vector2[Main.maxNPCs];
        private Vector2 _pushCenter;

        public override void SetDefaults()
        {
            Projectile.width  = 32;
            Projectile.height = 32;
            Projectile.friendly   = true;
            Projectile.hostile    = false;
            Projectile.DamageType   = DamageClass.Generic;
            Projectile.penetrate  = -1;
            Projectile.timeLeft   = 900;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }

        // Rain Barrier
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead) { Projectile.Kill(); return; }
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (!mPlayer.standOut) { Projectile.Kill(); return; }

            float anchorX;
            int standIdx = (int)Projectile.ai[0];
            if (standIdx >= 0 && standIdx < Main.maxProjectiles
                && Main.projectile[standIdx].active
                && Main.projectile[standIdx].owner == Projectile.owner)
            {
                anchorX = Main.projectile[standIdx].Center.X + BARRIER_FORWARD_X * player.direction + BARRIER_WORLD_X_OFFSET;
            }
            else
            {
                anchorX = player.Center.X + (-19f + BARRIER_FORWARD_X) * player.direction + BARRIER_WORLD_X_OFFSET;
            }

            Projectile.Center = new Vector2(anchorX, player.Center.Y + CENTER_Y_OFFSET);
            _pushCenter        = new Vector2(anchorX, player.Center.Y);

            BarrierVisuals();
            DeflectProjectiles();
            HandleNPCs(player);
            Lighting.AddLight(Projectile.Center, 0.15f, 0.35f, 0.7f);
        }

        private bool InDamage(Vector2 p)
        {
            float nx = Math.Abs((p.X - Projectile.Center.X) / DAMAGE_X);
            float ny = Math.Abs((p.Y - Projectile.Center.Y) / DAMAGE_Y);
            return (float)(Math.Pow(nx, SHAPE_EXP) + Math.Pow(ny, SHAPE_EXP)) < 1f;
        }

        private bool InPush(Vector2 p)
        {
            float nx = Math.Abs((p.X - _pushCenter.X) / PUSH_X);
            float ny = Math.Abs((p.Y - _pushCenter.Y) / PUSH_Y);
            return (float)(Math.Pow(nx, SHAPE_EXP) + Math.Pow(ny, SHAPE_EXP)) < 1f;
        }

        private bool SegmentEntersPush(Vector2 a, Vector2 b)
        {
            if (InPush(a) || InPush(b)) return true;
            const int STEPS = 6;
            for (int s = 1; s < STEPS; s++)
            {
                Vector2 p = Vector2.Lerp(a, b, s / (float)STEPS);
                if (InPush(p)) return true;
            }
            return false;
        }

        private bool InDeflect(Vector2 p)
        {
            float nx = Math.Abs((p.X - Projectile.Center.X) / DEFLECT_X);
            float ny = Math.Abs((p.Y - Projectile.Center.Y) / DEFLECT_Y);
            return (float)(Math.Pow(nx, SHAPE_EXP) + Math.Pow(ny, SHAPE_EXP)) < 1f;
        }

        private float PushEscapeRadius(Vector2 outDir)
        {
            float ax = Math.Abs(outDir.X);
            float ay = Math.Abs(outDir.Y);
            if (ax < 0.0001f && ay < 0.0001f) return PUSH_X;
            float fx = ax / PUSH_X;
            float fy = ay / PUSH_Y;
            float denom = (float)(Math.Pow(fx, SHAPE_EXP) + Math.Pow(fy, SHAPE_EXP));
            return (float)Math.Pow(1.0 / denom, 1.0 / SHAPE_EXP);
        }

        private Vector2 GetOutDir(Vector2 npcCenter)
        {
            Vector2 d = npcCenter - _pushCenter;
            float len = d.Length();
            if (len < 1f) d = Vector2.UnitX;
            else d /= len;
            if (d.Y < 0f)
            {
                d.Y = 0f;
                float xl = Math.Abs(d.X);
                if (xl < 0.001f) d.X = 1f;
                else d.X /= xl;
            }
            return d;
        }

        private void PushOut(NPC npc)
        {
            Vector2 outDir = GetOutDir(npc.Center);
            float escapeR  = PushEscapeRadius(outDir);
            npc.Center = _pushCenter + outDir * (escapeR * 1.20f + npc.width * 0.5f + 4f);
            float dot = Vector2.Dot(npc.velocity, -outDir);
            if (dot > 0f) npc.velocity += outDir * dot;
            if (npc.velocity.Y < 0f) npc.velocity.Y = 0f;
            bool isBrainFight = (npc.type == NPCID.BrainofCthulhu || npc.type == NPCID.Creeper);
            if (!npc.boss || isBrainFight)
            {
                float resist = MathHelper.Clamp(npc.knockBackResist, 0f, 1f);
                npc.velocity += outDir * (KNOCKBACK_FORCE * resist);
            }
        }

        private bool InShape(float nx, float ny)
        {
            float ax = Math.Abs(nx);
            float ay = Math.Abs(ny);
            return (float)(Math.Pow(ax, SHAPE_EXP) + Math.Pow(ay, SHAPE_EXP)) < 1f;
        }

        private void BarrierVisuals()
        {
            visualTimer++;
            if (visualTimer < 1) return;
            visualTimer = 0;
            Vector2 c = Projectile.Center;

            int filled = 0;
            int tries = 0;
            while (filled < 55 && tries < 200)
            {
                tries++;
                float rx = Main.rand.NextFloat(-DAMAGE_X, DAMAGE_X);
                float ry = Main.rand.NextFloat(-DAMAGE_Y, DAMAGE_Y);
                if (!InShape(rx / DAMAGE_X, ry / DAMAGE_Y)) continue;
                int d = Dust.NewDust(new Vector2(c.X + rx, c.Y + ry), 2, 2, DustID.Water,
                    Main.rand.NextFloat(-0.2f, 0.2f), Main.rand.NextFloat(0.5f, 2f),
                    60, default, Main.rand.NextFloat(0.8f, 1.3f));
                Main.dust[d].noGravity = true;
                filled++;
            }

            int edgePoints = 14;
            for (int i = 0; i < edgePoints; i++)
            {
                float angle = (i / (float)edgePoints) * MathHelper.TwoPi + Main.rand.NextFloat(-0.05f, 0.05f);
                float ca = (float)Math.Cos(angle);
                float sa = (float)Math.Sin(angle);
                float ax = Math.Abs(ca);
                float ay = Math.Abs(sa);
                float denom = (float)(Math.Pow(ax / DAMAGE_X, SHAPE_EXP) + Math.Pow(ay / DAMAGE_Y, SHAPE_EXP));
                if (denom < 0.0001f) continue;
                float r = (float)Math.Pow(1.0 / denom, 1.0 / SHAPE_EXP);
                float rx2 = ca * r * (0.92f + Main.rand.NextFloat(0f, 0.08f));
                float ry2 = sa * r * (0.92f + Main.rand.NextFloat(0f, 0.08f));
                int d = Dust.NewDust(new Vector2(c.X + rx2, c.Y + ry2),
                    3, 3, DustID.Water, Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(0.5f, 2f),
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
                    proj.hostile  = false;
                    for (int d = 0; d < 5; d++)
                        Dust.NewDust(proj.Center, 4, 4, DustID.Water,
                            Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 1f), 0, default, 1f);
                }
            }
        }

        private void HandleNPCs(Player player)
        {
            if (Projectile.owner != Main.myPlayer) return;
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            int dmgInterval = Math.Max(DAMAGE_INTERVAL - mPlayer.standSpeedBoosts / 2, 1);
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly)
                {
                    prevNPCPos[i] = Vector2.Zero;
                    if (damageTimers.ContainsKey(i)) damageTimers.Remove(i);
                    continue;
                }

                bool isTargetDummy = npc.type == NPCID.TargetDummy;
                bool inPush =
                    !isTargetDummy && (
                           InPush(npc.Center)
                        || InPush(new Vector2(npc.position.X,             npc.position.Y))
                        || InPush(new Vector2(npc.position.X + npc.width, npc.position.Y))
                        || InPush(new Vector2(npc.position.X,             npc.position.Y + npc.height))
                        || InPush(new Vector2(npc.position.X + npc.width, npc.position.Y + npc.height)));

                bool crossed = !isTargetDummy
                            && prevNPCPos[i] != Vector2.Zero
                            && SegmentEntersPush(prevNPCPos[i], npc.Center);

                if (inPush || crossed)
                {
                    PushOut(npc);
                    prevNPCPos[i] = npc.Center;
                    continue;
                }

                prevNPCPos[i] = npc.Center;

                if (npc.dontTakeDamage) continue;

                bool inDamage = InDamage(npc.Center)
                    || InDamage(new Vector2(npc.position.X,             npc.position.Y))
                    || InDamage(new Vector2(npc.position.X + npc.width, npc.position.Y))
                    || InDamage(new Vector2(npc.position.X,             npc.position.Y + npc.height))
                    || InDamage(new Vector2(npc.position.X + npc.width, npc.position.Y + npc.height))
                    || InDamage(new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y))
                    || InDamage(new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height))
                    || InDamage(new Vector2(npc.position.X,                    npc.position.Y + npc.height * 0.5f))
                    || InDamage(new Vector2(npc.position.X + npc.width,        npc.position.Y + npc.height * 0.5f));

                if (inDamage)
                {
                    npc.velocity *= DAMAGE_SLOW;
                    if (!damageTimers.ContainsKey(i)) damageTimers[i] = 0;
                    damageTimers[i]++;
                    if (damageTimers[i] >= dmgInterval)
                    {
                        damageTimers[i] = 0;
                        bool crit = Main.rand.NextFloat(1, 100 + 1) <= mPlayer.standCritChangeBoosts;
                        player.ApplyDamageToNPC(npc, (int)(Projectile.damage * 0.85f), 0f, Projectile.direction, crit, DamageClass.Generic);
                        for (int d = 0; d < 3; d++)
                            Dust.NewDust(npc.position, npc.width, npc.height,
                                DustID.Water, Main.rand.NextFloat(-2f, 2f), -1.5f, 0, default, 1f);
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (damageTimers.ContainsKey(i)) damageTimers.Remove(i);
                }
            }
        }
    }
}
