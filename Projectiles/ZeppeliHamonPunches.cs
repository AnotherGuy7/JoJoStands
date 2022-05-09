using JoJoStands.NPCs.TownNPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class ZeppeliHamonPunches : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/HamonPunches"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 11;
        }

        public override void SetDefaults()      //look into ProjectileID.595. Done.
        {
            Projectile.width = 68;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.friendly = true;
            DrawOriginOffsetY = 20;
            Projectile.scale = (int)1.5;
        }

        private NPC zeppeli = null;

        public override void AI()
        {
            if (zeppeli == null)
            {
                for (int k = 0; k < Main.maxNPCs; k++)
                {
                    NPC npc = Main.npc[k];
                    if (npc.type == Mod.NPCType("HamonMaster"))
                    {
                        zeppeli = npc;
                    }
                }
            }

            if (zeppeli == null || !zeppeli.active)
            {
                Projectile.Kill();
                return;
            }

            bool enemiesNearby = false;
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 10f)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        if (!npc.friendly && !npc.immortal && zeppeli.Distance(npc.Center) <= NPCID.Sets.DangerDetectRange[zeppeli.type])
                        {
                            enemiesNearby = true;
                            break;
                        }
                    }
                }
                if (!enemiesNearby)
                {
                    Projectile.Kill();
                    return;
                }
                Projectile.ai[0] = 0f;
            }

            Projectile.frameCounter++;
            HamonMaster.punchesActive = true;
            if (Projectile.direction == 1)
            {
                DrawOffsetX = 25;
            }
            if (Projectile.direction == -1)
            {
                DrawOffsetX = -2;
            }
            if (Projectile.frameCounter >= 2)
            {
                Projectile.frame += 1;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
                Projectile.frameCounter = 0;
            }

            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                Projectile.soundDelay = 12;
            }
            Vector2 pos = zeppeli.Center + new Vector2(-40f, -40f) + new Vector2(20f * zeppeli.direction, 0f);
            if (Projectile.position != pos)
            {
                if (Projectile.Center.X > zeppeli.Center.X)
                {
                    Projectile.direction = 1;
                }
                else
                {
                    Projectile.direction = -1;
                }
                Projectile.netUpdate = true;
            }
            Projectile.position = pos;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            zeppeli.FaceTarget();
        }

        public override void Kill(int timeLeft)
        {
            HamonMaster.punchesActive = false;
        }
    }
}