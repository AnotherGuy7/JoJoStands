using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.UI;

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
                //HamonSkillTree.Visible = true;
                HamonSkillTree.OpenHamonSkillTree();
            }
        }

        public override string GetChat()       //Allows you to give this town NPC a chat message when a player talks to it.
        {
            Player player = Main.LocalPlayer;
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (!mPlayer.vampire)
            {
                switch (Main.rand.Next(0, 5))
                {
                    case 0:
                        return "Excuse me, " + player.name + ", would you happen to have some wine, preferably in a bottle? I had some, but it spilled.";
                    case 1:
                        return "I hope you are looking out for vampires, " + player.name + ". I have devoted my entire life to hunting them down.";
                    case 2:
                        return "Welcome back, friend. Are you here to train in the ways of Hamon?";
                    case 3:
                        return "Master Tonpetty told me I would meet my end if I completed my training... will it be here, in this strange land?";
                    case 4:
                        return "When you have free time, could you come back? I made a salad, with copious amounts of pepper, of course, and nobody else wants it.";
                    default:        //When would this one even appear?
                        return "When you have free time, could you come back? I made a salad, with copious amounts of pepper, of course, and nobody else wants it.";
                }
            }
            else
            {
                switch (Main.rand.Next(0, 2))
                {
                    case 0:
                        return "Something has changed about you... you have used the power of the Stone Mask for yourself, have you not? Get out of my sight.";
                    case 1:
                        return "You are a vampire now, are you not? How many people have you killed?! What did you say? 'How many breads have I eaten?' You monster.";
                    default:
                        return "Something has changed about you... you have used the power of the Stone Mask for yourself, have you not? Get out of my sight.";
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
                projType = mod.ProjectileType("ZeppeliHamonPunches");
                attackDelay = 1;
            }
        }
    }
}