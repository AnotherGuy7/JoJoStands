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
            Projectile.penetrate             = 1;
            Projectile.timeLeft              = 200;
            Projectile.ignoreWater           = true;
            Projectile.tileCollide           = true;
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
            else if (TryGetAnchor(out Vector2 anchorNow))
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

            Projectile.velocity.Y += 0.32f;
            if (Projectile.velocity.Y > 26f) Projectile.velocity.Y = 26f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

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

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.netMode == NetmodeID.Server) return false;

            Texture2D tex    = TextureAssets.Projectile[Projectile.type].Value;
            Vector2   origin = tex.Size() * 0.5f;

            DrawTrailLayer(tex, origin, new Color(60, 145, 255, 80),  0.55f);
            DrawTrailLayer(tex, origin, new Color(190, 230, 255, 220), 0.25f);

            float speed = Projectile.velocity.Length();
            float tipS  = MathHelper.Clamp(speed / 4f, 1.4f, 3.5f);
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null,
                new Color(220, 240, 255, 240), Projectile.rotation, origin,
                new Vector2(0.32f, tipS), SpriteEffects.None, 0);

            return false;
        }

        private void DrawTrailLayer(Texture2D tex, Vector2 origin, Color baseColor, float widthScale)
        {
            int len = Projectile.oldPos.Length;
            for (int i = 0; i < len - 1; i++)
            {
                Vector2 cur = Projectile.oldPos[i];
                Vector2 nxt = Projectile.oldPos[i + 1];
                if (cur == Vector2.Zero || nxt == Vector2.Zero) continue;

                Vector2 delta = cur - nxt;
                float dl = delta.Length();
                if (dl < 0.5f) continue;

                float rot     = delta.ToRotation() + MathHelper.PiOver2;
                Vector2 dPos  = cur + Projectile.Size * 0.5f - Main.screenPosition;
                float fade    = 1f - (i / (float)len) * 0.85f;
                float stretch = MathHelper.Clamp(dl / 4f, 1.2f, 4f);

                Main.EntitySpriteDraw(tex, dPos, null,
                    baseColor * fade, rot, origin,
                    new Vector2(widthScale, stretch), SpriteEffects.None, 0);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player owner = Main.player[Projectile.owner];
            MyPlayer mp = owner.GetModPlayer<MyPlayer>();
            bool crit = Main.rand.NextFloat(1, 100 + 1) <= mp.standCritChangeBoosts;
            if (crit) modifiers.SetCrit();
            modifiers.SourceDamage *= mp.standDamageBoosts;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 4; i++)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Water, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-1.5f, -0.3f),
                    100, new Color(140, 210, 255), Main.rand.NextFloat(0.7f, 0.9f));
            return true;
        }
    }
}
