using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using JoJoStands.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GoldExperienceT2 : StandItemClass
    {
        public override int standSpeed => 11;
        public override int standType => 1;
        public override string standProjectileName => "GoldExperience";
        public override int standTier => 2;

        public override string Texture
        {
            get { return Mod.Name + "/Items/GoldExperienceFinal"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Experience (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to use abilities! \nSpecial: Switches the abilities used for right-click!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 41;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            GoldExperienceAbilityWheel.OpenAbilityWheel(player.GetModPlayer<MyPlayer>(), 2);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoldExperienceT1>())
                .AddIngredient(ItemID.GoldBar, 12)
                .AddIngredient(ItemID.Acorn, 20)
                .AddIngredient(ItemID.LifeCrystal)
                .AddIngredient(ModContent.ItemType<WillToControl>())
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoldExperienceT1>())
                .AddIngredient(ItemID.PlatinumBar, 12)
                .AddIngredient(ItemID.Acorn, 20)
                .AddIngredient(ItemID.LifeCrystal)
                .AddIngredient(ModContent.ItemType<WillToControl>())
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
