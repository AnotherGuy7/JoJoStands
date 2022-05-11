using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class JoJoGlobalProjectile : GlobalProjectile
    {
        public int savedTimeLeft = 0;
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
        public float[] foresightRotations = new float[50];      //although this is a Vector2, I'm storing rotations in X and Direction in Y
        public int[] foresightDirections = new int[50];
        public bool stoppedInTime = false;
        //public bool checkedForImmunity = false;
        public bool timestopImmune = false;
        public bool autoModeSexPistols = false;
        public bool kickedBySexPistols = false;

        public override bool InstancePerEntity
        {
            get { return true; }
        }

        public override bool PreAI(Projectile Projectile)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            if (Mplayer.timestopActive)
            {
                timeLeftSave++;
                if (timeLeftSave >= 6 && savedTimeLeft == 0)
                    savedTimeLeft = Projectile.timeLeft - 5;     //so they stop don't stop immediately

                if (!stoppedInTime)
                {
                    Projectile.damage = (int)(Projectile.damage * 0.8f);        //projectiles in timestop lose 20% damage, so it's not as OP
                    if (player.HasBuff(ModContent.BuffType<TheWorldBuff>()) && JoJoStands.timestopImmune.Contains(Projectile.type))
                        timestopImmune = true;
                    stoppedInTime = true;
                }
                if (!timestopImmune)
                {
                    if ((Projectile.timeLeft <= savedTimeLeft) || Projectile.minion)
                    {
                        Projectile.frameCounter = 1;
                        if (savedTimeLeft > 0)      //for the projectiles that don't have enough time left before they die
                            Projectile.timeLeft = savedTimeLeft;
                        else
                            Projectile.timeLeft = 2;

                        return false;
                    }
                }
            }

            if (Mplayer.timeskipPreEffect)     //saves it, this is for projectiles like minions, controllable projectiles, etc.
                preSkipVel = Projectile.velocity;
            if (Mplayer.timeskipActive)        //deploys it
                Projectile.velocity = preSkipVel;
            else
                preSkipVel = Vector2.Zero;

            if (Mplayer.epitaphForesightActive && !Projectile.minion)
            {
                applyingForesightPositions = true;
                if (foresightSaveTimer > 0)
                    foresightSaveTimer--;

                if (foresightSaveTimer <= 0)
                {
                    foresightPosition[foresightPositionIndex] = Projectile.position;
                    foresightFrames[foresightPositionIndex] = Projectile.frame;
                    foresightRotations[foresightPositionIndex] = Projectile.rotation;
                    foresightDirections[foresightPositionIndex] = Projectile.direction;
                    foresightPositionIndex++;       //second so that something saves in [0] and goes up from there
                    foresightPositionIndexMax++;
                    foresightSaveTimer = 15;
                }
            }
            if (!Mplayer.epitaphForesightActive && applyingForesightPositions)
            {
                if (!foresightResetIndex)
                {
                    foresightPositionIndex = 0;
                    foresightResetIndex = true;
                }
                Projectile.velocity = Vector2.Zero;
                Projectile.position = foresightPosition[foresightPositionIndex];
                Projectile.rotation = foresightRotations[foresightPositionIndex];
                if (foresightSaveTimer > 0)
                    foresightSaveTimer--;

                if (foresightSaveTimer <= 1)
                {
                    foresightPositionIndex++;
                    foresightSaveTimer = 15;
                    if (foresightPositionIndex >= 1)
                    {
                        if (foresightPosition[foresightPositionIndex - 1] != Vector2.Zero)
                            foresightPosition[foresightPositionIndex - 1] = Vector2.Zero;
                        if (foresightDirections[foresightPositionIndex - 1] != 0)
                            foresightDirections[foresightPositionIndex - 1] = 0;
                        if (foresightRotations[foresightPositionIndex - 1] != 0f)
                            foresightRotations[foresightPositionIndex - 1] = 0f;
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
                    if (possibleTarget.active && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && !possibleTarget.townNPC && !possibleTarget.hide && Projectile.Distance(possibleTarget.Center) <= 48f)
                    {
                        kickedBySexPistols = true;
                        autoModeSexPistols = false;

                        Vector2 redirectionVelocity = possibleTarget.Center - Projectile.Center;
                        redirectionVelocity.Normalize();
                        redirectionVelocity *= 16f;
                        Projectile.velocity = redirectionVelocity;
                        SoundEngine.PlaySound(SoundID.Tink, Projectile.Center);
                        break;
                    }
                }
            }

            if (kickedBySexPistols)
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 204, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
            return true;
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            if (Mplayer.epitaphForesightActive || applyingForesightPositions)
            {
                for (int i = 0; i < 50; i++)
                {
                    if (foresightPosition[i] != Vector2.Zero)
                        continue;

                    SpriteEffects effects = SpriteEffects.None;
                    int frameHeight = TextureAssets.Projectile[projectile.type].Value.Height / Main.projFrames[projectile.type];
                    if (foresightDirections[i] == 1)
                        effects = SpriteEffects.FlipHorizontally;

                    Vector2 drawPosition = foresightPosition[i] - Main.screenPosition;
                    Rectangle animRect = new Rectangle(0, projectile.frame * frameHeight, projectile.width, frameHeight);
                    Vector2 drawOrigin = projectile.Size / 2f;
                    Main.EntitySpriteDraw(TextureAssets.Projectile[projectile.type].Value, drawPosition, animRect, Color.DarkRed, foresightRotations[i], drawOrigin, projectile.scale, effects, 0);
                }
            }
            return true;
        }

        public override void ModifyHitPlayer(Projectile Projectile, Player target, ref int damage, ref bool crit)
        {
            MyPlayer mPlayer = target.GetModPlayer<MyPlayer>());
            if (mPlayer.StandSlot.Item.type == ModContent.ItemType<DollyDaggerT1>())
                damage = (int)(damage * 0.35f);
            if (mPlayer.StandSlot.Item.type == ModContent.ItemType<DollyDaggerT2>())
                damage = (int)(damage * 0.7f);
        }

        public override void ModifyHitNPC(Projectile Projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (kickedBySexPistols)
                damage = (int)(damage * 1.05f);
        }

        public override bool ShouldUpdatePosition(Projectile Projectile)        //thanks, HellGoesOn for telling me this hook even existed
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (mPlayer.timestopActive && Projectile.timeLeft <= savedTimeLeft)        //the ones who can move in Za Warudo's projectiles, like minions, fists, every other Projectile should freeze
                return timestopImmune;      //if it's owner isn't a timestop owner, always stop the Projectile

            return true;
        }
    }
}