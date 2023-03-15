using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.ItemBuff;
using JoJoStands.DropConditions;
using JoJoStands.Items;
using JoJoStands.Items.Accessories;
using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Items.Tiles;
using JoJoStands.Items.Vampire;
using JoJoStands.NPCs.Enemies;
using JoJoStands.NPCs.TownNPCs;
using JoJoStands.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.Audio;
using Terraria.DataStructures;
using JoJoStands.Projectiles;

namespace JoJoStands.NPCs
{
    public class JoJoGlobalNPC : GlobalNPC
    {

        public bool frozenInTime = false;
        public bool affectedbyBtz = false;
        public bool taggedByButterfly = false;
        public bool applyingForesightPositions = false;
        public bool foresightResetIndex = false;
        public bool taggedWithPhantomMarker = false;
        public bool oneTimeEffectsApplied = false;
        public bool grabbedByHermitPurple = false;
        public bool taggedByKillerQueen = false;
        public bool sunTagged = false;
        public bool sunShackled = false;
        public bool stunnedByBindingEmerald = false;
        public bool removeZombieHighlightOnHit = false;
        public bool highlightedByTheHandMarker = false;
        public bool echoesFreezeTarget = false;
        public bool boundByStrings = false;
        public bool hitByCrossfireHurricane = false;
        public bool taggedByCrazyDiamondRestoration = false;
        public bool targetedBySoftAndWet = false;
        public int foresightSaveTimer = 0;
        public int foresightPositionIndex = 0;
        public int foresightPositionIndexMax = 0;
        public int btZSaveTimer = 0;
        public int btzTotalRewindTime = 0;
        public int btzTotalRewindTimer = 0;
        public int timeskipAIStyle = 0;
        public int lifeRegenIncrement = 0;
        public int lockRegenCounter = 0;
        public bool forceDeath = false;
        public int btzPositionIndex = 0;
        public int taggedForDeathLoop = 0;
        public int spawnedByDeathLoop = 0;
        public int deathLoopOwner = -1;
        public int deathTimer = 0;
        public int zombieHightlightTimer = 0;
        public int bindingEmeraldDurationTimer = 0;
        public int crossfireHurricaneEffectTimer = 0;
        public float kingCrimsonDonutMultiplier = 1f;
        public int vampireUserLastHitIndex = -1;        //An index of the vampiric player who last hit the enemy
        public int standDebuffEffectOwner = 0;
        public int lockFrameCounter;
        public int lockFrame;
        public Vector2 playerPositionOnSkip = Vector2.Zero;
        public Vector2 preTimestopVelocity = Vector2.Zero;
        public Vector2[] BtZPositions = new Vector2[400];
        public ForesightSaveData[] foresightData = new ForesightSaveData[120];

        public int crazyDiamondPunchCount = 0;
        public int echoesDebuffOwner = -1;
        public int echoesThreeFreezeTimer = 0;
        public int echoesSoundIntensity = 2;
        public int echoesSoundMaxIntensity = 48;
        public int towerOfGrayImmunityFrames = 0;

        public float echoesCrit = 5f;
        public float echoesDamageBoost = 1f;
        public float theLockCrit = 5f;
        public float theLockDamageBoost = 1f;

        public bool crazyDiamondFullHealth = false;
        public bool echoesKaboom = false;

        public int whitesnakeDISCImmune = 0;

        private int CDsavedAIstyle = 0;
        private int echoesThreeFreezeDamageTimer = 30; //3 Freeze
        public int echoesSmackDamageTimer = 60; //ACT 1 sounds
        private int fallDamage = 0;
        private int fallDamageStart = (int)Vector2.Zero.Y;
        private int resetCooldown = 0;

        public bool echoesSmackCritChance = false; //ACT 1 sounds
        private bool onlyOnce = false;
        private bool resetEffects = false;
        private bool savedTileCollide = false;

        private float CDsavedKnockbackRes = 0;

        private SoundStyle? CDsavedHitSound = SoundID.PlayerHit;
        private Vector2 echoesFallingPoint = Vector2.Zero;

        private int npcWhoAmI = 0;

        public override bool InstancePerEntity
        {
            get { return true; }
        }

        public struct ForesightSaveData
        {
            public Vector2 position;
            public Rectangle frame;
            public int direction;
            public float rotation;
        }

        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            if (!Main.npc[npcWhoAmI].SpawnedFromStatue)
            {
                globalLoot.Add(ItemDropRule.ByCondition(new SunDropletCondition(), ModContent.ItemType<SunDroplet>(), 10, 1, 3));

                globalLoot.Add(ItemDropRule.ByCondition(new WillToFightCondition(), ModContent.ItemType<WillToFight>(), 14));

                globalLoot.Add(ItemDropRule.ByCondition(new WillToProtectCondition(), ModContent.ItemType<WillToProtect>(), 14));

                globalLoot.Add(ItemDropRule.ByCondition(new WillToChangeCondition(), ModContent.ItemType<WillToChange>(), 14));

                globalLoot.Add(ItemDropRule.ByCondition(new WillToControlCondition(), ModContent.ItemType<WillToControl>(), 14));

                globalLoot.Add(ItemDropRule.ByCondition(new WillToDestroyCondition(), ModContent.ItemType<WillToDestroy>(), 14));

                globalLoot.Add(ItemDropRule.ByCondition(new WillToEscapeCondition(), ModContent.ItemType<WillToEscape>(), 14));

                globalLoot.Add(ItemDropRule.ByCondition(new SoulOfTimeCondition(), ModContent.ItemType<SoulofTime>(), 14));

                globalLoot.Add(ItemDropRule.ByCondition(new WillToChangeCondition(), ModContent.ItemType<HerbalTeaBag>(), 40));

                globalLoot.Add(ItemDropRule.ByCondition(new JoJoStandsHardmodeDungeonCondition(), ModContent.ItemType<TheFirstNapkin>(), 40));

                globalLoot.Add(ItemDropRule.ByCondition(new JoJoStandsCorruptionCondition(), ModContent.ItemType<SealedPokerDeck>(), 40));      //These two are world-alternates

                globalLoot.Add(ItemDropRule.ByCondition(new JoJoStandsCrimsonCondition(), ModContent.ItemType<UnderbossPhone>(), 40));
            }


