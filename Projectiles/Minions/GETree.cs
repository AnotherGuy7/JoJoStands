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
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 2f;
            DrawOriginOffsetX = -10;
            DrawOriginOffsetY = 21;
        }

        private bool timeLeftDeclared = false;
        private bool shrinking = false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (JoJoStands.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile otherProj = Main.projectile[p];
                    if (otherProj.active && Projectile.Hitbox.Intersects(otherProj.Hitbox))
                    {
                        Player otherPlayer = Main.player[otherProj.owner];
                        if (Projectile.owner != otherProj.owner && player.InOpposingTeam(otherPlayer))
                        {
                            Dust.NewDust(otherProj.position + otherProj.velocity, Projectile.width, Projectile.height, DustID.Torch);
                            otherPlayer.Hurt(PlayerDeathReason.ByCustomReason(otherPlayer.name + " loved the damage reflection given by " + player.name + "'s damage-reflecting tree a little too much."), otherProj.damage, otherPlayer.direction, true);
                            otherProj.Kill();

                            if (JoJoStands.Sounds)
                            {
                                SoundStyle punchSound = new SoundStyle("JoJoStands/Sounds/GameSounds/Punch_land");
                                punchSound.Volume = 0.6f;
                                punchSound.Pitch = 0f;
                                punchSound.PitchVariance = 0.2f;
                                SoundEngine.PlaySound(punchSound, Projectile.Center);
                            }
                        }
                    }
                }
            }

            Projectile.velocity.X = 0f;
            Projectile.velocity.Y = 3f;
            Projectile.direction = -1;
            if (!timeLeftDeclared)
            {
                Projectile.timeLeft = 900 + (300 * ((int)Projectile.ai[0] - 1));
                timeLeftDeclared = true;
            }

            if (Projectile.timeLeft <= 181)
                shrinking = true;

            if (!shrinking)
            {
                if (Projectile.frame <= 11)
                    Projectile.frameCounter++;

                if (Projectile.frameCounter >= 14)
                {
                    Projectile.frame += 1;
                    Projectile.frameCounter = 0;
                }
                if (Main.rand.Next(0, 3 + 1) == 0)
                {
                    int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IchorTorch, Main.rand.NextFloat(-1.1f, -0.6f + 1f), Scale: Main.rand.NextFloat(1.1f, 2.4f + 1f));
                    Main.dust[dustIndex].noGravity = true;
                }
            }
            if (shrinking)
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

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage.Base = target.damage;
            modifiers.Knockback.Base = Math.Abs(target.velocity.X);      //they're just gonna have to go back as fast as they were going
        }
    }
}