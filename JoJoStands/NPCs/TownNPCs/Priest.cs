using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.TownNPCs            //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    [AutoloadHead]
    public class Priest : ModNPC    //It's name is 'priest' so that when it dies, it says "Pucci the priest"
    {
        public static bool userIsAlive = false;

        public override void SetDefaults()
        {
            npc.townNPC = true;     //This defines if the npc is a town Npc or not
            npc.friendly = true;        //this defines if the npc can hur you or not()
            npc.width = 18;     //the npc sprite width
            npc.height = 46;        //the npc sprite height
            npc.aiStyle = 7;        //this is the npc ai style, 7 is Pasive Ai
            npc.defense = 27;       //the npc defense
            npc.lifeMax = 300;      // the npc life
            npc.HitSound = SoundID.NPCHit1;     //the npc sound when is hit
            npc.DeathSound = SoundID.NPCDeath1;     //the npc sound when he dies
            npc.knockBackResist = 1f;       //the npc knockback resistance
            Main.npcFrameCount[npc.type] = 26;      //this defines how many frames the npc sprite sheet has
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 250;       //this defines the npc danger detect range
            NPCID.Sets.AttackType[npc.type] = 1;        //this is the attack type,  0 (throwing), 1 (shooting), or 2 (magic). 3 (melee) 
            NPCID.Sets.HatOffsetY[npc.type] = 4;        //this defines the party hat position
            animationType = NPCID.Guide;        //this copy the guide animation
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

        public override string TownNPCName()        //Allows you to give this town NPC any name when it spawns
        {
            return "Pucci";
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

        public override void SetupShop(Chest shop, ref int nextSlot)        //Allows you to add items to this town NPC's shop. Add an item by setting the defaults of shop.item[nextSlot] then incrementing nextSlot.
        {
            for (int i = 0; i < JoJoStands.standTier1List.Count; i++)       //auto builds the list of items, also sets their price to 1 platinum without affecting original items
            {
                shop.item[i].SetDefaults(JoJoStands.standTier1List[i]);
                shop.item[i].value = Item.buyPrice(0, 50, 0, 0);
            }
        }

        public override string GetChat()       //Allows you to give this town NPC a chat message when a player talks to it.
        {
            switch (Main.rand.Next(6))    //this are the messages when you talk to the npc, if you want to add more cases, you first need to change the Main.rand to the number of cases (default is included!)
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
            damage = 40;        //npc damage
            knockback = 2f;     //npc knockback
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)     //Allows you to determine the cooldown between each of this town NPC's attack. The cooldown will be a number greater than or equal to the first parameter, and less then the sum of the two parameters.
        {
            cooldown = 5;
            randExtraCooldown = 10;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)       //Allows you to determine the projectile type of this town NPC's attack, and how long it takes for the projectile to actually appear
        {
            if (!Projectiles.NPCStands.WhitesnakeNPCStand.whitesnakeActive)
            {
                projType = mod.ProjectileType("WhitesnakeNPCStand");
                attackDelay = 1;
            }
        }
    }
}