using JoJoStands.Items;
using JoJoStands.Items.Tiles;
using JoJoStands.Projectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.TownNPCs
{
    [AutoloadHead]
    public class Gambler : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 26;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 150;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 20;
            NPCID.Sets.AttackAverageChance[NPC.type] = 10;
            NPCID.Sets.HatOffsetY[NPC.type] = 2;
            NPC.Happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Love);
            NPC.Happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Hate);
            NPC.Happiness.SetBiomeAffection<DungeonBiome>(AffectionLevel.Dislike);
            NPC.Happiness.SetBiomeAffection<HallowBiome>(AffectionLevel.Dislike);
            NPC.Happiness.SetNPCAffection<Priest>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection<MarineBiologist>(AffectionLevel.Hate);
            NPC.Happiness.SetNPCAffection<HamonMaster>(AffectionLevel.Hate);
            NPC.Happiness.SetNPCAffection(579, AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike);
            NPC.Happiness.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Hate);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 46;
            NPC.aiStyle = 7;
            NPC.defense = 27;
            NPC.lifeMax = 300;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 1f;
            AnimationType = NPCID.Guide;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement("Came all the way from Egypt after losing a life or death bet. He repeatedly promises to never cheat again.")
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return Main.hardMode && numTownNPCs >= 5;
        }

        public override List<string> SetNPCNameList()
        {
            List<string> possibleNames = new List<string>();

            possibleNames.Add("D'Arby");

            return possibleNames;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Buy Items";
            button2 = "Bet";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool openShop)
        {
            if (firstButton)
            {
                openShop = true;
            }
            else
            {
                UI.BetUI.Visible = true;
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<PackoCards>());
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<PokerChip>());
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<TarotTable>());
            nextSlot++;
        }

        public override string GetChat()
        {
            switch (Main.rand.Next(4))
            {
                case 0:
                    return "Why don't you try to bet? C'mon, I'll even go easy on you!";
                case 1:
                    return "I should've played... How could I have been fooled by such an obvious bluff...";
                case 2:
                    return "I believe it was a gamblers instinct that led me to this goldmine of items.";
                case 3:
                    return "Cheating? It's only cheating if you're caught!";
                default:
                    return "I am D'Arby, the worlds greatest gambler!";
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 37;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 1;
        }
        public override void DrawTownAttackGun(ref float scale, ref int Item, ref int closeness)
        {
            scale = 1f;
            closeness = 15;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<CardProjectile>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 7f;
        }
    }
}