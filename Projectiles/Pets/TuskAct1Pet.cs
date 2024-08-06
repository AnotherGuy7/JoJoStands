using JoJoStands.Projectiles.PlayerStands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Pets
{
    public class TuskAct1Pet : StandClass   //for SyncAndApplyDyeSlot method
    {
        public override string Texture => Mod.Name + "/Projectiles/Pets/TuskAct1Pet";
        public override string PoseSoundName => "ItsBeenARoundaboutPath";
        public override string SpawnSoundName => "Tusk Act 1";

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

        private bool playedSpawnSound = false;

        public override void AI()       //I unfortunately had to copy the DD2Pet AI style cause framecounter was made to stay at 5 on it...
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.poseSoundName = PoseSoundName;
            mPlayer.standType = 2;
            if (mPlayer.tuskActNumber == 1)
                Projectile.timeLeft = 2;

            if (JoJoStands.SoundsLoaded && !playedSpawnSound)
            {
                playedSpawnSound = true;
                SoundStyle spawnSound = new SoundStyle("JoJoStandsSounds/Sounds/SummonCries/" + SpawnSoundName);
                spawnSound.Volume = JoJoStands.ModSoundsVolume;
                SoundEngine.PlaySound(spawnSound, Projectile.position);
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= 4)
                    Projectile.frame = 0;
            }

            if (Projectile.owner == Main.myPlayer && Main.mouseRight)
            {
                if (Main.MouseWorld.X > player.position.X)
                    player.direction = 1;
                if (Main.MouseWorld.X < player.position.X)
                    player.direction = -1;
            }

            if (!mPlayer.standOut || player.dead)
                Projectile.Kill();

            float maximumDistance = 4f;
            Vector2 playerDirectionDistance = new Vector2(player.direction * 30f, -20f);
            Vector2 frontOfPlayer = player.MountedCenter + playerDirectionDistance;
            float playerBackDistance = Vector2.Distance(Projectile.Center, frontOfPlayer);
            if (playerBackDistance > 1000f)
                Projectile.Center = player.Center + playerDirectionDistance;

            Vector2 projectileDirection = frontOfPlayer - Projectile.Center;
            if (playerBackDistance < maximumDistance)
                Projectile.velocity *= 0.25f;

            if (projectileDirection != Vector2.Zero)
            {
                if (projectileDirection.Length() < maximumDistance * 0.5f)
                    Projectile.velocity = projectileDirection;
                else
                    Projectile.velocity = projectileDirection * 0.1f;
            }
            if (Projectile.velocity.Length() > 6f)
            {
                float estimatedRotation = Projectile.velocity.X * 0.08f + Projectile.velocity.Y * (float)Projectile.direction * 0.02f;
                if (Math.Abs(Projectile.rotation - estimatedRotation) >= (float)Math.PI)
                {
                    if (estimatedRotation < Projectile.rotation)
                        Projectile.rotation -= (float)Math.Tau;
                    else
                        Projectile.rotation += (float)Math.Tau;
                }
                float num8 = 12f;
                Projectile.rotation = (Projectile.rotation * (num8 - 1f) + estimatedRotation) / num8;
            }
            else
            {
                if (Projectile.rotation > (float)Math.PI)
                    Projectile.rotation -= (float)Math.Tau;

                if (Projectile.rotation > -0.005f && Projectile.rotation < 0.005f)
                    Projectile.rotation = 0f;
                else
                    Projectile.rotation *= 0.96f;
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

        public override DrawData BuildStandDesummonDrawData()
        {
            standTexture = (Texture2D)ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad);
            int frameHeight = standTexture.Height / Main.projFrames[Projectile.type];
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Rectangle animRect = new Rectangle(0, frameHeight * Projectile.frame, standTexture.Width, frameHeight);
            Vector2 standOrigin = new Vector2(standTexture.Width / 2f, frameHeight / 2f);
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                effects = SpriteEffects.FlipHorizontally;

            return new DrawData(standTexture, drawPosition, animRect, Color.White, Projectile.rotation, standOrigin, 1f, effects, 0);
        }
    }
}