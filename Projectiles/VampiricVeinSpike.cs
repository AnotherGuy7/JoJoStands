using System;
using JoJoStands.Items.Hamon;
using JoJoStands.Items.Vampire;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class VampiricVeinSpike : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.timeLeft = 800;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }

        private bool living = true;
        private const float MaxDistance = 24f * 16f;

        public override void AI()
        {
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
                player.direction = -1;
            }
            if (direction < 0)
            {
                projectile.direction = 1;
                player.direction = 1;
            }
            Vector2 rota = player.Center - projectile.Center;
            projectile.rotation = (-rota).ToRotation();
            float playerDistance = Vector2.Distance(projectile.Center, player.Center);
            if (living)
            {
                if (playerDistance > MaxDistance)
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
                if (playerDistance < 3 * 15f)
                    projectile.Kill();

                Vector2 directionToPlayer = player.Center - projectile.Center;
                directionToPlayer.Normalize();
                projectile.velocity = directionToPlayer * 10f;
                projectile.tileCollide = false;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();

            living = false;
            if (vPlayer.HasSkill(player, VampirePlayer.SavageInstincts))
                if (Main.rand.Next(0, 100) <= vPlayer.lacerationChance)
                    target.AddBuff(mod.BuffType("Lacerated"), (vPlayer.GetSkillLevel(player, VampirePlayer.SavageInstincts) * 4) * 60);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            living = false;
            return false;
        }

        private Texture2D veinPartTexture;
        private Vector2 veinPartOrigin;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];

            if (Main.netMode != NetmodeID.Server && veinPartTexture == null)
            {
                veinPartTexture = mod.GetTexture("Projectiles/VampiricVein_Part");
                veinPartOrigin = new Vector2(veinPartTexture.Width * 0.5f, veinPartTexture.Height * 0.5f);
            }

            Vector2 playerCenter = player.Center;
            float rotation = (playerCenter - projectile.Center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(projectile.Center, playerCenter) / veinPartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(projectile.Center, playerCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(veinPartTexture, pos, new Rectangle(0, 0, veinPartTexture.Width, veinPartTexture.Height), lightColor, rotation, veinPartOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}