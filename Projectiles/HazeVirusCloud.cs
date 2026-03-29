using JoJoStands.Buffs.Debuffs;
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
            Dust.NewDust(Projectile.position - new Vector2(gasRange / 2), gasRange, gasRange, ModContent.DustType<Dusts.GratefulDeadCloud>());

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