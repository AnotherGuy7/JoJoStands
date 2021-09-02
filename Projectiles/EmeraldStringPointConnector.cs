using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class EmeraldStringPointConnector : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/EmeraldStringPoint"; }
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
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
                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile otherProj = Main.projectile[p];
                    if (otherProj.active)
                    {
                        if (otherProj.type == mod.ProjectileType("EmeraldStringPoint") && otherProj.ai[0] == 0f)
                        {
                            linkWhoAmI = otherProj.whoAmI;
                            otherProj.ai[0] = 1f;      //Marks the other point as linked
                            break;
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

            Projectile linkedProjectile = Main.projectile[linkWhoAmI];
            if (linkedProjectile == null || !linkedProjectile.active)
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

                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        if (Collision.CheckAABBvLineCollision(npc.position, new Vector2(npc.width, npc.height), projectile.Center, linkedProjectile.Center))
                        {
                            float numberProjectiles = 6;
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                int proj = Projectile.NewProjectile(player.position.X + (Main.screenWidth / 2) * npc.direction, npc.position.Y + Main.rand.NextFloat(-10f, 11f), (10f * -npc.direction) - Main.rand.NextFloat(0f, 3f), 0f, mod.ProjectileType("Emerald"), 32 + (int)projectile.ai[0], 7f, projectile.owner);
                                Main.projectile[proj].netUpdate = true;
                                Main.projectile[proj].tileCollide = false;
                            }
                            linkedProjectile.Kill();
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
                            if (otherPlayer.whoAmI != player.whoAmI && Collision.CheckAABBvLineCollision(otherPlayer.position, new Vector2(otherPlayer.width, otherPlayer.height), projectile.Center, linkedProjectile.Center))
                            {
                                float numberProjectiles = 6;
                                for (int i = 0; i < numberProjectiles; i++)
                                {
                                    int proj = Projectile.NewProjectile(player.position.X + (Main.screenWidth / 2) * otherPlayer.direction, otherPlayer.position.Y + Main.rand.NextFloat(-10f, 11f), (10f * -otherPlayer.direction) - Main.rand.NextFloat(0f, 3f), 0f, mod.ProjectileType("Emerald"), 32 + (int)projectile.ai[0], 7f, projectile.owner);
                                    Main.projectile[proj].netUpdate = true;
                                    Main.projectile[proj].tileCollide = false;
                                }
                                linkedProjectile.Kill();
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
                Vector2 projectileCenter = projectile.Center;

                float stringRotation = (linkCenter - projectileCenter).ToRotation();
                float stringScale = 0.6f;
                Texture2D stringTexture = mod.GetTexture("Projectiles/EmeraldString");
                for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(projectileCenter, linkCenter) / (stringTexture.Width * stringScale)))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
                {
                    Vector2 pos = Vector2.Lerp(projectileCenter, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                    spriteBatch.Draw(stringTexture, pos, new Rectangle(0, 0, stringTexture.Width, stringTexture.Height), lightColor, stringRotation, new Vector2(stringTexture.Width * 0.5f, stringTexture.Height * 0.5f), projectile.scale * stringScale, SpriteEffects.None, 0f);
                }
            }

            Vector2 origin = new Vector2(projectile.width / 2f, projectile.height / 2f);
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}