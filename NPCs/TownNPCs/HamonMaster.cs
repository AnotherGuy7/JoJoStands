using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.TownNPCs
{
    [AutoloadHead]
    public class HamonMaster : ModNPC
    {
        public static bool punchesActive = false;

        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 46;
            npc.aiStyle = 7;
            npc.defense = 29;
            npc.lifeMax = 460;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 1f;
            Main.npcFrameCount[npc.type] = 25;
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 48;
            NPCID.Sets.AttackType[npc.type] = 1;
            NPCID.Sets.HatOffsetY[npc.type] = 4;
            animationType = NPCID.Guide;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return true;
        }

        public override string TownNPCName()
        {
            return "Zeppeli";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Learn Skills";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool openShop)
        {
            if (firstButton)
            {

            }
        }

        public override string GetChat()       //Allows you to give this town NPC a chat message when a player talks to it.
        {
            switch (Main.rand.Next(4))    //this are the messages when you talk to the npc, if you want to add more cases, you first need to change the Main.rand to the number of cases (default is included!)
            {
                case 0:
                    return "I am Zeppeli.";
                case 1:
                    return "What can I teach you today?";
                case 2:
                    return "3";
                case 3:
                    return "4";
                default:
                    return "0";
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
                projType = mod.ProjectileType("ZeppeliHamonPunches");
                attackDelay = 1;
            }
        }
    }
}