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
        private const int concentratedThresholdSeconds = 5;
        private const int initialBuffTicks = 30 * 60 * 60;
        private const int concentratedThresholdTicks = concentratedThresholdSeconds * 60;

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
            int hazeVirusType = ModContent.BuffType<HazeVirus>();
            int concentratedType = ModContent.BuffType<ConcentratedHazeVirus>();
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (!npc.active || Projectile.Distance(npc.Center) >= gasRange)
                    continue;
                int buffIndex = npc.FindBuffIndex(hazeVirusType);
                if (buffIndex >= 0)
                {
                    int ticksRemaining = npc.buffTime[buffIndex];
                    int ticksElapsed = initialBuffTicks - ticksRemaining;
                    if (ticksElapsed >= concentratedThresholdTicks)
                    {
                        npc.AddBuff(concentratedType, 30 * 60 * 60);
                        npc.DelBuff(buffIndex);
                    }
                    else
                    {

                    }
                }
                else if (npc.FindBuffIndex(concentratedType) < 0)
                {
                    npc.AddBuff(hazeVirusType, initialBuffTicks);
                }
            }
            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player otherPlayer = Main.player[p];
                if (!otherPlayer.active || Projectile.Distance(otherPlayer.Center) >= gasRange)
                    continue;
                int buffIndex = otherPlayer.FindBuffIndex(hazeVirusType);
                if (buffIndex >= 0)
                {
                    int ticksRemaining = otherPlayer.buffTime[buffIndex];
                    int ticksElapsed = initialBuffTicks - ticksRemaining;
                    if (ticksElapsed >= concentratedThresholdTicks)
                    {
                        otherPlayer.AddBuff(concentratedType, 30 * 60 * 60);
                        otherPlayer.DelBuff(buffIndex);
                    }
                }
                else if (otherPlayer.FindBuffIndex(concentratedType) < 0)
                {
                    otherPlayer.AddBuff(hazeVirusType, initialBuffTicks);
                }
            }
        }
    }
}