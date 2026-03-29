using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Dusts
{
    public class HazeCloud : ModDust
    {
        public override string Texture => Mod.Name + "/Dusts/HazeCloud_1";

        private const int DriftTicks = 40;
        private const int FadeTicks = 60;

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
            dust.alpha = 255;
            dust.scale = Main.rand.NextFloat(1.2f, 2.0f);
            float angle = Main.rand.NextFloat(MathHelper.TwoPi);
            float speed = Main.rand.NextFloat(0.8f, 2.8f);
            dust.velocity = new Vector2(
                (float)System.Math.Cos(angle),
                (float)System.Math.Sin(angle)) * speed;
            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            int spinSteps = Main.rand.Next(-6, 7);
            dust.color = new Color((byte)(spinSteps + 128), 0, 0, 0);
            dust.fadeIn = 0f;
            SetSize(dust);
        }

        public virtual void SetSize(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 32, 32);
        }

        private const int FadeInTicks = 20;

        public override bool Update(Dust dust)
        {
            float tick = dust.fadeIn;
            float spinSpeed = ((int)dust.color.R - 128) * 0.005f;

            if (tick < FadeInTicks)
            {
                float t = tick / (float)FadeInTicks;
                dust.alpha = (int)MathHelper.Lerp(255, 0, t);
                dust.position += dust.velocity;
                dust.rotation += spinSpeed;
                dust.velocity *= 0.98f;
            }
            else if (tick < DriftTicks)
            {
                dust.alpha = 0;
                dust.position += dust.velocity;
                dust.rotation += spinSpeed;
                dust.velocity *= 0.98f;
            }
            else
            {
                float t = (tick - DriftTicks) / (float)FadeTicks;
                if (t > 1f) t = 1f;
                dust.velocity *= 0.93f;
                dust.position += dust.velocity;
                dust.rotation += spinSpeed * (1f - t);
                dust.alpha = (int)MathHelper.Lerp(0, 255, t);
                if (dust.alpha >= 252 || t >= 1f)
                {
                    dust.active = false;
                    return false;
                }
            }

            dust.fadeIn = tick + 1f;
            return false;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            float a = 1f - dust.alpha / 255f;
            return new Color(160, 80, 210) * a;
        }
    }

    public class HazeCloud2 : HazeCloud
    {
        public override string Texture => Mod.Name + "/Dusts/HazeCloud_2";

        public override void SetSize(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 40, 38);
        }
    }

    public class HazeCloud3 : HazeCloud
    {
        public override string Texture => Mod.Name + "/Dusts/HazeCloud_3";

        public override void SetSize(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 30, 28);
        }
    }

    public static class HazeCloudBurst
    {
        private static int[]? _types;

        public static int[] Types()
        {
            return _types ??= new[]
            {
                ModContent.DustType<HazeCloud>(),
                ModContent.DustType<HazeCloud2>(),
                ModContent.DustType<HazeCloud3>()
            };
        }

        public static void Spawn(Vector2 center, int count = 8)
        {
            int[] types = Types();
            for (int i = 0; i < count; i++)
                Dust.NewDust(center, 1, 1, types[Main.rand.Next(types.Length)]);
        }
    }
}