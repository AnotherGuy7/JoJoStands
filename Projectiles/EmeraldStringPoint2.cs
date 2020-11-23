using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class EmeraldStringPoint2 : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/EmeraldStringPoint"; }
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;     //Either a string or an attack that comes from all sides of the screen to the middle
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        private bool searchingForLink = true;
        private int linkWhoAmI = -1;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (searchingForLink)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile otherProj = Main.projectile[i];
                    if (otherProj.active)
                    {
                        if (otherProj.type == mod.ProjectileType("EmeraldStringPoint") && otherProj.ai[0] == 0f)
                        {
                            linkWhoAmI = otherProj.whoAmI;
                            otherProj.ai[0] = 1f;      //meaning, linked
                        }
                    }
                }
                if (linkWhoAmI == -1)
                {
                    projectile.Kill();
                    return;
                }
                searchingForLink = false;
            }
            if (!Main.projectile[linkWhoAmI].active || Main.projectile[linkWhoAmI].timeLeft <= 0)
            {
                projectile.Kill();
                return;
            }
            if (!searchingForLink && linkWhoAmI != -1)
            {
                if (projectile.ai[1] == 0f)
                {
                    projectile.timeLeft = 1200;
                    projectile.ai[1] = 1f;
                }
                for (int k = 0; k < Main.maxNPCs; k++)
                {
                    NPC npc = Main.npc[k];
                    if (npc.active)
                    {
                        if (Collision.CheckAABBvLineCollision(npc.position, new Vector2(npc.width, npc.height), projectile.Center, Main.projectile[linkWhoAmI].Center))
                        {
                            float numberProjectiles = 6;
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                int proj = Projectile.NewProjectile(player.position.X + (Main.screenWidth / 2) * npc.direction, npc.position.Y + Main.rand.NextFloat(-10f, 11f), (10f * -npc.direction) - Main.rand.NextFloat(0f, 3f), 0f, mod.ProjectileType("Emerald"), 32 + (int)projectile.ai[0], 7f, projectile.owner);
                                Main.projectile[proj].netUpdate = true;
                                Main.projectile[proj].tileCollide = false;
                            }
                            projectile.Kill();
                        }
                    }
                }
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        Player otherPlayer = Main.player[p];
                        if (otherPlayer.active)
                        {
                            if (otherPlayer.whoAmI != player.whoAmI && Collision.CheckAABBvLineCollision(otherPlayer.position, new Vector2(otherPlayer.width, otherPlayer.height), projectile.Center, Main.projectile[linkWhoAmI].Center))
                            {
                                float numberProjectiles = 6;
                                for (int i = 0; i < numberProjectiles; i++)
                                {
                                    int proj = Projectile.NewProjectile(player.position.X + (Main.screenWidth / 2) * otherPlayer.direction, otherPlayer.position.Y + Main.rand.NextFloat(-10f, 11f), (10f * -otherPlayer.direction) - Main.rand.NextFloat(0f, 3f), 0f, mod.ProjectileType("Emerald"), 32 + (int)projectile.ai[0], 7f, projectile.owner);
                                    Main.projectile[proj].netUpdate = true;
                                    Main.projectile[proj].tileCollide = false;
                                }
                                projectile.Kill();
                            }
                        }
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (linkWhoAmI != -1)
            {
                Vector2 linkCenter = Main.projectile[linkWhoAmI].Center;
                Vector2 center = projectile.Center;
                float rotation = (linkCenter - center).ToRotation();
                Texture2D texture = mod.GetTexture("Projectiles/EmeraldString");
                for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / texture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
                {
                    Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                    spriteBatch.Draw(texture, pos, new Rectangle(0, 0, texture.Width, texture.Height), lightColor, rotation, new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), projectile.scale, SpriteEffects.None, 0f);
                }
            }
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Main.projectile[linkWhoAmI].Kill();
        }
    }
}