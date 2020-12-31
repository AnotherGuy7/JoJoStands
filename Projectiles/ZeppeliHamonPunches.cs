using JoJoStands.NPCs.TownNPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ZeppeliHamonPunches : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/HamonPunches"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 11;
        }

        public override void SetDefaults()      //look into ProjectileID.595. Done.
        {
            projectile.width = 68;
            projectile.height = 64;
            projectile.friendly = true;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.friendly = true;
            drawOriginOffsetY = 20;
            projectile.scale = (int)1.5;
        }

        private NPC zeppeli = null;

        public override void AI()
        {
            if (zeppeli == null)
            {
                for (int k = 0; k < Main.maxNPCs; k++)
                {
                    NPC npc = Main.npc[k];
                    if (npc.type == mod.NPCType("HamonMaster"))
                    {
                        zeppeli = npc;
                    }
                }
            }

            if (zeppeli == null || !zeppeli.active)
            {
                projectile.Kill();
                return;
            }

            bool enemiesNearby = false;
            projectile.ai[0]++;
            if (projectile.ai[0] >= 10f)
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
                    projectile.Kill();
                    return;
                }
                projectile.ai[0] = 0f;
            }

            projectile.frameCounter++;
            HamonMaster.punchesActive = true;
            if (projectile.direction == 1)
            {
                drawOffsetX = 25;
            }
            if (projectile.direction == -1)
            {
                drawOffsetX = -2;
            }
            if (projectile.frameCounter >= 2)
            {
                projectile.frame += 1;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
                projectile.frameCounter = 0;
            }

            projectile.soundDelay--;
            if (projectile.soundDelay <= 0)
            {
                Main.PlaySound(SoundID.Item1, projectile.Center);
                projectile.soundDelay = 12;
            }
            Vector2 pos = zeppeli.Center + new Vector2(-40f, -40f) + new Vector2(20f * zeppeli.direction, 0f);
            if (projectile.position != pos)
            {
                if (projectile.Center.X > zeppeli.Center.X)
                {
                    projectile.direction = 1;
                }
                else
                {
                    projectile.direction = -1;
                }
                projectile.netUpdate = true;
            }
            projectile.position = pos;
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 2;
            zeppeli.FaceTarget();
        }

        public override void Kill(int timeLeft)
        {
            HamonMaster.punchesActive = false;
        }
    }
}