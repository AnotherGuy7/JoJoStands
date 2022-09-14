using JoJoStands.Projectiles.PlayerStands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace JoJoStands.Projectiles.Pets
{
    public class TuskAct2Pet : StandClass
    {
        public override string Texture => Mod.Name + "/Projectiles/Pets/TuskAct2Pet";
        public override string PoseSoundName => "ItsBeenARoundaboutPath";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 38;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.manualDirectionChange = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.poseSoundName = PoseSoundName;
            if (mPlayer.tuskActNumber == 2)
            {
                Projectile.timeLeft = 2;
            }

            float maximumDistance = 4f;
            Vector2 playerDirection = new Vector2(player.direction * 30f, -20f);
            Vector2 frontOfPlayer = player.MountedCenter + playerDirection;
            float distanceToPlayerFront = Vector2.Distance(Projectile.Center, frontOfPlayer);
            if (distanceToPlayerFront > 1000f)
            {
                Projectile.Center = player.Center + playerDirection;
            }
            Vector2 velocityDirection = frontOfPlayer - Projectile.Center;
            if (distanceToPlayerFront < maximumDistance)
            {
                Projectile.velocity *= 0.25f;
            }
            if (velocityDirection != Vector2.Zero)
            {
                if (velocityDirection.Length() < maximumDistance * 0.5f)
                {
                    Projectile.velocity = velocityDirection;
                }
                else
                {
                    Projectile.velocity = velocityDirection * 0.1f;
                }
            }
            if (Projectile.velocity.Length() > 6f)
            {
                float estimatedRotation = Projectile.velocity.X * 0.08f + Projectile.velocity.Y * (float)Projectile.spriteDirection * 0.02f;
                if (Math.Abs(Projectile.rotation - estimatedRotation) >= 3.14159274f)
                {
                    if (estimatedRotation < Projectile.rotation)
                    {
                        Projectile.rotation -= 6.28318548f;
                    }
                    else
                    {
                        Projectile.rotation += 6.28318548f;
                    }
                }
                float num8 = 12f;
                Projectile.rotation = (Projectile.rotation * (num8 - 1f) + estimatedRotation) / num8;
            }
            else
            {
                if (Projectile.rotation > 3.14159274f)
                {
                    Projectile.rotation -= 6.28318548f;
                }
                if (Projectile.rotation > -0.005f && Projectile.rotation < 0.005f)
                {
                    Projectile.rotation = 0f;
                }
                else
                {
                    Projectile.rotation *= 0.96f;
                }
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.netUpdate = true;
            Projectile.direction = Projectile.spriteDirection = player.direction;
        }

        public override bool PreDraw(ref Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            SyncAndApplyDyeSlot();
            return true;
        }

        public override void PostDraw(Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);        //starting a draw with dyes that work
        }
    }
}