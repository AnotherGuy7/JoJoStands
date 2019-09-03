using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class JoJoGlobalProjectile : GlobalProjectile
    {
        public int timeLeft = 0;
        public int timeLeftSave = 0;
        public float velocityX = 0f;
        public float velocityY = 0f;

        public override bool InstancePerEntity
        {
            get { return true; }
        }

        public override bool PreAI(Projectile projectile)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (timeLeftSave >= 6 && timeLeft == 0)
            {
                timeLeft = projectile.timeLeft - 5;     //so they stop don't stop immediately
            }
            if (player.TheWorldEffect)        //the ones who can move in Za Warudo's projectiles, like minions, fists, every other projectile should freeze
            {
                timeLeftSave++;
                projectile.damage /= (int)1.25;
                foreach (int going in MyPlayer.stopimmune)
                {
                    if (projectile.type == going  && Main.player[projectile.owner].HasBuff(mod.BuffType("TheWorldBuff")) && projectile.timeLeft <= timeLeft)       //if it's in the list, keep it going
                    {
                        return true;
                    }
                    if ((projectile.type != going || !Main.player[projectile.owner].HasBuff(mod.BuffType("TheWorldBuff"))) && projectile.timeLeft <= timeLeft)
                    {
                        projectile.frameCounter = 1;
                        projectile.timeLeft = timeLeft;
                        return false;
                    }
                }
            }

            if (player.TimeSkipPreEffect)     //saves it, this is for projectiles like minions, controllable projectiles, etc.
            {
                velocityX = projectile.velocity.X;
                velocityY = projectile.velocity.Y;
            }
            if (player.TimeSkipEffect)        //deploys it
            {
                projectile.velocity.X = velocityX;
                projectile.velocity.Y = velocityY;
            }
            else
            {
                velocityX = 0;
                velocityY = 0;
            }
            return true;
        }

        public override bool ShouldUpdatePosition(Projectile projectile)        //thanks, HellGoesOn for telling me this hook even existed
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (player.TheWorldEffect && projectile.timeLeft <= timeLeft)        //the ones who can move in Za Warudo's projectiles, like minions, fists, every other projectile should freeze
            {
                foreach (int going in MyPlayer.stopimmune)
                {
                    if (projectile.type == going && Main.player[projectile.owner].HasBuff(mod.BuffType("TheWorldBuff")))       //if it's in the list, keep it going
                    {
                        return true;
                    }
                    if ((projectile.type != going) || !Main.player[projectile.owner].HasBuff(mod.BuffType("TheWorldBuff")))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}