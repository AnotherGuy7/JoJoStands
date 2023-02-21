using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Buffs.Debuffs;

namespace JoJoStands.Projectiles
{
    public class HermitPurpleGrab : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/HermitPurpleVine_End"; }
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 12;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private bool living = true;
        private NPC heldNPC = null;

        public override void AI()       //all this so that the other chain doesn't draw... yare yare. It was mostly just picking out types
        {
            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);     //aiStyle 13 without the types
            Player player = Main.player[Projectile.owner];
            if (player.dead)
            {
                Projectile.Kill();
                return;
            }

            float direction = player.Center.X - Projectile.Center.X;
            if (direction > 0)
            {
                Projectile.direction = -1;
                player.ChangeDir(-1);
            }
            if (direction < 0)
            {
                Projectile.direction = 1;
                player.ChangeDir(1);
            }
            //Projectile.spriteDirection = Projectile.direction;

            if (!Main.mouseRight)
            {
                if (heldNPC != null)
                {
                    heldNPC.GetGlobalNPC<NPCs.JoJoGlobalNPC>().grabbedByHermitPurple = false;
                    heldNPC = null;
                }
                living = false;
            }

            Vector2 rotation = player.Center - Projectile.Center;
            Projectile.rotation = (-rotation).ToRotation();
            if (living)
            {
                if (heldNPC == null)
                {
                    Projectile.rotation = (-rotation).ToRotation();
                    if (Projectile.owner == Main.myPlayer)
                    {
                        float distance = Projectile.Distance(Main.MouseWorld);
                        if (distance >= 20f)
                        {
                            Projectile.velocity = Main.MouseWorld - Projectile.Center;
                            Projectile.velocity.Normalize();
                            Projectile.velocity *= 10f;
                        }
                        else
                        {
                            Projectile.velocity = Vector2.Zero;
                        }
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (heldNPC == null || !heldNPC.active)
                    {
                        living = false;
                        return;
                    }

                    Projectile.timeLeft = 2;
                    heldNPC.GetGlobalNPC<NPCs.JoJoGlobalNPC>().grabbedByHermitPurple = true;
                    heldNPC.velocity = Vector2.Zero;
                    Projectile.position = heldNPC.Center;
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
                float distance = Projectile.Distance(player.Center);
                if (distance >= 20f)
                {
                    Projectile.velocity = player.Center - Projectile.Center;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 10f;
                }
                else
                {
                    Projectile.Kill();
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            if (heldNPC != null)
            {
                heldNPC.GetGlobalNPC<NPCs.JoJoGlobalNPC>().grabbedByHermitPurple = false;
                heldNPC = null;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                crit = true;
            if (!target.boss && !target.immortal && living)
                heldNPC = target;
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.Next(1, 100 + 1) >= 60)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            living = false;
            return false;
        }

        private Texture2D hermitPurpleVinePartTexture;

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            if (Main.netMode != NetmodeID.Server)
                hermitPurpleVinePartTexture = ModContent.Request<Texture2D>("JoJoStands/Projectiles/HermitPurpleVine_Part").Value;

            Vector2 offset = new Vector2(12f * player.direction, 0f);
            Vector2 linkCenter = player.Center + offset;
            Vector2 center = Projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / hermitPurpleVinePartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                Main.EntitySpriteDraw(hermitPurpleVinePartTexture, pos, new Rectangle(0, 0, hermitPurpleVinePartTexture.Width, hermitPurpleVinePartTexture.Height), lightColor, rotation, new Vector2(hermitPurpleVinePartTexture.Width * 0.5f, hermitPurpleVinePartTexture.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}