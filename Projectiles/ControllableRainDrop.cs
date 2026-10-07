using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Projectiles.PlayerStands.NovemberRain;

namespace JoJoStands.Projectiles
{
    public class ControllableRainDrop : ModProjectile
    {
        public override string Texture => "JoJoStands/Projectiles/ControllableRainDrop";

        private const int   SUB_STEPS = 6;
        private const float STEP_SCALE = 1f / SUB_STEPS;
        private const float STEP_SCALE2 = 1f / (SUB_STEPS * SUB_STEPS);

        private const int   DEFAULT_SPAWN_GROW_FRAMES = 24;
        private const int   DROP_TIME_LEFT = 600;
        private int spawnGrowFrames = DEFAULT_SPAWN_GROW_FRAMES;

        private const float APEX_HORIZ_OFFSET = 28f;
        private const float APEX_VERT_OFFSET  = 55f;
        private const float APEX_REACHED_DIST = 16f;

        private const float APEX_ACCEL        = 2.6f;
        private const float APEX_MAX_SPEED    = 34f;

        private const float RISE_ACCEL        = 2.9f;
        private const float RISE_MAX_SPEED    = 39f;
        private const float RISE_BRAKE_DIST   = 50f;
        private const float RISE_MIN_SPEED    = 21f;
        private const float PERP_DAMP         = 0.9789f;

        private const float TRACK_ZONE        = 70f;
        private const float TRACK_ACCEL       = 18f;
        private const float TRACK_MAX_SPEED   = 90f;
        private const float TRACK_BRAKE_DIST  = 32f;
        private const float TRACK_MIN_SPEED   = 14f;
        private const float CURSOR_LOCK_DIST  = 12f;
        private const float CURSOR_TIP_Y_OFFSET = 3f;

        private const float FREE_GRAVITY      = 0.42f;
        private const float CONTROL_RANGE_T2   = 250f;
        private const float CONTROL_RANGE_T3   = 260f;
        private const float CONTROL_RANGE_T4   = 270f;

        private Vector2 spawnPos          = Vector2.Zero;
        private Vector2 apexPos           = Vector2.Zero;
        private bool    apexLocked        = false;
        private bool    inApexPhase       = true;
        private bool    reachedCursorZone = false;
        private bool    firstFrame        = true;
        private bool    wasMouseRight     = false;
        private int     spawnTimer        = 0;
        private bool    inWallPhase       = false;
        private float   controlRange      = CONTROL_RANGE_T3;
        private Vector2 prevCenter        = Vector2.Zero;

        private bool pendingKill = false;
        private int subStep = 0;

        private const float DROP_X_OFFSET = 17f;
        private const float DROP_Y_OFFSET = -21f;

