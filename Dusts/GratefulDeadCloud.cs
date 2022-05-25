using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Dusts
{
    public class GratefulDeadCloud : ModDust
    {
        private const int FrameHeight = 32;

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.alpha = 255;
            dust.color = Color.Lerp(Color.White, Color.Purple, Main.rand.Next(20 - 15, 45 + 1 - 15) / 100f);
            dust.color.A = (byte)dust.alpha;
            dust.frame = new Rectangle(0, FrameHeight * Main.rand.Next(0, 1 + 1), 68, FrameHeight);
            dust.fadeIn = 1f;
            dust.velocity.X = Main.rand.Next(-6, 6 + 1) / 10f;
            dust.velocity.Y = 0;
        }

        public override bool Update(Dust dust)
        {
            if (dust.fadeIn == 1f)
            {
                dust.alpha--;
                if (dust.alpha <= 128)
                    dust.fadeIn = 0f;
            }
            else
            {
                dust.alpha++;
                if (dust.alpha >= 255)
                    dust.active = false;
            }
            dust.position += dust.velocity;
            return false;
        }
    }
}