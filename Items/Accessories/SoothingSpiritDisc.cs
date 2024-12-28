using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class SoothingSpiritDisc : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Soothing Spirit Disc");
            // Tooltip.SetDefault("Stand Damage is increased by 2% for every enemy around you.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(gold: 4);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            MyPlayer myPlayer = player.GetModPlayer<MyPlayer>();
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active && npc.lifeMax > 5 && !npc.townNPC && !npc.friendly && !npc.SpawnedFromStatue && npc.Distance(player.Center) <= 48 * 16)
                    myPlayer.standDamageBoosts += 0.02f;
            }
        }
    }
}