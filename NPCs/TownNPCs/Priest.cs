using JoJoStands.Projectiles.NPCStands;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.TownNPCs            //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    [AutoloadHead]
    public class Priest : ModNPC    //It's name is 'priest' so that when it dies, it says "Pucci the priest"
    {
        public static bool userIsAlive = false;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 26;      //this defines how many frames the NPC sprite sheet has
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 250;       //this defines the NPC danger detect range
            NPCID.Sets.AttackType[NPC.type] = 1;        //this is the attack type,  0 (throwing), 1 (shooting), or 2 (magic). 3 (melee) 
            NPCID.Sets.HatOffsetY[NPC.type] = 1;        //this defines the party hat position
            NPC.Happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Love);
            NPC.Happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Like);
            NPC.Happiness.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate);
            NPC.Happiness.SetBiomeAffection<SnowBiome>(AffectionLevel.Hate);
            NPC.Happiness.SetBiomeAffection<HallowBiome>(AffectionLevel.Dislike);
            NPC.Happiness.SetNPCAffection<Gambler>(AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection<HamonMaster>(AffectionLevel.Dislike);
            NPC.Happiness.SetNPCAffection<MarineBiologist>(AffectionLevel.Hate);
            NPC.Happiness.SetNPCAffection(NPCID.Dryad, AffectionLevel.Hate);
            NPC.Happiness.SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;     //This defines if the NPC is a town Npc or not
            NPC.friendly = true;        //this defines if the NPC can hur you or not()
            NPC.width = 18;     //the NPC sprite width
            NPC.height = 46;        //the NPC sprite height
            NPC.aiStyle = 7;        //this is the NPC ai style, 7 is Pasive Ai
            NPC.defense = 27;       //the NPC defense
            NPC.lifeMax = 300;      // the NPC life
            NPC.HitSound = SoundID.NPCHit1;     //the NPC sound when is hit
            NPC.DeathSound = SoundID.NPCDeath1;     //the NPC sound when he dies
            NPC.knockBackResist = 1f;       //the NPC knockback resistance
            AnimationType = NPCID.Guide;        //this copy the guide animation
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement("A Priest visiting from Florida who came here after hearing of a certain someone's arrival. He says he's trying to achieve heaven... What does that even mean?")
            });
        }

        public override bool CheckActive()
        {
            userIsAlive = true;
            return true;
        }

        public override bool CheckDead()
        {
            userIsAlive = false;
            return true;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)        //Whether or not the conditions have been met for this town NPC to be able to move into town.
        {
            return Main.hardMode;
        }

        public override List<string> SetNPCNameList()
        {
            List<string> possibleNames = new List<string>();

            possibleNames.Add("Pucci");

            return possibleNames;
        }

        public override void SetChatButtons(ref string button, ref string button2)      //Allows you to set the text for the buttons that appear on this town NPC's chat window. 
        {
            button = "Buy Stands";      //this defines the buy button name
        }
        public override void OnChatButtonClicked(bool firstButton, ref bool openShop)       //Allows you to make something happen whenever a button is clicked on this town NPC's chat window. The firstButton parameter tells whether the first button or second button (button and button2 from SetChatButtons) was clicked. Set the shop parameter to true to open this NPC's shop.
        {
            if (firstButton)
                openShop = true;        //so when you click on buy button opens the shop
        }

        public override void SetupShop(Chest shop, ref int nextSlot)        //Allows you to add items to this town NPC's shop. Add an Item by setting the defaults of shop.Item[nextSlot] then incrementing nextSlot.
        {
            for (int i = 0; i < JoJoStands.standTier1List.Count; i++)       //auto builds the list of items, also sets their price to 1 platinum without affecting original items
            {
                shop.item[i].SetDefaults(JoJoStands.standTier1List[i]);
                shop.item[i].value = Item.buyPrice(0, 50, 0, 0);
            }
        }

        public override string GetChat()       //Allows you to give this town NPC a chat message when a player talks to it.
        {
            switch (Main.rand.Next(6))    //this are the messages when you talk to the NPC, if you want to add more cases, you first need to change the Main.rand to the number of cases (default is included!)
            {
                case 0:
                    return "I sell stand discs to people who want them... Want one?";
                case 1:
                    return "My White Snake is the only stand capable of defeating Star Platinum right now...";
                case 2:
                    return "That boy will pay for ruining my perfect universe!";
                case 3:
                    return "Weather Report and his stand... what a nuisance";
                case 4:
                    return "Jolyne will never see me coming after I obtain heaven...";
                case 5:
                    return "You seem like you could've been a fitting servant for DIO... If only you were in Egypt at the time.";
                default:
                    return "You know... I was able to get Star Platinum from Jotaro... Interested?";

            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)     //Allows you to determine the damage and knockback of this town NPC attack
        {
            damage = 40;        //NPC damage
            knockback = 2f;     //NPC knockback
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)     //Allows you to determine the cooldown between each of this town NPC's attack. The cooldown will be a number greater than or equal to the first parameter, and less then the sum of the two parameters.
        {
            cooldown = 5;
            randExtraCooldown = 10;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)       //Allows you to determine the Projectile type of this town NPC's attack, and how long it takes for the Projectile to actually appear
        {
            if (!Projectiles.NPCStands.WhitesnakeNPCStand.whitesnakeActive)
            {
                projType = ModContent.ProjectileType<WhitesnakeNPCStand>();
                attackDelay = 1;
            }
        }
    }
}