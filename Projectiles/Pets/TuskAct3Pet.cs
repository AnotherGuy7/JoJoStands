using JoJoStands.Projectiles.PlayerStands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Pets
{
    public class TuskAct3Pet : StandClass
    {
        public override string Texture => Mod.Name + "/Projectiles/Pets/TuskAct3Pet";
        public override string PoseSoundName => "TuskAct3";
        public override string SpawnSoundName => "Tusk Act 3";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 26;
        }

        private bool playedSpawnSound = false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.poseSoundName = PoseSoundName;
            mPlayer.standType = 2;
            if (mPlayer.tuskActNumber == 3)
                Projectile.timeLeft = 2;

            if (JoJoStands.SoundsLoaded && !playedSpawnSound)
            {
                playedSpawnSound = true;
                SoundStyle spawnSound = new SoundStyle("JoJoStandsSounds/Sounds/SummonCries/" + SpawnSoundName);
                spawnSound.Volume = JoJoStands.ModSoundsVolume;
                SoundEngine.PlaySound(spawnSound, Projectile.position);
            }

            Vector2 behindPlayer = player.Center;
            behindPlayer.X -= (float)((12 + player.width / 2) * player.direction);
            behindPlayer.Y -= 25f;
            Projectile.Center = Vector2.Lerp(Projectile.Center, behindPlayer, 0.2f);
            Projectile.velocity *= 0.8f;
            Projectile.spriteDirection = Projectile.direction = player.direction;

            if (!mPlayer.standOut)
                Projectile.Kill();

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= 4)
                    Projectile.frame = 0;
            }
            //Projectile.netUpdate = true;
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