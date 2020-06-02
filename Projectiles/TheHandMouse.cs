using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class TheHandMouse : ModProjectile
    {
        public override string Texture => mod.Name + "/Projectiles/PlayerStands/StandPlaceholder";

        private Projectile projOwner;

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 0;
            projectile.timeLeft = 6;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projOwner = Main.projectile[(int)projectile.ai[0]];
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active)
                {
                    if (projectile.Hitbox.Intersects(npc.Hitbox))       //if the mouse is on an NPC, bring it
                    {
                        projOwner.ai[0] = 2f;
                        projOwner.ai[1] = npc.whoAmI;
                        projectile.Kill();
                    }
                }
            }
            projectile.position = Main.MouseWorld;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projOwner.ai[0] = 3f;       //if the mouse is on a tile break it
            return true;
        }

        public override void Kill(int timeLeft)
        {
            if (projOwner.ai[0] == 0f)      //if any of the 2 from the top didn't work, teleport
            {
                projOwner.ai[0] = 1f;

            }
        }
    }
}