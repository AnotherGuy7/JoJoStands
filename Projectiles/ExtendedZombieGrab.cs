using JoJoStands.Items.Vampire;
using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ExtendedZombieGrab : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        private bool living = true;
        private NPC heldNPC = null;
        private bool alreadyGrabbedNPC = false;
        private int heldEnemyTimer = 0;

        private const float MaxDistance = 12f * 16f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (Main.player[Projectile.owner].dead)
            {
                Projectile.Kill();
                return;
            }

            float direction = player.Center.X - Projectile.Center.X;
            player.direction = -1;
            Projectile.direction = -1;
            if (direction < 0)
            {
                Projectile.direction = 1;
                player.direction = 1;
            }
            Vector2 rota = player.Center - Projectile.Center;
            Projectile.rotation = (-rota).ToRotation();
            float distance = Vector2.Distance(player.Center, Projectile.Center);

            if (heldNPC != null)
            {
                if (!heldNPC.active || distance > MaxDistance || !Main.mouseRight)
                {
                    living = false;
                    heldNPC = null;
                    return;
                }

                Projectile.timeLeft = 300;
                Projectile.position = heldNPC.Center - new Vector2(Projectile.width / 2f, Projectile.height / 2f);

                if (!heldNPC.boss)
                {
                    heldNPC.velocity.X = 0f;
                    heldNPC.GetGlobalNPC<JoJoGlobalNPC>().vampireUserLastHitIndex = player.whoAmI;
                }

                heldEnemyTimer++;
                if (heldEnemyTimer >= 60)
                {
                    vPlayer.StealHealthFrom(heldNPC, heldNPC.lifeMax, 0f, 16, true);
                    SoundEngine.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 3, 1f, -0.8f);
                    heldEnemyTimer = 0;
                }
                Projectile.frame = 1;
            }
            else
            {
                Projectile.frame = 0;
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
            else if (!living)
            {
                Projectile.tileCollide = false;
                Vector2 returnVel = player.Center - Projectile.Center;
                returnVel.Normalize();
                returnVel *= 9f;
                Projectile.velocity = returnVel;

                if (distance < 4 * 16f)
                {
                    Projectile.Kill();
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!alreadyGrabbedNPC)
            {
                heldNPC = target;
                alreadyGrabbedNPC = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (alreadyGrabbedNPC && heldNPC != null)
            {
                damage = 0;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            living = false;
            return false;
        }

        private Texture2D armPartTexture;

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            if (Main.netMode != NetmodeID.Server && armPartTexture == null)
                armPartTexture = ModContent.Request<Texture2D>("JoJoStands/Projectiles/ExtendedZombieGrab_Part").Value;

            Vector2 linkCenter = player.Center;
            Vector2 center = Projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / armPartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                Main.EntitySpriteDraw(armPartTexture, pos, new Rectangle(0, 0, armPartTexture.Width, armPartTexture.Height), lightColor, rotation, new Vector2(armPartTexture.Width * 0.5f, armPartTexture.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}