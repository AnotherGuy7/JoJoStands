using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class SheerHeartAttack : ModProjectile
    {
        int oneTime = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.melee = true;
            projectile.magic = true;
            projectile.timeLeft = 900;      //15 seconds
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.maxPenetrate = 1;
        }

        public override bool PreAI()
        {
            if (projectile.active)
            {
                MyPlayer.SHAactive = true;
            }
            Player player = Main.LocalPlayer;
            if (JoJoStands.ItemHotKey.JustPressed && (player.HeldItem.type == mod.ItemType("KillerQueenT3") || player.HeldItem.type == mod.ItemType("KillerQueenFinal")) && projectile.active)
            {
                projectile.position = player.position;
                return true;
            }
            return true;
        }

        public override void Kill(int timeLeft)
        {
            MyPlayer.SHAactive = false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            projectile.DirectionTo(target.position);
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.FlameBurst, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
            Main.PlaySound(SoundID.Item62);
        }

        public override void AI()
        {
            if (projectile.direction == -1)     //sprite turns depending on direction
            {
                projectile.spriteDirection = 1;
            }
            if (projectile.direction == 1)
            {
                projectile.spriteDirection = -1;
            }
            if (projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref projectile.velocity);
                projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 400f;      //from examplemods Wisp.cs
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                {
                    Vector2 newMove = Main.npc[k].Center - projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target == true)
            {
                oneTime += 1;
            }
            else
            {
                oneTime = 0;
            }
            if (oneTime == 1)           //I had to make this little system to make this sound only play once. oneTime can only equal 1 once
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Kocchiwomiro"));
            }
            if (target)
            {
                AdjustMagnitude(ref move);
                projectile.velocity = (10 * projectile.velocity + move) / 11f;
                AdjustMagnitude(ref projectile.velocity);
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)        //also from ExampleMod's Wisp.cs
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f)
            {
                vector *= 6f / magnitude;
            }
        }
    }
}