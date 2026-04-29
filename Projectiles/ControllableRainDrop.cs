using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ControllableRainDrop : ModProjectile
    {
        public override string Texture => "JoJoStands/Projectiles/ControllableRainDrop";

        private const int   SPAWN_GROW_FRAMES = 30;

        private const float APEX_HORIZ_OFFSET = 28f;
        private const float APEX_VERT_OFFSET  = 55f;
        private const float APEX_REACHED_DIST = 12f;

        private const float APEX_ACCEL        = 1.15f;
        private const float APEX_MAX_SPEED    = 23f;

        private const float RISE_ACCEL        = 1.30f;
        private const float RISE_MAX_SPEED    = 26f;
        private const float RISE_BRAKE_DIST   = 50f;
        private const float RISE_MIN_SPEED    = 14f;
        private const float PERP_DAMP         = 0.92f;

        private const float TRACK_ZONE        = 70f;
        private const float TRACK_ACCEL       = 3.20f;
        private const float TRACK_MAX_SPEED   = 48f;
        private const float TRACK_BRAKE_DIST  = 32f;
        private const float TRACK_MIN_SPEED   = 14f;
        private const float CURSOR_LOCK_DIST  = 12f;
        private const float CURSOR_TIP_Y_OFFSET = 14f;

        private const float FREE_GRAVITY      = 0.42f;
        private const float CONTROL_RANGE     = 260f;

        private Vector2 spawnPos          = Vector2.Zero;
        private Vector2 apexPos           = Vector2.Zero;
        private bool    apexLocked        = false;
        private bool    inApexPhase       = true;
        private bool    reachedCursorZone = false;
        private bool    firstFrame        = true;
        private bool    wasMouseRight     = false;
        private int     spawnTimer        = 0;

        public override void SetDefaults()
        {
            Projectile.width        = 14;
            Projectile.height       = 28;
            Projectile.friendly     = true;
            Projectile.hostile      = false;
            Projectile.DamageType   = DamageClass.Melee;
            Projectile.penetrate    = 3;
            Projectile.timeLeft     = 600;
            Projectile.ignoreWater  = true;
            Projectile.tileCollide  = false;
            Projectile.alpha        = 30;
            Projectile.netImportant = true;
        }

        private static bool IsTrueSolid(int tx, int ty)
        {
            if (tx < 0 || tx >= Main.maxTilesX || ty < 0 || ty >= Main.maxTilesY) return false;
            var t = Main.tile[tx, ty];
            return t.HasTile && Main.tileSolid[t.TileType] && !TileID.Sets.Platforms[t.TileType];
        }

        private bool HitsSolidTile()
        {
            int x0 = (int)(Projectile.position.X / 16f);
            int x1 = (int)((Projectile.position.X + Projectile.width)  / 16f);
            int y0 = (int)(Projectile.position.Y / 16f);
            int y1 = (int)((Projectile.position.Y + Projectile.height) / 16f);
            for (int tx = x0; tx <= x1; tx++)
                for (int ty = y0; ty <= y1; ty++)
                    if (IsTrueSolid(tx, ty)) return true;
            return false;
        }

        private void ComputeApex(Vector2 cursorWorld)
        {
            float dx = cursorWorld.X - spawnPos.X;
            float horizSign = dx >= 0f ? 1f : -1f;
            float apexX = spawnPos.X + horizSign * APEX_HORIZ_OFFSET;
            float apexY = spawnPos.Y + APEX_VERT_OFFSET;
            apexPos = new Vector2(apexX, apexY);
            apexLocked = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (firstFrame)
            {
                Projectile.velocity = Vector2.Zero;
                spawnPos = Projectile.Center;
                firstFrame = false;

                if (Projectile.owner == Main.myPlayer)
                {
                    int myType = Projectile.type;
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        Projectile p = Main.projectile[i];
                        if (p.active && p.whoAmI != Projectile.whoAmI
                            && p.owner == Projectile.owner
                            && p.type == myType)
                        {
                            p.ai[0] = 1f;
                            p.netUpdate = true;
                        }
                    }
                }
            }

            if (spawnTimer < SPAWN_GROW_FRAMES)
            {
                spawnTimer++;

                int   dirSign      = player.direction;
                float coneTipX     = player.Center.X + (-20f) * dirSign + (20f) * dirSign;
                float coneTipY     = player.Center.Y + (-73f) + (-22f);
                spawnPos           = new Vector2(coneTipX, coneTipY);
                Projectile.Center  = spawnPos;
                Projectile.velocity = Vector2.Zero;

                if (Main.rand.NextBool(3))
                {
                    int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                        DustID.Water, 0f, 0f, 100, default, Main.rand.NextFloat(0.6f, 1.0f));
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0.3f;
                }
                Lighting.AddLight(Projectile.Center, 0.0f, 0.1f, 0.2f);

                if (Projectile.owner == Main.myPlayer)
                    wasMouseRight = Main.mouseRight;
                return;
            }

            if (Projectile.owner == Main.myPlayer && Projectile.ai[0] < 1f)
            {
                if (wasMouseRight && !Main.mouseRight)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.netUpdate = true;
                }
                wasMouseRight = Main.mouseRight;
            }

            bool released = Projectile.ai[0] >= 1f;

            if (Projectile.owner == Main.myPlayer)
            {
                if (!released)
                {
                    Vector2 standCenter = player.Center + new Vector2(0f, -73f);
                    bool    inRange     = Vector2.Distance(Projectile.Center, standCenter) < CONTROL_RANGE;

                    if (!inRange)
                    {
                        Projectile.ai[0] = 1f;
                        Projectile.velocity.Y += FREE_GRAVITY;
                        Projectile.netUpdate = true;
                    }
                    else if (Main.mouseRight)
                    {
                        Vector2 cursorTarget = Main.MouseWorld + new Vector2(0f, CURSOR_TIP_Y_OFFSET);
                        Vector2 toMouse = cursorTarget - Projectile.Center;
                        float   dist    = toMouse.Length();

                        if (!inApexPhase && !reachedCursorZone && dist < TRACK_ZONE)
                            reachedCursorZone = true;

                        if (reachedCursorZone)
                        {
                            if (dist <= CURSOR_LOCK_DIST)
                            {
                                Projectile.Center = cursorTarget;
                                Projectile.velocity = Vector2.Zero;
                            }
                            else if (dist > 0.5f)
                            {
                                Vector2 dir  = toMouse / dist;
                                Vector2 perp = new Vector2(-dir.Y, dir.X);

                                float along = Vector2.Dot(Projectile.velocity, dir);
                                float side  = Vector2.Dot(Projectile.velocity, perp);

                                along += TRACK_ACCEL;

                                float speedCap = TRACK_MAX_SPEED;
                                if (dist < TRACK_BRAKE_DIST)
                                    speedCap = MathHelper.Lerp(TRACK_MIN_SPEED, TRACK_MAX_SPEED, dist / TRACK_BRAKE_DIST);
                                if (along > speedCap) along = speedCap;
                                if (along < -TRACK_MAX_SPEED * 0.4f) along = -TRACK_MAX_SPEED * 0.4f;

                                side *= 0.75f;

                                Projectile.velocity = dir * along + perp * side;
                            }
                            else
                            {
                                Projectile.velocity *= 0.7f;
                            }
                        }
                        else
                        {
                            if (inApexPhase)
                            {
                                if (!apexLocked)
                                    ComputeApex(Main.MouseWorld);

                                Vector2 toApex = apexPos - Projectile.Center;
                                float   apexDist = toApex.Length();

                                if (apexDist < APEX_REACHED_DIST || Projectile.Center.Y >= apexPos.Y)
                                {
                                    inApexPhase = false;
                                }
                                else
                                {
                                    Vector2 apexDir = toApex / apexDist;
                                    Projectile.velocity += apexDir * APEX_ACCEL;
                                    float spd = Projectile.velocity.Length();
                                    if (spd > APEX_MAX_SPEED)
                                        Projectile.velocity = (Projectile.velocity / spd) * APEX_MAX_SPEED;
                                }
                            }
                            else if (dist > 0.5f)
                            {
                                Vector2 dir  = toMouse / dist;
                                Vector2 perp = new Vector2(-dir.Y, dir.X);

                                float along = Vector2.Dot(Projectile.velocity, dir);
                                float side  = Vector2.Dot(Projectile.velocity, perp);

                                along += RISE_ACCEL;

                                float speedCap = RISE_MAX_SPEED;
                                if (dist < RISE_BRAKE_DIST)
                                    speedCap = MathHelper.Lerp(RISE_MIN_SPEED, RISE_MAX_SPEED, dist / RISE_BRAKE_DIST);
                                if (along > speedCap)  along = speedCap;
                                if (along < -RISE_MAX_SPEED * 0.5f)  along = -RISE_MAX_SPEED * 0.5f;

                                side *= PERP_DAMP;

                                Projectile.velocity = dir * along + perp * side;
                            }
                        }
                        Projectile.netUpdate = true;
                    }
                    else
                    {
                        Projectile.velocity.Y += FREE_GRAVITY;
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    Projectile.velocity.Y += FREE_GRAVITY;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Projectile.velocity.Y += FREE_GRAVITY;
                if (Projectile.velocity.Y > 13f) Projectile.velocity.Y = 13f;
            }

            if (HitsSolidTile())
            {
                SplashDust();
                Projectile.Kill();
                return;
            }

            if (Projectile.velocity.LengthSquared() > 0.5f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(2))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Water, Projectile.velocity.X * 0.12f, Projectile.velocity.Y * 0.12f,
                    100, default, Main.rand.NextFloat(0.85f, 1.25f));
                Main.dust[d].noGravity = true;
            }
            Lighting.AddLight(Projectile.Center, 0.0f, 0.1f, 0.2f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (spawnTimer < SPAWN_GROW_FRAMES)
            {
                Texture2D tex   = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
                float     t     = (float)spawnTimer / SPAWN_GROW_FRAMES;
                float     scale = 1f - (1f - t) * (1f - t);
                Vector2   origin = new Vector2(tex.Width * 0.5f, tex.Height * 0.5f);
                Vector2   pos    = Projectile.Center - Main.screenPosition;

                Main.EntitySpriteDraw(tex, pos, null, lightColor * t, Projectile.rotation,
                    origin, scale, SpriteEffects.None, 0);
                return false;
            }
            return true;
        }

        private void SplashDust()
        {
            for (int i = 0; i < 10; i++)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Water, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-3f, -0.5f),
                    0, default, Main.rand.NextFloat(1f, 1.4f));
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.boss) target.velocity *= 0.3f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SplashDust();
            return true;
        }
    }
}
