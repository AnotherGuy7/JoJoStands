using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.TownNPCs
{
    [AutoloadHead]
    public class Gambler : ModNPC
    {
        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 46;
            npc.aiStyle = 7;
            npc.defense = 27;
            npc.lifeMax = 300;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 1f;
            Main.npcFrameCount[npc.type] = 26;
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 150;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 20;
            NPCID.Sets.AttackAverageChance[npc.type] = 10;
            NPCID.Sets.HatOffsetY[npc.type] = 2;
            animationType = NPCID.Guide;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return Main.hardMode && numTownNPCs >= 5;
        }

        public override string TownNPCName()
        {
            return "D'Arby";
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
            shop.item[nextSlot].SetDefaults(mod.ItemType("PackoCards"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("PokerChip"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("TarotTable"));
            nextSlot++;
        }

        public override string GetChat()
        {
            switch (Main.rand.Next(4))
            {
                case 0:
                    return "Why don't you try to bet? C'mon, i'll even go easy on you!";
                case 1:
                    return "I should've played... How could I have been fooled by such an obvious bluff...";
                case 2:
                    return "I believe it was a gamblers instinct that led me to this goldmine of items.";
                case 3:
                    return "Cheating? It's only cheating if you're caught!";
                default:
                    return "I am D'Arby, the worlds best gambler!";
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
        public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
        {
            scale = 1f;
            closeness = 15;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = mod.ProjectileType("Card");
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 7f;
        }
    }
}