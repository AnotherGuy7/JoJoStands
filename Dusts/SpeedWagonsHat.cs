using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Dusts
{
    public class SpeedWagonsHat : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
        }

        public override bool Update(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 26, 18);
            if (!WorldGen.SolidTile((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f) + 1))
            {
                dust.velocity.Y = 0.3f;
                dust.position += dust.velocity;
            }
            else
            {
                dust.alpha++;
                dust.rotation = 0f;
                if (dust.alpha >= 255)
                    dust.active = false;
            }
            return false;
        }
    }
}