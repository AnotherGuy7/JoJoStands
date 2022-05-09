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
        public override string Texture { get { return "Terraria/NPC_" + NPCID.ScorpionBlack; } }

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
            NPC npcTarget = null;
            for (int n = 0; n < 200; n++)
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
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile otherProj = Main.projectile[p];
                    Player otherPlayer = Main.player[otherProj.owner];
                    if (otherProj.active && Projectile.Hitbox.Intersects(otherProj.Hitbox))
                    {
                        if (Projectile.owner != otherProj.owner && player.team != otherPlayer.team && Projectile.damage < 75)
                        {
                            Dust.NewDust(Main.projectile[p].position + Main.projectile[p].velocity, Projectile.width, Projectile.height, DustID.FlameBurst, Main.projectile[p].velocity.X * -0.5f, Main.projectile[p].velocity.Y * -0.5f);
                            if (MyPlayer.Sounds)
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/sound/Punch_land").WithVolume(.3f));

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

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
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