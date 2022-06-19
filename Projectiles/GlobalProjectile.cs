using JoJoStands.Buffs.EffectBuff;
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
        public ForesightData[] foresightData = new ForesightData[50];
        public bool stoppedInTime = false;
        //public bool checkedForImmunity = false;
        public bool timestopImmune = false;
        public bool autoModeSexPistols = false;
        public bool kickedBySexPistols = false;

        public struct ForesightData
        {
            public Vector2 position;
            public int frame;
            public float rotation;
            public int direction;
        }

        public override bool InstancePerEntity
        {
            get { return true; }
        }

        public override bool PreAI(Projectile Projectile)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.timestopActive)
            {
                if (!stoppedInTime)
                {
                    stoppedInTime = true;
                    Projectile.damage = (int)(Projectile.damage * 0.8f);        //projectiles in timestop lose 20% damage, so it's not as OP
                    if (player.HasBuff(ModContent.BuffType<TheWorldBuff>()) && mPlayer.timestopOwner && JoJoStands.timestopImmune.Contains(Projectile.type))
                        timestopImmune = true;
                }

                if (timestopImmune)
                {
                    if (!player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                        timestopImmune = false;

                    return true;
                }

                timeLeftSave++;
                if (timeLeftSave >= 6 && savedTimeLeft == 0)
                    savedTimeLeft = Projectile.timeLeft - 5;     //so they stop don't stop immediatel

                if ((Projectile.timeLeft <= savedTimeLeft) || Projectile.minion)
                {
                    Projectile.frameCounter = 2;
                    if (savedTimeLeft > 0)      //for the projectiles that don't have enough time left before they die
                        Projectile.timeLeft = savedTimeLeft;
                    else
                        Projectile.timeLeft = 2;

                    if (mPlayer.ableToOverrideTimestop)
                    {
                        if (player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                        {
                            timeLeftSave = 0;
                            savedTimeLeft = 0;
                            timestopImmune = true;
                        }

                        return true;
                    }

                    return false;
                }
            }
            if (!mPlayer.timestopActive && timeLeftSave > 0)
            {
                timeLeftSave = 0;
                savedTimeLeft = 0;
                stoppedInTime = false;
                timestopImmune = false;
            }

            if (mPlayer.timeskipPreEffect)     //saves it, this is for projectiles like minions, controllable projectiles, etc.
                preSkipVel = Projectile.velocity;
            if (mPlayer.timeskipActive)        //deploys it
                Projectile.velocity = preSkipVel;
            else
                preSkipVel = Vector2.Zero;

            if (mPlayer.epitaphForesightActive && !Projectile.minion)
            {
                applyingForesightPositions = true;
                if (foresightSaveTimer > 0)
                    foresightSaveTimer--;

                if (foresightSaveTimer <= 0)
                {
                    ForesightData data = new ForesightData()
                    {
                        position = Projectile.position,
                        frame = Projectile.frame,
                        rotation = Projectile.rotation,
                        direction = Projectile.direction
                    };
                    foresightData[foresightPositionIndex] = data;
                    foresightPositionIndex++;       //second so that something saves in [0] and goes up from there
                    foresightPositionIndexMax++;
                    foresightSaveTimer = 15;
                    if (foresightPositionIndex >= 50)
                    {
                        foresightPositionIndex = 49;
                        foresightPositionIndexMax = 49;
                    }
                }
            }
            if (!mPlayer.epitaphForesightActive && applyingForesightPositions)
            {
                if (!foresightResetIndex)
                {
                    foresightPositionIndex = 0;
                    foresightResetIndex = true;
                }
                Projectile.velocity = Vector2.Zero;
                Projectile.position = foresightData[foresightPositionIndex].position;
                Projectile.rotation = foresightData[foresightPositionIndex].rotation;
                Projectile.direction = foresightData[foresightPositionIndex].direction;
                if (foresightSaveTimer > 0)
                    foresightSaveTimer--;

                if (foresightSaveTimer <= 1)
                {
                    foresightPositionIndex++;
                    foresightSaveTimer = 15;
                    if (foresightPositionIndex >= 1)
                        foresightData[foresightPositionIndex - 1].position = Vector2.Zero;
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
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TreasureSparkle, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);

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
                    if (foresightData[i].position == Vector2.Zero)
                        continue;

                    SpriteEffects effects = SpriteEffects.None;
                    int frameHeight = TextureAssets.Projectile[projectile.type].Value.Height / Main.projFrames[projectile.type];
                    if (foresightData[i].direction == 1)
                        effects = SpriteEffects.FlipHorizontally;

                    Vector2 drawPosition = foresightData[i].position - Main.screenPosition;
                    Rectangle animRect = new Rectangle(0, projectile.frame * frameHeight, projectile.width, frameHeight);
                    Vector2 drawOrigin = projectile.Size / 2f;
                    Main.EntitySpriteDraw(TextureAssets.Projectile[projectile.type].Value, drawPosition, animRect, Color.DarkRed, foresightData[i].rotation, drawOrigin, projectile.scale, effects, 0);
                }
            }
            return true;
        }

        public override void ModifyHitPlayer(Projectile Projectile, Player target, ref int damage, ref bool crit)
        {
            MyPlayer mPlayer = target.GetModPlayer<MyPlayer>();
            if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<DollyDaggerT1>())
                damage = (int)(damage * 0.35f);
            if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<DollyDaggerT2>())
                damage = (int)(damage * 0.7f);
        }

        public override void ModifyHitNPC(Projectile Projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (kickedBySexPistols)
                damage = (int)(damage * 1.05f);
        }

        public override bool ShouldUpdatePosition(Projectile Projectile)        //thanks, HellGoesOn for telling me this hook even existed
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (timestopImmune)
                return true;

            if (mPlayer.timestopActive && !mPlayer.timestopOwner && Projectile.timeLeft <= savedTimeLeft)
                return false;

            return true;
        }
    }
}