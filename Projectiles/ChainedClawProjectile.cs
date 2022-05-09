using System;
using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles
{
    public class ChainedClawProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        private bool living = true;
        private NPC grabbedNPC = null;
        private bool alreadyGrabbedNPC = false;
        private int heldNPCTimer = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>());
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
            float distance = Vector2.Distance(player.Center, Projectile.Center);

            if (grabbedNPC != null)
            {
                if (!grabbedNPC.active || distance > 18f * 16f || !Main.mouseLeft)
                {
                    living = false;
                    grabbedNPC = null;
                    return;
                }

                Projectile.timeLeft = 300;
                Projectile.position = grabbedNPC.Center - new Vector2(Projectile.width / 2f, Projectile.height / 2f);

                heldNPCTimer++;
                if (hPlayer.amountOfHamon >= 5 && heldNPCTimer >= 80)
                {
                    grabbedNPC.StrikeNPC(Projectile.damage, 0f, Projectile.direction);
                    grabbedNPC.AddBuff(ModContent.BuffType<Sunburn>(), 5 * 60);
                    hPlayer.amountOfHamon -= 5;
                    heldNPCTimer = 0;
                }
            }

            if (living)
            {
                if (distance > 18f * 16f)
                {
                    living = false;
                }
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
                Vector2 returnVel = player.Center - Projectile.Center;
                returnVel.Normalize();
                returnVel *= 9f;
                Projectile.velocity = returnVel;

                if (distance < 50f)
                {
                    Projectile.Kill();
                }
            }

            player.itemTime = 2;
            player.itemAnimation = 2;
            if (hPlayer.amountOfHamon >= 5)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169);
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!alreadyGrabbedNPC)
            {
                grabbedNPC = target;
                alreadyGrabbedNPC = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (alreadyGrabbedNPC && grabbedNPC != null)
            {
                damage = 0;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            living = false;
            return false;
        }

        private Texture2D stringPartTexture;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            if (Main.netMode != NetmodeID.Server && stringPartTexture == null)
                stringPartTexture = Mod.GetTexture("Projectiles/ChainedClaw_Chain>();

            Vector2 linkCenter = player.Center;
            Vector2 center = Projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / stringPartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(stringPartTexture, pos, new Rectangle(0, 0, stringPartTexture.Width, stringPartTexture.Height), lightColor, rotation, new Vector2(stringPartTexture.Width * 0.5f, stringPartTexture.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}