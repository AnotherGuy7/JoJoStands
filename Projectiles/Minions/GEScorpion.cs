using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Minions
{
    public class GEScorpion : ModProjectile
    {
        public override string Texture { get { return "Terraria/Images/NPC_" + NPCID.ScorpionBlack; } }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 6;
            Projectile.timeLeft = 800;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            DrawOriginOffsetY = -5;
        }

        private const float DetectionDistance = 20f * 16f;
        private bool walking = false;
        private bool spawnEffectsPlayed = false;

        public override void AI()
        {
            SelectFrame();
            Projectile.ai[0]--;
            if (Projectile.ai[0] <= 0f)
            {
                Projectile.ai[0] = 0f;
            }
            Projectile.velocity.Y += 1.5f;
            if (Projectile.velocity.Y >= 6f)
            {
                Projectile.velocity.Y = 6f;
            }
            if (Projectile.velocity.X == 0f)
            {
                walking = false;
            }
            else
            {
                walking = true;
            }
            if (!spawnEffectsPlayed)
            {
                for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IchorTorch, Main.rand.NextFloat(-1.1f, 1.1f + 1f), Main.rand.NextFloat(-1.1f, 1.1f + 1f), Scale: Main.rand.NextFloat(1.1f, 2.4f + 1f));
                    Main.dust[dustIndex].noGravity = true;
                }
                spawnEffectsPlayed = true;
            }
            NPC npcTarget = null;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.lifeMax > 5 && npc.type != NPCID.TargetDummy && npc.type != NPCID.CultistTablet)
                {
                    npcTarget = npc;
                    float distance = Projectile.Distance(npc.Center);
                    if (distance < DetectionDistance)
                    {
                        npcTarget = npc;
                    }
                }
            }
            if (npcTarget != null)
            {
                if (npcTarget.position.X > Projectile.position.X)
                {
                    Projectile.velocity.X = 1f;
                    Projectile.direction = 1;
                    Projectile.spriteDirection = -1;
                }
                if (npcTarget.position.X < Projectile.position.X)
                {
                    Projectile.velocity.X = -1f;
                    Projectile.direction = -1;
                    Projectile.spriteDirection = 1;
                }
                if (WorldGen.SolidTile((int)(Projectile.Center.X / 16f) + Projectile.direction, (int)(Projectile.Center.Y / 16f)) && Projectile.ai[0] <= 0f)
                {
                    Projectile.ai[0] = 50f;
                    Projectile.velocity.Y = -6f;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Projectile.velocity.X = 0f;
            }

            Player player = Main.player[Projectile.owner];
            if (JoJoStands.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile otherProj = Main.projectile[p];
                    Player otherPlayer = Main.player[otherProj.owner];
                    if (otherProj.active && Projectile.Hitbox.Intersects(otherProj.Hitbox))
                    {
                        if (Projectile.owner != otherProj.owner && player.team != otherPlayer.team && Projectile.damage < 75)
                        {
                            int dustIndex = Dust.NewDust(otherProj.position + otherProj.velocity, Projectile.width, Projectile.height, DustID.Torch);
                            Main.dust[dustIndex].noGravity = true;
                            if (JoJoStands.Sounds)
                            {
                                SoundStyle punchSound = new SoundStyle("JoJoStands/Sounds/GameSounds/Punch_land");
                                punchSound.Volume = 0.6f;
                                punchSound.Pitch = 0f;
                                punchSound.PitchVariance = 0.2f;
                                SoundEngine.PlaySound(punchSound, Projectile.Center);
                            }

                            otherPlayer.Hurt(PlayerDeathReason.ByCustomReason(otherPlayer.name + " couldn't resist hurting " + player.name + "'s damage-reflecting scorpion."), otherProj.damage, 1, true);
                            otherProj.Kill();
                            Projectile.Kill();
                        }
                        if (Projectile.owner != otherProj.owner && player.team != otherPlayer.team && Projectile.damage > 75)
                        {
                            otherProj.Kill();
                            Projectile.Kill();
                        }
                    }
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (damage <= 75)
            {
                damage = target.damage - Main.rand.Next(0, 2);
                target.AddBuff(BuffID.Poisoned, 120);
            }
            if (damage > 75)
            {
                damage = 75;
                Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
            }
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        private void SelectFrame()
        {
            Projectile.frameCounter++;
            if (walking)
            {
                if (Projectile.frameCounter >= 8)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame += 1;
                }
                if (Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            else
            {
                Projectile.frame = 0;
            }
        }
    }
}