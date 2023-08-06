using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class StarFinger : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        private bool living = true;
        private bool playedSound = false;
        private int amountOfPierces = 3;
        private const float MaxTravelDistance = 32f * 16f;
        private const float DespawnDistance = 3f * 16f;

        public override void AI()       //all this so that the other chain doesn't draw... yare yare. It was mostly just picking out types
        {
            if (!playedSound && JoJoStands.SoundsLoaded)
            {
                SoundStyle starFinger = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/StarFinger");
                starFinger.Volume = JoJoStands.ModSoundsVolume;
                SoundEngine.PlaySound(starFinger, Projectile.Center);
                playedSound = true;
            }
            Projectile ownerProj = Main.projectile[(int)Projectile.ai[0]];
            if (Main.player[Projectile.owner].dead || !ownerProj.active)
            {
                Projectile.Kill();
                return;
            }

            Projectile.direction = ownerProj.Center.X < Projectile.Center.X ? 1 : -1;
            ownerProj.direction = Projectile.direction;
            Vector2 rota = ownerProj.Center - Projectile.Center;
            Projectile.rotation = (-rota).ToRotation();
            if (Projectile.alpha == 0)
            {
                if (Projectile.position.X + (float)(Projectile.width / 2) > ownerProj.position.X + (float)(ownerProj.width / 2))
                    ownerProj.direction = ownerProj.spriteDirection = 1;
                else
                    ownerProj.direction = ownerProj.spriteDirection = -1;
            }

            float distanceFromOwner = Vector2.Distance(ownerProj.Center, Projectile.Center);
            if (living)
            {
                if (distanceFromOwner > MaxTravelDistance)
                    living = false;

                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] > 5f)
                    Projectile.alpha = 0;
                if (Projectile.ai[1] >= 10f)
                    Projectile.ai[1] = 15f;
            }
            else if (!living)
            {
                Projectile.tileCollide = false;
                if (distanceFromOwner < DespawnDistance)
                    Projectile.Kill();
                Vector2 returnVelocity = ownerProj.Center - Projectile.Center;
                returnVelocity.Normalize();
                returnVelocity *= 16f;
                Projectile.velocity = returnVelocity;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return amountOfPierces > 0;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            amountOfPierces--;
            if (amountOfPierces <= 0)
                living = false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            living = false;
            return false;
        }

        private Vector2 offset;
        private readonly Vector2 FingerOrigin = new Vector2(3);
        private Texture2D starFingerPartTexture;

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile ownerProj = Main.projectile[(int)Projectile.ai[0]];
            if (ownerProj == null || !ownerProj.active)
                return false;

            if (ownerProj.direction == 1)
                offset = new Vector2(20f, -2f);
            else
                offset = new Vector2(-31f, -2f);

            if (Main.netMode != NetmodeID.Server)
            {
                if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().currentTextureDye == MyPlayer.StandTextureDye.Part4)
                    starFingerPartTexture = ModContent.Request<Texture2D>("JoJoStands/Projectiles/StarFingerPart_P4").Value;
                else
                    starFingerPartTexture = ModContent.Request<Texture2D>("JoJoStands/Projectiles/StarFingerPart").Value;
            }


            Vector2 linkCenter = ownerProj.Center + offset;
            Vector2 normalizedVelocity = Projectile.velocity;
            normalizedVelocity.Normalize();
            Vector2 center = Projectile.Center + normalizedVelocity;
            float rotation = (linkCenter - center).ToRotation();

            float increment = 1 / (Vector2.Distance(center, linkCenter) / starFingerPartTexture.Width);
            for (float k = increment; k <= 1; k += increment)     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                Main.EntitySpriteDraw(starFingerPartTexture, pos, new Rectangle(0, 0, starFingerPartTexture.Width, starFingerPartTexture.Height), lightColor, rotation, new Vector2(starFingerPartTexture.Width * 0.5f, starFingerPartTexture.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0);
            }

            Texture2D projectileTexture = Main.player[Projectile.owner].GetModPlayer<MyPlayer>().currentTextureDye == MyPlayer.StandTextureDye.Part4
                ? ModContent.Request<Texture2D>("JoJoStands/Projectiles/StarFinger_P4").Value : ModContent.Request<Texture2D>("JoJoStands/Projectiles/StarFinger").Value;

            Main.EntitySpriteDraw(projectileTexture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, FingerOrigin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}