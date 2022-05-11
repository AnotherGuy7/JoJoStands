using JoJoStands.Buffs.Debuffs;
using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ChainedKunaiProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        private bool living = true;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Main.player[Projectile.owner].dead)
            {
                Projectile.Kill();
                return;
            }
            float direction = player.Center.X - Projectile.Center.X;
            if (direction > 0)
            {
                Projectile.direction = -1;
                player.direction = -1;
            }
            if (direction < 0)
            {
                Projectile.direction = 1;
                player.direction = 1;
            }
            Vector2 rota = player.Center - Projectile.Center;
            Projectile.rotation = (-rota).ToRotation();
            Vector2 vector14 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
            float num166 = player.position.X + (float)(player.width / 2) - vector14.X;
            float num167 = player.position.Y + (float)(player.height / 2) - vector14.Y;
            float num168 = (float)Math.Sqrt((double)(num166 * num166 + num167 * num167));
            if (living)
            {
                if (num168 > 20f * 16f)
                {
                    living = false;
                }
                //Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] > 5f)
                {
                    Projectile.alpha = 0;
                }
                if (Projectile.ai[1] >= 10f)
                {
                    Projectile.ai[1] = 15f;
                }
            }
            else if (!living)
            {
                Projectile.tileCollide = false;
                //Projectile.rotation = (float)Math.Atan2((double)num167, (double)num166) - 1.57f;
                float num169 = 20f;
                if (num168 < 50f)
                {
                    Projectile.Kill();
                }
                num168 = num169 / num168;
                num166 *= num168;
                num167 *= num168;
                Projectile.velocity.X = num166;
                Projectile.velocity.Y = num167;
            }
            int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169);
            Main.dust[dustIndex].noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();

            living = false;
            if (hPlayer.amountOfHamon >= 4 && Main.rand.Next(0, 1 + 1) == 0)
            {
                hPlayer.amountOfHamon -= 4;
                target.AddBuff(ModContent.BuffType<Sunburn>(), 12 * 60);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            living = false;
            return false;
        }

        private Vector2 offset;
        private Texture2D stringPartTexture;

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            if (Main.netMode != NetmodeID.Server && stringPartTexture == null)
                stringPartTexture = Mod.Assets.Request<Texture2D>("Projectiles/ChainedKunai_StringPart").Value;

            Vector2 linkCenter = player.Center + offset;
            Vector2 center = Projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / stringPartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                Main.EntitySpriteDraw(stringPartTexture, pos, new Rectangle(0, 0, stringPartTexture.Width, stringPartTexture.Height), lightColor, rotation, new Vector2(stringPartTexture.Width * 0.5f, stringPartTexture.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}