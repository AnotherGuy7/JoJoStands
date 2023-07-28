using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Dusts
{
    public class StandSummonParticles : ModDust
    {
        private const int FrameHeight = 6;

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
            dust.frame = new Rectangle(0, FrameHeight * Main.rand.Next(0, 1 + 1), 6, FrameHeight);
        }
    }

    public class StandSummonShine1 : ModDust
    {
        private const int FrameWidth = 44;
        private const int FrameHeight = 40;

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
            dust.frame = new Rectangle(0, 0, 44, FrameHeight);
            dust.customData = 0;
            dust.position -= new Vector2(FrameWidth, FrameHeight) / 2f;
        }

        public override bool Update(Dust dust)
        {
            int frameCounter = (int)dust.customData;
            frameCounter++;
            if (frameCounter >= 3)
            {
                dust.frame.Y += FrameHeight;
                frameCounter = 0;
                if (dust.frame.Y / FrameHeight >= 3)
                {
                    if (Main.rand.Next(0, 3 + 1) == 0)
                    {
                        dust.position += new Vector2(Main.rand.Next(-16, 16 + 1), Main.rand.Next(-16, 16 + 1));
                        dust.frame.Y = 0;
                    }
                    else
                        dust.active = false;
                }
            }
            dust.customData = frameCounter;
            return false;
        }
    }

    public class StandSummonShine2 : ModDust
    {
        private const int FrameSize = 38;

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
            dust.frame = new Rectangle(0, 0, FrameSize, FrameSize);
            dust.customData = 0;
            dust.position -= new Vector2(FrameSize / 2f);
        }

        public override bool Update(Dust dust)
        {
            int frameCounter = (int)dust.customData;
            frameCounter++;
            if (frameCounter >= 3)
            {
                dust.frame.Y += FrameSize;
                frameCounter = 0;
                if (dust.frame.Y / FrameSize >= 3)
                {
                    if (Main.rand.Next(0, 2 + 1) == 0)
                    {
                        dust.position += new Vector2(Main.rand.Next(-4, 4 + 1), Main.rand.Next(-4, 4 + 1));
                        dust.frame.Y = 0;
                    }
                    else
                        dust.active = false;
                }
            }
            dust.customData = frameCounter;
            return false;
        }
    }
}