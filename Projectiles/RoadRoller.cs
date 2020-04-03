using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class RoadRoller : ModProjectile
    {
        public Vector2 velocityAdd = Vector2.Zero;
        public float damageMult = 1f;
        public int sparkDust = 0;
        public bool landed = false;

        public override void SetDefaults()
        {
            projectile.width = 144;
            projectile.height = 74;
            projectile.aiStyle = 0;
            projectile.timeLeft = 1800;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        public override void AI()
        {
            
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (projectile.timeLeft < 256)
            {
                projectile.alpha = -projectile.timeLeft + 255;
            }
            projectile.spriteDirection = projectile.direction;
            projectile.rotation = (projectile.velocity * new Vector2(projectile.spriteDirection, projectile.spriteDirection)).ToRotation();
            //Main.NewText(projectile.rotation + "; " + projectile.direction + "; " + projectile.spriteDirection);

            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                if (Main.projectile[p].active && Main.projectile[p].type == mod.ProjectileType("Fists"))
                {
                    if (projectile.Hitbox.Intersects(Main.projectile[p].Hitbox) && modPlayer.TheWorldEffect)
                    {
                        velocityAdd.X += Main.projectile[p].velocity.X / 100f;
                        velocityAdd.Y += Main.projectile[p].velocity.Y / 100f;
                        if (damageMult <= 2f)
                        {
                            damageMult += Main.projectile[p].damage / 75;
                        }
                        //Main.NewText("Mult: " + damageMult + "; Current Vel: " + projectile.velocity + "; X: " + velocityAdd.X + "; Y: " + velocityAdd.Y);
                        sparkDust = Dust.NewDust(Main.projectile[p].position + Main.projectile[p].velocity, projectile.width, projectile.height, DustID.FlameBurst, Main.projectile[p].velocity.X * -0.5f, Main.projectile[p].velocity.Y * -0.5f);
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Punch_land").WithVolume(.3f));
                        Main.projectile[p].Kill();
                        //Main.NewText(projectile.velocity + velocityAdd);
                    }
                }
            }
            if (landed && projectile.ai[0] < 120f)
            {
                projectile.ai[0]++;
            }
            if (projectile.ai[0] >= 120f)
            {
                projectile.damage = 0;
            }
            if (sparkDust != 0)
            {
                Main.dust[sparkDust].scale -= 0.017f;
            }
            if (!modPlayer.TheWorldEffect && velocityAdd != Vector2.Zero)
            {
                projectile.velocity += velocityAdd;
                projectile.damage += (int)damageMult;
                //Main.NewText("New Vel: " + projectile.velocity + "; New D: " + projectile.damage);
                damageMult = 1f;
                velocityAdd = Vector2.Zero;
            }
            if (!modPlayer.TheWorldEffect)
            {
                projectile.velocity.Y += 0.1f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            landed = true;
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (modPlayer.TheWorldEffect)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}