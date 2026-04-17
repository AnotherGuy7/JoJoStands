using JoJoStands.Projectiles;
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
        public override string PoseSoundName => "NovemberRain";
        public override string SpawnSoundName => "NovemberRain";
        public override StandAttackType StandType => StandAttackType.Melee;
        public override int PunchDamage => 18;
        public override int PunchTime => 8;
        public override int HalfStandHeight => 37;
        public override float MaxDistance => 80f;

        protected virtual float RAIN_W => 155f;
        protected virtual float RAIN_DOWN => 280f;
        protected virtual float RAIN_UP => 260f;
        protected virtual float RAIN_SLOW => 0.65f;
        protected virtual float MISS_CHANCE => 0.15f;
        protected virtual int HIT_INTERVAL => 22;
        protected virtual int PRECISE_CD => 7;
        protected const float MAX_UPWARD_RATIO = 0.5f;
        protected const int TRAP_FORM_TICKS = 120;
        protected const int TRAP_EXPIRE_TICKS = 600;

        private int preciseTimer = 0;
        private int visualTimer = 0;

        protected int[] npcTimers = new int[Main.maxNPCs];
        protected bool[] npcWasInArea = new bool[Main.maxNPCs];
        protected int[] npcStunTimers = new int[Main.maxNPCs];
        protected Dictionary<long, int> tileRainTimers = new Dictionary<long, int>();
        protected Dictionary<Vector2, int> activeTrapExpiry = new Dictionary<Vector2, int>();

        protected bool InRainDome(Vector2 pos, float w, float down, float up)
        {
            float dx = Math.Abs(pos.X - Projectile.Center.X);
            float dy = pos.Y - Projectile.Center.Y;
            if (dy <= 0)
            {
                float nx = dx / w;
                float ny = (-dy) / up;
                return nx * nx + ny * ny < 1f;
            }
            return dx < w && dy < down;
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
            CheckTrapTriggers(mPlayer);
            ExpireTraps();

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                currentAnimationState = AnimationState.Attack;
                RainVisuals(RAIN_W, RAIN_DOWN, RAIN_UP);
                AreaDamage(mPlayer, player);
            }
            else
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && preciseTimer <= 0)
                    {
                        currentAnimationState = AnimationState.Attack;
                        FirePrecise(player);
                        preciseTimer = PRECISE_CD;
                    }
                    else if (!Main.mouseLeft)
                    {
                        currentAnimationState = AnimationState.Idle;
                    }
                }
            }

            if (mPlayer.posing) currentAnimationState = AnimationState.Pose;
        }

        protected void SnapAbovePlayer(Player player)
        {
            Projectile.Center = player.Center + new Vector2(0f, -56f);
            Projectile.velocity = Vector2.Zero;
            Projectile.direction = player.direction;
            Projectile.spriteDirection = player.direction;
        }

        protected void FirePrecise(Player player)
        {
            if (Projectile.owner != Main.myPlayer) return;
            Vector2 dir = Main.MouseWorld - Projectile.Center;
            if (dir == Vector2.Zero) dir = new Vector2(0f, 1f);
            if (dir.Y < 0)
            {
                float limit = -Math.Abs(dir.X) * MAX_UPWARD_RATIO;
                if (dir.Y < limit) dir.Y = limit;
                if (Math.Abs(dir.X) < 5f) dir.Y = Math.Max(dir.Y, -1.5f);
            }
            dir.Normalize();
            bool crit = Main.rand.Next(100) < player.GetTotalCritChance<MeleeDamageClass>();
            int dmg = crit ? newPunchDamage * 2 : newPunchDamage;
            int idx = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, dir * 11f,
                ModContent.ProjectileType<PreciseRainDrop>(), dmg, 2f, Main.myPlayer);
            Main.projectile[idx].netUpdate = true;
        }

        protected void FireControllableDrop(Player player)
        {
            if (Projectile.owner != Main.myPlayer) return;
            Vector2 dir = Main.MouseWorld - Projectile.Center;
            if (dir == Vector2.Zero) dir = new Vector2(0f, 1f);
            dir.Normalize();
            bool crit = Main.rand.Next(100) < player.GetTotalCritChance<MeleeDamageClass>();
            int dmg = (int)(newPunchDamage * 3.5f);
            if (crit) dmg *= 2;
            int idx = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, dir * 14f,
                ModContent.ProjectileType<ControllableRainDrop>(), dmg, 3f, Main.myPlayer);
            Main.projectile[idx].netUpdate = true;
        }

        protected void RainVisuals(float w, float down, float up)
        {
            visualTimer++;
            if (visualTimer < 2) return;
            visualTimer = 0;
            Vector2 c = Projectile.Center;
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
            for (int i = 0; i < 4; i++)
            {
                float angle2 = Main.rand.NextFloat(MathHelper.Pi, MathHelper.TwoPi);
                float r2 = Main.rand.NextFloat(0.4f, 0.85f);
                int d = Dust.NewDust(new Vector2(c.X + (float)Math.Cos(angle2) * w * r2, c.Y + (float)Math.Sin(angle2) * up * r2),
                    3, 11, DustID.Water, Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.NextFloat(8f, 13f), 160, default, Main.rand.NextFloat(0.55f, 0.75f));
                Main.dust[d].noGravity = true;
            }
            for (int i = 0; i < 3; i++)
            {
                int d = Dust.NewDust(new Vector2(c.X + Main.rand.NextFloat(-w * 0.7f, w * 0.7f), c.Y + Main.rand.NextFloat(0f, down * 0.85f)),
                    2, 6, DustID.Water, 0f, Main.rand.NextFloat(5f, 9f), 200, default, Main.rand.NextFloat(0.3f, 0.48f));
                Main.dust[d].noGravity = true;
            }
            Lighting.AddLight(Projectile.Center, 0.05f, 0.12f, 0.25f);
        }

        protected void AreaDamage(MyPlayer mPlayer, Player player)
        {
            if (Projectile.owner != Main.myPlayer) return;
            float boost = Math.Max(0.5f, mPlayer.standDamageBoosts);
            int interval = Math.Max((int)(HIT_INTERVAL / boost), 8);
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.dontTakeDamage) { npcTimers[i] = 0; npcWasInArea[i] = false; continue; }
                bool inArea = InRainDome(npc.Center, RAIN_W, RAIN_DOWN, RAIN_UP);
                if (inArea)
                {
                    if (!npcWasInArea[i]) { npcWasInArea[i] = true; npcTimers[i] = interval - 1; if (!npc.boss) npc.velocity *= RAIN_SLOW; }
                    npcTimers[i]++;
                    if (npcTimers[i] >= interval)
                    {
                        npcTimers[i] = 0;
                        if (Main.rand.NextFloat() < MISS_CHANCE) continue;
                        if (!npc.boss) npc.velocity *= RAIN_SLOW;
                        bool crit = Main.rand.Next(100) < player.GetTotalCritChance<MeleeDamageClass>();
                        int dmg = crit ? newPunchDamage * 2 : newPunchDamage;
                        npc.SimpleStrikeNPC(dmg, Projectile.direction, crit: crit, knockBack: 0.8f);
                        npc.velocity += new Vector2(Projectile.direction * 0.8f, 0.4f);
                        for (int d = 0; d < 4; d++)
                            Dust.NewDust(npc.position, npc.width, npc.height, DustID.Water, Main.rand.NextFloat(-2f, 2f), -1.5f, 0, default, 1f);
                        Projectile.netUpdate = true;
                    }
                }
                else { npcTimers[i] = 0; npcWasInArea[i] = false; }
            }
        }

        protected void PassiveTrapBuilder(float w, float down, float up)
        {
            if (Projectile.owner != Main.myPlayer) return;
            int tileRange = (int)(w / 16f) + 2;
            int cx = (int)(Projectile.Center.X / 16f);
            int cy = (int)(Projectile.Center.Y / 16f);
            int downTiles = (int)(down / 16f) + 2;
            int upTiles = (int)(up / 16f) + 2;

            for (int tx = cx - tileRange; tx <= cx + tileRange; tx++)
            {
                for (int ty = cy - upTiles; ty <= cy + downTiles; ty++)
                {
                    if (tx < 0 || tx >= Main.maxTilesX || ty < 0 || ty >= Main.maxTilesY) continue;
                    Terraria.Tile tile = Main.tile[tx, ty];
                    bool isPlatform = TileID.Sets.Platforms[tile.TileType];
                    bool isSolid = tile.HasTile && Main.tileSolid[tile.TileType];
                    bool hasWall = tile.WallType > 0;
                    if (!(isPlatform || isSolid || hasWall)) continue;
                    Vector2 tileWorldPos = new Vector2(tx * 16f + 8f, ty * 16f + 8f);
                    if (!InRainDome(tileWorldPos, w, down, up)) continue;
                    long key = ((long)tx << 32) | (uint)ty;
                    if (!tileRainTimers.ContainsKey(key)) tileRainTimers[key] = 0;
                    tileRainTimers[key]++;
                    if (tileRainTimers[key] >= TRAP_FORM_TICKS)
                    {
                        tileRainTimers.Remove(key);
                        Vector2 trapPos = new Vector2(tx * 16f + 8f, ty * 16f);
                        bool exists = false;
                        foreach (var kv in activeTrapExpiry)
                            if (Vector2.Distance(kv.Key, trapPos) < 20f) { exists = true; break; }
                        if (!exists)
                        {
                            activeTrapExpiry[trapPos] = TRAP_EXPIRE_TICKS;
                            for (int d = 0; d < 8; d++)
                                Dust.NewDust(trapPos - new Vector2(24f, 4f), 48, 8, DustID.Water, Main.rand.NextFloat(-2f, 2f), -1f, 0, default, 1f);
                        }
                    }
                }
            }

            List<long> toRemove = new List<long>();
            foreach (var kv in tileRainTimers)
            {
                int tx2 = (int)((ulong)kv.Key >> 32);
                int ty2 = (int)(kv.Key & 0xFFFFFFFFL);
                if (!InRainDome(new Vector2(tx2 * 16f + 8f, ty2 * 16f + 8f), w, down, up)) toRemove.Add(kv.Key);
            }
            foreach (var k in toRemove) tileRainTimers.Remove(k);
        }

        protected void ExpireTraps()
        {
            List<Vector2> toExpire = new List<Vector2>();
            List<Vector2> keys = new List<Vector2>(activeTrapExpiry.Keys);
            foreach (var k in keys) { activeTrapExpiry[k]--; if (activeTrapExpiry[k] <= 0) toExpire.Add(k); }
            foreach (var k in toExpire) activeTrapExpiry.Remove(k);
        }

        protected void CheckTrapTriggers(MyPlayer mPlayer)
        {
            if (Projectile.owner != Main.myPlayer) return;
            List<Vector2> toRemove = new List<Vector2>();
            foreach (var kv in activeTrapExpiry)
            {
                Vector2 trapPos = kv.Key;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.active || npc.friendly || npc.dontTakeDamage) continue;
                    float ndx = Math.Abs(npc.Center.X - trapPos.X);
                    float ndy = npc.Center.Y - trapPos.Y;
                    if (ndx < 48f && ndy > -35f && ndy < 18f)
                    {
                        npc.SimpleStrikeNPC(newPunchDamage * 3, Projectile.direction, crit: false, knockBack: 2f);
                        npcStunTimers[i] = 90;
                        for (int d = 0; d < 14; d++)
                            Dust.NewDust(new Vector2(trapPos.X - 48f, trapPos.Y - 8f), 96, 16, DustID.Water, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 0f), 0, default, 1.1f);
                        toRemove.Add(trapPos);
                        Projectile.netUpdate = true;
                        break;
                    }
                }
            }
            foreach (var k in toRemove) activeTrapExpiry.Remove(k);
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
            if (activeTrapExpiry.Count > 0 && Main.netMode != NetmodeID.Server)
            {
                Texture2D tex = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PuddleTrap");
                foreach (var kv in activeTrapExpiry)
                {
                    float alpha = Math.Min(1f, kv.Value / 60f);
                    Main.EntitySpriteDraw(tex, kv.Key - Main.screenPosition, null,
                        new Color(100, 160, 255, (int)(180 * alpha)), 0f,
                        new Vector2(tex.Width / 2f, tex.Height / 2f),
                        new Vector2(1.6f, 0.5f), SpriteEffects.None, 0);
                }
            }
            return true;
        }

        public override void SelectAnimation()
        {
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0; Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState; Projectile.netUpdate = true;
            }
            if (currentAnimationState == AnimationState.Idle) PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Attack) PlayAnimation("Attack");
            else if (currentAnimationState == AnimationState.Pose) PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/NovemberRain", "NovemberRain_" + animationName);
            switch (animationName)
            {
                case "Idle":   AnimateStand(animationName, 4, 12, loop: true); break;
                case "Attack": AnimateStand(animationName, 4, 8,  loop: true); break;
                case "Pose":   AnimateStand(animationName, 2, 12, loop: true); break;
            }
        }
    }
}
