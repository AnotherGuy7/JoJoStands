using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class PreciseRainDrop : ModProjectile
    {
        public override string Texture => "JoJoStands/Projectiles/RainDrop";

        private bool    firstFrame    = true;
        private Vector2 lastAnchorPos = Vector2.Zero;

        private bool    fading         = false;
        private bool    fadingFromNpc  = false;
        private int     fadeAge        = 0;
        private Vector2 fadeStopCenter = Vector2.Zero;
        private Vector2 fadeStopDir    = Vector2.UnitY;
        private const int FadeFrames = 8;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[Projectile.type]     = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width                = 6;
            Projectile.height                = 14;
            Projectile.friendly              = true;
            Projectile.hostile               = false;
            Projectile.DamageType   = DamageClass.Generic;
            Projectile.penetrate             = -1;
            Projectile.timeLeft              = 200;
            Projectile.ignoreWater           = true;
            Projectile.tileCollide           = false;
            Projectile.alpha                 = 60;
            Projectile.usesLocalNPCImmunity  = true;
            Projectile.localNPCHitCooldown   = 20;
        }

        private bool TryGetAnchor(out Vector2 anchorCenter)
        {
            if (Projectile.owner >= 0 && Projectile.owner < Main.player.Length)
            {
                Player p = Main.player[Projectile.owner];
                if (p != null && p.active && !p.dead)
                {
                    anchorCenter = p.Center;
                    return true;
                }
            }
            anchorCenter = Vector2.Zero;
            return false;
        }

        public override void AI()
        {
            if (firstFrame)
            {
                firstFrame = false;
                if (TryGetAnchor(out Vector2 a0)) lastAnchorPos = a0;
                else                              lastAnchorPos = Projectile.Center;
            }
            else if (!fading && TryGetAnchor(out Vector2 anchorNow))
            {
                Vector2 anchorDelta = anchorNow - lastAnchorPos;
                if (anchorDelta != Vector2.Zero)
                {
                    Projectile.position += anchorDelta;
                    int len = Projectile.oldPos.Length;
                    for (int i = 0; i < len; i++)
                    {
                        if (Projectile.oldPos[i] != Vector2.Zero)
                            Projectile.oldPos[i] += anchorDelta;
                    }
                }
                lastAnchorPos = anchorNow;
            }

            if (fading)
            {
                Projectile.velocity *= 0.55f;
                fadeAge++;
                if (fadeAge >= FadeFrames)
                {
                    Projectile.Kill();
                    return;
                }
                float lf = 1f - fadeAge / (float)FadeFrames;
                Lighting.AddLight(fadeStopCenter, 0.05f * lf, 0.10f * lf, 0.18f * lf);
                return;
            }

            Projectile.velocity.Y += 0.92f;
            if (Projectile.velocity.Y > 45f) Projectile.velocity.Y = 45f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (HitboxInSolidAt(Projectile.position) || ScanPathHitsSolid())
            {
                StartFading(fromNpc: false);
                return;
            }

            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.Center - new Vector2(3f), 6, 6,
                    DustID.Water, 0f, 0f, 120,
                    new Color(140, 210, 255), Main.rand.NextFloat(0.7f, 0.9f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.25f;
            }
            Lighting.AddLight(Projectile.Center, 0.05f, 0.10f, 0.18f);
        }

        private void StartFading(bool fromNpc)
        {
            if (fading) return;
            fading              = true;
            fadingFromNpc       = fromNpc;
            fadeAge             = 0;
            fadeStopCenter      = Projectile.Center;
            Vector2 v = Projectile.velocity;
            float vl = v.Length();
            fadeStopDir         = vl > 0.01f ? v / vl : Vector2.UnitY;
            Projectile.friendly = false;
            Projectile.damage   = 0;
        }

        private bool HitboxInSolidAt(Vector2 topLeft)
        {
            int x1 = (int)topLeft.X;
            int y1 = (int)topLeft.Y;
            int x2 = x1 + Projectile.width  - 1;
            int y2 = y1 + Projectile.height - 1;
            int tileX1 = x1 / 16;
            int tileY1 = y1 / 16;
            int tileX2 = x2 / 16;
            int tileY2 = y2 / 16;
            for (int tx = tileX1; tx <= tileX2; tx++)
            {
                for (int ty = tileY1; ty <= tileY2; ty++)
                {
                    if (tx < 0 || ty < 0 || tx >= Main.maxTilesX || ty >= Main.maxTilesY) continue;
                    Tile t = Main.tile[tx, ty];
                    if (t == null || !t.HasTile) continue;
                    if (Main.tileSolid[t.TileType] && !Main.tileSolidTop[t.TileType])
                        return true;
                }
            }
            return false;
        }

        private bool ScanPathHitsSolid()
        {
            Vector2 vel = Projectile.velocity;
            float stepLen = vel.Length();
            if (stepLen <= 0.01f) return false;
            Vector2 stepDir = vel / stepLen;
            const float step = 4f;
            for (float t = step; t <= stepLen + step; t += step)
            {
                float tt = MathHelper.Min(t, stepLen);
                Vector2 testPos = Projectile.position + stepDir * tt;
                if (HitboxInSolidAt(testPos)) return true;
            }
            return false;
        }

        private bool PointInSolid(Vector2 worldPos)
        {
            int tx = (int)(worldPos.X / 16f);
            int ty = (int)(worldPos.Y / 16f);
            if (tx < 0 || ty < 0 || tx >= Main.maxTilesX || ty >= Main.maxTilesY) return false;
            Tile t = Main.tile[tx, ty];
            if (t == null || !t.HasTile) return false;
            return Main.tileSolid[t.TileType] && !Main.tileSolidTop[t.TileType];
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (fading) return false;
            return null;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (fading) return false;
            if (projHitbox.Intersects(targetHitbox)) return true;
            Vector2 a = new Vector2(projHitbox.Center.X, projHitbox.Center.Y);
            Vector2 b = a + Projectile.velocity;
            if (LineHitsRect(a, b, targetHitbox)) return true;
            return null;
        }

        private bool LineHitsRect(Vector2 a, Vector2 b, Rectangle r)
        {
            Vector2 d = b - a;
            float len = d.Length();
            if (len < 0.5f) return r.Contains((int)a.X, (int)a.Y);
            d /= len;
            const float step = 3f;
            for (float t = 0f; t <= len; t += step)
            {
                Vector2 p = a + d * t;
                if (r.Contains((int)p.X, (int)p.Y)) return true;
            }
            return r.Contains((int)b.X, (int)b.Y);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!fading) StartFading(fromNpc: true);
        }

        private bool LineCrossesSolid(Vector2 a, Vector2 b)
        {
            Vector2 d = b - a;
            float len = d.Length();
            if (len < 0.5f) return PointInSolid(a);
            d /= len;
            const float step = 2f;
            for (float t = 0f; t <= len; t += step)
            {
                if (PointInSolid(a + d * t)) return true;
            }
            return PointInSolid(b);
        }

        private int FindTrailWallBreak()
        {
            int len = Projectile.oldPos.Length;
            for (int j = 0; j < len - 1; j++)
            {
                Vector2 curJ = Projectile.oldPos[j];
                Vector2 nxtJ = Projectile.oldPos[j + 1];
                if (curJ == Vector2.Zero || nxtJ == Vector2.Zero) continue;
                Vector2 curCenterJ = curJ + Projectile.Size * 0.5f;
                Vector2 nxtCenterJ = nxtJ + Projectile.Size * 0.5f;
                if (LineCrossesSolid(nxtCenterJ, curCenterJ))
                    return j;
            }
            return -1;
        }

        private bool ClipSpriteSegment(Vector2 dirVec, float texHeight, ref Vector2 center, ref float stretch)
        {
            float halfLen = (texHeight * 0.5f) * stretch;
            if (halfLen < 0.5f) return true;

            Vector2 endBack = center - dirVec * halfLen;
            if (PointInSolid(endBack)) return false;

            float fullLen = halfLen * 2f;
            const float step = 2f;
            for (float t = step; t <= fullLen; t += step)
            {
                Vector2 p = endBack + dirVec * t;
                if (PointInSolid(p))
                {
                    float clipped = t - step;
                    if (clipped < 1f) return false;
                    center  = endBack + dirVec * (clipped * 0.5f);
                    stretch = clipped / texHeight;
                    return true;
                }
            }
            return true;
        }

        private bool ClipAgainstFadeStop(Vector2 dirVec, float texHeight, ref Vector2 center, ref float stretch)
        {
            float halfLen = (texHeight * 0.5f) * stretch;
            if (halfLen < 0.5f) return true;

            Vector2 endFront = center + dirVec * halfLen;
            Vector2 endBack  = center - dirVec * halfLen;

            float sFront = Vector2.Dot(endFront - fadeStopCenter, fadeStopDir);
            float sBack  = Vector2.Dot(endBack  - fadeStopCenter, fadeStopDir);

            if (sFront > 0f && sBack > 0f) return false;

            if (sFront > 0f && sBack <= 0f)
            {
                float dot = Vector2.Dot(dirVec, fadeStopDir);
                if (Math.Abs(dot) < 1e-4f) return true;
                float full = halfLen * 2f;
                float tCross = -sBack / dot;
                tCross = MathHelper.Clamp(tCross, 0f, full);
                if (tCross < 1f) return false;
                center  = endBack + dirVec * (tCross * 0.5f);
                stretch = tCross / texHeight;
                return true;
            }
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.netMode == NetmodeID.Server) return false;

            Texture2D tex    = TextureAssets.Projectile[Projectile.type].Value;
            Vector2   origin = tex.Size() * 0.5f;

            float fadeAlpha = fading
                ? MathHelper.Clamp(1f - fadeAge / (float)FadeFrames, 0f, 1f)
                : 1f;

            int wallBreak = FindTrailWallBreak();

            DrawTrailLayer(tex, origin, new Color(60, 145, 255, 80)  * fadeAlpha, 0.55f, wallBreak);
            DrawTrailLayer(tex, origin, new Color(190, 230, 255, 220) * fadeAlpha, 0.25f, wallBreak);

            if (!fading && wallBreak < 0)
            {
                Vector2 headCenter = Projectile.Center;
                float   speed      = Projectile.velocity.Length();
                float   tipS       = MathHelper.Max(speed / 4f, 1.4f);
                Vector2 headDir    = speed > 0.01f ? Projectile.velocity / speed : Vector2.UnitY;

                if (ClipSpriteSegment(headDir, tex.Height, ref headCenter, ref tipS))
                {
                    Main.EntitySpriteDraw(tex, headCenter - Main.screenPosition, null,
                        new Color(220, 240, 255, 240), Projectile.rotation, origin,
                        new Vector2(0.32f, tipS), SpriteEffects.None, 0);
                }
            }

            return false;
        }

        private void DrawTrailLayer(Texture2D tex, Vector2 origin, Color baseColor, float widthScale, int wallBreak)
        {
            int   len       = Projectile.oldPos.Length;
            float texHeight = tex.Height;

            for (int i = 0; i < len - 1; i++)
            {
                if (wallBreak >= 0 && i < wallBreak) continue;

                Vector2 cur = Projectile.oldPos[i];
                Vector2 nxt = Projectile.oldPos[i + 1];
                if (cur == Vector2.Zero || nxt == Vector2.Zero) continue;

                Vector2 curCenter = cur + Projectile.Size * 0.5f;

                Vector2 delta = cur - nxt;
                float   dl    = delta.Length();
                if (dl < 0.5f) continue;

                float   rot     = delta.ToRotation() + MathHelper.PiOver2;
                float   fade    = 1f - (i / (float)len) * 0.85f;
                float   stretch = MathHelper.Max(dl / 4f, 1.2f);
                Vector2 dirVec  = delta / dl;

                Vector2 drawCenter  = curCenter;
                float   drawStretch = stretch;

                if (!ClipSpriteSegment(dirVec, texHeight, ref drawCenter, ref drawStretch))
                    continue;

                if (fading && fadingFromNpc)
                {
                    if (!ClipAgainstFadeStop(dirVec, texHeight, ref drawCenter, ref drawStretch))
                        continue;
                }

                Vector2 dPos = drawCenter - Main.screenPosition;
                Main.EntitySpriteDraw(tex, dPos, null,
                    baseColor * fade, rot, origin,
                    new Vector2(widthScale, drawStretch), SpriteEffects.None, 0);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player owner = Main.player[Projectile.owner];
            MyPlayer mp = owner.GetModPlayer<MyPlayer>();
            bool crit = Main.rand.NextFloat(1, 100 + 1) <= mp.standCritChangeBoosts;
            if (crit) modifiers.SetCrit();
            modifiers.SourceDamage *= mp.standDamageBoosts * 0.85f;
        }
    }
}
