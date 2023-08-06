using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using JoJoStands.Buffs.Debuffs;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles
{
    public class NailSlasher : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/ControllableNail"; }
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
                if (Main.mouseRight)
                {
                    direction = Main.MouseWorld - player.Center;        //Cause it has to be found client side
                    direction.Normalize();
                    Vector2 pos = player.Center + (direction * DistanceFromPlayer);
                    if (Projectile.Center != pos)
                    {
                        Projectile.direction = 1;
                        if (Projectile.Center.X < player.Center.X)
                            Projectile.direction = -1;
                        Projectile.netUpdate = true;
                    }
                    Projectile.Center = pos;
                }
                else
                {
                    Projectile.Kill();
                }
            }

            Projectile.timeLeft = 2;
            Projectile.spriteDirection = Projectile.direction;

            //player.heldProj = Projectile.whoAmI;
            //player.itemTime = 2;
            //player.itemAnimation = 2;
            //player.itemRotation = (float)Math.Atan2(direction.Y * Projectile.direction, direction.X * Projectile.direction);      //Gives an index OOB error when there's no Item
            Projectile.rotation += player.direction * 0.8f;

            int dustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 202, Projectile.velocity.X * -0.3f, Projectile.velocity.Y * -0.3f);
            Main.dust[dustIndex].noGravity = true;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
        }
    }
}