            /*IItemDropRule sunDropletRule = new LeadingConditionRule(new SunDropletCondition());
            sunDropletRule.OnSuccess(new CommonDrop(ModContent.ItemType<SunDroplet>(), 10, 1, 3));
            globalLoot.Add(sunDropletRule);

            IItemDropRule willToFightRule = new LeadingConditionRule(new WillToFightCondition());
            willToFightRule.OnSuccess(new CommonDrop(ModContent.ItemType<WillToFight>(), 14));
            globalLoot.Add(willToFightRule);

            IItemDropRule willToProtectRule = new LeadingConditionRule(new WillToProtectCondition());
            willToProtectRule.OnSuccess(new CommonDrop(ModContent.ItemType<WillToProtect>(), 14));
            globalLoot.Add(willToProtectRule);

            IItemDropRule willToDestroyRule = new LeadingConditionRule(new WillToDestroyCondition());
            willToDestroyRule.OnSuccess(new CommonDrop(ModContent.ItemType<WillToDestroy>(), 14));
            globalLoot.Add(willToDestroyRule);

            IItemDropRule willToControlRule = new LeadingConditionRule(new WillToControlCondition());
            willToControlRule.OnSuccess(new CommonDrop(ModContent.ItemType<WillToControl>(), 14));
            globalLoot.Add(willToControlRule);

            IItemDropRule willToEscapeRule = new LeadingConditionRule(new WillToEscapeCondition());
            willToEscapeRule.OnSuccess(new CommonDrop(ModContent.ItemType<WillToEscape>(), 14));
            globalLoot.Add(willToEscapeRule);

            IItemDropRule willToChangeRule = new LeadingConditionRule(new WillToChangeCondition());
            willToChangeRule.OnSuccess(new CommonDrop(ModContent.ItemType<WillToChange>(), 14));
            globalLoot.Add(willToChangeRule);

            IItemDropRule soulOfTimeRule = new LeadingConditionRule(new SoulOfTimeCondition());
            soulOfTimeRule.OnSuccess(new CommonDrop(ModContent.ItemType<SoulofTime>(), 14));
            globalLoot.Add(soulOfTimeRule);*/
        }

