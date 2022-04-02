using Microsoft.Xna.Framework;
using System;
using Terraria;
using JoJoStands.Projectiles.PlayerStands;
using Microsoft.Xna.Framework.Graphics;

namespace JoJoStands.Projectiles.Pets
{
    public class TuskAct2Pet : StandClass
    {
        public override string Texture => mod.Name + "/Projectiles/Pets/TuskAct2Pet";
		public override string poseSoundName => "ItsBeenARoundaboutPath";

        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 38;
            projectile.penetrate = -1;
            projectile.netImportant = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.manualDirectionChange = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
			mPlayer.poseSoundName = poseSoundName;
            if (mPlayer.tuskActNumber == 2)
            {
                projectile.timeLeft = 2;
            }

            float maximumDistance = 4f;
            Vector2 playerDirection = new Vector2(player.direction * 30f, -20f);
            Vector2 frontOfPlayer = player.MountedCenter + playerDirection;
            float distanceToPlayerFront = Vector2.Distance(projectile.Center, frontOfPlayer);
            if (distanceToPlayerFront > 1000f)
            {
                projectile.Center = player.Center + playerDirection;
            }
            Vector2 velocityDirection = frontOfPlayer - projectile.Center;
            if (distanceToPlayerFront < maximumDistance)
            {
                projectile.velocity *= 0.25f;
            }
            if (velocityDirection != Vector2.Zero)
            {
                if (velocityDirection.Length() < maximumDistance * 0.5f)
                {
                    projectile.velocity = velocityDirection;
                }
                else
                {
                    projectile.velocity = velocityDirection * 0.1f;
                }
            }
            if (projectile.velocity.Length() > 6f)
            {
                float estimatedRotation = projectile.velocity.X * 0.08f + projectile.velocity.Y * (float)projectile.spriteDirection * 0.02f;
                if (Math.Abs(projectile.rotation - estimatedRotation) >= 3.14159274f)
                {
                    if (estimatedRotation < projectile.rotation)
                    {
                        projectile.rotation -= 6.28318548f;
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
            projectile.netUpdate = true;
            projectile.direction = projectile.spriteDirection = player.direction;
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