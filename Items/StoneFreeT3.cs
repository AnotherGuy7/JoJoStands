using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StoneFreeT3 : StandItemClass
    {
        public override int StandSpeed => 11;
        public override int StandType => 1;
        public override string StandProjectileName => "StoneFree";
        public override int StandTier => 3;
        public override Color StandTierDisplayColor => Color.MediumAquamarine;

        public override string Texture
        {
            get { return Mod.Name + "/Items/StoneFreeT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Free (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to create a string trap!\nSpecial: This ability is set by the ability selected in the Abiliy Wheel.\nSecond Special: Toggles the Ability Wheel.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.width = 50;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            StoneFreeAbilityWheel.OpenAbilityWheel(player.GetModPlayer<MyPlayer>(), 4);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StoneFreeT2>())
                .AddIngredient(ItemID.HallowedBar, 14)
                .AddIngredient(ItemID.Silk, 16)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 2)
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
