using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class EmeraldStringPointConnector : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/EmeraldStringPoint"; }
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;     //Either a string or an attack that comes from all sides of the screen to the middle
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private bool searchingForLink = true;
        private int linkWhoAmI = -1;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (searchingForLink)
            {
                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile otherProj = Main.projectile[p];
                    if (otherProj.active)
                    {
                        if (otherProj.type == ModContent.ProjectileType<EmeraldStringPoint>() && otherProj.ai[0] == 0f)
                        {
                            linkWhoAmI = otherProj.whoAmI;
                            otherProj.ai[0] = 1f;      //Marks the other point as linked
                            break;
                        }
                    }
                }
                if (linkWhoAmI == -1)
                {
                    Projectile.Kill();
                    return;
                }
                searchingForLink = false;
            }

            Projectile linkedProjectile = Main.projectile[linkWhoAmI];
            if (linkedProjectile == null || !linkedProjectile.active)
            {
                Projectile.Kill();
                return;
            }

            if (!searchingForLink && linkWhoAmI != -1)
            {
                if (Projectile.ai[1] == 0f)
                {
                    Projectile.timeLeft = 1200;
                    Projectile.ai[1] = 1f;
                }

                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        if (Collision.CheckAABBvLineCollision(npc.position, new Vector2(npc.width, npc.height), Projectile.Center, linkedProjectile.Center))
                        {
                            float numberProjectiles = 6;
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.position.X + (Main.screenWidth / 2) * npc.direction, npc.position.Y + Main.rand.NextFloat(-10f, 11f), (10f * -npc.direction) - Main.rand.NextFloat(0f, 3f), 0f, ModContent.ProjectileType<Emerald>(), 32 + (int)Projectile.ai[0], 7f, Projectile.owner);
                                Main.projectile[proj].netUpdate = true;
                                Main.projectile[proj].tileCollide = false;
                            }
                            linkedProjectile.Kill();
                            Projectile.Kill();
                        }
                    }
                }
                if (MyPlayer.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer)
                {
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        Player otherPlayer = Main.player[p];
                        if (otherPlayer.active)
                        {
                            if (otherPlayer.whoAmI != player.whoAmI && Collision.CheckAABBvLineCollision(otherPlayer.position, new Vector2(otherPlayer.width, otherPlayer.height), Projectile.Center, linkedProjectile.Center))
                            {
                                float numberProjectiles = 6;
                                for (int i = 0; i < numberProjectiles; i++)
                                {
                                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.position.X + (Main.screenWidth / 2) * otherPlayer.direction, otherPlayer.position.Y + Main.rand.NextFloat(-10f, 11f), (10f * -otherPlayer.direction) - Main.rand.NextFloat(0f, 3f), 0f, ModContent.ProjectileType<Emerald>(), 32 + (int)Projectile.ai[0], 7f, Projectile.owner);
                                    Main.projectile[proj].netUpdate = true;
                                    Main.projectile[proj].tileCollide = false;
                                }
                                linkedProjectile.Kill();
                                Projectile.Kill();
                            }
                        }
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (linkWhoAmI != -1)
            {
                Vector2 linkCenter = Main.projectile[linkWhoAmI].Center;
                Vector2 projectileCenter = Projectile.Center;

                float stringRotation = (linkCenter - projectileCenter).ToRotation();
                float stringScale = 0.6f;
                Texture2D stringTexture = ModContent.Request<Texture2D>("JoJoStands/Projectiles/EmeraldString").Value;
                for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(projectileCenter, linkCenter) / (stringTexture.Width * stringScale)))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
                {
                    Vector2 pos = Vector2.Lerp(projectileCenter, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                    Main.EntitySpriteDraw(stringTexture, pos, new Rectangle(0, 0, stringTexture.Width, stringTexture.Height), lightColor, stringRotation, new Vector2(stringTexture.Width * 0.5f, stringTexture.Height * 0.5f), Projectile.scale * stringScale, SpriteEffects.None, 0);
                }
            }

            Vector2 origin = new Vector2(Projectile.width / 2f, Projectile.height / 2f);
            Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}