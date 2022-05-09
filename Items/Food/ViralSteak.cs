using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ViralSteak : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("An already warm, juicy, and mediun-rare steak.\nBoosts Stand Range and Stand Damage for 3m.");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.value = Item.buyPrice(0, 0, 15, 25);
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
            player.AddBuff(ModContent.BuffType<CoordinatedEyes>(), 180 * 60);
            player.AddBuff(ModContent.BuffType<StrongWill>(), 180 * 60);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralPowder>(), 4)
                .AddIngredient(ItemID.Bunny)
                .AddIngredient(ItemID.Squirrel)
                .AddTile(TileID.Campfire)
                .Register();
        }
    }
}
