using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Minions
{
    public class GEFrog : ModProjectile
    {

        public override string Texture { get { return "Terraria/NPC_" + NPCID.Frog; } }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 10;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private const float searchDistance = 18f * 16f;

        private int maxReflection = 15;
        private int jumpTimer = 0;
        private bool walking = false;

        public override void AI()
        {
            SelectFrame();
            jumpTimer--;
            if (jumpTimer <= 0)
            {
                jumpTimer = 0;
            }
            Projectile.velocity.Y += 1.5f;
            if (Projectile.velocity.Y >= 6f)
            {
                Projectile.velocity.Y = 6f;
            }

            maxReflection = (int)Projectile.ai[0] * 15;
            if (Projectile.ai[1] == 0f)
            {
                Projectile.penetrate = 1;
                Projectile.ai[1] = 4f;
            }
            if (Projectile.ai[1] == 1f)
            {
                Projectile.penetrate = 2;
                Projectile.ai[1] = 4f;
            }
            if (Projectile.ai[1] == 2f)
            {
                Projectile.penetrate = 3;
                Projectile.ai[1] = 4f;
            }
            if (Projectile.ai[1] == 3f)
            {
                Projectile.penetrate = 4;
                Projectile.ai[1] = 4f;
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
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC possibleTarget = Main.npc[n];
                if (possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.townNPC && possibleTarget.type != NPCID.TargetDummy && possibleTarget.type != NPCID.CultistTablet)
                {
                    npcTarget = possibleTarget;
                    float distance = Projectile.Distance(possibleTarget.Center);
                    if (distance < searchDistance)
                    {
                        npcTarget = possibleTarget;
                    }
                    break;
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
                if (WorldGen.SolidTile((int)(Projectile.Center.X / 16f) + Projectile.direction, (int)(Projectile.Center.Y / 16f)) && jumpTimer <= 0)
                {
                    jumpTimer = 40;
                    Projectile.velocity.Y = -10f;
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
                        if (Projectile.owner != otherProj.owner && player.team != otherPlayer.team && Projectile.damage < maxReflection)
                        {
                            Dust.NewDust(Main.projectile[p].position + Main.projectile[p].velocity, Projectile.width, Projectile.height, DustID.FlameBurst, Main.projectile[p].velocity.X * -0.5f, Main.projectile[p].velocity.Y * -0.5f);
                            if (MyPlayer.Sounds)
                            {
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/sound/Punch_land").WithVolume(.3f));
                            }
                            otherPlayer.Hurt(PlayerDeathReason.ByCustomReason(otherPlayer.name + " missed his target and hit " + player.name + "'s damage-reflecting frog."), otherProj.damage, 1, true);
                            otherProj.Kill();
                            Projectile.Kill();
                        }
                        if (Projectile.owner != otherProj.owner && player.team != otherPlayer.team && Projectile.damage > maxReflection)
                        {
                            otherProj.Kill();
                            Projectile.Kill();
                        }
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (damage <= maxReflection)
            {
                damage = target.damage - Main.rand.Next(0, 3);
            }
            if (damage > maxReflection)
            {
                damage = maxReflection;
                Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);
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
                if (Projectile.frame >= 10)
                {
                    Projectile.frame = 6;
                }
            }
            else
            {
                if (Projectile.frameCounter >= 12)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame += 1;
                }
                if (Projectile.frame >= 6)
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}