using JoJoStands.Projectiles.PlayerStands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Pets
{
    public class TuskAct1Pet : StandClass   //for SyncAndApplyDyeSlot method
    {
        public override string Texture => Mod.Name + "/Projectiles/Pets/TuskAct1Pet";
        public override string poseSoundName => "ItsBeenARoundaboutPath";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 26;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.manualDirectionChange = true;
        }

        public override void AI()       //I unfortunately had to copy the DD2Pet AI style cause framecounter was made to stay at 5 on it...
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.poseSoundName = poseSoundName;
            if (mPlayer.tuskActNumber == 1)
                Projectile.timeLeft = 2;

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
            Projectile.direction = Projectile.spriteDirection = player.direction;


            float maximumDistance = 4f;
            Vector2 playerDirectionDistance = new Vector2(player.direction * 30f, -20f);
            Vector2 frontOfPlayer = player.MountedCenter + playerDirectionDistance;
            float playerBackDistance = Vector2.Distance(Projectile.Center, frontOfPlayer);
            if (playerBackDistance > 1000f)
                Projectile.Center = player.Center + playerDirectionDistance;

            Vector2 projectileDirection = frontOfPlayer - Projectile.Center;
            if (playerBackDistance < maximumDistance)
            {
                Projectile.velocity *= 0.25f;
            }
            if (projectileDirection != Vector2.Zero)
            {
                if (projectileDirection.Length() < maximumDistance * 0.5f)
                {
                    Projectile.velocity = projectileDirection;
                }
                else
                {
                    Projectile.velocity = projectileDirection * 0.1f;
                }
            }
            if (Projectile.velocity.Length() > 6f)
            {
                float estimatedRotation = Projectile.velocity.X * 0.08f + Projectile.velocity.Y * (float)Projectile.spriteDirection * 0.02f;
                if (Math.Abs(Projectile.rotation - estimatedRotation) >= 3.14159274f)       //Pi?
                {
                    if (estimatedRotation < Projectile.rotation)
                    {
                        Projectile.rotation -= 6.28318548f;     //Pi * 2?
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
            Projectile.netUpdate = true;
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