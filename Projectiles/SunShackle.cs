using System;
using JoJoStands.Items.Hamon;
using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles
{
    public class SunShackle : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        private NPC grabbedNPC = null;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            grabbedNPC = Main.npc[(int)Projectile.ai[0]];
            if (Main.player[Projectile.owner].dead || grabbedNPC == null)
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
            float distance = Vector2.Distance(player.Center, Projectile.Center);

            if (grabbedNPC != null)
            {
                if (!grabbedNPC.active)
                {
                    grabbedNPC = null;
                    return;
                }
                if (distance > 25f * 16f)
                {
                    grabbedNPC.GetGlobalNPC<JoJoGlobalNPC>().sunShackled = false;
                    Projectile.Kill();
                    return;
                }

                Projectile.timeLeft = 300;
                Projectile.position = grabbedNPC.Center - new Vector2(Projectile.width / 2f, Projectile.height / 2f);
            }

            int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169);
            Main.dust[dustIndex].noGravity = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        private Texture2D shackleChainTexture;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            if (Main.netMode != NetmodeID.Server && shackleChainTexture == null)
                shackleChainTexture = Mod.GetTexture("Projectiles/SunShackle_Chain");

            Vector2 linkCenter = player.Center;
            Vector2 center = Projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / shackleChainTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(shackleChainTexture, pos, new Rectangle(0, 0, shackleChainTexture.Width, shackleChainTexture.Height), Color.White * Projectile.ai[1], rotation, new Vector2(shackleChainTexture.Width * 0.5f, shackleChainTexture.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.ai[1];
        }
    }
}