using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class TheFirstNapkin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The First Napkin");
            Tooltip.SetDefault("5% chance to reduce Ability Cooldown when attacking an enemy\nOnce every three minutes completely removes Ability Cooldown");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(gold: 8);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().theFirstNapkin = true;
        }
    }
}