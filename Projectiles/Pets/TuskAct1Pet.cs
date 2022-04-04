using Microsoft.Xna.Framework;
using System;
using Terraria;
using JoJoStands.Projectiles.PlayerStands;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace JoJoStands.Projectiles.Pets
{
    public class TuskAct1Pet : StandClass   //for SyncAndApplyDyeSlot method
    {
        public override string Texture => mod.Name + "/Projectiles/Pets/TuskAct1Pet";
		public override string poseSoundName => "ItsBeenARoundaboutPath";

        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 26;
            projectile.penetrate = -1;
            projectile.netImportant = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.manualDirectionChange = true;
        }

        public override void AI()       //I unfortunately had to copy the DD2Pet AI style cause framecounter was made to stay at 5 on it...
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.poseSoundName = poseSoundName;
            if (mPlayer.tuskActNumber == 1)
                projectile.timeLeft = 2;

            projectile.frameCounter++;
            if (projectile.frameCounter >= 10)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            projectile.direction = projectile.spriteDirection = player.direction;


            float maximumDistance = 4f;
            Vector2 playerDirectionDistance = new Vector2(player.direction * 30f, -20f);
            Vector2 frontOfPlayer = player.MountedCenter + playerDirectionDistance;
            float playerBackDistance = Vector2.Distance(projectile.Center, frontOfPlayer);
            if (playerBackDistance > 1000f)
                projectile.Center = player.Center + playerDirectionDistance;

            Vector2 projectileDirection = frontOfPlayer - projectile.Center;
            if (playerBackDistance < maximumDistance)
            {
                projectile.velocity *= 0.25f;
            }
            if (projectileDirection != Vector2.Zero)
            {
                if (projectileDirection.Length() < maximumDistance * 0.5f)
                {
                    projectile.velocity = projectileDirection;
                }
                else
                {
                    projectile.velocity = projectileDirection * 0.1f;
                }
            }
            if (projectile.velocity.Length() > 6f)
            {
                float estimatedRotation = projectile.velocity.X * 0.08f + projectile.velocity.Y * (float)projectile.spriteDirection * 0.02f;
                if (Math.Abs(projectile.rotation - estimatedRotation) >= 3.14159274f)       //Pi?
                {
                    if (estimatedRotation < projectile.rotation)
                    {
                        projectile.rotation -= 6.28318548f;     //Pi * 2?
                    }
                    else
                    {
                        projectile.rotation += 6.28318548f;
                    }
                }
                float num8 = 12f;
                projectile.rotation = (projectile.rotation * (num8 - 1f) + estimatedRotation) / num8;
            }
            else
            {
                if (projectile.rotation > 3.14159274f)
                {
                    projectile.rotation -= 6.28318548f;
                }
                if (projectile.rotation > -0.005f && projectile.rotation < 0.005f)
                {
                    projectile.rotation = 0f;
                }
                else
                {
                    projectile.rotation *= 0.96f;
                }
            }
            projectile.netUpdate = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            SyncAndApplyDyeSlot();
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);        //starting a draw with dyes that work
        }
    }
}