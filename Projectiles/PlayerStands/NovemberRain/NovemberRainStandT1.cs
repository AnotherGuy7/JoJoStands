using JoJoStands.Projectiles;
using JoJoStands.Buffs.Debuffs;
using System;
using System.Collections.Generic;
using JoJoStands;
using JoJoStands.Projectiles.PlayerStands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
        public override float MaxDistance => 80f;

        protected virtual float RAIN_W => 155f;
        protected virtual float RAIN_DOWN => 280f;
        protected virtual float RAIN_UP => 260f;
        protected virtual float RAIN_SLOW => 0.65f;
        protected virtual float MISS_CHANCE => 0.15f;
        protected virtual int HIT_INTERVAL => 22;
        protected virtual int PRECISE_CD => 7;
        protected const float MAX_UPWARD_RATIO = 0.5f;
        protected const float STAND_Y_OFFSET = -73f;
        protected const float STAND_X_OFFSET = -20f;
        protected const float FIRE_X_OFFSET = 20f;
        protected const float FIRE_Y_OFFSET = -22f;

        protected virtual int TRAP_SPAWN_TICKS => 0;
        protected virtual int TRAP_BASE_TICKS => 0;
        protected virtual int TRAP_MAX_TICKS => 0;
        protected const int MAX_TRAP_AREAS = 3;

        protected const int CEIL_TOTAL_FRAMES = 15;
        protected const int CEIL_FRAME_HEIGHT = 20;
        protected const int CEIL_FRAME_WIDTH  = 18;

        protected const int CEIL_PUDDLE_ANIM_FRAMES = 5;
        protected const int CEIL_FALL_FRAMES        = 3;
        protected const int CEIL_IMPACT_FRAMES      = 5;

        protected const int CEIL_PUDDLE_TICKS_PER_FRAME = 18;
        protected const int CEIL_FALL_TICKS_TOTAL       = 36;
        protected const int CEIL_IMPACT_TICKS_PER_FRAME = 8;

        protected const int CEIL_OVERLAY_TICKS_PER_FRAME = 9;

        protected const int TRAP_PUDDLE_END = CEIL_PUDDLE_ANIM_FRAMES * CEIL_PUDDLE_TICKS_PER_FRAME;
        protected const int TRAP_DROP_END   = TRAP_PUDDLE_END + CEIL_FALL_TICKS_TOTAL;
        protected const int TRAP_CYCLE      = TRAP_DROP_END   + CEIL_IMPACT_FRAMES * CEIL_IMPACT_TICKS_PER_FRAME;

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
        }

        protected List<TrapArea> ActiveTraps = new List<TrapArea>();

        protected bool InRainDome(Vector2 pos, float w, float down, float up, Vector2 center)
        {
            float dx = Math.Abs(pos.X - center.X);
            float dy = pos.Y - center.Y;
            if (dy <= 0) { float nx = dx / w; float ny = (-dy) / up; return nx * nx + ny * ny < 1f; }
            return dx < w && dy < down;
        }

        private int FindDripLandY(Vector2 startWorld)
        {
            int tx = (int)(startWorld.X / 16f);
            int startTY = (int)(startWorld.Y / 16f) + 1;
            for (int ty = startTY; ty < startTY + 60; ty++)
            {
                if (ty < 0 || ty >= Main.maxTilesY) break;
                var t = Main.tile[tx, ty];
                if (t.HasTile && (Main.tileSolid[t.TileType] || TileID.Sets.Platforms[t.TileType]))
                    return ty * 16;
            }
            return startTY * 16 + 200;
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
                    npc.SimpleStrikeNPC(bonusDmg, direction, crit: true, knockBack: 0f);
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
            npc.SimpleStrikeNPC(baseDmg, direction, crit: crit, knockBack: 0.8f);
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
                RainVisuals(RAIN_W, RAIN_DOWN, RAIN_UP, Projectile.Center);
                AreaDamage(mPlayer, player, RAIN_W, RAIN_DOWN, RAIN_UP, HIT_INTERVAL, RAIN_SLOW, Projectile.Center);
                if (TRAP_SPAWN_TICKS > 0) BuildTraps(player);
            }
            else
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && preciseTimer <= 0)
                    {
                        currentAnimationState = AnimationState.Idle;
                        FirePrecise(mPlayer);
                        preciseTimer = PRECISE_CD;
                    }
                    else currentAnimationState = AnimationState.Idle;
                }
            }
            if (mPlayer.posing) currentAnimationState = AnimationState.Pose;
        }

        protected void SnapAbovePlayer(Player player)
        {
            Projectile.Center = player.Center + new Vector2(STAND_X_OFFSET * player.direction, STAND_Y_OFFSET);
            Projectile.velocity = Vector2.Zero;
            Projectile.direction = player.direction;
            Projectile.spriteDirection = player.direction;
        }

        protected void FirePrecise(MyPlayer mPlayer)
        {
            if (Projectile.owner != Main.myPlayer) return;
            Vector2 firePos = Projectile.Center + new Vector2(FIRE_X_OFFSET * Projectile.spriteDirection, FIRE_Y_OFFSET);
            Vector2 dir = Main.MouseWorld - firePos;
            if (dir == Vector2.Zero) dir = new Vector2(0f, 1f);
            if (dir.Y < 0)
            {
                float limit = -Math.Abs(dir.X) * MAX_UPWARD_RATIO;
                if (dir.Y < limit) dir.Y = limit;
                if (Math.Abs(dir.X) < 5f) dir.Y = Math.Max(dir.Y, -1.5f);
            }
            dir.Normalize();
            int idx = Projectile.NewProjectile(Projectile.GetSource_FromThis(), firePos, dir * 11f,
                ModContent.ProjectileType<PreciseRainDrop>(), newPunchDamage, 2f, Main.myPlayer);
            Main.projectile[idx].netUpdate = true;
        }

        protected void FireControllableDrop(MyPlayer mPlayer)
        {
            if (Projectile.owner != Main.myPlayer) return;
            Vector2 firePos = Projectile.Center + new Vector2(FIRE_X_OFFSET * Projectile.spriteDirection, FIRE_Y_OFFSET);
            Vector2 toCursor = Main.MouseWorld - firePos;
            if (toCursor != Vector2.Zero) toCursor.Normalize();
            int dmg = (int)(newPunchDamage * 3.5f);
            int idx = Projectile.NewProjectile(Projectile.GetSource_FromThis(), firePos, new Vector2(toCursor.X * 2f, 1.5f),
                ModContent.ProjectileType<ControllableRainDrop>(), dmg, 3f, Main.myPlayer);
            Main.projectile[idx].netUpdate = true;
        }

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

        protected void AreaDamage(MyPlayer mPlayer, Player player, float w, float down, float up, int baseInterval, float slow, Vector2 center)
        {
            if (Projectile.owner != Main.myPlayer) return;
            float boost = Math.Max(0.5f, mPlayer.standDamageBoosts);
            int interval = Math.Max((int)(baseInterval / boost), 8);
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.dontTakeDamage)
                { npcTimers[i] = 0; npcWasInArea[i] = false; continue; }
                bool inArea = InRainDome(npc.Center, w, down, up, center);
                if (inArea)
                {
                    if (!npcWasInArea[i]) { npcWasInArea[i] = true; npcTimers[i] = interval - 1; if (!npc.boss) npc.velocity *= slow; }
                    npcTimers[i]++;
                    if (npcTimers[i] >= interval)
                    {
                        npcTimers[i] = 0;
                        if (Main.rand.NextFloat() < MISS_CHANCE) continue;
                        if (!npc.boss) npc.velocity *= slow;
                        HitNPCWithAccessories(player, mPlayer, npc, newPunchDamage, Projectile.direction);
                        npc.velocity += new Vector2(Projectile.direction * 0.8f, 0.4f);
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

        protected List<TrapSurface> ScanSurfaces(Vector2 domeCenter, float w, float down, float up, Vector2 playerCenter)
        {
            var surfaces = new List<TrapSurface>();
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
                    bool solid = IsSolid(tx, ty);
                    if (solid)
                    {
                        if (ty >= playerTileY && IsAir(tx, ty - 1))
                        {
                            surfaces.Add(new TrapSurface
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
                            surfaces.Add(new TrapSurface
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
                        surfaces.Add(new TrapSurface
                        {
                            WorldPos = new Vector2(tx * 16f + 8f, ty * 16f + 8f),
                            Type = SurfaceType.BackWall,
                            DripTimerOffset = Main.rand.Next(0, TRAP_CYCLE)
                        });
                    }
                }
            }
            return surfaces;
        }

        protected void BuildTraps(Player player)
        {
            if (Projectile.owner != Main.myPlayer) return;
            if (ActiveTraps.Count >= MAX_TRAP_AREAS) return;
            foreach (var existing in ActiveTraps)
                if (InRainDome(Projectile.Center, RAIN_W * 0.85f, RAIN_DOWN * 0.85f, RAIN_UP * 0.85f, existing.Center)) return;
            trapFormTimer++;
            if (trapFormTimer < TRAP_SPAWN_TICKS) return;
            trapFormTimer = 0;
            var surfaces = ScanSurfaces(Projectile.Center, RAIN_W, RAIN_DOWN, RAIN_UP, player.Center);
            if (surfaces.Count == 0) return;
            ActiveTraps.Add(new TrapArea
            {
                Center = Projectile.Center,
                DurationTicks = TRAP_BASE_TICKS,
                BaseDuration = TRAP_BASE_TICKS,
                MaxDuration = TRAP_MAX_TICKS,
                Surfaces = surfaces,
                PlayerNearby = true
            });
            Projectile.netUpdate = true;
            for (int d = 0; d < 10; d++)
                Dust.NewDust(Projectile.Center + new Vector2(Main.rand.NextFloat(-RAIN_W * 0.5f, RAIN_W * 0.5f), Main.rand.NextFloat(-20f, 20f)),
                    8, 8, DustID.Water, 0f, -1f, 0, default, 1.2f);
        }

        protected void UpdateTraps(MyPlayer mPlayer, Player player)
        {
            if (Projectile.owner != Main.myPlayer) return;
            for (int i = ActiveTraps.Count - 1; i >= 0; i--)
            {
                var trap = ActiveTraps[i];
                trap.Surfaces.RemoveAll(s => s.Triggered);
                if (trap.Surfaces.Count == 0) { ActiveTraps.RemoveAt(i); trapFormTimer = 0; continue; }
                bool nearby = InRainDome(Projectile.Center, RAIN_W * 1.05f, RAIN_DOWN * 1.05f, RAIN_UP * 1.05f, trap.Center);
                if (nearby)
                {
                    if (!trap.PlayerNearby && trap.DurationTicks < trap.BaseDuration) trap.DurationTicks = trap.BaseDuration;
                    trap.PlayerNearby = true;
                    trap.DurationTicks = Math.Min(trap.DurationTicks + 1, trap.MaxDuration);
                }
                else
                {
                    trap.PlayerNearby = false;
                    trap.DurationTicks -= 2;
                    if (trap.DurationTicks <= 0) { ActiveTraps.RemoveAt(i); trapFormTimer = 0; continue; }
                }
                foreach (var surf in trap.Surfaces)
                {
                    if (surf.Type != SurfaceType.Ceiling || surf.Triggered) continue;
                    surf.DripTimer++;
                    int phase = (surf.DripTimer + surf.DripTimerOffset) % TRAP_CYCLE;
                    if (phase == TRAP_DROP_END)
                    {
                        int landY = surf.DripLandFound ? surf.DripLandY : (int)surf.WorldPos.Y + 200;
                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            NPC npc = Main.npc[n];
                            if (!npc.active || npc.friendly || npc.dontTakeDamage) continue;
                            if (Math.Abs(npc.Center.X - surf.WorldPos.X) < 40f &&
                                npc.Center.Y > surf.WorldPos.Y &&
                                npc.Center.Y < landY + 32f)
                            {
                                bool crit = Main.rand.Next(100) < player.GetTotalCritChance<MeleeDamageClass>();
                                npc.SimpleStrikeNPC((int)(newPunchDamage * 0.5f), Projectile.direction, crit: crit, knockBack: 0f);
                                Dust.NewDust(new Vector2(surf.WorldPos.X, landY), 16, 8, DustID.Water,
                                    Main.rand.NextFloat(-3f, 3f), -2f, 0, default, 1.2f);
                                Projectile.netUpdate = true;
                            }
                        }
                    }
                }
            }
        }

        protected void CheckTrapTriggers(MyPlayer mPlayer)
        {
            if (Projectile.owner != Main.myPlayer) return;
            foreach (var trap in ActiveTraps)
            {
                foreach (var surf in trap.Surfaces)
                {
                    if (surf.Triggered) continue;
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
                            npc.SimpleStrikeNPC((int)(newPunchDamage * 3f), Projectile.direction, crit: false, knockBack: 2f);
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

        public override bool PreDrawExtras()
        {
            if (ActiveTraps.Count == 0 || Main.netMode == NetmodeID.Server) return true;
            int loopFrame = (int)(Main.GameUpdateCount / 18) % 4;

            foreach (var trap in ActiveTraps)
            {
                float alpha = Math.Min(1f, trap.DurationTicks / 80f);
                Color col = new Color(120, 180, 255, (int)(200 * alpha));

                foreach (var surf in trap.Surfaces)
                {
                    if (surf.Triggered) continue;
                    try
                    {
                        if (surf.Type == SurfaceType.Floor)
                            DrawFloor("JoJoStands/Projectiles/TrapFloor", surf, col, loopFrame);
                        else if (surf.Type == SurfaceType.BackWall)
                            DrawBackWall("JoJoStands/Projectiles/TrapBackWall", surf, col, loopFrame);
                        else if (surf.Type == SurfaceType.Ceiling)
                            DrawCeiling(surf, col);
                    }
                    catch { }
                }
            }
            return true;
        }

        private void DrawCeiling(TrapSurface surf, Color col)
        {
            try
            {
                Texture2D tex = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/TrapCeiling");
                int fw = CEIL_FRAME_WIDTH;
                int fh = CEIL_FRAME_HEIGHT;

                int phase = (surf.DripTimer + surf.DripTimerOffset) % TRAP_CYCLE;
                int landY = surf.DripLandFound ? surf.DripLandY : (int)surf.WorldPos.Y + 200;

                float anchorX   = surf.WorldPos.X - fw * 0.5f;
                float ceilTopY  = surf.WorldPos.Y - 2f;
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
                        float t    = (phase - TRAP_PUDDLE_END) / (float)CEIL_FALL_TICKS_TOTAL;
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

        private void DrawFloor(string texPath, TrapSurface surf, Color col, int loopFrame)
        {
            try
            {
                Texture2D tex = (Texture2D)ModContent.Request<Texture2D>(texPath);
                int fc  = Math.Max(tex.Height / Math.Max(tex.Width, 8), 1);
                int fh  = tex.Height / fc;
                int f   = loopFrame % fc;
                float anchorX = surf.WorldPos.X - tex.Width * 0.5f;
                float topY    = surf.WorldPos.Y - fh;
                Main.EntitySpriteDraw(tex,
                    new Vector2(anchorX, topY) - Main.screenPosition,
                    new Rectangle(0, f * fh, tex.Width, fh),
                    col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            catch { }
        }

        private void DrawBackWall(string texPath, TrapSurface surf, Color col, int loopFrame)
        {
            try
            {
                Texture2D tex = (Texture2D)ModContent.Request<Texture2D>(texPath);
                int fc    = Math.Max(tex.Height / Math.Max(tex.Width, 8), 1);
                int fh    = tex.Height / fc;
                int f     = loopFrame % fc;
                float anchorX = surf.WorldPos.X - tex.Width * 0.5f;
                float anchorY = surf.WorldPos.Y - fh * 0.5f;
                Main.EntitySpriteDraw(tex,
                    new Vector2(anchorX, anchorY) - Main.screenPosition,
                    new Rectangle(0, f * fh, tex.Width, fh),
                    col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            catch { }
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
