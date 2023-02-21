using JoJoStands.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Minions
{
    public class MatureStrayCatMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 16;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.netImportant = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.hostile = false;
            Projectile.minion = true;
            Projectile.timeLeft = 2;
            Projectile.minionSlots = 0.5f;
            DrawOffsetX = 8;
            DrawOriginOffsetY = 6;
            Projectile.penetrate = -1;
            Projectile.damage = 0;
        }

        private bool canShoot = false;
        private int shootCount = 0;

        public override void AI()       ////I really just ported over the Stray Cat NPC AI
        {
            SelectFrame();
            Player player = Main.player[Projectile.owner];
            Projectile.damage = 0;
            Projectile.timeLeft = 2;
            if (shootCount > 0)
                shootCount--;

            NPC target = null;
            if (target == null)
            {
                for (int k = 0; k < Main.maxNPCs; k++)
                {
                    NPC potentialTarget = Main.npc[k];
                    if (potentialTarget.active && !potentialTarget.dontTakeDamage && !potentialTarget.friendly && potentialTarget.lifeMax > 5 && potentialTarget.type != NPCID.TargetDummy && potentialTarget.type != NPCID.CultistTablet && !potentialTarget.townNPC && Projectile.Distance(potentialTarget.Center) <= 400f)
                    {
                        target = potentialTarget;
                    }
                }
            }
            if (player.HeldItem.type == ModContent.ItemType<StrayCat>() && player.altFunctionUse == 2)
            {
                Projectile.Kill();
            }
            Projectile.velocity.Y = 2f;
            if (target != null)
            {
                if (Projectile.ai[1] == 0f)
                {
                    Projectile.ai[0]++;
                }
                if (Projectile.ai[0] > 0f && Projectile.ai[1] == 1f)
                {
                    Projectile.ai[0]--;
                }
                if (Projectile.ai[0] >= 210f)
                {
                    Projectile.ai[0] = 209f;
                    Projectile.ai[1] = 1f;
                }
                if (Projectile.ai[1] == 1f)
                {
                    if (canShoot && shootCount <= 0)
                    {
                        shootCount += 40;
                        Vector2 shootVel = target.Center - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 2f;
                        int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<AirBubble>(), 104, 1f, Projectile.owner);
                        Main.projectile[projIndex].hostile = false;
                        Main.projectile[projIndex].friendly = true;
                        Main.projectile[projIndex].netUpdate = true;
                        canShoot = false;
                    }
                }

                Projectile.direction = 1;
                if (target.position.X < Projectile.position.X)
                    Projectile.direction = -1;

                Projectile.spriteDirection = Projectile.direction;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public void SelectFrame()
        {
            if (Projectile.ai[1] == 1f)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 20)
                {
                    Projectile.frame += 1;
                    Projectile.frameCounter = 0;
                }
                if (!canShoot && Projectile.frame == 5)
                {
                    canShoot = true;
                }
                if (Projectile.frame >= 6)
                {
                    Projectile.frame = 0;
                    Projectile.ai[1] = 0f;
                    canShoot = false;
                }
            }
        }
    }
}