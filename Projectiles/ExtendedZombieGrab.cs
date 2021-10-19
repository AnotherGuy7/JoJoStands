using JoJoStands.Items.Vampire;
using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class ExtendedZombieGrab : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 18;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }

        private bool living = true;
        private NPC heldNPC = null;
        private bool alreadyGrabbedNPC = false;
        private int heldEnemyTimer = 0;

        private const float MaxDistance = 12f * 16f;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (Main.player[projectile.owner].dead)
            {
                projectile.Kill();
                return;
            }

            float direction = player.Center.X - projectile.Center.X;
            player.direction = -1;
            projectile.direction = -1;
            if (direction < 0)
            {
                projectile.direction = 1;
                player.direction = 1;
            }
            Vector2 rota = player.Center - projectile.Center;
            projectile.rotation = (-rota).ToRotation();
            float distance = Vector2.Distance(player.Center, projectile.Center);

            if (heldNPC != null)
            {
                if (!heldNPC.active || distance > MaxDistance || !Main.mouseRight)
                {
                    living = false;
                    heldNPC = null;
                    return;
                }

                projectile.timeLeft = 300;
                projectile.position = heldNPC.Center - new Vector2(projectile.width / 2f, projectile.height / 2f);

                if (!heldNPC.boss)
                {
                    heldNPC.velocity.X = 0f;
                    heldNPC.GetGlobalNPC<JoJoGlobalNPC>().vampireUserLastHitIndex = player.whoAmI;
                }

                heldEnemyTimer++;
                if (heldEnemyTimer >= 60)
                {
                    vPlayer.StealHealthFrom(heldNPC, heldNPC.lifeMax, 0f, 16, true);
                    Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 3, 1f, -0.8f);
                    heldEnemyTimer = 0;
                }
                projectile.frame = 1;
            }
            else
            {
                projectile.frame = 0;
            }

            if (living)
            {
                if (distance > MaxDistance)
                {
                    living = false;
                }
                projectile.ai[1] += 1f;
                if (projectile.ai[1] > 5f)
                {
                    projectile.alpha = 0;
                }
                if (projectile.ai[1] >= 10f)
                {
                    projectile.ai[1] = 15f;
                }
            }
            else if (!living)
            {
                projectile.tileCollide = false;
                Vector2 returnVel = player.Center - projectile.Center;
                returnVel.Normalize();
                returnVel *= 9f;
                projectile.velocity = returnVel;

                if (distance < 4 * 16f)
                {
                    projectile.Kill();
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

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];

            if (Main.netMode != NetmodeID.Server && armPartTexture == null)
                armPartTexture = mod.GetTexture("Projectiles/ExtendedZombieGrab_Part");

            Vector2 linkCenter = player.Center;
            Vector2 center = projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / armPartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(armPartTexture, pos, new Rectangle(0, 0, armPartTexture.Width, armPartTexture.Height), lightColor, rotation, new Vector2(armPartTexture.Width * 0.5f, armPartTexture.Height * 0.5f), projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}