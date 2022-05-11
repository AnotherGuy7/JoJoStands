using JoJoStands.Items.Vampire;
using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class KnifeSlashes : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.friendly = true;
        }

        private const float DistanceFromPlayer = 2f * 16f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Vector2 direction = Vector2.Zero;
            if (Projectile.owner == Main.myPlayer)
            {
                if (Main.mouseLeft)
                {
                    direction = Main.MouseWorld - player.Center;        //Cause it has to be found client side
                    direction.Normalize();
                    Vector2 pos = player.Center + (direction * DistanceFromPlayer);
                    if (Projectile.Center != pos)
                    {
                        Projectile.direction = 1;
                        if (Projectile.Center.X < player.Center.X)
                        {
                            Projectile.direction = -1;
                        }
                        player.ChangeDir(Projectile.direction);
                        Projectile.netUpdate = true;
                    }
                    Projectile.Center = pos;
                    Projectile.timeLeft = 2;
                }
                else
                {
                    Projectile.Kill();
                }
            }

            Projectile.spriteDirection = Projectile.direction;

            /*player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(direction.Y * Projectile.direction, direction.X * Projectile.direction);*/      //Gives an index OOB error when there's no Item

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                    Projectile.rotation = MathHelper.ToRadians(Main.rand.Next(0, 360 + 1));
                    SoundEngine.PlaySound(2, (int)player.position.X, (int)player.position.Y, 1, 1f, 0.6f + Main.rand.NextFloat(-0.1f, 0.1f));
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();

            target.GetGlobalNPC<JoJoGlobalNPC>()).vampireUserLastHitIndex = player.whoAmI;
            if (vPlayer.HasSkill(player, VampirePlayer.SavageInstincts))
                if (Main.rand.Next(0, 100) <= vPlayer.lacerationChance)
                    target.AddBuff(ModContent.BuffType<Lacerated>(), (vPlayer.GetSkillLevel(player, VampirePlayer.SavageInstincts) * 4) * 60);
        }
    }
}