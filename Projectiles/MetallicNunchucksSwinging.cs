using JoJoStands.Buffs.Debuffs;
using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class MetallicNunchucksSwinging : ModProjectile
    {
        public override string Texture => Mod.Name + "/Projectiles/MetallicNunchucksProjectile";

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 6;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        private float rotation = 0f;
        private int hamonConsumptionTimer = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
            if (player.dead)
            {
                Projectile.Kill();
                return;
            }

            Vector2 rota = player.Center - Projectile.Center;
            Projectile.rotation = (-rota).ToRotation();

            if (Main.mouseLeft)
            {
                Projectile.timeLeft = 2;

                rotation += 24f * player.direction;
                if (rotation >= 360f)
                {
                    rotation = rotation - 360f;
                }
                if (rotation <= 0)
                {
                    rotation = rotation + 360f;
                }

                hamonConsumptionTimer++;
                if (hamonConsumptionTimer > 120)
                {
                    hPlayer.amountOfHamon -= 2;
                    hamonConsumptionTimer = 0;
                }

                Projectile.Center = player.Center + (MathHelper.ToRadians(rotation).ToRotationVector2() * 32f);
                Projectile.velocity = Vector2.Zero;
                player.itemTime = 2;
                player.itemAnimation = 2;
            }
            else
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<MetallicNunchucksProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile.Kill();
            }

            int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169);
            Main.dust[dustIndex].noGravity = true;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 0.4f);
            knockback *= 0.4f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
            if (hPlayer.amountOfHamon >= 4 && Main.rand.Next(0, 4 + 1) == 0)
            {
                hPlayer.amountOfHamon -= 4;
                target.AddBuff(ModContent.BuffType<Sunburn>(), 4 * 60);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        private Texture2D chainTexture;

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            if (Main.netMode != NetmodeID.Server && chainTexture == null)
                chainTexture = ModContent.Request<Texture2D>("JoJoStands/Projectiles/ChainedClaw_Chain").Value;

            Vector2 linkCenter = player.Center;
            Vector2 center = Projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / chainTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                Main.EntitySpriteDraw(chainTexture, pos, new Rectangle(0, 0, chainTexture.Width, chainTexture.Height), lightColor, rotation, new Vector2(chainTexture.Width * 0.5f, chainTexture.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}