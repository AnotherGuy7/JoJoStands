using Humanizer;
using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Items.Hamon;
using JoJoStands.Items.Vampire;
using JoJoStands.Projectiles;
using JoJoStands.UI;
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
    public class HamonMaster : ModNPC
    {
        public static bool punchesActive = false;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 48;
            NPCID.Sets.AttackType[NPC.type] = 1;
            NPCID.Sets.HatOffsetY[NPC.type] = 4;
            NPC.Happiness.SetBiomeAffection<ForestBiome>(AffectionLevel.Love);
            NPC.Happiness.SetBiomeAffection<DungeonBiome>(AffectionLevel.Hate);
            NPC.Happiness.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate);
            NPC.Happiness.SetBiomeAffection<SnowBiome>(AffectionLevel.Like);
            NPC.Happiness.SetBiomeAffection<HallowBiome>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection<Priest>(AffectionLevel.Hate);
            NPC.Happiness.SetNPCAffection<MarineBiologist>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.Dryad, AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.Nurse, AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Dislike);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 46;
            NPC.aiStyle = 7;
            NPC.defense = 29;
            NPC.lifeMax = 460;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 1f;
            AnimationType = NPCID.Guide;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement(Language.GetText("Mods.JoJoStands.NPCFlavorText.HamonMaster").Value)
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            return true;
        }

        public override List<string> SetNPCNameList() => new List<string> { "Zeppeli" };

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetText("Mods.JoJoStands.NPCButtonText.LearnSkills").Value;
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            Player player = Main.LocalPlayer;
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
            if (firstButton)
            {
                if (hPlayer.learnedHamon)
                {
                    HamonSkillTree.OpenHamonSkillTree();
                    return;
                }

                if (player.CountItem(ModContent.ItemType<SunDroplet>()) >= 5)
                {
                    for (int i = 0; i < 5; i++)
                        player.ConsumeItem(ModContent.ItemType<SunDroplet>());
                    hPlayer.learnedHamon = true;
                    hPlayer.skillPointsAvailable = 1;

                    if (Main.rand.Next(1, 3 + 1) != 1)
                        Main.npcChatText = Language.GetText("Mods.JoJoStands.NPCDialogue.HamonMasterTrainingStart").Value;
                    else
                    {
                        hPlayer.skillPointsAvailable = 3;
                        Main.npcChatText = Language.GetText("Mods.JoJoStands.NPCDialogue.HamonMasterTrainingStartSpecial").Value;
                    }
                    HamonBar.ShowHamonBar();
                }
                else
                {
                    Main.npcChatText = Language.GetText("Mods.JoJoStands.NPCDialogue.HamonMasterTrainPrompt").Value;
                }
            }
        }

        public override string GetChat()       //Allows you to give this town NPC a chat message when a player talks to it.
        {
            Player player = Main.LocalPlayer;
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (!vPlayer.vampire)
            {
                switch (Main.rand.Next(0, 3 + 1))
                {
                    case 0:
                        return Language.GetText("Mods.JoJoStands.NPCDialogue.HamonMaster1").Value.FormatWith(player.name);
                    case 1:
                        return Language.GetText("Mods.JoJoStands.NPCDialogue.HamonMaster2").Value.FormatWith(player.name);
                    case 2:
                        return Language.GetText("Mods.JoJoStands.NPCDialogue.HamonMaster3").Value;
                    case 3:
                        return Language.GetText("Mods.JoJoStands.NPCDialogue.HamonMaster4").Value;
                    case 4:
                        return Language.GetText("Mods.JoJoStands.NPCDialogue.HamonMaster5").Value;
                    default:
                        return Language.GetText("Mods.JoJoStands.NPCDialogue.HamonMaster5").Value;
                }
            }
            else
            {
                switch (Main.rand.Next(0, 1 + 1))
                {
                    case 0:
                        return Language.GetText("Mods.JoJoStands.NPCDialogue.HamonMasterEvil1").Value;
                    case 1:
                        return Language.GetText("Mods.JoJoStands.NPCDialogue.HamonMasterEvil2").Value;
                    default:
                        return Language.GetText("Mods.JoJoStands.NPCDialogue.HamonMasterEvil2").Value;
                }
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            if (!Main.hardMode)
            {
                damage = 39;
                knockback = 3f;
            }
            if (Main.hardMode)
            {
                damage = 64;
                knockback = 5f;
            }
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 2;
            randExtraCooldown = 0;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            if (!punchesActive)
            {
                projType = ModContent.ProjectileType<ZeppeliHamonPunches>();
                attackDelay = 1;
            }
        }
    }
}