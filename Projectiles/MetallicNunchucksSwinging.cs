using System;
using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class MetallicNunchucksSwinging : ModProjectile
    {
        public override string Texture => mod.Name + "/Projectiles/MetallicNunchucksProjectile";

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 6;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }

        private float rotation = 0f;
        private int hamonConsumptionTimer = 0;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
            if (player.dead)
            {
                projectile.Kill();
                return;
            }

            Vector2 rota = player.Center - projectile.Center;
            projectile.rotation = (-rota).ToRotation();

            if (Main.mouseLeft)
            {
                projectile.timeLeft = 2;

                rotation += 24f * player.direction;
                if (rotation >= 360f)
                {
                    rotation = rotation - 360f;
                }
                if (rotation <= 0)
                {
                    rotation = rotation + 360f;
                }

                hamonConsumptionTimer++;
                if (hamonConsumptionTimer > 120)
                {
                    hPlayer.amountOfHamon -= 2;
                    hamonConsumptionTimer = 0;
                }

                projectile.Center = player.Center + (MathHelper.ToRadians(rotation).ToRotationVector2() * 32f);
                projectile.velocity = Vector2.Zero;
                player.itemTime = 2;
                player.itemAnimation = 2;
            }
            else
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("MetallicNunchucksProjectile"), projectile.damage, projectile.knockBack, projectile.owner);
                projectile.Kill();
            }

            int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 169);
            Main.dust[dustIndex].noGravity = true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)(damage * 0.4f);
            knockback *= 0.4f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
            if (hPlayer.amountOfHamon >= 4 && Main.rand.Next(0, 4 + 1) == 0)
            {
                hPlayer.amountOfHamon -= 4;
                target.AddBuff(mod.BuffType("Sunburn"), 4 * 60);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        private Texture2D chainTexture;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];

            if (Main.netMode != NetmodeID.Server && chainTexture == null)
                chainTexture = mod.GetTexture("Projectiles/ChainedClaw_Chain");

            Vector2 linkCenter = player.Center;
            Vector2 center = projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / chainTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(chainTexture, pos, new Rectangle(0, 0, chainTexture.Width, chainTexture.Height), lightColor, rotation, new Vector2(chainTexture.Width * 0.5f, chainTexture.Height * 0.5f), projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}