        /*public override bool SpecialNPCLoot(NPC npc)
        {
            if (taggedByButterfly)      //increases the drop chances of loot by calling it again when called, cause it's gonna normally call NPCLoot and call it again here
            {
                npc.NPCLoot();
                npc.value = 0;
            }
            return base.SpecialNPCLoot(npc);
        }*/

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (!npc.SpawnedFromStatue)
            {
                if (npc.type == NPCID.WallofFlesh)
                    npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<StandEmblem>(), ModContent.ItemType<HamonEmblem>()));
                if (npc.type == NPCID.Zombie || npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinScout || npc.type == NPCID.GoblinSorcerer || npc.type == NPCID.GoblinSummoner || npc.type == NPCID.GoblinThief || npc.type == NPCID.GoblinTinkerer || npc.type == NPCID.GoblinWarrior || npc.townNPC)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Hand>(), 25));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ArrowEarring>(), 40));
                }
                if (npc.type == ModContent.NPCType<MarineBiologist>())
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FamilyPhoto>(), 1));
                if (npc.type == NPCID.BigMimicCrimson)
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VampiricBangle>(), 4));
                if (npc.type == NPCID.BigMimicCorruption)
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SiliconLifeformCarapace>(), 4));
                if (npc.type == NPCID.BigMimicHallow)
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoothingSpiritDisc>(), 4));
            }
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            if (JoJoStands.SecretReferences)
            {
                if (npc.type == ModContent.NPCType<MarineBiologist>() && Main.rand.Next(0, 101) <= 3)      //Placement contributor reference
                {
                    chat = "I knew a guy who loved to name things. He’s still around, and he’s probably still naming everything he can find. I wonder what kind of Placement he went through in the dimension shift...";
                }
                if (npc.type == ModContent.NPCType<MarineBiologist>() && Main.rand.Next(0, 101) <= 3)      //Nekro contributor reference             
                {
                    //Removed at Nekro's behest for reasons described as "cringe" and "bad"
                    //chat = "There was a man with splt personalities named Nekro and Sektor, they named about 10 of the stands you have access to, kinda reminds me of a friend from Egypt...";
                    chat = "Did you know animals can develop stands too? I even once saw an eagle who got struck with the arrow become humanoid once it developed a stand capable of turning people to glass. I even managed to teach it my old cigarette trick.";
                }
                if (npc.type == ModContent.NPCType<MarineBiologist>() && Main.rand.Next(0, 101) <= 5)      //Techno contributor reference
                {
                    chat = "Some weirdo with an afro once zoomed past me at the speed of a train, with his Stand carrying him in a box. Gramps seemed to approve. Who knows where that lunatic is now.";
                }
                if (npc.type == NPCID.Nurse && Main.rand.Next(0, 100) <= 5)     //ciocollata reference
                {
                    chat = "I heard there was a surgeon fired for killing patients and recording it, there are some sick people in this world.";
                }
                if (npc.type == NPCID.Demolitionist && Main.rand.Next(0, 100) <= 10)        //obviously, a Killer Queen reference
                {
                    chat = Main.LocalPlayer.name + " do you know what a 'Killer Queen' is? I heard it can make anything explode...";
                }
                if (npc.type == NPCID.Guide && Main.rand.Next(0, 100) <= 4)     //Betty contributor reference
                {
                    chat = "Hey " + Main.LocalPlayer.name + ", the other day one small girl calling herself 'The Dead Princess' came to me asking for a new name to her list... I'm not sure what was she talking about...";
                }
                if (npc.type == NPCID.Mechanic && Main.rand.Next(0, 100) <= 5)      //Phil contributer reference
                {
                    chat = "I've heard of someone named Phil selling something called 'Flex Tape' that can fix everything, mind if you get some for me?";
                }
                if (npc.type == NPCID.Mechanic && Main.rand.Next(0, 100) <= 5)      //Archerous contributer reference
                {
                    chat = "Have the lights been acting weird lately? If I didn't know better, I'd say Archerous has something to do with this. That 'Lucy in the Sky' of his has been causing me some real trouble!";
                }
                if (npc.type == NPCID.Cyborg && Main.rand.Next(0, 100) <= 5)      //AG contributer reference
                {
                    chat = "Hey, if you ever see me acting weird, just tie me up and lock me inside of my home. I hear there's someone called 'A. Guy' or something of the sort looking to mess with me.";
                }
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            Player player = Main.player[Main.myPlayer];
            if (type == NPCID.Merchant)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Sunscreen>());
                nextSlot++;
            }
            if (type == NPCID.TravellingMerchant && ((Main.hardMode && Main.rand.Next(0, 101) >= 90) || NPC.downedPlantBoss))
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<ViralPearlRing>());
                nextSlot++;
            }
            if (type == NPCID.Painter)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<IWouldntLose>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<OfficersRegret>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<QuietLife>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<ShotintheDark>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<BloodForTheKing>());
                nextSlot++;
            }
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.type == type && npc.HasBuff(ModContent.BuffType<BelieveInMe>()) && npc.active && player.talkNPC == npc.whoAmI)
                {
                    for (int i = 0; i < shop.item.Length; i++)
                        shop.item[i].value -= (int)(shop.item[i].value * 0.2f);
                    for (int i = 0; i < 54; i++)
                    {
                        Item ItemSelect = player.inventory[i];
                        ItemSelect.value += (int)(ItemSelect.value * 0.2f);
                    }
                }
            }
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            npcWhoAmI = npc.whoAmI;
        }

        public override bool PreAI(NPC npc)
        {
            MyPlayer mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
            if (taggedForDeathLoop > 0)
            {
                taggedForDeathLoop--;
                if (deathLoopOwner != -1)
                {
                    if (!Main.player[deathLoopOwner].active || Main.player[deathLoopOwner].dead)
                    {
                        Main.player[deathLoopOwner].ClearBuff(ModContent.BuffType<DeathLoop>());
                        taggedForDeathLoop = 0;
                        deathLoopOwner = -1;
                    }    
                }
            }
            if (taggedForDeathLoop == 0)
            {
                if (deathLoopOwner != -1)
                {
                    Main.player[deathLoopOwner].ClearBuff(ModContent.BuffType<DeathLoop>());
                    deathLoopOwner = -1;
                }
            }
            if (!mPlayer.timeskipActive && !mPlayer.timestopActive && !mPlayer.backToZeroActive)
            {
                if (npc.aiStyle == 101510150 || npc.HasBuff(ModContent.BuffType<ImproperRestoration>()) || echoesThreeFreezeTimer != 0)
                    resetCooldown = 4;
                if (npc.aiStyle != 101510150 && !npc.HasBuff(ModContent.BuffType<ImproperRestoration>()) && echoesThreeFreezeTimer == 0) //reset some effect with npc ai and tile collision
                {
                    if (resetCooldown > 0)
                        resetCooldown--;
                    if (resetCooldown == 0)
                        savedTileCollide = npc.noTileCollide;
                    if (resetEffects)
                    {
                        resetEffects = false;
                        onlyOnce = false;
                        npc.noTileCollide = savedTileCollide;
                        fallDamageStart = (int)Vector2.Zero.Y;
                        fallDamage = 0;
                    }
                }

                if (towerOfGrayImmunityFrames > 0)
                    towerOfGrayImmunityFrames--;

                if (echoesKaboom) //echoes act 2 stuff
                {
                    if (npc.velocity.Y < 0)
                        echoesFallingPoint = npc.Bottom;
                    fallDamage = (int)npc.Center.Y - (int)echoesFallingPoint.Y;
                    if (npc.collideY && fallDamage > 1)
                    {
                        echoesKaboom = false;
                        npc.StrikeNPC(((int)Main.rand.NextFloat((int)(fallDamage * 0.85f), (int)(fallDamage * 1.15f)) + npc.defense / 4), 0f, 0, true, true, true);
                    }
                }

                if (echoesThreeFreezeTimer > 0)       //echoes act 3 stuff
                {
                    echoesThreeFreezeTimer--;
                    if (echoesThreeFreezeDamageTimer > 0)
                        echoesThreeFreezeDamageTimer--;

                    bool echoesThreeFreezeCrit = Main.rand.NextFloat(1, 100 + 1) <= echoesCrit;
                    int defence = echoesThreeFreezeCrit ? 4 : 2;
                    if (fallDamageStart == 0)
                        fallDamageStart = (int)npc.Center.Y;
                    fallDamage = (int)npc.Center.Y - fallDamageStart;
                    if (!onlyOnce && npc.collideY && fallDamage > 50)
                    {
                        onlyOnce = true;
                        npc.StrikeNPC((int)Main.rand.NextFloat((int)(fallDamage * 0.85f), (int)(fallDamage * 1.15f) + npc.defense / defence), 0f, 0, echoesThreeFreezeCrit);
                    }
                    if (echoesThreeFreezeDamageTimer <= 0 && npc.collideY)
                    {
                        echoesThreeFreezeDamageTimer = 30;
                        int freezeDamage = (int)(136 * echoesDamageBoost);
                        npc.StrikeNPC((int)Main.rand.NextFloat((int)(freezeDamage * 0.85f), (int)(freezeDamage * 1.15f)) + npc.defense / defence, 0f, 0, echoesThreeFreezeCrit);
                    }

                    if (npc.boss)
                        npc.velocity.X *= 0.66f;
                    else
                        npc.velocity.X *= 0.1f;
                    npc.noTileCollide = false;
                    npc.velocity = new Vector2(npc.velocity.X, 32f);
                    npc.velocity.Normalize();
                    if (!npc.noTileCollide)
                        npc.velocity.Y *= 12f;
                    if (echoesThreeFreezeTimer <= 2)
                    {
                        resetEffects = true;
                        echoesThreeFreezeTimer = 0;
                        echoesThreeFreezeDamageTimer = 30;
                    }
                }

                if (npc.HasBuff(ModContent.BuffType<ImproperRestoration>()))       //crazy diamond stuff
                {
                    crazyDiamondPunchCount = 0;
                    if (npc.aiStyle != -101510150)
                    {
                        CDsavedAIstyle = npc.aiStyle;
                        CDsavedHitSound = npc.HitSound;
                        CDsavedKnockbackRes = npc.knockBackResist;
                        npc.aiStyle = -101510150;
                        npc.HitSound = SoundID.NPCHit41;
                        npc.knockBackResist = 100f;
                        npc.noTileCollide = false;
                    }
                    if (fallDamageStart == (int)Vector2.Zero.Y)
                        fallDamageStart = (int)npc.Center.Y;
                    fallDamage = (int)npc.Center.Y - fallDamageStart;
                    if (!onlyOnce && npc.collideY && fallDamage > 200)
                    {
                        onlyOnce = true;
                        npc.StrikeNPCNoInteraction((fallDamage - 200 + npc.defense / 4) * 2, 0f, 0, true, true, true);
                    }
                }
                else
                {
                    if (npc.aiStyle == -101510150)
                    {
                        npc.aiStyle = CDsavedAIstyle;
                        npc.HitSound = CDsavedHitSound;
                        npc.knockBackResist = CDsavedKnockbackRes;
                        resetEffects = true;
                    }
                }
            }

            if (zombieHightlightTimer > 0)
                zombieHightlightTimer--;

            if (mPlayer.timestopActive || frozenInTime)
            {
                if (npc.velocity != Vector2.Zero)
                {
                    preTimestopVelocity = npc.velocity;
                }
                npc.velocity = Vector2.Zero;
                npc.frameCounter = 1;
                if (!npc.noGravity)
                {
                    npc.velocity.Y -= 0.3f;     //the default gravity value, so that if enemies have gravity enabled, this velocity counters that gravity
                }
                npc.netUpdate = true;
                return false;
            }
            if (mPlayer.backToZeroActive)
            {
                if (!affectedbyBtz)
                {
                    btZSaveTimer--;
                    if (btZSaveTimer <= 0)
                    {
                        if (btzPositionIndex < BtZPositions.Length)
                        {
                            btzPositionIndex += 1;
                            BtZPositions[btzPositionIndex] = npc.position;
                            btZSaveTimer = 30;
                        }
                    }
                }
                if (affectedbyBtz)
                {
                    if (btzTotalRewindTimer > 0)
                        btzTotalRewindTimer--;

                    btZSaveTimer--;
                    if (btZSaveTimer <= 0)
                    {
                        if (btzPositionIndex > 1)
                        {
                            btzPositionIndex -= 1;
                            npc.position = BtZPositions[btzPositionIndex];
                            if (npc.position == BtZPositions[btzPositionIndex])
                                Array.Clear(BtZPositions, btzPositionIndex, 1);

                            btZSaveTimer = 5;
                        }
                        npc.netUpdate = true;
                    }
                    npc.velocity = Vector2.Zero;
                    npc.AddBuff(BuffID.Confused, 600);
                    return false;
                }
            }
            else
            {
                if (npc.HasBuff(ModContent.BuffType<AffectedByBtZ>()))
                {
                    int buffIndex = npc.FindBuffIndex(ModContent.BuffType<AffectedByBtZ>());
                    npc.DelBuff(buffIndex);
                }
                btZSaveTimer = 0;
                btzPositionIndex = 0;
                btzTotalRewindTime = 0;
                btzTotalRewindTimer = 0;
                affectedbyBtz = false;
            }
            if (!mPlayer.timeskipActive)
            {
                timeskipAIStyle = 0;
                playerPositionOnSkip = Vector2.Zero;
            }
            if (mPlayer.timeskipActive && !npc.townNPC && !npc.friendly && !npc.boss && !npc.immortal)
            {
                if (playerPositionOnSkip == Vector2.Zero)
                {
                    int earliestTime = 10 * 60;
                    int chosenPlayerIndex = 0;
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        playerPositionOnSkip = Main.player[Main.myPlayer].position;
                    }
                    else
                    {
                        for (int p = 0; p < Main.maxPlayers; p++)
                        {
                            Player otherPlayer = Main.player[p];
                            if (otherPlayer.HasBuff<SkippingTime>() && otherPlayer.buffTime[otherPlayer.FindBuffIndex(ModContent.BuffType<SkippingTime>())] <= earliestTime)
                            {
                                earliestTime = otherPlayer.buffTime[otherPlayer.FindBuffIndex(ModContent.BuffType<SkippingTime>())];
                                chosenPlayerIndex = p;
                            }
                        }
                    }
                    playerPositionOnSkip = Main.player[chosenPlayerIndex].position;
                }
                if (timeskipAIStyle == 0 && npc.aiStyle != 0)
                {
                    timeskipAIStyle = npc.aiStyle;
                    npc.aiStyle = 0;
                }
                if (npc.aiStyle == 0)
                {
                    npc.velocity /= 2;
                    npc.spriteDirection = -npc.direction;
                    if (npc.noGravity)
                    {
                        Vector2 velocity = npc.Center - playerPositionOnSkip;
                        velocity.Normalize();
                        npc.velocity = velocity;
                        npc.direction = 1;
                        if (velocity.X < 0f)
                            npc.direction = -1;
                    }
                    else
                    {
                        if (playerPositionOnSkip.X > npc.position.X)
                        {
                            npc.velocity.X = 1f;
                            npc.direction = 1;
                        }
                        if (playerPositionOnSkip.X < npc.position.X)
                        {
                            npc.velocity.X = -1f;
                            npc.direction = -1;
                        }
                        int tilesInFront = (int)((npc.Center.X + (float)(15 * npc.direction)) / 16f);
                        int tilesUnder = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                        if (WorldGen.SolidTile((int)(npc.Center.X / 16f) + npc.direction, (int)(npc.Center.Y / 16f)) && !Collision.SolidTilesVersatile(tilesInFront - npc.direction * 2, tilesInFront - npc.direction, tilesUnder - 5, tilesUnder - 1) && !Collision.SolidTiles(tilesInFront, tilesInFront, tilesUnder - 5, tilesUnder - 3) && npc.ai[1] == 0f)
                        {
                            npc.velocity.Y = -6f;
                            npc.netUpdate = true;
                        }
                    }
                }
                return false;
            }
            if (mPlayer.timeskipActive && npc.boss)
            {
                npc.defense /= 2;
            }
            if (!mPlayer.timeskipActive && npc.aiStyle == 0)
            {
                playerPositionOnSkip = Vector2.Zero;
                npc.aiStyle = timeskipAIStyle;
                timeskipAIStyle = 0;
            }
            if (mPlayer.epitaphForesightActive && !npc.immortal)
            {
                applyingForesightPositions = true;
                if (foresightSaveTimer > 0)
                    foresightSaveTimer--;

                if (foresightSaveTimer <= 0)
                {
                    foresightData[foresightPositionIndex] = new ForesightSaveData();
                    foresightData[foresightPositionIndex].position = npc.position;
                    foresightData[foresightPositionIndex].frame = npc.frame;
                    foresightData[foresightPositionIndex].rotation = npc.rotation;
                    foresightData[foresightPositionIndex].direction = npc.direction;
                    foresightPositionIndex++;       //second so that something saves in [0] and goes up from there
                    foresightPositionIndexMax++;
                    foresightSaveTimer = 5;
                    if (foresightPositionIndex >= foresightData.Length)
                    {
                        foresightPositionIndex--;
                        foresightPositionIndexMax--;
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
                npc.velocity = Vector2.Zero;
                npc.position = foresightData[foresightPositionIndex].position;
                npc.frame = foresightData[foresightPositionIndex].frame;
                npc.rotation = foresightData[foresightPositionIndex].rotation;
                npc.direction = (int)foresightData[foresightPositionIndex].direction;
                if (foresightSaveTimer > 0)
                    foresightSaveTimer--;

                if (foresightSaveTimer <= 0)
                {
                    if (foresightData[foresightPositionIndex].position != Vector2.Zero)
                        foresightData[foresightPositionIndex].position = Vector2.Zero;
                    if (foresightData[foresightPositionIndex].rotation != 0f)
                        foresightData[foresightPositionIndex].rotation = 0f;
                    if (foresightData[foresightPositionIndex].direction != 0)
                        foresightData[foresightPositionIndex].direction = 0;
                    foresightPositionIndex++;
                    foresightSaveTimer = 5;
                }
                if (foresightPositionIndex >= foresightPositionIndexMax)
                {
                    applyingForesightPositions = false;
                    foresightPositionIndex = 0;
                    foresightPositionIndexMax = 0;
                    foresightResetIndex = false;
                }
                /*if (foresightPositionIndex >= 49)       //a failsafe to prevent Index Out of Bounds in extended multiplayer timeskips
                {
                    foresightPositionIndex = 0;
                    foresightPositionIndexMax = 0;
                }*/
                return false;
            }
            if (taggedByButterfly)
            {
                if (Main.rand.Next(0, 3 + 1) == 0)
                {
                    int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, DustID.IchorTorch, SpeedY: Main.rand.NextFloat(-1.1f, -0.6f + 1f), Scale: Main.rand.NextFloat(1.1f, 2.4f + 1f));
                    Main.dust[dustIndex].noGravity = true;
                }
            }
            if (spawnedByDeathLoop > 0)
            {
                deathTimer++;
                if ((deathTimer >= 30 && !npc.boss) || (deathTimer >= 60 && npc.boss))
                {
                    if (npc.immortal || npc.hide)
                        npc.StrikeNPCNoInteraction(999999999, 0f, 1, noEffect: true);
                    if (!npc.immortal)
                        npc.StrikeNPCNoInteraction(npc.lifeMax + 10, 0f, 1, noEffect: true);
                    deathTimer = 0;
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/GEDeathLoop"), npc.Center);
                }
            }

            if (npc.HasBuff(ModContent.BuffType<RedBindDebuff>()))
            {
                npc.velocity.X = 0f;
                if (npc.velocity.Y > 8f)
                    npc.velocity.Y += 0.3f;
                return false;
            }
            if (stunnedByBindingEmerald)
            {
                if (bindingEmeraldDurationTimer > 0)
                    bindingEmeraldDurationTimer--;
                else
                    stunnedByBindingEmerald = false;

                if (npc.boss)
                {
                    if (Vector2.Distance(npc.position, npc.position + npc.velocity) > 3f)
                        npc.velocity *= 0.94f;

                    return true;
                }

                npc.velocity.X = 0f;
                return false;
            }
            if (boundByStrings)
            {
                if (npc.boss)
                {
                    npc.velocity *= 0.6f;
                    return true;
                }

                npc.velocity.X = 0f;
                if (npc.velocity.Y > 8f)
                    npc.velocity.Y += 0.3f;
                return false;
            }
            if (npc.HasBuff(ModContent.BuffType<Stolen>()) && !npc.boss)
                return false;
            if (grabbedByHermitPurple)
                return false;
            return true;
        }

        public override void PostAI(NPC npc)
        {
            if (hitByCrossfireHurricane)
            {
                crossfireHurricaneEffectTimer--;
                if (crossfireHurricaneEffectTimer % 15 == 0)
                {
                    float angle = Main._rand.Next(0, 360);
                    Vector2 randomPosition = npc.Center + (angle.ToRotationVector2() * (npc.Size.Length() * 2f));
                    Vector2 velocity = npc.Center - randomPosition;
                    velocity.Normalize();
                    velocity *= 4f;

                    int projIndex = Projectile.NewProjectile(npc.GetSource_FromThis(), randomPosition, velocity, ModContent.ProjectileType<FireAnkh>(), 60 + (24 * (3 - Main.player[standDebuffEffectOwner].GetModPlayer<MyPlayer>().standTier)), 0f, standDebuffEffectOwner, 40, 5 * 60);
                    Main.projectile[projIndex].tileCollide = false;
                    Main.projectile[projIndex].netUpdate = true;
                    Main.projectile[projIndex].penetrate = -1;
                    Main.projectile[projIndex].timeLeft = (int)((npc.Size.Length() * 4f) / 4f);
                    Main.projectile[projIndex].scale = 0.6f;
                }
                if (crossfireHurricaneEffectTimer <= 0)
                    hitByCrossfireHurricane = false;
            }
        }

        public override void OnKill(NPC npc)
        {
            if (taggedForDeathLoop > 0 && spawnedByDeathLoop == 0)     
            {
                Player player = Main.player[deathLoopOwner];
                if (npc.boss && deathLoopOwner == player.whoAmI)
                {
                    int spawnedNPC = NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.position.X, (int)npc.position.Y, npc.type);
                    Main.npc[spawnedNPC].GetGlobalNPC<JoJoGlobalNPC>().spawnedByDeathLoop = 3;
                }
                if (!npc.boss && deathLoopOwner == player.whoAmI)
                {
                    int spawnedNPC = NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.position.X, (int)npc.position.Y, npc.type);
                    Main.npc[spawnedNPC].GetGlobalNPC<JoJoGlobalNPC>().spawnedByDeathLoop = 10;
                }
            }
            if (spawnedByDeathLoop > 1)
            {
                int spawnedNPC = NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.position.X, (int)npc.position.Y, npc.type);
                Main.npc[spawnedNPC].GetGlobalNPC<JoJoGlobalNPC>().spawnedByDeathLoop = npc.GetGlobalNPC<JoJoGlobalNPC>().spawnedByDeathLoop - 1;
            }
            if (vampireUserLastHitIndex != -1)
            {
                Player player = Main.player[vampireUserLastHitIndex];
                VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
                vPlayer.enemyTypesKilled[npc.type] += 1;
                if ((!npc.boss && vPlayer.enemyTypesKilled[npc.type] == 10) || (npc.boss && vPlayer.enemyTypesKilled[npc.type] == 0))
                {
                    vPlayer.vampireSkillPointsAvailable += 1;
                    vPlayer.totalVampireSkillPointsEarned += 1;
                    Main.NewText("You have obtained another Vampiric Skill Point!");
                    if (vPlayer.totalVampireSkillPointsEarned % 5 == 0)
                        vPlayer.vampiricLevel += 1;
                }
            }
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (player.epitaphForesightActive || applyingForesightPositions)
            {
                for (int i = 0; i < foresightPositionIndexMax; i++)
                {
                    SpriteEffects effects = SpriteEffects.None;
                    if (foresightData[i].direction == 1)
                        effects = SpriteEffects.FlipHorizontally;

                    float alpha = Math.Abs(foresightPositionIndex - i) / 10f;
                    alpha = Math.Clamp(alpha, 0f, 1f);
                    if (foresightData[i].position != Vector2.Zero)
                        spriteBatch.Draw(TextureAssets.Npc[npc.type].Value, foresightData[i].position - Main.screenPosition, foresightData[i].frame, Color.DarkRed * alpha, foresightData[i].rotation, Vector2.Zero, npc.scale, effects, 0f);
                }
            }
            if (player.backToZeroActive && btzTotalRewindTimer != 0)
            {
                for (int a = 0; a < BtZPositions.Length - 1; a++)
                {
                    float alpha = 1f / (Math.Abs((float)(btzTotalRewindTimer - (5 * a))) + 1f);
                    SpriteEffects spriteEffect = SpriteEffects.None;
                    if (npc.direction == 1)
                        spriteEffect = SpriteEffects.FlipHorizontally;

                    Vector2 position = BtZPositions[a] - Main.screenPosition;
                    spriteBatch.Draw(TextureAssets.Npc[npc.type].Value, position, npc.frame, Color.White * alpha, npc.rotation, Vector2.Zero, npc.scale, spriteEffect, 0f);
                }
            }

            if (taggedByKillerQueen)
            {
                Texture2D bombTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Bomb").Value;
                Vector2 position = npc.Center - new Vector2(bombTexture.Width / 2f, (npc.height / 2f) + 18f);
                spriteBatch.Draw(bombTexture, position - Main.screenPosition, Color.White);
            }

            if (npc.HasBuff(ModContent.BuffType<SMACK>()))
            {
                Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Extras/Echoes_SMACK").Value;
                Vector2 drawPosition = new Vector2(npc.Center.X, npc.Center.Y - npc.height) - Main.screenPosition;
                int randomRange = (echoesSmackDamageTimer / 5) / 4;
                drawPosition += new Vector2(Main.rand.Next(-randomRange, randomRange + 1), Main.rand.Next(-randomRange, randomRange + 1));

                spriteBatch.Draw(texture, drawPosition, null, Color.White, 0f, texture.Size() / 2f, 1f, SpriteEffects.None, 0);
            }

            if (npc.HasBuff(ModContent.BuffType<BelieveInMe>()))
            {
                Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Extras/Echoes_BelieveInMe").Value;
                Vector2 drawPosition = new Vector2(npc.Center.X, npc.Center.Y - npc.height / 2) - Main.screenPosition;

                spriteBatch.Draw(texture, drawPosition, null, Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
            }
            return true;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (sunTagged)
                drawColor = Color.Yellow;

            if (zombieHightlightTimer > 0)
                drawColor = Color.Orange;

            if (npc.HasBuff(ModContent.BuffType<Lacerated>()) && Main.player[Main.myPlayer].GetModPlayer<VampirePlayer>().anyMaskForm)
                drawColor = Color.OrangeRed;

            if (taggedForDeathLoop > 0)
                drawColor = Color.Purple;

            if (echoesFreezeTarget)
            {
                if (echoesThreeFreezeTimer == 0)
                    echoesFreezeTarget = false;
                drawColor = Color.Lerp(Color.White, Color.LightGreen, MathHelper.Clamp(echoesThreeFreezeTimer / 15f, 0f, 1f));
            }

            if (highlightedByTheHandMarker)
            {
                highlightedByTheHandMarker = false;
                drawColor = Color.LightBlue;
            }

            if (targetedBySoftAndWet)
                drawColor = Color.Cyan;

            if (npc.HasBuff(ModContent.BuffType<ImproperRestoration>()))
                drawColor = Color.Gray;
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.HasBuff(ModContent.BuffType<RedBindDebuff>()))
            {
                float newHeight = npc.height / 18;
                float newWidht = npc.width / 16;
                float scale = (newHeight + newWidht) / 2;
                Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Extras/BoundByRedBind").Value;
                spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, Color.White, npc.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, 0);
            }
            if (stunnedByBindingEmerald)
            {
                Texture2D emeraldStringWebTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/EmeraldStringWeb").Value;
                int textureWidth = 19;
                int textureHeight = 19;

                Vector2 position = npc.Center - Main.screenPosition;
                Vector2 origin = new Vector2(textureWidth / 2f, textureHeight / 2f);
                float scale = npc.width;
                if (npc.height > npc.width)
                    scale = npc.height;

                scale -= 4f;
                scale /= (float)textureWidth;
                scale += (float)Math.Abs(Math.Sin(bindingEmeraldDurationTimer / 100f)) * 0.5f;
                spriteBatch.Draw(emeraldStringWebTexture, position, null, drawColor, 0f, origin, scale, SpriteEffects.None, 0);
            }
            if (boundByStrings)
            {
                Texture2D bombTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/BoundByStrings").Value;
                Vector2 scale = new Vector2(npc.width, npc.height) / new Vector2(22, 14);
                Vector2 origin = new Vector2(11, 7);
                spriteBatch.Draw(bombTexture, npc.Center - Main.screenPosition, null, npc.color, npc.rotation, origin, scale, SpriteEffects.None, 0);
            }

            if (npc.HasBuff(ModContent.BuffType<Locked>()))
            {
                SpriteEffects effects = SpriteEffects.None;
                if (npc.spriteDirection == -1)
                    effects = SpriteEffects.FlipHorizontally;

                if (lockFrame < 3)
                {
                    lockFrameCounter++;
                    if (lockFrameCounter >= 5)
                    {
                        lockFrameCounter = 0;
                        lockFrame++;
                    }
                }

                float newHeight = npc.height / 22;
                float newWidth = npc.width / 22;
                float scale = (newHeight * 0.75f) + (newWidth * 0.25f);
                Texture2D theLockOverlay = ModContent.Request<Texture2D>("JoJoStands/Extras/TheLock_Overlay").Value;
                Rectangle animRect = new Rectangle(0, lockFrame * 22, 22, 22);
                spriteBatch.Draw(theLockOverlay, npc.Center - Main.screenPosition, animRect, drawColor, npc.rotation, new Vector2(11, 11), scale, effects, 0);
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            MyPlayer mPlayer = target.GetModPlayer<MyPlayer>();
            if (target.HasBuff(ModContent.BuffType<BacktoZero>()))     //only affects the ones with the buff, everyone's bool should turn on and save positions normally
            {
                npc.AddBuff(ModContent.BuffType<AffectedByBtZ>(), 2);
                npc.StrikeNPC(damage, npc.knockBackResist, -npc.direction);
                btzTotalRewindTime = 5 * btzPositionIndex;
                btzTotalRewindTimer = 5 * btzPositionIndex;
            }
            if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<DollyDaggerT1>())
            {
                npc.StrikeNPC((int)(npc.damage * 0.35f), 4f, -npc.direction);       //Dolly dagger is reflecting 35% of damage here, 70% in tier 2
            }
            if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<DollyDaggerT2>())
            {
                npc.StrikeNPC((int)(npc.damage * 0.7f), 6f, -npc.direction);
            }
            if (target.GetModPlayer<VampirePlayer>().vampire)
            {
                npc.AddBuff(BuffID.Frostburn, 240);
            }
            if (target.HasBuff(ModContent.BuffType<LockActiveBuff>()))
            {
                theLockCrit = mPlayer.standCritChangeBoosts;
                theLockDamageBoost = mPlayer.standDamageBoosts;
                if (npc.boss)
                {
                    lockRegenCounter += 4;
                    npc.AddBuff(ModContent.BuffType<Locked>(), (3 * mPlayer.standTier) * 60);
                }
                else
                {
                    lockRegenCounter += 8;
                    npc.AddBuff(ModContent.BuffType<Locked>(), (5 * mPlayer.standTier) * 60);
                }
            }
            if (removeZombieHighlightOnHit)
                zombieHightlightTimer = 0;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (forceDeath)
                npc.lifeRegen = -4;

            if (crazyDiamondFullHealth)
            {
                int restoration = npc.lifeMax - npc.life;
                npc.life += restoration;
                npc.HealEffect(restoration, true);
                crazyDiamondFullHealth = false;
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (JoJoStandsWorld.VampiricNight)
            {
                pool.Clear();
                pool.Add(NPCID.Zombie, SpawnCondition.OverworldNightMonster.Chance);
                pool.Add(ModContent.NPCType<GladiatorZombie>(), 0.09f);
                pool.Add(ModContent.NPCType<BaldZombie>(), 0.09f);
                pool.Add(ModContent.NPCType<ChimeraBird>(), 0.09f);
                pool.Add(ModContent.NPCType<Doobie>(), 0.02f);
                pool.Add(ModContent.NPCType<WangChan>(), 0.02f);
                pool.Add(ModContent.NPCType<JackTheRipper>(), 0.02f);
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (npc.HasBuff(ModContent.BuffType<ImproperRestoration>()))
                damage = (int)(damage * 0.1f);
            if (npc.HasBuff(ModContent.BuffType<BelieveInMe>()))
                damage = (int)(damage * 0.5f);
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (npc.HasBuff(ModContent.BuffType<ImproperRestoration>()))
                damage = (int)(damage * 0.1f);
            if (npc.HasBuff(ModContent.BuffType<BelieveInMe>()))
                damage = (int)(damage * 0.5f);
        }
    }
}