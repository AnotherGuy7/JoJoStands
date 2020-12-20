using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class HermitPurpleGrab : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/HermitPurpleVine_End"; }
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 12;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        private bool living = true;
        private NPC heldNPC = null;

        public override void AI()       //all this so that the other chain doesn't draw... yare yare. It was mostly just picking out types
        {
            //projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);     //aiStyle 13 without the types
            Player player = Main.player[projectile.owner];
            if (player.dead)
            {
                projectile.Kill();
                return;
            }

            float direction = player.Center.X - projectile.Center.X;
            if (direction > 0)
            {
                projectile.direction = -1;
                player.ChangeDir(-1);
            }
            if (direction < 0)
            {
                projectile.direction = 1;
                player.ChangeDir(1);
            }
            //projectile.spriteDirection = projectile.direction;

            if (!Main.mouseRight)
            {
                heldNPC.GetGlobalNPC<NPCs.JoJoGlobalNPC>().grabbedByHermitPurple = false;
                heldNPC = null;
                living = false;
            }

            Vector2 rotation = player.Center - projectile.Center;
            projectile.rotation = (-rotation).ToRotation();
            if (living)
            {
                if (heldNPC == null)
                {
                    projectile.rotation = (-rotation).ToRotation();
                    if (projectile.owner == Main.myPlayer)
                    {
                        float distance = projectile.Distance(Main.MouseWorld);
                        if (distance >= 20f)
                        {
                            projectile.velocity = Main.MouseWorld - projectile.Center;
                            projectile.velocity.Normalize();
                            projectile.velocity *= 10f;
                        }
                        else
                        {
                            projectile.velocity = Vector2.Zero;
                        }
                        projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (heldNPC == null)
                    {
                        living = false;
                        return;
                    }

                    projectile.timeLeft = 2;
                    heldNPC.GetGlobalNPC<NPCs.JoJoGlobalNPC>().grabbedByHermitPurple = true;
                    heldNPC.velocity = Vector2.Zero;
                    projectile.position = heldNPC.Center;
                    rotation.Normalize();
                    rotation *= 0.3f;
                    heldNPC.velocity.X = rotation.X;
                    if (heldNPC.velocity.Y < 6f)
                    {
                        heldNPC.velocity.Y += 0.3f;
                    }
                }
            }
            else
            {
                float distance = projectile.Distance(player.Center);
                if (distance >= 20f)
                {
                    projectile.velocity = player.Center - projectile.Center;
                    projectile.velocity.Normalize();
                    projectile.velocity *= 10f;
                }
                else
                {
                    projectile.Kill();
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            heldNPC.GetGlobalNPC<NPCs.JoJoGlobalNPC>().grabbedByHermitPurple = false;
            heldNPC = null;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
            if (!target.boss && living)
            {
                heldNPC = target;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            living = false;
            return false;
        }

        private Texture2D hermitPurpleVinePartTexture;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];

            if (Main.netMode != NetmodeID.Server)
                hermitPurpleVinePartTexture = mod.GetTexture("Projectiles/HermitPurpleVine_Part");

            Vector2 offset = new Vector2(12f * player.direction, 0f);
            Vector2 linkCenter = player.Center + offset;
            Vector2 center = projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / hermitPurpleVinePartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(hermitPurpleVinePartTexture, pos, new Rectangle(0, 0, hermitPurpleVinePartTexture.Width, hermitPurpleVinePartTexture.Height), lightColor, rotation, new Vector2(hermitPurpleVinePartTexture.Width * 0.5f, hermitPurpleVinePartTexture.Height * 0.5f), projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}