        private Projectile FindOwnerStand()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile sp = Main.projectile[i];
                if (sp.active && sp.owner == Projectile.owner
                    && (sp.type == ModContent.ProjectileType<NovemberRainStandT1>()
                        || sp.type == ModContent.ProjectileType<NovemberRainStandT2>()
                        || sp.type == ModContent.ProjectileType<NovemberRainStandT3>()
                        || sp.type == ModContent.ProjectileType<NovemberRainStandFinal>()))
                    return sp;
            }
            return null;
        }

        private int GetSpawnGrowFrames(int standType)
        {
            if (standType == ModContent.ProjectileType<NovemberRainStandT2>()) return 28 * SUB_STEPS;
            if (standType == ModContent.ProjectileType<NovemberRainStandT3>()) return 20 * SUB_STEPS;
            if (standType == ModContent.ProjectileType<NovemberRainStandFinal>()) return 14 * SUB_STEPS;
            return DEFAULT_SPAWN_GROW_FRAMES * SUB_STEPS;
        }

        private float GetControlRange(int standType)
        {
            if (standType == ModContent.ProjectileType<NovemberRainStandT2>()) return CONTROL_RANGE_T2;
            if (standType == ModContent.ProjectileType<NovemberRainStandFinal>()) return CONTROL_RANGE_T4;
            return CONTROL_RANGE_T3;
        }

        public override void SetDefaults()
        {
            Projectile.width        = 14;
            Projectile.height       = 6;
            Projectile.friendly     = true;
            Projectile.hostile      = false;
            Projectile.penetrate    = 3;
            Projectile.timeLeft     = DROP_TIME_LEFT * SUB_STEPS;
            Projectile.ignoreWater  = true;
            Projectile.tileCollide  = true;
            Projectile.extraUpdates = SUB_STEPS - 1;
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
            float bottomCheckY = Projectile.position.Y + Projectile.height - 1f;
            int x0 = (int)(Projectile.position.X / 16f);
            int x1 = (int)((Projectile.position.X + Projectile.width) / 16f);
            int ty = (int)(bottomCheckY / 16f);
            for (int tx = x0; tx <= x1; tx++)
                if (IsTrueSolid(tx, ty)) return true;
            return false;
        }

        private void ComputeApex(Vector2 cursorWorld)
        {
            float dx = cursorWorld.X - spawnPos.X;
            float horizSign = dx >= 0f ? 1f : -1f;
            float horizOff = Math.Min(Math.Abs(dx) * 0.6f, APEX_HORIZ_OFFSET);
            if (horizOff < 2f) horizOff = 2f;
            float apexX = spawnPos.X + horizSign * horizOff;
            float apexY = spawnPos.Y + APEX_VERT_OFFSET;
            apexPos = new Vector2(apexX, apexY);
            apexLocked = true;
        }

        public override void AI()
        {
            if (pendingKill) { SplashDust(); Projectile.Kill(); return; }

            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead) { Projectile.Kill(); return; }
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (!mPlayer.standOut) { Projectile.Kill(); return; }

            prevCenter = Projectile.Center;
            subStep++;
            bool frameTick = (subStep % SUB_STEPS) == 0;

            if (firstFrame)
            {
                Projectile.velocity = Vector2.Zero;
                firstFrame = false;

                Projectile stand = FindOwnerStand();
                if (stand != null)
                {
                    spawnGrowFrames = GetSpawnGrowFrames(stand.type);
                    controlRange = GetControlRange(stand.type);
                }

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

            if (spawnTimer < spawnGrowFrames)
            {
                spawnTimer++;

                Projectile stand = FindOwnerStand();
                int dirSign = player.direction;
                Vector2 standPos = player.Center;
                if (stand != null)
                {
                    standPos = stand.Center;
                    dirSign = stand.spriteDirection;
                    Projectile.spriteDirection = stand.spriteDirection;
                    Projectile.direction = stand.spriteDirection;
                }
                spawnPos = new Vector2(
                    standPos.X + DROP_X_OFFSET * dirSign,
                    standPos.Y + DROP_Y_OFFSET);

                Projectile.Center = spawnPos;
                Projectile.velocity = Vector2.Zero;
                prevCenter = Projectile.Center;

                if (frameTick && Main.rand.NextBool(3))
                {
                    int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                        DustID.Water, 0f, 0f, 100, default, Main.rand.NextFloat(0.6f, 1.0f));
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0.3f;
                }
                if (frameTick) Lighting.AddLight(Projectile.Center, 0.05f, 0.10f, 0.18f);

                if (Projectile.owner == Main.myPlayer)
                    wasMouseRight = Main.mouseRight;
                if (spawnTimer == spawnGrowFrames)
                    inWallPhase = IsTrueSolid((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));
                return;
            }

            if (inWallPhase && !IsTrueSolid((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f)))
                inWallPhase = false;

            Projectile.tileCollide = !inWallPhase;


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
            if (!released) Projectile.timeLeft = DROP_TIME_LEFT * SUB_STEPS;

            if (Projectile.owner == Main.myPlayer)
            {
                if (!released)
                {
                    Vector2 standCenter = player.Center;
                    bool    inRange     = Vector2.Distance(Projectile.Center, standCenter) < controlRange;

                    if (!inRange)
                    {
                        Projectile.ai[0] = 1f;
                        Projectile.velocity.Y += FREE_GRAVITY * STEP_SCALE2;
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

                                along += TRACK_ACCEL * STEP_SCALE2;

                                float speedCap = TRACK_MAX_SPEED * STEP_SCALE;
                                if (dist < TRACK_BRAKE_DIST)
                                    speedCap = MathHelper.Lerp(TRACK_MIN_SPEED * STEP_SCALE, TRACK_MAX_SPEED * STEP_SCALE, dist / TRACK_BRAKE_DIST);
                                if (along > speedCap) along = speedCap;
                                if (along < -TRACK_MAX_SPEED * STEP_SCALE * 0.4f) along = -TRACK_MAX_SPEED * STEP_SCALE * 0.4f;

                                if (along > 0f && along > dist)
                                    along = dist;

                                side *= 0.75f;

                                Vector2 newVelocity = dir * along + perp * side;
                                Vector2 nextPos = Projectile.Center + newVelocity;
                                if (Vector2.Dot(cursorTarget - Projectile.Center, cursorTarget - nextPos) <= 0f)
                                {
                                    Projectile.Center = cursorTarget;
                                    Projectile.velocity = Vector2.Zero;
                                }
                                else
                                {
                                    Projectile.velocity = newVelocity;
                                }
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
                                    if (cursorTarget.Y > apexPos.Y)
                                        reachedCursorZone = true;
                                }
                                else
                                {
                                    Vector2 apexDir = toApex / apexDist;
                                    Projectile.velocity += apexDir * (APEX_ACCEL * STEP_SCALE2);
                                    float spd = Projectile.velocity.Length();
                                    if (spd > APEX_MAX_SPEED * STEP_SCALE)
                                        Projectile.velocity = (Projectile.velocity / spd) * (APEX_MAX_SPEED * STEP_SCALE);
                                }
                            }
                            else if (dist > 0.5f)
                            {
                                Vector2 dir  = toMouse / dist;
                                Vector2 perp = new Vector2(-dir.Y, dir.X);

                                float along = Vector2.Dot(Projectile.velocity, dir);
                                float side  = Vector2.Dot(Projectile.velocity, perp);

                                along += RISE_ACCEL * STEP_SCALE2;

                                float speedCap = RISE_MAX_SPEED * STEP_SCALE;
                                if (dist < RISE_BRAKE_DIST)
                                    speedCap = MathHelper.Lerp(RISE_MIN_SPEED * STEP_SCALE, RISE_MAX_SPEED * STEP_SCALE, dist / RISE_BRAKE_DIST);
                                if (along > speedCap)  along = speedCap;
                                if (along < -RISE_MAX_SPEED * STEP_SCALE * 0.5f)  along = -RISE_MAX_SPEED * STEP_SCALE * 0.5f;

                                side *= PERP_DAMP;

                                Projectile.velocity = dir * along + perp * side;
                            }
                        }
                        Projectile.netUpdate = true;
                    }
                    else
                    {
                        Projectile.velocity.Y += FREE_GRAVITY * STEP_SCALE2;
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    Projectile.velocity.Y += FREE_GRAVITY * STEP_SCALE2;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Projectile.velocity.Y += FREE_GRAVITY * STEP_SCALE2;
                if (Projectile.velocity.Y > 13f * STEP_SCALE) Projectile.velocity.Y = 13f * STEP_SCALE;
            }

            if (Projectile.velocity.LengthSquared() > 0.5f)
            {
                float tilt = MathHelper.Clamp(Projectile.velocity.X * 0.06f, -0.52f, 0.52f);
                Projectile.rotation = MathHelper.Lerp(Projectile.rotation, tilt, 0.15f);
            }
            else
                Projectile.rotation = MathHelper.Lerp(Projectile.rotation, 0f, 0.15f);

            if (frameTick && Main.rand.NextBool(2))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Water, Projectile.velocity.X * 0.12f, Projectile.velocity.Y * 0.12f,
                    100, default, Main.rand.NextFloat(0.85f, 1.25f));
                Main.dust[d].noGravity = true;
            }
            if (frameTick) Lighting.AddLight(Projectile.Center, 0.0f, 0.1f, 0.2f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex   = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2   origin = new Vector2(tex.Width * 0.5f, tex.Height * 0.5f);
            Vector2   pos    = Projectile.Center - Main.screenPosition;
            Color glowColor = new Color(220, 240, 255, 240);

            if (spawnTimer < spawnGrowFrames)
            {
                float t     = (float)spawnTimer / spawnGrowFrames;
                float scale = 1f - (1f - t) * (1f - t);
                Main.EntitySpriteDraw(tex, pos, null, glowColor * t, Projectile.rotation,
                    origin, scale, SpriteEffects.None, 0);
            }
            else
            {
                Main.EntitySpriteDraw(tex, pos, null, glowColor, Projectile.rotation,
                    origin, 1f, SpriteEffects.None, 0);
            }
            return false;
        }

        private void SplashDust()
        {
            for (int i = 0; i < 10; i++)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Water, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-3f, -0.5f),
                    0, default, Main.rand.NextFloat(1f, 1.4f));
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player owner = Main.player[Projectile.owner];
            MyPlayer mp = owner.GetModPlayer<MyPlayer>();
            bool crit = Main.rand.NextFloat(1, 100 + 1) <= mp.standCritChangeBoosts;
            if (crit) modifiers.SetCrit();
            modifiers.SourceDamage *= mp.standDamageBoosts;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.boss) target.velocity *= 0.3f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            pendingKill = true;
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox)) return true;
            if (prevCenter != Vector2.Zero)
            {
                float collisionPoint = 0f;
                if (Collision.CheckAABBvLineCollision(
                    new Vector2(targetHitbox.X, targetHitbox.Y),
                    new Vector2(targetHitbox.Width, targetHitbox.Height),
                    prevCenter, Projectile.Center,
                    Math.Max(Projectile.width, Projectile.height) * 0.5f,
                    ref collisionPoint))
                    return true;
            }
            return null;
        }

    }
}