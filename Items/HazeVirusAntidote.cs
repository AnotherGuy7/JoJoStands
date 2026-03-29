using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HazeVirusAntidote : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Haze Virus Antidote");
            // Tooltip.SetDefault("Grants temporary immunity to the Haze Virus and Concentrated Haze Virus.\nConsumed on use.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item3;
            Item.value = Item.buyPrice(0, 1, 0, 0);
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<Buffs.ItemBuff.HazeVirusImmunity>(), 60 * 60);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Frog, 2)
                .AddIngredient(ItemID.FlaskofPoison)
                .AddIngredient(ItemID.Bottle)
                .AddIngredient(ItemID.Stinger)
                .AddIngredient(ModContent.ItemType<DormantVirusSample>())
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}