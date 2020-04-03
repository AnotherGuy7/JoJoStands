using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class Bubble : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Projectile ownerProj = Main.projectile[(int)projectile.ai[1]];
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 21, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
            if (Main.mouseRight && projectile.owner == Main.myPlayer && projectile.ai[0] == 1f && player.ownedProjectileCounts[mod.ProjectileType("KillerQueenBTDStand")] == 1 && ownerProj.ai[0] == 1f)
            {
                projectile.Kill();
                projectile.netUpdate = true;
            }
            if (!ownerProj.active)
            {
                projectile.Kill();
                projectile.netUpdate = true;
            }
            if (MyPlayer.AutomaticActivations)
            {
                for (int i = 0; i < 200; i++)
                {
                    float npcDistance = Vector2.Distance(Main.npc[i].Center, projectile.Center);
                    if (npcDistance < 30f && Main.npc[i].active && !Main.npc[i].immortal && !Main.npc[i].hide)
                    {
                        projectile.Kill();
                    }
                }
            }
        }

        public virtual void OnHitAnything()
        {
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 17, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            var explosion = Projectile.NewProjectile(projectile.position, projectile.velocity, ProjectileID.GrenadeIII, (int)(180f * Mplayer.standDamageBoosts), 8f, Main.myPlayer);
            Main.projectile[explosion].timeLeft = 2;
            Main.projectile[explosion].netUpdate = true;
            Main.PlaySound(SoundID.Item62);
        }
    }
}