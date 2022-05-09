using System;
using JoJoStands.Items.Hamon;
using JoJoStands.Items.Vampire;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles
{
    public class VampiricVeinGrab : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 10;
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
        private const float MaxDistance = 16f * 16f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (player.dead)
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
                if (!grabbedNPC.active || distance > MaxDistance || !Main.mouseRight)
                {
                    living = false;
                    grabbedNPC = null;
                    return;
                }

                Projectile.timeLeft = 300;
                Projectile.position = grabbedNPC.Center - new Vector2(Projectile.width / 2f, Projectile.height / 2f);

                heldNPCTimer++;
                if (heldNPCTimer >= 30)
                {
                    vPlayer.StealHealthFrom(grabbedNPC, Projectile.damage, 0f, 45, true);
                    heldNPCTimer = 0;
                }
            }

            if (living)
            {
                if (distance > MaxDistance)
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
            else
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

        private Texture2D veinPartTexture;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            if (Main.netMode != NetmodeID.Server && veinPartTexture == null)
                veinPartTexture = Mod.GetTexture("Projectiles/VampiricVein_Part");

            Vector2 linkCenter = player.Center;
            Vector2 center = Projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / veinPartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(veinPartTexture, pos, new Rectangle(0, 0, veinPartTexture.Width, veinPartTexture.Height), lightColor, rotation, new Vector2(veinPartTexture.Width * 0.5f, veinPartTexture.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}