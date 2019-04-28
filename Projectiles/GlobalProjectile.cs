using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class JoJoGlobalProjectile : GlobalProjectile
    {
        public override bool PreAI(Projectile projectile)
        {
            if (MyPlayer.TheWorldEffect && !projectile.friendly) //comments below are just ideas
            {
                projectile.velocity.X *= 0.01f;
                projectile.velocity.Y *= 0.01f;
                projectile.frameCounter = 1;
                projectile.soundDelay = 540;
                projectile.timeLeft++;
                return false;
            }
            if (MyPlayer.TheWorldAfterEffect == true)
            {
                projectile.timeLeft *= 0;
                return true;
            }
            return true;
        }
    }
}