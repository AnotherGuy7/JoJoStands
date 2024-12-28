using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class LockT3 : StandItemClass
    {
        public override string Texture => Mod.Name + "/Items/LockT1";
        public override int StandTier => 3;
        public override string StandIdentifierName => "Lock";
        public override Color StandTierDisplayColor => Color.LightGray;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Lock (Tier 3)");
            // Tooltip.SetDefault("Make people that harm you overwhelmed with Guilt! \nSpecial: Damage yourself and make everyone in a 40 tile radius guilty about it.");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standAccessory = true;
            mPlayer.standType = 1;
            mPlayer.poseSoundName = "TheGuiltierYouFeel";
            player.AddBuff(ModContent.BuffType<LockActiveBuff>(), 10);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LockT2>())
                .AddIngredient(ItemID.HallowedBar, 8)
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 2)
                .AddIngredient(ModContent.ItemType<WillToControl>(), 1)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}