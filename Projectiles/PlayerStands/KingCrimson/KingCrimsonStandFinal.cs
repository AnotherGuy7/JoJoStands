using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.KingCrimson
{
    public class KingCrimsonStandFinal : StandClass
    {
        public override int PunchDamage => 186;
        public override float PunchKnockback => 5f;
        public override int PunchTime => 20;      //KC's punch timings are based on it's frame, so punchTime has to be 3 frames longer than the duration of the frame KC punches in
        public override int HalfStandHeight => 32;
        public override int FistWhoAmI => 6;
        public override int TierNumber => 4;
        public override int StandOffset => 0;
        public override string PoseSoundName => "AllThatRemainsAreTheResults";
        public override string SpawnSoundName => "King Crimson";
        public override StandAttackType StandType => StandAttackType.Melee;

        private int timeskipStartDelay = 0;
        private int blockSearchTimer = 0;
        private bool preparingTimeskip = false;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (SpecialKeyPressed() && !player.HasBuff(ModContent.BuffType<SkippingTime>()) && timeskipStartDelay <= 0)
            {
                if (!JoJoStands.SoundsLoaded)
                    timeskipStartDelay = 80;
                else
                {
                    SoundStyle kingCrimson = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/KingCrimson");
                    kingCrimson.Volume = MyPlayer.ModSoundsVolume;
                    SoundEngine.PlaySound(kingCrimson, Projectile.position);
                    timeskipStartDelay = 0;
                }
                preparingTimeskip = true;
            }
            if (preparingTimeskip)
            {
                timeskipStartDelay++;
                if (timeskipStartDelay >= 80)
                {
                    shootCount += 15;
                    player.AddBuff(ModContent.BuffType<PreTimeSkip>(), 10);
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/TimeSkip"));
                    timeskipStartDelay = 0;
                    preparingTimeskip = false;
                    mPlayer.kingCrimsonAbilityCooldownTime = 30;
                }
            }
            if (player.HasBuff(ModContent.BuffType<SkippingTime>()) && player.HasBuff(ModContent.BuffType<ForesightBuff>()))
                mPlayer.kingCrimsonAbilityCooldownTime = 45;

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && mPlayer.canStandBasicAttack && !secondaryAbilityFrames && !player.HasBuff(ModContent.BuffType<SkippingTime>()))
                {
                    HandleDrawOffsets();
                    idleFrames = false;
                    attackFrames = true;
                    Projectile.netUpdate = true;

                    float rotaY = Main.MouseWorld.Y - Projectile.Center.Y;
                    Projectile.rotation = MathHelper.ToRadians((rotaY * Projectile.spriteDirection) / 6f);

                    Projectile.direction = 1;
                    if (Main.MouseWorld.X < Projectile.position.X)
                        Projectile.direction = -1;

                    Projectile.spriteDirection = Projectile.direction;

                    Vector2 velocityAddition = Main.MouseWorld - Projectile.position;
                    velocityAddition.Normalize();
                    velocityAddition *= 5f + mPlayer.standTier;
                    float mouseDistance = Vector2.Distance(Main.MouseWorld, Projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        Projectile.velocity = player.velocity + velocityAddition;
                    }
                    if (mouseDistance <= 40f)
                    {
                        Projectile.velocity = Vector2.Zero;
                    }

                    if (shootCount <= 0 && (Projectile.frame == 0 || Projectile.frame == 4))
                    {
                        shootCount += newPunchTime / 2;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= ProjectileSpeed;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), newPunchDamage, PunchKnockback, Projectile.owner, FistWhoAmI);
                        Main.projectile[proj].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                    LimitDistance();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (Main.mouseRight && Projectile.owner == Main.myPlayer && !playerHasAbilityCooldown && !player.HasBuff(ModContent.BuffType<SkippingTime>()))
                {
                    GoInFront();
                    idleFrames = false;
                    attackFrames = false;
                    secondaryAbilityFrames = true;

                    if (blockSearchTimer > 0)
                    {
                        blockSearchTimer--;
                        return;
                    }

                    int rectWidth = 56;
                    int rectHeight = 64;
                    Rectangle blockRect = new Rectangle((int)Projectile.Center.X - (rectWidth / 2), (int)Projectile.Center.Y - (rectHeight / 2), rectWidth, rectHeight);
                    for (int p = 0; p < Main.maxProjectiles; p++)
                    {
                        Projectile otherProj = Main.projectile[p];
                        if (otherProj.active)
                        {
                            if (blockRect.Intersects(otherProj.Hitbox) && otherProj.type != Projectile.type && !otherProj.friendly)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    Dust.NewDust(player.position, player.width, player.height, DustID.Clentaminator_Red);
                                }

                                otherProj.penetrate -= 1;
                                if (otherProj.penetrate <= 0)
                                    otherProj.Kill();

                                secondaryAbilityFrames = false;

                                Vector2 repositionOffset = new Vector2(5f * 16f * -player.direction, 0f);
                                while (WorldGen.SolidTile((int)(player.Center.X + repositionOffset.X) / 16, (int)(player.Center.Y + repositionOffset.Y) / 16))
                                {
                                    repositionOffset.Y -= 16f;
                                }
                                player.position += repositionOffset;
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(4));
                                SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/TimeSkip"));
                                for (int i = 0; i < 20; i++)
                                {
                                    Dust.NewDust(player.position, player.width, player.height, DustID.Clentaminator_Red);
                                }
                            }
                        }
                    }
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.lifeMax > 5 && !npc.immortal && !npc.townNPC && !npc.friendly && !npc.hide && npc.Hitbox.Intersects(blockRect))
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                Dust.NewDust(player.position, player.width, player.height, DustID.Clentaminator_Red);
                            }

                            Vector2 repositionOffset = new Vector2(5f * 16f * -player.direction, 0f);
                            while (WorldGen.SolidTile((int)(player.Center.X + repositionOffset.X) / 16, (int)(player.Center.Y + repositionOffset.Y) / 16))
                            {
                                repositionOffset.Y -= 16f;
                            }
                            player.position += repositionOffset;
                            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(4));
                            npc.StrikeNPC(newPunchDamage * 2, PunchKnockback * 1.5f, Projectile.direction);
                            SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/TimeSkip"));

                            for (int i = 0; i < 20; i++)
                            {
                                Dust.NewDust(player.position, player.width, player.height, DustID.Clentaminator_Red);
                            }
                        }
                    }
                    blockSearchTimer += 5;
                }
                else
                {
                    secondaryAbilityFrames = false;
                }
                if (!attackFrames && !secondaryAbilityFrames)
                {
                    StayBehind();
                }
                if (SecondSpecialKeyPressed() && shootCount <= 0 && !player.HasBuff(ModContent.BuffType<ForesightBuff>()) && !preparingTimeskip && Projectile.owner == Main.myPlayer)
                {
                    player.AddBuff(ModContent.BuffType<ForesightBuff>(), 540);
                    mPlayer.epitaphForesightActive = true;
                    mPlayer.kingCrimsonAbilityCooldownTime = 30;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        ModNetHandler.effectSync.SendForesight(256, player.whoAmI, true, player.whoAmI);
                }
            }
            if (mPlayer.standAutoMode)
            {
                BasicPunchAI();
            }
            Projectile.shouldFallThrough = true;
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (secondaryAbilityFrames)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Block");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().posing)
            {
                idleFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/KingCrimson/KingCrimson_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 15, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 6, newPunchTime / 2, true);
            }
            if (animationName == "Block")
            {
                AnimateStand(animationName, 4, 15, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 2, true);
            }
        }
    }
}