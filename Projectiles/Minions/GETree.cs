using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Minions
{
    public class GETree : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 13;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 41;
            Projectile.friendly = true;
            Projectile.penetrate = 9999;
            Projectile.timeLeft = 5;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 2f;
            DrawOriginOffsetX = -10;
            DrawOriginOffsetY = 21;
        }

        private bool timeLeftDeclared = false;
        private bool shrinkAndDie = false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile otherProj = Main.projectile[p];
                    if (otherProj.active && Projectile.Hitbox.Intersects(otherProj.Hitbox))
                    {
                        Player otherPlayer = Main.player[otherProj.owner];
                        if (Projectile.owner != otherProj.owner && player.team != otherPlayer.team)
                        {
                            if (MyPlayer.Sounds)
                            {
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/GameSounds/Punch_land").WithVolume(.3f));
                            }
                            Dust.NewDust(otherProj.position + otherProj.velocity, Projectile.width, Projectile.height, DustID.FlameBurst, otherProj.velocity.X * -0.5f, otherProj.velocity.Y * -0.5f);
                            otherPlayer.Hurt(PlayerDeathReason.ByCustomReason(otherPlayer.name + " loved the damage reflection given by " + player.name + "'s damage-reflecting tree too much."), otherProj.damage, 1, true);
                            otherProj.Kill();
                        }
                    }
                }
            }

            Projectile.velocity.X = 0f;
            Projectile.velocity.Y = 3f;
            Projectile.direction = -1;
            if (Projectile.ai[0] == 2f && !timeLeftDeclared)
            {
                Projectile.timeLeft = 900;
                timeLeftDeclared = true;
            }
            if (Projectile.ai[0] == 3f && !timeLeftDeclared)
            {
                Projectile.timeLeft = 1200;
                timeLeftDeclared = true;
            }
            if (Projectile.ai[0] == 4f && !timeLeftDeclared)
            {
                Projectile.timeLeft = 1500;
                timeLeftDeclared = true;
            }
            if (Projectile.ai[0] == 5f && !timeLeftDeclared)
            {
                Projectile.timeLeft = 1800;
                timeLeftDeclared = true;
            }

            if (Projectile.timeLeft <= 181)
            {
                shrinkAndDie = true;
            }
            if (!shrinkAndDie)
            {
                if (Projectile.frame <= 11)
                    Projectile.frameCounter++;

                if (Projectile.frameCounter >= 14)
                {
                    Projectile.frame += 1;
                    Projectile.frameCounter = 0;
                }
            }
            if (shrinkAndDie)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 14)
                {
                    Projectile.frame -= 1;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame <= 0)
                {
                    Projectile.Kill();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = target.damage;
            knockback = Math.Abs(target.velocity.X);      //they're just gonna have to go back as fast as they were going
        }
    }
}