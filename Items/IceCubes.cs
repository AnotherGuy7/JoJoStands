using JoJoStands.Buffs.ItemBuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class IceCubes : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ice Cubes");
            // Tooltip.SetDefault("Ice cubes that are in perfect size for eating.");
            Item.ResearchUnlockCount = 10;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 0, 0, 60);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item2;
            Item.consumable = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.buffType = ModContent.BuffType<CooledOut>();
            Item.buffTime = 3000;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ItemID.IceBlock, 1)
                .Register();
        }
    }
}