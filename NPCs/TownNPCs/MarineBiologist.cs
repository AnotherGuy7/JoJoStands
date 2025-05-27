using Humanizer;
using JoJoStands.Items;
using JoJoStands.Items.Hamon;
using JoJoStands.Items.Vanities;
using JoJoStands.Projectiles.NPCStands;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.TownNPCs
{
    [AutoloadHead]
    public class MarineBiologist : ModNPC
    {
        public static bool UserIsAlive = false;
        public static int standDamage = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 50; //this defines the NPC danger detect range
            NPCID.Sets.AttackType[NPC.type] = 1; //this is the attack type,  0 (throwing), 1 (shooting), or 2 (magic). 3 (melee) 
            NPCID.Sets.HatOffsetY[NPC.type] = 4; //this defines the party hat position
            NPC.Happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Love);
            NPC.Happiness.SetBiomeAffection<ForestBiome>(AffectionLevel.Like);
            NPC.Happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Hate);
            NPC.Happiness.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Dislike);
            NPC.Happiness.SetNPCAffection<Priest>(AffectionLevel.Hate);
            NPC.Happiness.SetNPCAffection<Gambler>(AffectionLevel.Hate);
            NPC.Happiness.SetNPCAffection(NPCID.Angler, AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Dislike);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18; //the NPC sprite width
            NPC.height = 46;  //the NPC sprite height
            NPC.aiStyle = 7; //this is the NPC ai style, 7 is Pasive Ai
            NPC.defense = 34;
            NPC.lifeMax = 500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 1f;
            AnimationType = NPCID.Guide;  //this copy the guide animation
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement(Language.GetText("Mods.JoJoStands.NPCFlavorText.MarineBiologist").Value)
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            return true;
        }

        public override List<string> SetNPCNameList() => new List<string> { "Jotaro" };

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetText("Mods.JoJoStands.NPCButtonText.StandHelp").Value;
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            Player player = Main.LocalPlayer;
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (firstButton)
            {
                if (mPlayer.awaitingViralMeteoriteTip)
                {
                    mPlayer.awaitingViralMeteoriteTip = false;
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.ViralMeteoriteTip").Value;
                    return;
                }

                int standSlotItemType = mPlayer.StandSlot.SlotItem.type;
                if (player.HeldItem.type == ModContent.ItemType<Hamon>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.Hamon").Value;

                else if (standSlotItemType == ModContent.ItemType<StarPlatinumT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StarPlatinumT1").Value;
                else if (standSlotItemType == ModContent.ItemType<StarPlatinumT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StarPlatinumT2").Value;
                else if (standSlotItemType == ModContent.ItemType<StarPlatinumT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StarPlatinumT3").Value;
                else if (standSlotItemType == ModContent.ItemType<StarPlatinumFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StarPlatinumFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<HierophantGreenT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.HierophantGreenT1").Value;
                else if (standSlotItemType == ModContent.ItemType<HierophantGreenT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.HierophantGreenT2").Value;
                else if (standSlotItemType == ModContent.ItemType<HierophantGreenT3>() || standSlotItemType == ModContent.ItemType<HierophantGreenFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.HierophantGreenT3Final").Value;

                else if (standSlotItemType == ModContent.ItemType<TheWorldT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TheWorldT1").Value;
                else if (standSlotItemType == ModContent.ItemType<TheWorldT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TheWorldT2").Value;
                else if (standSlotItemType == ModContent.ItemType<TheWorldT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TheWorldT3").Value;
                else if (standSlotItemType == ModContent.ItemType<TheWorldFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TheWorldFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<KillerQueenT1>() || standSlotItemType == ModContent.ItemType<KillerQueenT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.KillerQueenT1T2").Value;
                else if (standSlotItemType == ModContent.ItemType<KillerQueenT3>() || standSlotItemType == ModContent.ItemType<KillerQueenFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.KillerQueenT3Final").Value;
                else if (standSlotItemType == ModContent.ItemType<KillerQueenBTD>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.KillerQueenBTD").Value;

                else if (standSlotItemType == ModContent.ItemType<AchtungBaby>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.").Value;

                else if (standSlotItemType == ModContent.ItemType<GoldExperienceT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.GoldExperienceT1").Value;
                else if (standSlotItemType == ModContent.ItemType<GoldExperienceT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.GoldExperienceT2").Value;
                else if (standSlotItemType == ModContent.ItemType<GoldExperienceT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.GoldExperienceT3").Value;
                else if (standSlotItemType == ModContent.ItemType<GoldExperienceFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.GoldExperienceFinal").Value;
                else if (standSlotItemType == ModContent.ItemType<GoldExperienceRequiem>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.GoldExperienceRequiem").Value;

                else if (standSlotItemType == ModContent.ItemType<TuskAct1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TuskAct1").Value.FormatWith(Main.LocalPlayer.name);
                else if (standSlotItemType == ModContent.ItemType<TuskAct2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TuskAct2").Value;
                else if (standSlotItemType == ModContent.ItemType<TuskAct3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TuskAct3").Value;
                else if (standSlotItemType == ModContent.ItemType<TuskAct4>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TuskAct4").Value;

                else if (standSlotItemType == ModContent.ItemType<StickyFingersT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StickyFingersT1").Value;
                else if (standSlotItemType == ModContent.ItemType<StickyFingersT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StickyFingersT2").Value;
                else if (standSlotItemType == ModContent.ItemType<StickyFingersT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StickyFingersT3").Value;
                else if (standSlotItemType == ModContent.ItemType<StickyFingersFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StickyFingersFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<SexPistolsT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.SexPistolsT1").Value;
                else if (standSlotItemType == ModContent.ItemType<SexPistolsT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.SexPistolsT2").Value;
                else if (standSlotItemType == ModContent.ItemType<SexPistolsT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.SexPistolsT3").Value;
                else if (standSlotItemType == ModContent.ItemType<SexPistolsFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.SexPistolsFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<KingCrimsonT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.KingCrimsonT1").Value;
                else if (standSlotItemType == ModContent.ItemType<KingCrimsonT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.KingCrimsonT2").Value;
                else if ((standSlotItemType == ModContent.ItemType<KingCrimsonT3>() || standSlotItemType == ModContent.ItemType<KingCrimsonFinal>()))
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.KingCrimsonT3Final").Value;

                else if (standSlotItemType == ModContent.ItemType<CenturyBoyT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.CenturyBoyT1").Value;
                else if (standSlotItemType == ModContent.ItemType<CenturyBoyT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.CenturyBoyT2").Value;

                else if (player.HeldItem.type == ModContent.ItemType<DollyDaggerT1>() || standSlotItemType == ModContent.ItemType<DollyDaggerT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.DollyDaggerT1").Value;

                else if (player.HeldItem.type == ModContent.ItemType<DollyDaggerT2>() || standSlotItemType == ModContent.ItemType<DollyDaggerT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.DollyDaggerT2").Value;

                else if (standSlotItemType == ModContent.ItemType<MagiciansRedT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.MagiciansRedT1").Value;
                else if (standSlotItemType == ModContent.ItemType<MagiciansRedT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.MagiciansRedT2").Value;
                else if ((standSlotItemType == ModContent.ItemType<MagiciansRedT3>() || standSlotItemType == ModContent.ItemType<MagiciansRedFinal>()))
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.MagiciansRedT3Final").Value;

                else if (standSlotItemType == ModContent.ItemType<AerosmithT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.AerosmithT1").Value;
                else if (standSlotItemType == ModContent.ItemType<AerosmithT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.AerosmithT2").Value;
                else if (standSlotItemType == ModContent.ItemType<AerosmithT3>() || standSlotItemType == ModContent.ItemType<AerosmithFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.AerosmithT3Final").Value;

                else if (standSlotItemType == ModContent.ItemType<TheHandT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TheHandT1").Value;
                else if (standSlotItemType == ModContent.ItemType<TheHandT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TheHandT2").Value;
                else if (standSlotItemType == ModContent.ItemType<TheHandT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TheHandT3").Value;
                else if (standSlotItemType == ModContent.ItemType<TheHandFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TheHandFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<GratefulDeadT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.GratefulDeadT1").Value;
                else if (standSlotItemType == ModContent.ItemType<GratefulDeadT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.GratefulDeadT2").Value;
                else if (standSlotItemType == ModContent.ItemType<GratefulDeadT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.GratefulDeadT3").Value;
                else if (standSlotItemType == ModContent.ItemType<GratefulDeadFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.GratefulDeadFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<LockT1>() || standSlotItemType == ModContent.ItemType<LockT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.LockT1T2").Value;
                else if (standSlotItemType == ModContent.ItemType<LockT3>() || standSlotItemType == ModContent.ItemType<LockFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.LockT3Final").Value;

                else if (standSlotItemType == ModContent.ItemType<WhitesnakeT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.WhitesnakeT1").Value;
                else if (standSlotItemType == ModContent.ItemType<WhitesnakeT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.WhitesnakeT2").Value;
                else if (standSlotItemType == ModContent.ItemType<WhitesnakeT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.WhitesnakeT3").Value;
                else if (standSlotItemType == ModContent.ItemType<WhitesnakeFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.WhitesnakeFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<SilverChariotT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.SilverChariotT1").Value;
                else if (standSlotItemType == ModContent.ItemType<SilverChariotT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.SilverChariotT2").Value;
                else if (standSlotItemType == ModContent.ItemType<SilverChariotT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.SilverChariotT3").Value;
                else if (standSlotItemType == ModContent.ItemType<SilverChariotFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.SilverChariotFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<CreamT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.CreamT1").Value;
                else if (standSlotItemType == ModContent.ItemType<CreamT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.CreamT2").Value;
                else if (standSlotItemType == ModContent.ItemType<CreamT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.CreamT3").Value;
                else if (standSlotItemType == ModContent.ItemType<CreamFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.CreamFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<HermitPurpleT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.HermitPurpleT1").Value;
                else if (standSlotItemType == ModContent.ItemType<HermitPurpleT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.HermitPurpleT2").Value;
                else if (standSlotItemType == ModContent.ItemType<HermitPurpleT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.HermitPurpleT3").Value;
                else if (standSlotItemType == ModContent.ItemType<HermitPurpleFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.HermitPurpleFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<BadCompanyT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.BadCompanyT1").Value;
                else if (standSlotItemType == ModContent.ItemType<BadCompanyT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.BadCompanyT2").Value;
                else if (standSlotItemType == ModContent.ItemType<BadCompanyT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.BadCompanyT3").Value;
                else if (standSlotItemType == ModContent.ItemType<BadCompanyFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.BadCompanyFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<StoneFreeT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StoneFreeT1").Value;
                else if (standSlotItemType == ModContent.ItemType<StoneFreeT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StoneFreeT2").Value;
                else if (standSlotItemType == ModContent.ItemType<StoneFreeT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StoneFreeT3").Value;
                else if (standSlotItemType == ModContent.ItemType<StoneFreeFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StoneFreeFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<SoftAndWetT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.SoftAndWetT1").Value;
                else if (standSlotItemType == ModContent.ItemType<SoftAndWetT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.SoftAndWetT2").Value;
                else if (standSlotItemType == ModContent.ItemType<SoftAndWetT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.SoftAndWetT3").Value;
                else if (standSlotItemType == ModContent.ItemType<SoftAndWetFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.SoftAndWetFinal").Value;
                else if (standSlotItemType == ModContent.ItemType<SoftAndWetGoBeyond>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.SoftAndWetGoBeyond").Value;

                else if (standSlotItemType == ModContent.ItemType<CrazyDiamondT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.CrazyDiamondT1").Value;
                else if (standSlotItemType == ModContent.ItemType<CrazyDiamondT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.CrazyDiamondT2").Value;
                else if (standSlotItemType == ModContent.ItemType<CrazyDiamondT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.CrazyDiamondT3").Value;
                else if (standSlotItemType == ModContent.ItemType<CrazyDiamondFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.CrazyDiamondFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<TowerOfGrayT1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TowerOfGrayT1").Value;
                else if (standSlotItemType == ModContent.ItemType<TowerOfGrayT2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TowerOfGrayT2").Value;
                else if (standSlotItemType == ModContent.ItemType<TowerOfGrayT3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TowerOfGrayT3").Value;
                else if (standSlotItemType == ModContent.ItemType<TowerOfGrayFinal>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.TowerOfGrayFinal").Value;

                else if (standSlotItemType == ModContent.ItemType<EchoesAct0>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.EchoesAct0").Value;
                else if (standSlotItemType == ModContent.ItemType<EchoesAct1>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.EchoesAct1").Value;
                else if (standSlotItemType == ModContent.ItemType<EchoesAct2>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.EchoesAct2").Value;
                else if (standSlotItemType == ModContent.ItemType<EchoesAct3>())
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.EchoesAct3").Value;
                else if (JoJoStands.FanStandsLoaded)
                {
                    string suffix = mPlayer.standTier == 4 ? "Final" : ("T" + mPlayer.standTier);      //Needs support for T5
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue." + mPlayer.standName + suffix).Value;
                }

                if (mPlayer.StandSlot.SlotItem.IsAir)
                {
                    int helpText = Main.rand.Next(0, 6 + 1);
                    switch (helpText)
                    {
                        case 0:
                            Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StandHelpText0").Value;
                            break;
                        case 1:
                            Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StandHelpText1").Value;
                            break;
                        case 2:
                            Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StandHelpText2").Value;
                            break;
                        case 3:
                            Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StandHelpText3").Value;
                            break;
                        case 4:
                            Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StandHelpText4").Value;
                            break;
                        case 5:
                            Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StandHelpText5").Value;
                            break;
                        case 6:
                            Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.StandHelpText6").Value;
                            break;
                    }

                    if (!mPlayer.receivedArrowShard)
                    {
                        Main.npcChatText = Language.GetText("Mods.JoJoStands.JotaroCustomDialogue.ArrowShard").Value;
                        player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<ArrowShard>());
                        mPlayer.receivedArrowShard = true;
                    }
                }
            }
        }

        public override string GetChat()       //Allows you to give this town NPC a chat message when a player talks to it.
        {
            switch (Main.rand.Next(4))    //this are the messages when you talk to the NPC, if you want to add more cases, you first need to change the Main.rand to the number of cases (default is included!)
            {
                case 0:
                    return Language.GetText("Mods.JoJoStands.NPCDialogue.MarineBiologist1").Value;
                case 1:
                    return Language.GetText("Mods.JoJoStands.NPCDialogue.MarineBiologist2").Value;
                case 2:
                    return Language.GetText("Mods.JoJoStands.NPCDialogue.MarineBiologist3").Value;
                case 3:
                    return Language.GetText("Mods.JoJoStands.NPCDialogue.MarineBiologist4").Value.FormatWith(Main.LocalPlayer.name);
                default:
                    return Language.GetText("Mods.JoJoStands.NPCDialogue.MarineBiologist5").Value;
            }
        }

        public override bool CheckActive()
        {
            UserIsAlive = true;
            return true;
        }

        public override bool CheckDead()
        {
            UserIsAlive = false;
            return true;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)//  Allows you to determine the damage and knockback of this town NPC attack
        {
            if (!Main.hardMode)
            {
                standDamage = 40;  //NPC damage
                knockback = 2f;   //NPC knockback
            }
            if (Main.hardMode)
            {
                standDamage = 62;
                knockback = 3f;
            }
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)  //Allows you to determine the cooldown between each of this town NPC's attack. The cooldown will be a number greater than or equal to the first parameter, and less then the sum of the two parameters.
        {
            cooldown = 5;
            randExtraCooldown = 10;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)//Allows you to determine the Projectile type of this town NPC's attack, and how long it takes for the Projectile to actually appear
        {
            if (!StarPlatinumNPCStand.SPActive)
            {
                projType = ModContent.ProjectileType<StarPlatinumNPCStand>();
                attackDelay = 1;
            }
        }
    }
}