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
            if (MyPlayer.AutomaticActivations)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active)
                    {
                        float npcDistance = Vector2.Distance(npc.Center, projectile.Center);
                        if (npcDistance < 30f && !npc.immortal && !npc.hide && !npc.friendly && npc.lifeMax > 5)
                        {
                            projectile.Kill();
                        }
                    }
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            int explosion = Projectile.NewProjectile(projectile.position, projectile.velocity, ProjectileID.GrenadeIII, (int)(180f * Mplayer.standDamageBoosts), 8f, projectile.owner);
            Main.projectile[explosion].timeLeft = 2;
            Main.projectile[explosion].netUpdate = true;
            Main.PlaySound(SoundID.Item62);

            for (int i = 0; i < 60; i++)
            {
                float circlePos = i;
                Vector2 spawnPos = projectile.Center + (circlePos.ToRotationVector2() * 90f);
                Vector2 velocity = projectile.Center + (circlePos.ToRotationVector2() * 90f);
                velocity.Normalize();
                Dust dustIndex = Dust.NewDustPerfect(spawnPos, 17, velocity * 3f, Scale: Main.rand.NextFloat(0.8f, 1.2f));
                dustIndex.noGravity = true;
            }
        }
    }
}