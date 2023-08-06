using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.Debuffs;
using JoJoStands.Items;
using JoJoStands.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace JoJoStands.Projectiles
{
    public class JoJoGlobalProjectile : GlobalProjectile
    {
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
        public bool kickedByStarPlatinum = false;
        public bool bulletsCenturyBoy = false;
        public bool standProjectile = false;
        public float timestopFreezeProgress = 0f;
        public int timestopStartTimeLeft = 0;
        public Vector2 preSkipVel = Vector2.Zero;

        public int echoesTailTipType = 0; // 1 - boing, 2 - kaboom, 3 - wooosh, 4 - sizzle
        public int echoesTailTipTier = 0;
        public int echoesTailTipStage = 0;

        public bool exceptionForSCParry = false;

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

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (projectile.DamageType.CountsAsClass<RangedDamageClass>() && mPlayer.centuryBoyActive && !projectile.arrow)
                    bulletsCenturyBoy = true;
            if (projectile.type == ModContent.ProjectileType<Fists>() || projectile.type == ModContent.ProjectileType<PlayerStands.Cream.Void>() || projectile.type == ModContent.ProjectileType<PlayerStands.Cream.DashVoid>() || projectile.type == ModContent.ProjectileType<StarFinger>() || projectile.type == ModContent.ProjectileType<HermitPurpleGrab>() || projectile.type == ModContent.ProjectileType<HermitPurpleHook>() || projectile.type == ModContent.ProjectileType<RedBind>() || projectile.type == ModContent.ProjectileType<BindingEmeraldString>() || projectile.type == ModContent.ProjectileType<StickyFingersFistExtended>() || projectile.type == ModContent.ProjectileType<StoneFreeBindString>() || projectile.type == ModContent.ProjectileType<PhantomMarker>())
                exceptionForSCParry = true;
            for (int i = 0; i < JoJoStands.standProjectileList.Count - 1; i++)
            {
                if (JoJoStands.standProjectileList[i] == projectile.type || kickedByStarPlatinum || kickedBySexPistols || bulletsCenturyBoy)
                {
                    if (mPlayer.standName == "TowerOfGray")
                        projectile.ArmorPenetration += mPlayer.standArmorPenetration + 10 * mPlayer.standTier;
                    else
                        projectile.ArmorPenetration += mPlayer.standArmorPenetration + 10;

                    if (mPlayer.standType == 2 && mPlayer.herbalTeaBag && mPlayer.herbalTeaBagCount > 0 && projectile.type != ModContent.ProjectileType<Fists>() && projectile.type != ModContent.ProjectileType<NailSlasher>())
                    {
                        projectile.ArmorPenetration += 10;
                        projectile.damage += 10;
                        projectile.damage += (int)(projectile.damage * 0.1f);
                        projectile.penetrate += 1;
                        mPlayer.herbalTeaBagCount -= 1;
                    }
                    if (mPlayer.herbalTeaBagCooldown > 0)
                        mPlayer.herbalTeaBagCooldown -= 15;
                }
                if (JoJoStands.standProjectileList[i] == projectile.type)
                    standProjectile = true;
            }
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
                    timestopStartTimeLeft = Projectile.timeLeft;
                    if (player.HasBuff(ModContent.BuffType<TheWorldBuff>()) && mPlayer.timestopOwner && JoJoStands.timestopImmune.Contains(Projectile.type))
                        timestopImmune = true;
                }

                if (timestopImmune)
                {
                    if (!player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                        timestopImmune = false;

                    return true;
                }
                if (timestopFreezeProgress < 1f)
                {
                    timestopFreezeProgress += 0.1f;
                    Projectile.velocity *= 0.9f;
                }

                if (timestopFreezeProgress >= 1f || Projectile.minion)
                {
                    Projectile.frameCounter = 2;
                    if (timestopStartTimeLeft > 2)      //for the projectiles that don't have enough time left before they die
                        Projectile.timeLeft = timestopStartTimeLeft;
                    else
                        Projectile.timeLeft = 2;

                    if (mPlayer.ableToOverrideTimestop && JoJoStands.timestopImmune.Contains(Projectile.type))
                    {
                        if (player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                            timestopImmune = true;

                        return true;
                    }

                    return false;
                }
            }
            else
            {
                if (timestopFreezeProgress > 0f)
                {
                    timestopFreezeProgress -= 0.1f;
                    Projectile.velocity *= 1.1f;
                }
                else
                {
                    stoppedInTime = false;
                    timestopImmune = false;
                    timestopFreezeProgress = 0f;
                    timestopStartTimeLeft = 0;
                }
            }

            if (mPlayer.timeskipActive)        //deploys it
            {
                if (preSkipVel == Vector2.Zero)
                    preSkipVel = Projectile.velocity;

                Projectile.velocity = preSkipVel;
            }
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
                    if (possibleTarget.active && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && !possibleTarget.townNPC && !possibleTarget.hide && Projectile.Distance(possibleTarget.Center) <= (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standTier + 2) * 16f)
                    {
                        kickedBySexPistols = true;
                        autoModeSexPistols = false;

                        int amountOfKickDusts = 15;
                        for (int i = 0; i < amountOfKickDusts; i++)
                        {
                            float rotation = MathHelper.ToRadians(((360 / 15) * i) - 90f);        //60 since it's the max amount of dusts that is supposed to circle it
                            Vector2 dustPosition = Projectile.Center + (rotation.ToRotationVector2() * 4f);
                            int dustIndex = Dust.NewDust(dustPosition, 1, 1, DustID.IchorTorch);
                            Main.dust[dustIndex].noGravity = true;
                            Main.dust[dustIndex].noLight = true;
                            Vector2 velocity = Projectile.Center + ((rotation + 0.01f).ToRotationVector2() * 4f) - dustPosition;
                            velocity.Normalize();
                            velocity *= 1.6f;
                            Main.dust[dustIndex].velocity = velocity;
                            Main.dust[dustIndex].scale = 1f;
                        }

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
                Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, DustID.TreasureSparkle, Projectile.velocity.X * -0.3f, Projectile.velocity.Y * -0.3f);

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

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            MyPlayer mPlayer = target.GetModPlayer<MyPlayer>();
            if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<DollyDaggerT1>())
                modifiers.FinalDamage *= 0.35f;
            else if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<DollyDaggerT2>())
                modifiers.FinalDamage *= 0.7f;
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (info.PvP)
            {
                if (standProjectile || kickedByStarPlatinum || kickedBySexPistols || bulletsCenturyBoy)
                {
                    if ((mPlayer.crackedPearlEquipped || mPlayer.viralPearlEarringEquipped) && player.whoAmI == Main.myPlayer && Main.rand.Next(1, 100 + 1) <= 40)
                    {
                        target.AddBuff(ModContent.BuffType<Infected>(), 5 * 60);
                        SyncCall.SyncOtherPlayerDebuff(player.whoAmI, target.whoAmI, ModContent.BuffType<Infected>(), 5 * 60);
                    }
                }
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (player.HasBuff(ModContent.BuffType<CenturyBoyBuff>()) && projectile.DamageType.CountsAsClass<SummonDamageClass>())
                modifiers.FinalDamage /= 2;
            if (kickedByStarPlatinum || kickedBySexPistols || bulletsCenturyBoy)
            {
                if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                    modifiers.SetCrit();
                modifiers.FinalDamage *= 1.05f * mPlayer.standDamageBoosts;
            }
            if (standProjectile || kickedByStarPlatinum || kickedBySexPistols || bulletsCenturyBoy)
            {
                if (mPlayer.underbossPhoneEquipped)
                {
                    mPlayer.underbossPhoneCount += 1;
                    if (mPlayer.underbossPhoneCount >= 5)
                    {
                        projectile.ArmorPenetration = target.defense;
                        modifiers.FinalDamage *= 1.1f + 10;
                        mPlayer.underbossPhoneCount = 0;
                    }
                }
                if (mPlayer.iceCreamEquipped)
                {
                    mPlayer.iceCreamEnemyHitCount += 1;
                    if (mPlayer.iceCreamEnemyHitCount >= 8)
                    {
                        mPlayer.iceCreamEnemyHitCount = 0;
                        projectile.ArmorPenetration = target.defense;
                        modifiers.SetCrit();
                    }
                }
                if (player.HasBuff(ModContent.BuffType<Rampage>()))
                    projectile.ArmorPenetration = target.defense;
                if (mPlayer.manifestedWillEmblem)
                    modifiers.CritDamage *= 1.5f;
                if (mPlayer.iceCreamEquipped)
                    modifiers.CritDamage *= 1.7f;
                if (mPlayer.sealedPokerDeckEquipped && mPlayer.sealedPokerDeckCooldown <= 0)
                {
                    modifiers.SetCrit();
                    modifiers.FinalDamage *= 1.25f;
                    mPlayer.sealedPokerDeckCooldown += 5 * 60;
                }
                if (mPlayer.firstNapkinEquipped && target.life - modifiers.FinalDamage.Base <= 0 && !target.SpawnedFromStatue && player.HasBuff<AbilityCooldown>() && player.buffTime[player.FindBuffIndex(ModContent.BuffType<AbilityCooldown>())] > 60)
                    player.buffTime[player.FindBuffIndex(ModContent.BuffType<AbilityCooldown>())] -= 60;
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (standProjectile || kickedByStarPlatinum || kickedBySexPistols || bulletsCenturyBoy)
            {
                if (mPlayer.crackedPearlEquipped || mPlayer.viralPearlEarringEquipped)
                {
                    if (player.whoAmI == Main.myPlayer && Main.rand.Next(1, 100 + 1) <= 40)
                        target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
                }
                if (mPlayer.standTarget != target.whoAmI)
                {
                    mPlayer.fightingSpiritEmblemStack -= 6;
                    if (mPlayer.fightingSpiritEmblemStack < 0)
                        mPlayer.fightingSpiritEmblemStack = 0;
                    mPlayer.standTarget = target.whoAmI;
                }
                if (mPlayer.fightingSpiritEmblem && mPlayer.fightingSpiritEmblemStack <= 30)
                    mPlayer.fightingSpiritEmblemStack += 1;
                if (mPlayer.polaroidEquipped && mPlayer.polaroidTokens < 5 && mPlayer.polaroidTokenGainTimer <= 0 && target.life <= 0)
                {
                    mPlayer.polaroidTokens += 1;
                    mPlayer.polaroidTokenGainTimer += 5 * 60;
                }
                if (mPlayer.viralPearlEarringEquipped)
                {
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && !npc.hide && !npc.immortal && !npc.friendly && npc.lifeMax > 5)
                        {
                            if (projectile.Distance(npc.Center) <= 12 * 16 && npc.whoAmI != target.whoAmI)
                            {
                                float arrowEarringDamage = 0.1f;
                                if (mPlayer.arrowEarringCooldown <= 0)
                                    arrowEarringDamage = 0.2f;
                                bool critCheck = Main.rand.NextFloat(1, 100 + 1) <= mPlayer.standCritChangeBoosts;
                                NPC.HitInfo hitInfo = new NPC.HitInfo()
                                {
                                    Damage = (int)Main.rand.NextFloat((int)(damageDone * arrowEarringDamage * 0.85f), (int)(damageDone * arrowEarringDamage * 1.15f)) + npc.defense / 2,
                                    Knockback = 0f,
                                    HitDirection = 1,
                                    Crit = critCheck
                                };
                                npc.StrikeNPC(hitInfo);
                                if (mPlayer.arrowEarringCooldown <= 0)
                                    target.AddBuff(ModContent.BuffType<Infected>(), 5 * 60);
                                SyncCall.SyncArrowEarringInfo(player.whoAmI, npc.whoAmI, hitInfo.Damage, critCheck);
                            }
                        }
                    }
                }
                else if (mPlayer.arrowEarringEquipped)
                {
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && !npc.hide && !npc.immortal && !npc.friendly && npc.lifeMax > 5)
                        {
                            if (projectile.Distance(npc.Center) <= 8 * 16 && npc.whoAmI != target.whoAmI)
                            {
                                float arrowEarringDamage = 0.1f;
                                if (mPlayer.arrowEarringCooldown <= 0)
                                    arrowEarringDamage = 0.2f;
                                bool critCheck = Main.rand.NextFloat(1, 100 + 1) <= mPlayer.standCritChangeBoosts;
                                NPC.HitInfo hitInfo = new NPC.HitInfo()
                                {
                                    Damage = (int)Main.rand.NextFloat((int)(damageDone * arrowEarringDamage * 0.85f), (int)(damageDone * arrowEarringDamage * 1.15f)) + npc.defense / 2,
                                    Knockback = 0f,
                                    HitDirection = 1,
                                    Crit = critCheck
                                };
                                npc.StrikeNPC(hitInfo);
                                SyncCall.SyncArrowEarringInfo(player.whoAmI, npc.whoAmI, hitInfo.Damage, critCheck);
                            }
                        }
                    }
                }
                if (mPlayer.vampiricBangleEquipped || mPlayer.polaroidEquipped)
                    player.Heal(Main.rand.Next(1, 5 + 1));
            }
        }

        public override bool ShouldUpdatePosition(Projectile Projectile)        //thanks, HellGoesOn for telling me this hook even existed
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (timestopImmune)
                return true;

            if (mPlayer.timestopActive && timestopFreezeProgress >= 1f)
                return false;

            return true;
        }
    }
}