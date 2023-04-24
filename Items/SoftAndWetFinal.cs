using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace JoJoStands.Items
{
    public class SoftAndWetFinal : StandItemClass
    {
        public override int StandSpeed => 10;
        public override int StandType => 1;
        public override string StandProjectileName => "SoftAndWet";
        public override int StandTier => 4;
        public override Color StandTierDisplayColor => Color.LightBlue;

        public override string Texture
        {
            get { return Mod.Name + "/Items/SoftAndWetT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Soft and Wet (Final Tier)");
            /* Tooltip.SetDefault("Punch enemies at a fast rate and right-click on empty space to create a Plunder Bubble or hold right-click on a tile to plant a bubble bomb!" +
                "\nSpecial: Bubble Offensive" +
                "\nSecond Special: Bubble Barrier!" +
                "\nPassive: Bubble Generation" +
                "\nBubble Offensive creates bubbles around Soft & Wet that can be manipulated." +
                "\nBubbles in Bubble Offensive mode can be detonated." +
                "\nBubbles in Bubble Offensive can plunder debuff effects from enemies and tiles." +
                "\nBubble Generation allows Soft & Wet to generate bubbles while barraging." +
                "\nUsed in Stand Slot"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 79;
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SoftAndWetT3>())
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 2)
                .AddIngredient(ItemID.Dynamite, 5)
                .AddIngredient(ItemID.ChlorophyteBar, 10)
                .AddIngredient(ModContent.ItemType<RighteousLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
