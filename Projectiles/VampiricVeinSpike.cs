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
    public class VampiricVeinSpike : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 800;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        private bool living = true;
        private const float MaxDistance = 24f * 16f;

        public override void AI()
        {
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
                player.direction = -1;
            }
            if (direction < 0)
            {
                Projectile.direction = 1;
                player.direction = 1;
            }
            Vector2 rota = player.Center - Projectile.Center;
            Projectile.rotation = (-rota).ToRotation();
            float playerDistance = Vector2.Distance(Projectile.Center, player.Center);
            if (living)
            {
                if (playerDistance > MaxDistance)
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
                if (playerDistance < 3 * 15f)
                    Projectile.Kill();

                Vector2 directionToPlayer = player.Center - Projectile.Center;
                directionToPlayer.Normalize();
                Projectile.velocity = directionToPlayer * 10f;
                Projectile.tileCollide = false;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>());

            living = false;
            if (vPlayer.HasSkill(player, VampirePlayer.SavageInstincts))
                if (Main.rand.Next(0, 100) <= vPlayer.lacerationChance)
                    target.AddBuff(ModContent.BuffType<Lacerated>(), (vPlayer.GetSkillLevel(player, VampirePlayer.SavageInstincts) * 4) * 60);
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
            Player player = Main.player[Projectile.owner];

            if (Main.netMode != NetmodeID.Server && veinPartTexture == null)
            {
                veinPartTexture = Mod.GetTexture("Projectiles/VampiricVein_Part>();
                veinPartOrigin = new Vector2(veinPartTexture.Width * 0.5f, veinPartTexture.Height * 0.5f);
            }

            Vector2 playerCenter = player.Center;
            float rotation = (playerCenter - Projectile.Center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(Projectile.Center, playerCenter) / veinPartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(Projectile.Center, playerCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(veinPartTexture, pos, new Rectangle(0, 0, veinPartTexture.Width, veinPartTexture.Height), lightColor, rotation, veinPartOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}