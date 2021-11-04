using System;
using JoJoStands.Items.Vampire;
using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class KnifeSlashes : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.friendly = true;
        }

        private const float DistanceFromPlayer = 2f * 16f;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            Vector2 direction = Vector2.Zero;
            if (projectile.owner == Main.myPlayer)
            {
                if (Main.mouseLeft)
                {
                    direction = Main.MouseWorld - player.Center;        //Cause it has to be found client side
                    direction.Normalize();
                    Vector2 pos = player.Center + (direction * DistanceFromPlayer);
                    if (projectile.Center != pos)
                    {
                        projectile.direction = 1;
                        if (projectile.Center.X < player.Center.X)
                        {
                            projectile.direction = -1;
                        }
                        player.ChangeDir(projectile.direction);
                        projectile.netUpdate = true;
                    }
                    projectile.Center = pos;
                    projectile.timeLeft = 2;
                }
                else
                {
                    projectile.Kill();
                }
            }

            projectile.spriteDirection = projectile.direction;

            /*player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(direction.Y * projectile.direction, direction.X * projectile.direction);*/      //Gives an index OOB error when there's no item

            projectile.frameCounter++;
            if (projectile.frameCounter >= 2)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                    projectile.rotation = MathHelper.ToRadians(Main.rand.Next(0, 360 + 1));
                    Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 1, 1f, 0.6f + Main.rand.NextFloat(-0.1f, 0.1f));
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();

            target.GetGlobalNPC<JoJoGlobalNPC>().vampireUserLastHitIndex = player.whoAmI;
            if (vPlayer.HasSkill(player, VampirePlayer.SavageInstincts))
                if (Main.rand.Next(0, 100) <= vPlayer.lacerationChance)
                    target.AddBuff(mod.BuffType("Lacerated"), (vPlayer.GetSkillLevel(player, VampirePlayer.SavageInstincts) * 4) * 60);
        }
    }
}