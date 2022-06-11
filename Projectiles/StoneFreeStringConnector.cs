using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class StoneFreeStringConnector : ModProjectile
    {
        public override string Texture => Mod.Name + "/Projectiles/PlayerStands/StandPlaceholder";

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        private int linkWhoAmI = -1;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] < 0 || Projectile.ai[0] >= Main.maxProjectiles || !Main.projectile[(int)Projectile.ai[0]].active)
            {
                Projectile.Kill();
                return;
            }

            linkWhoAmI = (int)Projectile.ai[0];
            Projectile linkedProjectile = Main.projectile[linkWhoAmI];
            if (linkedProjectile == null || !linkedProjectile.active)
            {
                Projectile.Kill();
                return;
            }
            else
            {
                Projectile.timeLeft = 4;
                linkedProjectile.timeLeft = 4;
            }

            bool objectHit = false;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (Collision.CheckAABBvLineCollision(npc.position, new Vector2(npc.width, npc.height), Projectile.Center, linkedProjectile.Center))
                    {
                        npc.StrikeNPC((int)Projectile.ai[1], 0f, -npc.direction);
                        objectHit = true;
                        linkedProjectile.Kill();
                        Projectile.Kill();
                        break;
                    }
                }
            }
            if (objectHit)
                return;

            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile otherProj = Main.projectile[p];
                if (otherProj.active)
                {
                    if (!Projectile.friendly && Collision.CheckAABBvLineCollision(otherProj.position, new Vector2(otherProj.width, otherProj.height), Projectile.Center, linkedProjectile.Center))
                    {
                        otherProj.Kill();
                        objectHit = true;
                        linkedProjectile.Kill();
                        Projectile.Kill();
                        break;
                    }
                }
            }

            if (objectHit)
                return;

            if (MyPlayer.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active)
                    {
                        if (otherPlayer.whoAmI != player.whoAmI && Collision.CheckAABBvLineCollision(otherPlayer.position, new Vector2(otherPlayer.width, otherPlayer.height), Projectile.Center, linkedProjectile.Center))
                        {
                            otherPlayer.Hurt(PlayerDeathReason.ByCustomReason(otherPlayer.name + " fell right into " + player.name + "'s string trap!"), (int)Projectile.ai[1], -otherPlayer.direction);
                            linkedProjectile.Kill();
                            Projectile.Kill();
                            break;
                        }
                    }
                }
            }
        }

        private int sinTimer = 0;
        private Texture2D stringTexture;
        private Color drawColor;

        public override bool PreDraw(ref Color lightColor)
        {
            sinTimer++;
            if (sinTimer >= 360)
                sinTimer = 0;

            if (stringTexture == null)
                stringTexture = ModContent.Request<Texture2D>("JoJoStands/Projectiles/StoneFreeString_Part").Value;

            if (linkWhoAmI != -1)
            {
                Vector2 linkCenter = Main.projectile[linkWhoAmI].Center;
                Vector2 projectileCenter = Projectile.Center;
                drawColor = lightColor;

                float stringRotation = (linkCenter - projectileCenter).ToRotation();
                float stringScale = 0.6f;
                float loopIncrement = 1 / (Vector2.Distance(projectileCenter, linkCenter) / (stringTexture.Width * stringScale));
                float lightLevelIndex = 0f;
                for (float k = 0; k <= 1; k += loopIncrement)     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
                {
                    float sinOffset = 1 + ((float)Math.Sin((k * 3) + (sinTimer / 20f)) * 1.6f);
                    if (k == 0)
                        sinOffset = 0f;

                    lightLevelIndex += loopIncrement;
                    Vector2 pos = Vector2.Lerp(projectileCenter, linkCenter + new Vector2((float)Math.Cos(sinOffset), (float)Math.Sin(sinOffset)), k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                    if (lightLevelIndex >= 0.1f)        //Gets new light levels every 10% of the string.
                    {
                        drawColor = Lighting.GetColor((int)(pos.X + Main.screenPosition.X) / 16, (int)(pos.Y + Main.screenPosition.Y) / 16);
                        lightLevelIndex = 0f;
                    }

                    Main.EntitySpriteDraw(stringTexture, pos, new Rectangle(0, 0, stringTexture.Width, stringTexture.Height), drawColor, stringRotation, new Vector2(stringTexture.Width * 0.5f, stringTexture.Height * 0.5f), Projectile.scale * stringScale, SpriteEffects.None, 0);
                }
            }
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}