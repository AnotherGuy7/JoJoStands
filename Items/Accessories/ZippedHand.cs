using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class ZippedHand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zipped Hand");
            Tooltip.SetDefault("Gives some time after death\nUndead has increased damage and critical hit chance");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 2);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().zippedHand = true;
        }
    }
}