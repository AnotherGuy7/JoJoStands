using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class JoJoGlobalProjectile : GlobalProjectile
    {
        public int timeLeft = 0;
        public int timeLeftSave = 0;
        public Vector2 preSkipVel = Vector2.Zero;

        //Epitaph stuff
        public bool applyingForesightPositions = false;
        public bool foresightResetIndex = false;
        public int foresightSaveTimer = 0;
        public int foresightPositionIndex = 0;
        public int foresightPositionIndexMax = 0;
        public Vector2[] foresightPosition = new Vector2[50];
        public int[] foresightFrames = new int[50];
        public Vector2[] foresightRotations = new Vector2[50];      //although this is a Vector2, I'm storing rotations in X and Direction in Y
        public bool stoppedInTime = false;
        //public bool checkedForImmunity = false;
        public bool timestopImmune = false;
        public bool autoModeSexPistols = false;
        public bool kickedBySexPistols = false;

        public override bool InstancePerEntity
        {
            get { return true; }
        }

        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            if (Mplayer.TheWorldEffect)
            {
                timeLeftSave++;
                if (timeLeftSave >= 6 && timeLeft == 0)
                {
                    timeLeft = projectile.timeLeft - 5;     //so they stop don't stop immediately
                }
                if (!stoppedInTime)
                {
                    projectile.damage = (int)(projectile.damage * 0.8f);        //projectiles in timestop lose 20% damage, so it's not as OP
                    if (player.HasBuff(mod.BuffType("TheWorldBuff")))
                    {
                        for (int i = 0; i < MyPlayer.stopImmune.Count; i++)
                        {
                            if (projectile.type == MyPlayer.stopImmune[i])
                            {
                                timestopImmune = true;
                            }
                        }
                    }
                    stoppedInTime = true;
                }
                if (!timestopImmune)
                {
                    if ((projectile.timeLeft <= timeLeft) || projectile.minion)
                    {
                        projectile.frameCounter = 1;
                        if (timeLeft > 0)      //for the projectiles that don't have enough time left before they die
                        {
                            projectile.timeLeft = timeLeft;
                        }
                        else
                        {
                            projectile.timeLeft = 2;
                        }
                        return false;
                    }
                }
            }

            if (Mplayer.TimeSkipPreEffect)     //saves it, this is for projectiles like minions, controllable projectiles, etc.
            {
                preSkipVel = projectile.velocity;
            }
            if (Mplayer.TimeSkipEffect)        //deploys it
            {
                projectile.velocity = preSkipVel;
            }
            else
            {
                preSkipVel = Vector2.Zero;
            }

            if (Mplayer.epitaphForesight && !projectile.minion)
            {
                applyingForesightPositions = true;
                if (foresightSaveTimer > 0)
                {
                    foresightSaveTimer--;
                }
                if (foresightSaveTimer <= 1)
                {
                    foresightPosition[foresightPositionIndex] = projectile.position;
                    foresightFrames[foresightPositionIndex] = projectile.frame;
                    foresightRotations[foresightPositionIndex].X = projectile.rotation;
                    foresightRotations[foresightPositionIndex].Y = projectile.direction;
                    foresightPositionIndex++;       //second so that something saves in [0] and goes up from there
                    foresightPositionIndexMax++;
                    foresightSaveTimer = 15;
                }
            }
            if (!Mplayer.epitaphForesight && applyingForesightPositions)
            {
                if (!foresightResetIndex)
                {
                    foresightPositionIndex = 0;
                    foresightResetIndex = true;
                }
                projectile.velocity = Vector2.Zero;
                projectile.position = foresightPosition[foresightPositionIndex];
                projectile.rotation = foresightRotations[foresightPositionIndex].X;
                if (foresightSaveTimer > 0)
                {
                    foresightSaveTimer--;
                }
                if (foresightSaveTimer <= 1)
                {
                    foresightPositionIndex++;
                    foresightSaveTimer = 15;
                    if (foresightPositionIndex >= 1)
                    {
                        if (foresightPosition[foresightPositionIndex - 1] != Vector2.Zero)
                        {
                            foresightPosition[foresightPositionIndex - 1] = Vector2.Zero;
                        }
                        if (foresightRotations[foresightPositionIndex - 1].X != 0f)
                        {
                            foresightRotations[foresightPositionIndex - 1].X = 0f;
                        }
                        if (foresightRotations[foresightPositionIndex - 1].Y != 0f)
                        {
                            foresightRotations[foresightPositionIndex - 1].Y = 0f;
                        }
                    }
                }
                if (foresightPositionIndex >= foresightPositionIndexMax)
                {
                    applyingForesightPositions = false;
                    foresightPositionIndex = 0;
                    foresightPositionIndexMax = 0;
                    foresightResetIndex = false;
                }
                return false;
            }
            if (autoModeSexPistols)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC possibleTarget = Main.npc[n];
                    if (possibleTarget.active && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && !possibleTarget.townNPC && !possibleTarget.hide && projectile.Distance(possibleTarget.Center) <= 48f)
                    {
                        kickedBySexPistols = true;
                        autoModeSexPistols = false;

                        Vector2 redirectionVelocity = possibleTarget.Center - projectile.Center;
                        redirectionVelocity.Normalize();
                        redirectionVelocity *= 16f;
                        projectile.velocity = redirectionVelocity;
                        Main.PlaySound(SoundID.Tink, projectile.Center);
                        break;
                    }
                }
            }
            if (kickedBySexPistols)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 204, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
            }
            return true;
        }

        public override bool PreDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            if (Mplayer.epitaphForesight || applyingForesightPositions)
            {
                for (int i = 0; i < 50; i++)
                {
                    SpriteEffects effects = SpriteEffects.None;
                    int frameHeight = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
                    if (foresightRotations[i].Y == 1)
                    {
                        effects = SpriteEffects.FlipHorizontally;
                    }
                    if (foresightPosition[i] != Vector2.Zero)
                    {
                        spriteBatch.Draw(Main.projectileTexture[projectile.type], foresightPosition[i] - Main.screenPosition, new Rectangle(0, projectile.frame * frameHeight, projectile.width, frameHeight), Color.DarkRed, foresightRotations[i].X, projectile.Size / 2f, projectile.scale, effects, 0f);
                    }
                }
            }
            return base.PreAI(projectile);
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            MyPlayer mPlayer = target.GetModPlayer<MyPlayer>();
            if (mPlayer.StandSlot.Item.type == mod.ItemType("DollyDaggerT1"))
            {
                damage = (int)(damage * 0.35f);
            }
            if (mPlayer.StandSlot.Item.type == mod.ItemType("DollyDaggerT2"))
            {
                damage = (int)(damage * 0.7f);
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (kickedBySexPistols)
                damage = (int)(damage * 1.05f);
        }

        public override bool ShouldUpdatePosition(Projectile projectile)        //thanks, HellGoesOn for telling me this hook even existed
        {
            MyPlayer Mplayer = Main.player[projectile.owner].GetModPlayer<MyPlayer>();
            if (Mplayer.TheWorldEffect && projectile.timeLeft <= timeLeft)        //the ones who can move in Za Warudo's projectiles, like minions, fists, every other projectile should freeze
            {
                    return timestopImmune;      //if it's owner isn't a timestop owner, always stop the projectile
            }
            return base.ShouldUpdatePosition(projectile);
        }
    }
}