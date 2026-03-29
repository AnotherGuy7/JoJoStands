using JoJoStands.Buffs.Debuffs;
using JoJoStands.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class HazeVirusCloudFlask : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/ControllableNail"; }
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.timeLeft = 30 * 60;
        }

        private const int gasRange = 8 * 16;

        private float DamageMultiplier => Projectile.ai[0] > 0f ? Projectile.ai[0] : 1f;

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                HazeCloudBurst.Spawn(Projectile.Center, count: 5);
                Projectile.localAI[0] = 1f;
            }

            for (int i = 0; i < 3; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float dist = Main.rand.NextFloat(gasRange);
                Vector2 dustPos = Projectile.Center
                    + new Vector2((float)System.Math.Cos(angle),
                                  (float)System.Math.Sin(angle)) * dist;
                Dust.NewDust(dustPos, 1, 1, ModContent.DustType<GratefulDeadCloud>());
            }

            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (!npc.active || Projectile.Distance(npc.Center) >= gasRange)
                    continue;

                npc.AddBuff(ModContent.BuffType<HazeVirus>(), 30 * 60 * 60);

                float bonusMultiplier = DamageMultiplier - 1f;
                if (bonusMultiplier > 0f)
                {
                    int tileX = (int)(npc.Center.X / 16);
                    int tileY = (int)(npc.Center.Y / 16);
                    Color lightColor = Lighting.GetColor(tileX, tileY);
                    float light = (lightColor.R + lightColor.G + lightColor.B) / (255f * 3f);
                    float effectMultiplier = 1f - (MathHelper.Clamp(light, 0f, 1f) * 0.75f);

                    int baseHPS = 2;
                    npc.lifeRegen -= (int)(baseHPS * 2 * 120 * effectMultiplier * bonusMultiplier);
                }
            }

            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player otherPlayer = Main.player[p];
                if (otherPlayer.active && Projectile.Distance(otherPlayer.Center) < gasRange)
                    otherPlayer.AddBuff(ModContent.BuffType<HazeVirus>(), 30 * 60 * 60);
            }
        }
    }
}