using JoJoStands.Buffs.Debuffs;
using JoJoStands.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class HazeVirusCloud : ModProjectile
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
            Projectile.tileCollide = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 30 * 60;
        }

        private int gasRange = 15 * 16;

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                HazeCloudBurst.Spawn(Projectile.Center, count: 8);
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
                if (npc.active && Projectile.Distance(npc.Center) < gasRange)
                    npc.AddBuff(ModContent.BuffType<HazeVirus>(), 30 * 60 * 60);
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