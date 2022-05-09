using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ViralChickenLeg : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A dry, breaded (chicken?) leg that, when eaten, makes you feel much better.\nBoosts Stand Speed and player defense for 3m.");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.UseSound = SoundID.Item2;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            return true;
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<QuickThinking>(), (3 * 60) * 60);
            player.AddBuff(ModContent.BuffType<MentalFortitude>(), (3 * 60) * 60);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralPowder>(), 3)
                .AddIngredient(ItemID.Bunny)
                .AddIngredient(ItemID.Bone, 2)
                .AddTile(TileID.Campfire)
                .Register();
        }
    }
}
