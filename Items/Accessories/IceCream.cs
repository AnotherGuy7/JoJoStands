using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class IceCream : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ice Cream");
            /* Tooltip.SetDefault("Every fifth hit on enemies with Stand Attacks deal more damage." +
                "\nAttacks affected by this accessory ignore defense.\n15% increased stand damage\n50% increased stand crit damage"); */
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 36;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 9);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().underbossPhoneEquipped = true;
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.15f;
            player.GetModPlayer<MyPlayer>().manifestedWillEmblem = true;
            player.AddBuff(ModContent.BuffType<CooledOut>(), 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<UnderbossPhone>())
                .AddIngredient(ModContent.ItemType<ManifestedWillEmblem>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .AddIngredient(ModContent.ItemType<IceCubes>(), 5)
                .Register();
        }
    }
}