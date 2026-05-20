using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.NovemberRain
{
    public class NovemberRainStandT1 : StandClass
    {
        public override int TierNumber => 1;
        public override string PoseSoundName => "";
        public override string SpawnSoundName => "";
        public override StandAttackType StandType => StandAttackType.Melee;
        public override int PunchDamage => 18;
        public override int PunchTime => 8;
        public override int HalfStandHeight => 96;
        public override float MaxDistance => 154f - Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standRangeBoosts * 0.5f;

        protected virtual float RAIN_W => 154f + Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standRangeBoosts * 0.5f;
        protected virtual float RAIN_DOWN => 280f + Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standRangeBoosts * 0.8f;
        protected virtual float RAIN_UP => 260f + Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standRangeBoosts * 0.4f;
        protected virtual float RAIN_SLOW => 0.65f;
        protected virtual float MISS_CHANCE => 0.15f;
        protected virtual int HIT_INTERVAL => 22;
        protected virtual int PRECISE_CD => 7;
        protected const float MAX_UPWARD_RATIO = 2.0f;
        protected const float STAND_Y_OFFSET = -73f;
        protected const float STAND_X_OFFSET = -20f;
        protected const float FIRE_X_OFFSET = 16.5f;
        protected const float FIRE_Y_OFFSET = -34f;
        protected const float CONE_HALF_W = 14f;
        protected const float CONE_HEIGHT = 18f;

        protected virtual int TRAP_SPAWN_TICKS => 0;
        protected virtual int TRAP_BASE_TICKS => 0;
        protected virtual int TRAP_MAX_TICKS => 0;
        protected const int MAX_TRAP_AREAS = 3;

        protected const int CEIL_TOTAL_FRAMES = 15;
        protected const int CEIL_FRAME_HEIGHT = 20;
        protected const int CEIL_FRAME_WIDTH = 18;

        protected const int CEIL_PUDDLE_ANIM_FRAMES = 5;
        protected const int CEIL_FALL_FRAMES = 3;
        protected const int CEIL_IMPACT_FRAMES = 5;

        protected const int CEIL_PUDDLE_TICKS_PER_FRAME = 18;
        protected const int CEIL_FALL_TICKS_TOTAL = 36;
        protected const int CEIL_IMPACT_TICKS_PER_FRAME = 8;

        protected const int CEIL_OVERLAY_TICKS_PER_FRAME = 9;

        protected const int TRAP_PUDDLE_END = CEIL_PUDDLE_ANIM_FRAMES * CEIL_PUDDLE_TICKS_PER_FRAME;
        protected const int TRAP_DROP_END = TRAP_PUDDLE_END + CEIL_FALL_TICKS_TOTAL;
        protected const int TRAP_CYCLE = TRAP_DROP_END + CEIL_IMPACT_FRAMES * CEIL_IMPACT_TICKS_PER_FRAME;

        protected int preciseTimer = 0;
        private int visualTimer = 0;
        private int trapFormTimer = 0;

        protected int[] npcTimers = new int[Main.maxNPCs];
        protected bool[] npcWasInArea = new bool[Main.maxNPCs];
        protected int[] npcStunTimers = new int[Main.maxNPCs];

        public enum SurfaceType { Floor = 0, Ceiling = 1, BackWall = 2 }

        public class TrapSurface
        {
            public Vector2 WorldPos;
            public SurfaceType Type;
            public int DripTimer;
            public int DripTimerOffset;
            public bool Triggered;
            public int DripLandY;
            public bool DripLandFound;
        }

        public class TrapArea
        {
            public Vector2 Center;
            public int DurationTicks;
            public int BaseDuration;
            public int MaxDuration;
            public List<TrapSurface> Surfaces = new List<TrapSurface>();
            public bool PlayerNearby;
            public int LayersUsed;
            public int LayerTimer;
            public bool ExtraLayerAdded => LayersUsed > 1;
            public bool IsPlaceholder;
            public float FadeOffsetY;
        }

        protected List<TrapArea> ActiveTraps = new List<TrapArea>();

        protected bool InRainDome(Vector2 pos, float w, float down, float up, Vector2 center)
        {
            float dx = Math.Abs(pos.X - center.X);
            float dy = pos.Y - center.Y;
            if (dy <= 0) { float nx = dx / w; float ny = (-dy) / up; return nx * nx + ny * ny < 1f; }
            return dx < w && dy < down;
        }

        protected int FindDripLandY(Vector2 startWorld)
        {
            int tx = (int)(startWorld.X / 16f);
            int startTY = (int)(startWorld.Y / 16f) + 1;
            for (int ty = startTY; ty < Main.maxTilesY - 1; ty++)
            {
                if (tx < 0 || tx >= Main.maxTilesX) break;
                var t = Main.tile[tx, ty];
                if (t.HasTile && (Main.tileSolid[t.TileType] || TileID.Sets.Platforms[t.TileType]))
                    return ty * 16;
            }
            return (Main.maxTilesY - 2) * 16;
        }

        private void HitNPCWithAccessories(Player player, MyPlayer mPlayer, NPC npc, int baseDmg, int direction)
        {
            bool crit = Main.rand.Next(100) < player.GetTotalCritChance<MeleeDamageClass>();
            if (mPlayer.underbossPhoneEquipped)
            {
                mPlayer.underbossPhoneCount++;
                if (mPlayer.underbossPhoneCount >= 5)
                {
                    mPlayer.underbossPhoneCount = 0;
                    int bonusDmg = (int)(baseDmg * 11.1f);
                    player.ApplyDamageToNPC(npc, bonusDmg, 0f, direction, true, DamageClass.Generic);
                    for (int d = 0; d < 6; d++)
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.TreasureSparkle, 0f, -2f, 0, default, 1.2f);
                    return;
                }
            }
            if (mPlayer.iceCreamEquipped)
            {
                mPlayer.iceCreamEnemyHitCount++;
                if (mPlayer.iceCreamEnemyHitCount >= 8)
                {
                    mPlayer.iceCreamEnemyHitCount = 0;
                    crit = true;
                    baseDmg += npc.defense;
                }
            }
            player.ApplyDamageToNPC(npc, baseDmg, 0.8f, direction, crit, DamageClass.Generic);
        }

        public override void ExtraSpawnEffects()
        {
            if (Main.netMode == NetmodeID.Server) return;
            int idx = Main.rand.Next(1, 4);
            SoundStyle spawnSound = new SoundStyle("JoJoStandsSounds/Sounds/SummonCries/NovemberRain" + idx)
            {
                Volume = global::JoJoStands.JoJoStands.ModSoundsVolume
            };
            SoundEngine.PlaySound(spawnSound, Projectile.Center);
        }

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut) Projectile.timeLeft = 2;
            if (preciseTimer > 0) preciseTimer--;

            SnapAbovePlayer(player);
            ApplyStuns();
            UpdateTraps(mPlayer, player);
            CheckTrapTriggers(mPlayer);

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                currentAnimationState = AnimationState.Idle;
                RainVisuals(RAIN_W, RAIN_DOWN, RAIN_UP, player.Center);
                AreaDamage(mPlayer, player, RAIN_W, RAIN_DOWN, RAIN_UP, HIT_INTERVAL, RAIN_SLOW, player.Center);
                if (TRAP_SPAWN_TICKS > 0) BuildTraps(player);
            }
            else
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (PlayerLeftClick() && preciseTimer <= 0)
                    {
                        currentAnimationState = AnimationState.Idle;
                        FireThreeStreams(mPlayer);
                        preciseTimer = Math.Max(PRECISE_CD - mPlayer.standSpeedBoosts / 2, 2);
                    }
                    else currentAnimationState = AnimationState.Idle;
                }
            }
            if (mPlayer.posing) currentAnimationState = AnimationState.Pose;
        }

        protected void SnapAbovePlayer(Player player)
        {

            float standCenterY = (player.position.Y + player.height) + STAND_FROM_FEET_Y;

            Projectile.Center = new Vector2(
                player.Center.X + STAND_X_OFFSET * player.direction,
                standCenterY);

            Projectile.velocity = Vector2.Zero;
            Projectile.direction = player.direction;
            Projectile.spriteDirection = player.direction;
        }

        protected const float STAND_FROM_FEET_Y = -94f;

        // Precise Rain
        protected void FirePrecise(MyPlayer mPlayer)
        {
            if (Projectile.owner != Main.myPlayer) return;
            Vector2 coneBase = Projectile.Center + new Vector2(FIRE_X_OFFSET * Projectile.spriteDirection, FIRE_Y_OFFSET);
            float randX = Main.rand.NextFloat(-CONE_HALF_W, CONE_HALF_W);
            float randY = -(Math.Abs(randX) / CONE_HALF_W) * CONE_HEIGHT;
            Vector2 firePos = coneBase + new Vector2(randX, randY);

            Player owner = Main.player[Projectile.owner];
            float maxAimY = owner.Center.Y - 150f;
            Vector2 aimWorld = Main.MouseWorld;
            if (aimWorld.Y < maxAimY) aimWorld.Y = maxAimY;

            Vector2 dir = aimWorld - firePos;
            if (dir == Vector2.Zero) dir = new Vector2(0f, 1f);
            if (dir.Y < 0)
            {
                float limit = -Math.Abs(dir.X) * MAX_UPWARD_RATIO;
                if (dir.Y < limit) dir.Y = limit;
                if (Math.Abs(dir.X) < 5f) dir.Y = Math.Max(dir.Y, -1.5f);
            }
            dir.Normalize();

            const float SHOT_SPEED = 17f;
            const float GRAVITY = 0.92f;
            float vy0 = dir.Y * SHOT_SPEED;
            if (vy0 < 0f)
            {
                float peakY = firePos.Y - (vy0 * vy0) / (2f * GRAVITY);
                if (peakY < maxAimY)
                {
                    float allowed = firePos.Y - maxAimY;
                    if (allowed < 0f) allowed = 0f;
                    float maxVy = -(float)Math.Sqrt(2f * GRAVITY * allowed);
                    float vx0 = dir.X * SHOT_SPEED;
                    Vector2 newVel = new Vector2(vx0, maxVy);
                    int idx2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), firePos, newVel,
                        ModContent.ProjectileType<PreciseRainDrop>(), newPunchDamage, 2f, Main.myPlayer);
                    Main.projectile[idx2].netUpdate = true;
                    return;
                }
            }

            int idx = Projectile.NewProjectile(Projectile.GetSource_FromThis(), firePos, dir * SHOT_SPEED,
                ModContent.ProjectileType<PreciseRainDrop>(), newPunchDamage, 2f, Main.myPlayer);
            Main.projectile[idx].netUpdate = true;
        }

        // Three Streams
        protected void FireThreeStreams(MyPlayer mPlayer)
        {
            if (Projectile.owner != Main.myPlayer) return;

            float anchorX = Projectile.Center.X + FIRE_X_OFFSET * Projectile.spriteDirection;
            float anchorY = Projectile.Center.Y;
            Vector2 coneBase = new Vector2(anchorX, anchorY + FIRE_Y_OFFSET);

            const float WIDE_HALF = 18f;
            const float TOP_Y = -8f;
            float APEX_Y = CONE_HEIGHT;

            float u = Main.rand.NextFloat();
            float startX = Main.rand.NextBool() ? -WIDE_HALF : WIDE_HALF;
            float spawnX = MathHelper.Lerp(startX, 0f, u);
            float spawnY = MathHelper.Lerp(TOP_Y, APEX_Y, u);
            Vector2 spawn = coneBase + new Vector2(spawnX, spawnY);

            const float SHOT_SPEED = 17f;
            const float MAX_BALLISTIC = 60f;
            const float MAX_UPWARD_SPEED = 11f;
            const float G = 0.32f;
            const float SPEED_MULT = 1.7f;

            Player owner = Main.player[Projectile.owner];
            float maxAimY = owner.Center.Y - 150f;
            Vector2 aimWorld = Main.MouseWorld;
            if (aimWorld.Y < maxAimY) aimWorld.Y = maxAimY;

            Vector2 toCursor = aimWorld - coneBase;
            if (toCursor == Vector2.Zero) toCursor = new Vector2(0f, 1f);

            if (toCursor.Y < 0)
            {
                float ax = Math.Abs(toCursor.X);
                if (toCursor.Y < -ax * MAX_UPWARD_RATIO)
                {
                    float mag = toCursor.Length();
                    float signX = Math.Sign(toCursor.X);
                    if (signX == 0f)
                        signX = (aimWorld.X >= anchorX) ? 1f : -1f;
                    float r = MAX_UPWARD_RATIO;
                    float invH = 1f / (float)Math.Sqrt(1f + r * r);
                    toCursor = new Vector2(mag * invH * signX, -mag * r * invH);
                }
            }

            Vector2 target = coneBase + toCursor;
            Vector2 toTarget = target - spawn;
            float dist = toTarget.Length();
            float T_fly = MathHelper.Clamp(dist / SHOT_SPEED, 4f, 28f);
            float vx = toTarget.X / T_fly;
            float vy = toTarget.Y / T_fly - 0.5f * G * T_fly;
            Vector2 vel = new Vector2(vx, vy);

            float spd = vel.Length();
            if (spd > MAX_BALLISTIC) vel *= MAX_BALLISTIC / spd;
            if (spd < 4f && dist > 1f) vel = vel / Math.Max(spd, 0.001f) * 4f;

            if (vel.Y < -MAX_UPWARD_SPEED) vel.Y = -MAX_UPWARD_SPEED;

            vel *= SPEED_MULT;

            float vyFinal = vel.Y;
            if (vyFinal < 0f)
            {
                float gPerFrame = G * SPEED_MULT * SPEED_MULT;
                float peakY = spawn.Y - (vyFinal * vyFinal) / (2f * gPerFrame);
                if (peakY < maxAimY)
                {
                    float allowed = spawn.Y - maxAimY;
                    if (allowed < 0f) allowed = 0f;
                    vel.Y = -(float)Math.Sqrt(2f * gPerFrame * allowed);
                }
            }

            int idx = Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawn, vel,
                ModContent.ProjectileType<PreciseRainDrop>(), newPunchDamage, 2f, Main.myPlayer);
            if (idx >= 0 && idx < Main.maxProjectiles)
                Main.projectile[idx].netUpdate = true;
        }

        // Controllable Drop
        protected void FireControllableDrop(MyPlayer mPlayer)
        {
            if (Projectile.owner != Main.myPlayer) return;
            Vector2 firePos = Projectile.Center + new Vector2(FIRE_X_OFFSET * Projectile.spriteDirection, FIRE_Y_OFFSET + 3f);
            Vector2 toCursor = Main.MouseWorld - firePos;
            if (toCursor != Vector2.Zero) toCursor.Normalize();
            int dmg = (int)(newPunchDamage * 3.5f);
            int idx = Projectile.NewProjectile(Projectile.GetSource_FromThis(), firePos, new Vector2(toCursor.X * 2f, 1.5f),
                ModContent.ProjectileType<ControllableRainDrop>(), dmg, 3f, Main.myPlayer);
            Main.projectile[idx].netUpdate = true;
        }

        // Rain Visuals
        protected void RainVisuals(float w, float down, float up, Vector2 center)
        {
            visualTimer++;
            if (visualTimer < 2) return;
            visualTimer = 0;
            Vector2 c = center;
            for (int i = 0; i < 6; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.Pi, MathHelper.TwoPi);
                float r = Main.rand.NextFloat(0.5f, 1.0f);
                int d = Dust.NewDust(new Vector2(c.X + (float)Math.Cos(angle) * w * r, c.Y + (float)Math.Sin(angle) * up * r),
                    4, 18, DustID.Water, Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(12f, 18f), 100, default, Main.rand.NextFloat(0.9f, 1.2f));
                Main.dust[d].noGravity = true;
            }
            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(new Vector2(c.X + Main.rand.NextFloat(-w, w), c.Y + Main.rand.NextFloat(0f, down * 0.6f)),
                    4, 18, DustID.Water, Main.rand.NextFloat(-0.2f, 0.2f), Main.rand.NextFloat(12f, 18f), 120, default, Main.rand.NextFloat(0.85f, 1.15f));
                Main.dust[d].noGravity = true;
            }
            for (int i = 0; i < 3; i++)
            {
                float a2 = Main.rand.NextFloat(MathHelper.Pi, MathHelper.TwoPi);
                float r2 = Main.rand.NextFloat(0.35f, 0.8f);
                int d = Dust.NewDust(new Vector2(c.X + (float)Math.Cos(a2) * w * r2, c.Y + (float)Math.Sin(a2) * up * r2),
                    3, 11, DustID.Water, Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.NextFloat(8f, 13f), 160, default, Main.rand.NextFloat(0.55f, 0.75f));
                Main.dust[d].noGravity = true;
            }
            for (int i = 0; i < 3; i++)
            {
                int d = Dust.NewDust(new Vector2(c.X + Main.rand.NextFloat(-w * 0.65f, w * 0.65f), c.Y + Main.rand.NextFloat(0f, down * 0.85f)),
                    2, 6, DustID.Water, 0f, Main.rand.NextFloat(5f, 9f), 200, default, Main.rand.NextFloat(0.3f, 0.48f));
                Main.dust[d].noGravity = true;
            }
            Lighting.AddLight(c, 0.05f, 0.12f, 0.25f);
        }

        // Area Damage
        protected void AreaDamage(MyPlayer mPlayer, Player player, float w, float down, float up, int baseInterval, float slow, Vector2 center)
        {
            if (Projectile.owner != Main.myPlayer) return;
            int interval = Math.Max(baseInterval - mPlayer.standSpeedBoosts, 4);
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.dontTakeDamage)
                { npcTimers[i] = 0; npcWasInArea[i] = false; continue; }
                bool inArea = InRainDome(npc.Center, w, down, up, center);
                if (inArea)
                {
                    if (!npcWasInArea[i]) { npcWasInArea[i] = true; npcTimers[i] = interval - 1; if (!npc.boss && !npc.immortal) npc.velocity *= slow; }
                    npcTimers[i]++;
                    if (npcTimers[i] >= interval)
                    {
                        npcTimers[i] = 0;
                        if (Main.rand.NextFloat() < MISS_CHANCE) continue;
                        if (!npc.boss && !npc.immortal) npc.velocity *= slow;
                        HitNPCWithAccessories(player, mPlayer, npc, newPunchDamage, Projectile.direction);
                        if (!npc.immortal) npc.velocity += new Vector2(Projectile.direction * 0.8f, 0.4f);
                        for (int d = 0; d < 4; d++)
                            Dust.NewDust(npc.position, npc.width, npc.height, DustID.Water, Main.rand.NextFloat(-2f, 2f), -1.5f, 0, default, 1f);
                        Projectile.netUpdate = true;
                    }
                }
                else { npcTimers[i] = 0; npcWasInArea[i] = false; }
            }
        }

        private bool IsPlatform(int tx, int ty)
        {
            if (tx < 0 || tx >= Main.maxTilesX || ty < 0 || ty >= Main.maxTilesY) return false;
            var t = Main.tile[tx, ty];
            return t.HasTile && TileID.Sets.Platforms[t.TileType];
        }

        private bool IsSolid(int tx, int ty)
        {
            if (tx < 0 || tx >= Main.maxTilesX || ty < 0 || ty >= Main.maxTilesY) return false;
            var t = Main.tile[tx, ty];
            return t.HasTile && (Main.tileSolid[t.TileType] || TileID.Sets.Platforms[t.TileType]);
        }

        private bool IsAir(int tx, int ty)
        {
            if (tx < 0 || tx >= Main.maxTilesX || ty < 0 || ty >= Main.maxTilesY) return true;
            var t = Main.tile[tx, ty];
            return !t.HasTile || (!Main.tileSolid[t.TileType] && !TileID.Sets.Platforms[t.TileType]);
        }

        private bool HasWall(int tx, int ty)
        {
            if (tx < 0 || tx >= Main.maxTilesX || ty < 0 || ty >= Main.maxTilesY) return false;
            return Main.tile[tx, ty].WallType > 0;
        }

        // Trap
        protected List<TrapSurface> ScanSurfaces(Vector2 domeCenter, float w, float down, float up, Vector2 playerCenter)
        {
            return ScanSurfacesLimited(domeCenter, w, down, up, playerCenter, MaxFloorCeilingTraps);
        }

        protected virtual int MaxFloorCeilingTraps => 6;
        protected virtual float FloorTrapBias => 0.60f;

        protected List<TrapSurface> ScanSurfacesLimited(Vector2 domeCenter, float w, float down, float up, Vector2 playerCenter, int maxFC)
        {
            return ScanSurfacesLimited(domeCenter, w, down, up, playerCenter, maxFC, null);
        }

        protected List<TrapSurface> ScanSurfacesLimited(Vector2 domeCenter, float w, float down, float up, Vector2 playerCenter, int maxFC, HashSet<long> excludeTileKeys)
        {
            var floors = new List<TrapSurface>();
            var ceilings = new List<TrapSurface>();
            var backWalls = new List<TrapSurface>();
            int cx = (int)(domeCenter.X / 16f), cy = (int)(domeCenter.Y / 16f);
            int rX = (int)(w / 16f) + 2, rDown = (int)(down / 16f) + 2, rUp = (int)(up / 16f) + 2;
            int playerTileY = (int)(playerCenter.Y / 16f);

            for (int tx = cx - rX; tx <= cx + rX; tx++)
            {
                for (int ty = cy - rUp; ty <= cy + rDown; ty++)
                {
                    if (tx < 0 || tx >= Main.maxTilesX || ty < 0 || ty >= Main.maxTilesY) continue;
                    Vector2 tilePos = new Vector2(tx * 16f + 8f, ty * 16f + 8f);
                    if (!InRainDome(tilePos, w, down, up, domeCenter)) continue;
                    long key = ((long)tx << 20) | (long)ty;
                    if (excludeTileKeys != null && excludeTileKeys.Contains(key)) continue;
                    bool solid = IsSolid(tx, ty);
                    if (solid)
                    {
                        if (ty >= playerTileY && IsAir(tx, ty - 1))
                        {
                            floors.Add(new TrapSurface
                            {
                                WorldPos = new Vector2(tx * 16f + 8f, ty * 16f + 1f),
                                Type = SurfaceType.Floor,
                                DripTimerOffset = Main.rand.Next(0, TRAP_CYCLE)
                            });
                        }
                        else if (ty < playerTileY && IsAir(tx, ty + 1))
                        {
                            bool isPlatformTile = IsPlatform(tx, ty);
                            float ceilBottomY = isPlatformTile ? ty * 16f + 8f + 1f : ty * 16f + 16f + 1f;
                            int landY = FindDripLandY(new Vector2(tx * 16f + 8f, ceilBottomY));
                            ceilings.Add(new TrapSurface
                            {
                                WorldPos = new Vector2(tx * 16f + 8f, ceilBottomY),
                                Type = SurfaceType.Ceiling,
                                DripLandY = landY,
                                DripLandFound = true,
                                DripTimerOffset = Main.rand.Next(0, TRAP_CYCLE)
                            });
                        }
                    }
                    else if (!solid && HasWall(tx, ty))
                    {
                        int landY = FindDripLandY(new Vector2(tx * 16f + 8f, ty * 16f + 16f));
                        backWalls.Add(new TrapSurface
                        {
                            WorldPos = new Vector2(tx * 16f + 8f, ty * 16f + 8f),
                            Type = SurfaceType.BackWall,
                            DripTimerOffset = Main.rand.Next(0, TRAP_CYCLE),
                            DripLandY = landY,
                            DripLandFound = true
                        });
                    }
                }
            }

            var selected = SelectFloorCeilingMix(floors, ceilings, maxFC);

            var output = new List<TrapSurface>(selected.Count + backWalls.Count);
            output.AddRange(selected);
            output.AddRange(backWalls);
            return output;
        }

        protected List<TrapSurface> SelectFloorCeilingMix(List<TrapSurface> floors, List<TrapSurface> ceilings, int limit)
        {
            var picked = new List<TrapSurface>();
            var fPool = new List<TrapSurface>(floors);
            var cPool = new List<TrapSurface>(ceilings);

            int total = fPool.Count + cPool.Count;
            int target = Math.Min(limit, total);
            if (target <= 0) return picked;

            int wantFloors;
            int wantCeilings;
            if (cPool.Count == 0)
            {
                wantFloors = Math.Min(target, fPool.Count);
                wantCeilings = 0;
            }
            else if (fPool.Count == 0)
            {
                wantFloors = 0;
                wantCeilings = Math.Min(target, cPool.Count);
            }
            else
            {
                wantFloors = (int)Math.Round(target * FloorTrapBias);
                wantCeilings = target - wantFloors;

                if (wantFloors > fPool.Count)
                {
                    wantCeilings += wantFloors - fPool.Count;
                    wantFloors = fPool.Count;
                }
                if (wantCeilings > cPool.Count)
                {
                    wantFloors += wantCeilings - cPool.Count;
                    wantCeilings = cPool.Count;
                }
                wantFloors = Math.Min(wantFloors, fPool.Count);
                wantCeilings = Math.Min(wantCeilings, cPool.Count);
            }

            for (int i = 0; i < wantFloors; i++)
            {
                int idx = Main.rand.Next(fPool.Count);
                picked.Add(fPool[idx]);
                fPool.RemoveAt(idx);
            }
            for (int i = 0; i < wantCeilings; i++)
            {
                int idx = Main.rand.Next(cPool.Count);
                picked.Add(cPool[idx]);
                cPool.RemoveAt(idx);
            }
            return picked;
        }

        // Trap Build
        protected void BuildTraps(Player player)
        {
            if (Projectile.owner != Main.myPlayer) return;

            TrapArea hostArea = null;
            foreach (var existing in ActiveTraps)
            {
                if (existing.IsPlaceholder) continue;
                if (InRainDome(player.Center, RAIN_W * 0.85f, RAIN_DOWN * 0.85f, RAIN_UP * 0.85f, existing.Center))
                {
                    hostArea = existing;
                    break;
                }
            }

            if (hostArea != null)
            {
                if (hostArea.LayersUsed >= MAX_TRAP_AREAS) return;
                if (ActiveTraps.Count >= MAX_TRAP_AREAS) return;

                var occupied = new HashSet<long>();
                foreach (var s in hostArea.Surfaces)
                {
                    if (s.Type == SurfaceType.BackWall) continue;
                    int tx = (int)(s.WorldPos.X / 16f);
                    int ty = (int)(s.WorldPos.Y / 16f);
                    occupied.Add(((long)tx << 20) | (long)ty);
                }

                var preview = ScanSurfacesLimited(hostArea.Center, RAIN_W, RAIN_DOWN, RAIN_UP,
                                                  player.Center, MaxFloorCeilingTraps, occupied);
                preview.RemoveAll(s => s.Type == SurfaceType.BackWall);
                if (preview.Count == 0)
                {
                    hostArea.LayersUsed = MAX_TRAP_AREAS;
                    return;
                }

                hostArea.LayerTimer++;
                if (hostArea.LayerTimer < TRAP_SPAWN_TICKS) return;
                hostArea.LayerTimer = 0;

                var extra = preview;
                if (extra.Count == 0) return;

                hostArea.Surfaces.AddRange(extra);
                hostArea.LayersUsed++;

                ActiveTraps.Add(new TrapArea
                {
                    Center = hostArea.Center,
                    DurationTicks = hostArea.DurationTicks,
                    BaseDuration = hostArea.BaseDuration,
                    MaxDuration = hostArea.MaxDuration,
                    Surfaces = new List<TrapSurface>(),
                    PlayerNearby = true,
                    LayersUsed = MAX_TRAP_AREAS,
                    IsPlaceholder = true
                });

                Projectile.netUpdate = true;
                for (int d = 0; d < 8; d++)
                    Dust.NewDust(hostArea.Center + new Vector2(Main.rand.NextFloat(-RAIN_W * 0.5f, RAIN_W * 0.5f), Main.rand.NextFloat(-20f, 20f)),
                        8, 8, DustID.Water, 0f, -1f, 0, default, 1.0f);
                return;
            }

            if (ActiveTraps.Count >= MAX_TRAP_AREAS) return;

            float minSeparation = RAIN_W * 1.6f;
            foreach (var a in ActiveTraps)
            {
                if (a.IsPlaceholder) continue;
                if (Vector2.DistanceSquared(player.Center, a.Center) < minSeparation * minSeparation)
                    return;
            }

            trapFormTimer++;
            if (trapFormTimer < TRAP_SPAWN_TICKS) return;
            trapFormTimer = 0;
            var surfaces = ScanSurfaces(player.Center, RAIN_W, RAIN_DOWN, RAIN_UP, player.Center);
            if (surfaces.Count == 0) return;
            ActiveTraps.Add(new TrapArea
            {
                Center = player.Center,
                DurationTicks = TRAP_BASE_TICKS,
                BaseDuration = TRAP_BASE_TICKS,
                MaxDuration = TRAP_MAX_TICKS,
                Surfaces = surfaces,
                PlayerNearby = true,
                LayersUsed = 1
            });
            Projectile.netUpdate = true;
            for (int d = 0; d < 10; d++)
                Dust.NewDust(player.Center + new Vector2(Main.rand.NextFloat(-RAIN_W * 0.5f, RAIN_W * 0.5f), Main.rand.NextFloat(-20f, 20f)),
                    8, 8, DustID.Water, 0f, -1f, 0, default, 1.2f);
        }

        protected virtual bool IsAreaAbilityActive(MyPlayer mPlayer) =>
            mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto;

        // Trap Update
        protected void UpdateTraps(MyPlayer mPlayer, Player player)
        {
            if (Projectile.owner != Main.myPlayer) return;
            bool abilityActive = IsAreaAbilityActive(mPlayer);
            for (int i = ActiveTraps.Count - 1; i >= 0; i--)
            {
                var trap = ActiveTraps[i];

                if (trap.IsPlaceholder)
                {
                    bool hostStillExists = false;
                    foreach (var other in ActiveTraps)
                    {
                        if (!other.IsPlaceholder && other.Center == trap.Center)
                        { hostStillExists = true; break; }
                    }
                    if (!hostStillExists)
                    { ActiveTraps.RemoveAt(i); trapFormTimer = 0; continue; }
                    continue;
                }

                trap.Surfaces.RemoveAll(s => s.Triggered);
                if (trap.Surfaces.Count == 0)
                { ActiveTraps.RemoveAt(i); trapFormTimer = 0; continue; }

                bool nearby = abilityActive &&
                    InRainDome(player.Center, RAIN_W * 1.05f, RAIN_DOWN * 1.05f, RAIN_UP * 1.05f, trap.Center);

                if (nearby)
                {
                    if (!trap.PlayerNearby && trap.DurationTicks < trap.BaseDuration)
                        trap.DurationTicks = trap.BaseDuration;
                    trap.PlayerNearby = true;
                    trap.DurationTicks = Math.Min(trap.DurationTicks + 1, trap.MaxDuration);
                    trap.FadeOffsetY = 0f;
                }
                else
                {
                    trap.PlayerNearby = false;
                    trap.DurationTicks -= 2;
                    if (trap.DurationTicks <= 0) { ActiveTraps.RemoveAt(i); trapFormTimer = 0; continue; }
                }

                foreach (var surf in trap.Surfaces)
                {
                    if (surf.Triggered) continue;
                    if (surf.Type == SurfaceType.Ceiling)
                    {
                        surf.DripTimer++;
                        int phase = (surf.DripTimer + surf.DripTimerOffset) % TRAP_CYCLE;
                        if (phase == TRAP_DROP_END)
                        {
                            int landY = surf.DripLandFound ? surf.DripLandY : (Main.maxTilesY - 2) * 16;
                            for (int n = 0; n < Main.maxNPCs; n++)
                            {
                                NPC npc = Main.npc[n];
                                if (!npc.active || npc.friendly || npc.dontTakeDamage) continue;
                                if (Math.Abs(npc.Center.X - surf.WorldPos.X) < 40f &&
                                    npc.Center.Y > surf.WorldPos.Y &&
                                    npc.Center.Y < landY + 32f)
                                {
                                    bool crit = Main.rand.Next(100) < player.GetTotalCritChance<MeleeDamageClass>();
                                    player.ApplyDamageToNPC(npc, (int)(newPunchDamage * 0.5f), 0f, Projectile.direction, crit, DamageClass.Generic);
                                    Dust.NewDust(new Vector2(surf.WorldPos.X, landY), 16, 8, DustID.Water,
                                        Main.rand.NextFloat(-3f, 3f), -2f, 0, default, 1.2f);
                                    Projectile.netUpdate = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Trap Triggers
        protected void CheckTrapTriggers(MyPlayer mPlayer)
        {
            if (Projectile.owner != Main.myPlayer) return;
            Player player = Main.player[Projectile.owner];
            foreach (var trap in ActiveTraps)
            {
                foreach (var surf in trap.Surfaces)
                {
                    if (surf.Triggered) continue;
                    if (surf.Type == SurfaceType.BackWall) continue;
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (!npc.active || npc.friendly || npc.dontTakeDamage) continue;
                        float ndx = Math.Abs(npc.Center.X - surf.WorldPos.X);
                        float ndy = npc.Center.Y - surf.WorldPos.Y;
                        float halfW = Math.Max(npc.width / 2f, 20f);
                        float halfH = Math.Max(npc.height / 2f, 20f);
                        bool hit = false;
                        switch (surf.Type)
                        {
                            case SurfaceType.Floor: hit = ndx < halfW + 16f && ndy > -(halfH + 24f) && ndy < 16f; break;
                            case SurfaceType.Ceiling: hit = ndx < halfW + 16f && ndy > -8f && ndy < halfH + 28f; break;
                            case SurfaceType.BackWall: hit = ndx < halfW + 8f && Math.Abs(ndy) < halfH + 8f; break;
                        }
                        if (hit)
                        {
                            surf.Triggered = true;
                            player.ApplyDamageToNPC(npc, (int)(newPunchDamage * 3f), 2f, Projectile.direction, false, DamageClass.Generic);
                            if (!npc.boss) npcStunTimers[n] = 90;
                            for (int d = 0; d < 14; d++)
                                Dust.NewDust(surf.WorldPos, 16, 16, DustID.Water,
                                    Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 0f), 0, default, 1.1f);
                            Projectile.netUpdate = true;
                            break;
                        }
                    }
                }
            }
            for (int i = ActiveTraps.Count - 1; i >= 0; i--)
                ActiveTraps[i].Surfaces.RemoveAll(s => s.Triggered);
        }

        // Stun
        protected void ApplyStuns()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (npcStunTimers[i] > 0)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.boss) npc.velocity *= 0.05f;
                    npcStunTimers[i]--;
                }
            }
        }

        // Back-wall rain
        private const float BW_SPEED_MIN = 0.8f;
        private const float BW_SPEED_MAX = 4.0f;
        private const int BW_SPAWN_INTERVAL = 200;
        private const int BW_PHASE_SPREAD = 10;

        private static float BWRHash(int x, int y, int salt)
        {
            uint h = (uint)(x * 374761393) ^ (uint)(y * 668265263) ^ (uint)(salt * 2147483647);
            h = (h ^ (h >> 13)) * 1274126177u;
            h ^= h >> 16;
            return (h & 0xFFFFFF) / (float)0x1000000;
        }

        // Back-wall Rain
        private void DrawBackWallRain(Texture2D tex, Vector2 startWorldPos, int landY, bool landFound, float remain, bool fromCeiling)
        {
            int tileX = (int)((startWorldPos.X - 8f) / 16f);
            int tileY = (int)((startWorldPos.Y - 8f) / 16f);

            float startY = fromCeiling ? startWorldPos.Y - 2f : startWorldPos.Y - 8f;
            float floorY = landFound ? landY - tex.Height : float.MaxValue;

            int now = (int)Main.GameUpdateCount;
            int phaseOffset = (int)(BWRHash(tileX, tileY, 10) * BW_SPAWN_INTERVAL);
            int startCycle = now >= phaseOffset ? (now - phaseOffset) / BW_SPAWN_INTERVAL : 0;

            for (int di = 0; di <= BW_PHASE_SPREAD; di++)
            {
                int cycleNum = startCycle - di;
                if (cycleNum < 0) break;

                int dropStartTick = cycleNum * BW_SPAWN_INTERVAL + phaseOffset;
                int fallPhase = now - dropStartTick;
                if (fallPhase < 0) continue;

                float speedR = BWRHash(tileX * 7 + cycleNum, tileY * 11 + cycleNum, 5);
                float pxPerTick = BW_SPEED_MIN + speedR * (BW_SPEED_MAX - BW_SPEED_MIN);

                float dropY = startY + pxPerTick * fallPhase;

                if (dropY >= floorY)
                {
                    float prevY = startY + pxPerTick * (fallPhase - 1);
                    if (prevY < floorY && landFound && Projectile.owner == Main.myPlayer)
                    {
                        for (int d = 0; d < 2; d++)
                            Dust.NewDust(new Vector2(startWorldPos.X, landY), 6, 4, DustID.Water,
                                Main.rand.NextFloat(-1.5f, 1.5f), -0.8f, 0, default, 0.8f);
                    }
                    continue;
                }

                float xScatterR = BWRHash(tileX * 3 + cycleNum, tileY * 5 + cycleNum, 7);
                float dropX = startWorldPos.X - tex.Width * 0.5f + (xScatterR - 0.5f) * 16f;

                float fade = Math.Min(1f, fallPhase / 6f);
                if (fade <= 0f) continue;

                Color col = new Color(140, 200, 255, (int)(210 * remain * fade));
                Vector2 origin = new Vector2(tex.Width * 0.5f, tex.Height * 0.5f);
                Main.EntitySpriteDraw(tex,
                    new Vector2(dropX + tex.Width * 0.5f, dropY + tex.Height * 0.5f) - Main.screenPosition,
                    null, col, 0f, origin, 1f, SpriteEffects.None, 0);
            }
        }

        // Draw
        public override bool PreDrawExtras()
        {
            if (ActiveTraps.Count == 0 || Main.netMode == NetmodeID.Server) return true;
            int loopFrame = (int)(Main.GameUpdateCount / 18) % 4;

            Texture2D bwTex = null;
            try { bwTex = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/TrapBackWall"); } catch { }

            foreach (var trap in ActiveTraps)
            {
                float alpha = Math.Min(1f, trap.DurationTicks / 80f);
                Color col = new Color(120, 180, 255, (int)(200 * alpha));
                float remain = trap.BaseDuration > 0
                    ? Math.Max(0f, Math.Min(1f, trap.DurationTicks / (float)trap.BaseDuration))
                    : 1f;

                var ceilingCols = new System.Collections.Generic.HashSet<int>();
                var topWallCols = new System.Collections.Generic.List<int>();
                var topWallSurfs = new System.Collections.Generic.List<TrapSurface>();

                foreach (var surf in trap.Surfaces)
                {
                    if (surf.Triggered) continue;
                    if (surf.Type == SurfaceType.Ceiling)
                    {
                        ceilingCols.Add((int)(surf.WorldPos.X / 16f));
                    }
                    else if (surf.Type == SurfaceType.BackWall)
                    {
                        int colX = (int)(surf.WorldPos.X / 16f);
                        bool found = false;
                        for (int k = 0; k < topWallCols.Count; k++)
                        {
                            if (topWallCols[k] == colX)
                            {
                                if (surf.WorldPos.Y < topWallSurfs[k].WorldPos.Y)
                                    topWallSurfs[k] = surf;
                                found = true;
                                break;
                            }
                        }
                        if (!found) { topWallCols.Add(colX); topWallSurfs.Add(surf); }
                    }
                }

                foreach (var surf in trap.Surfaces)
                {
                    if (surf.Triggered) continue;
                    try
                    {
                        if (surf.Type == SurfaceType.Floor)
                            DrawFloor("JoJoStands/Projectiles/TrapFloor", surf, col, loopFrame);
                        else if (surf.Type == SurfaceType.Ceiling)
                        {
                            DrawCeiling(surf, col);
                            if (bwTex != null && remain > 0.05f)
                            {
                                int ceilTX = (int)(surf.WorldPos.X / 16f);
                                int ceilTY = (int)(surf.WorldPos.Y / 16f);
                                bool hasWallBelow = false;
                                for (int sy = ceilTY + 1; sy <= ceilTY + 4 && sy < Main.maxTilesY; sy++)
                                {
                                    if (ceilTX < 0 || ceilTX >= Main.maxTilesX) break;
                                    var tt = Main.tile[ceilTX, sy];
                                    if (tt.HasTile && Main.tileSolid[tt.TileType] && !TileID.Sets.Platforms[tt.TileType]) break;
                                    if (tt.WallType > 0) { hasWallBelow = true; break; }
                                }
                                if (hasWallBelow)
                                    DrawBackWallRain(bwTex, surf.WorldPos, surf.DripLandY, surf.DripLandFound, remain, true);
                            }
                        }
                    }
                    catch { }
                }

                if (bwTex != null && remain > 0.05f)
                {
                    for (int k = 0; k < topWallCols.Count; k++)
                    {
                        if (ceilingCols.Contains(topWallCols[k])) continue;
                        var tw = topWallSurfs[k];
                        if (tw.Triggered) continue;
                        DrawBackWallRain(bwTex, tw.WorldPos, tw.DripLandY, tw.DripLandFound, remain, false);
                    }
                }
            }
            return true;
        }

        // Draw Ceiling
        private void DrawCeiling(TrapSurface surf, Color col)
        {
            try
            {
                Texture2D tex = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/TrapCeiling");
                int fw = CEIL_FRAME_WIDTH;
                int fh = CEIL_FRAME_HEIGHT;

                int phase = (surf.DripTimer + surf.DripTimerOffset) % TRAP_CYCLE;
                int landY = surf.DripLandFound ? surf.DripLandY : (int)surf.WorldPos.Y + 200;

                float anchorX = surf.WorldPos.X - fw * 0.5f;
                float ceilTopY = surf.WorldPos.Y - 2f;
                float floorTopY = landY - fh + 3f;

                Main.EntitySpriteDraw(tex,
                    new Vector2(anchorX, ceilTopY) - Main.screenPosition,
                    new Rectangle(0, 0, fw, fh),
                    col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);

                if (phase < TRAP_PUDDLE_END)
                {
                    int pudFrame = Math.Min(phase / CEIL_PUDDLE_TICKS_PER_FRAME, CEIL_PUDDLE_ANIM_FRAMES - 1);
                    if (pudFrame > 0)
                    {
                        Main.EntitySpriteDraw(tex,
                            new Vector2(anchorX, ceilTopY) - Main.screenPosition,
                            new Rectangle(0, pudFrame * fh, fw, fh),
                            col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    }
                }
                else
                {
                    int afterForming = phase - TRAP_PUDDLE_END;
                    int overlayFrame;
                    if (afterForming < CEIL_OVERLAY_TICKS_PER_FRAME)
                        overlayFrame = CEIL_PUDDLE_ANIM_FRAMES;
                    else
                        overlayFrame = CEIL_PUDDLE_ANIM_FRAMES + 1;

                    Main.EntitySpriteDraw(tex,
                        new Vector2(anchorX, ceilTopY) - Main.screenPosition,
                        new Rectangle(0, overlayFrame * fh, fw, fh),
                        col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);

                    if (phase < TRAP_DROP_END)
                    {
                        float t = (phase - TRAP_PUDDLE_END) / (float)CEIL_FALL_TICKS_TOTAL;
                        if (t > 1f) t = 1f;
                        float ease = t * t;
                        float topY = ceilTopY + (floorTopY - ceilTopY) * ease;

                        int fIdx = (CEIL_PUDDLE_ANIM_FRAMES + 2) + Math.Min((int)(t * CEIL_FALL_FRAMES), CEIL_FALL_FRAMES - 1);
                        Main.EntitySpriteDraw(tex,
                            new Vector2(anchorX, topY) - Main.screenPosition,
                            new Rectangle(0, fIdx * fh, fw, fh),
                            col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    }
                    else
                    {
                        int impPhase = phase - TRAP_DROP_END;
                        int impFrame = Math.Min(impPhase / CEIL_IMPACT_TICKS_PER_FRAME, CEIL_IMPACT_FRAMES - 1);
                        int fIdx = (CEIL_PUDDLE_ANIM_FRAMES + 2) + CEIL_FALL_FRAMES + impFrame;
                        Main.EntitySpriteDraw(tex,
                            new Vector2(anchorX, floorTopY) - Main.screenPosition,
                            new Rectangle(0, fIdx * fh, fw, fh),
                            col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    }
                }
            }
            catch { }
        }

        // Draw Floor
        private void DrawFloor(string texPath, TrapSurface surf, Color col, int loopFrame)
        {
            try
            {
                Texture2D tex = (Texture2D)ModContent.Request<Texture2D>(texPath);
                int fc = Math.Max(tex.Height / Math.Max(tex.Width, 8), 1);
                int fh = tex.Height / fc;
                int f = loopFrame % fc;
                float anchorX = surf.WorldPos.X - tex.Width * 0.5f;
                float topY = surf.WorldPos.Y - fh;
                Main.EntitySpriteDraw(tex,
                    new Vector2(anchorX, topY) - Main.screenPosition,
                    new Rectangle(0, f * fh, tex.Width, fh),
                    col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            catch { }
        }

        public override void PostDraw(Color lightColor)
        {
            if (Main.netMode == 2) return;
            if (Projectile.owner != Main.myPlayer) return;
            if (!Main.mouseRight) return;

            Player p = Main.player[Projectile.owner];
            if (p == null || !p.active || p.dead) return;

            Texture2D px = Terraria.GameContent.TextureAssets.MagicPixel.Value;
            Vector2 center = p.Center - Main.screenPosition;
            float alpha = global::JoJoStands.JoJoStands.RangeIndicators
                ? global::JoJoStands.JoJoStands.RangeIndicatorAlpha
                : 0.55f;
            const float CTRL = 260f;
            Color ctrlCol = new Color(120, 200, 255) * alpha;
            int points = 720;
            for (int i = 0; i < points; i++)
            {
                float a = (i / (float)points) * MathHelper.TwoPi;
                Vector2 pos = center + new Vector2((float)Math.Cos(a) * CTRL, (float)Math.Sin(a) * CTRL);
                Main.EntitySpriteDraw(px, pos, new Rectangle(0, 0, 1, 1), ctrlCol, 0f, new Vector2(0.5f, 0.5f), 3f, SpriteEffects.None, 0);
            }
        }

        public override void SelectAnimation()
        {
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0; Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState; Projectile.netUpdate = true;
            }
            if (currentAnimationState == AnimationState.Idle) PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Pose) PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/NovemberRain", "NovemberRain_Idle");
            switch (animationName)
            {
                case "Idle": AnimateStand(animationName, 1, 12, loop: true); break;
                case "Pose": AnimateStand(animationName, 1, 12, loop: true); break;
            }
        }
    }
}
