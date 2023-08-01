using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class VampiricBangle : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Vampiric Bangle");
            /* Tooltip.SetDefault("Allows Stand Attacks to perform life steal." +
                "\n33% increased damage to user!"); */
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(gold: 4);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().vampiricBangleEquipped = true;
        }
    }
}