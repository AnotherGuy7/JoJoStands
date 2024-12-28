using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KillerQueenFinal : StandItemClass
    {
        public override int StandSpeed => 11;
        public override int StandType => 1;
        public override string StandIdentifierName => "KillerQueen";
        public override int StandTier => 4;
        public override Color StandTierDisplayColor => Color.LightPink;

        public override string Texture
        {
            get { return Mod.Name + "/Items/KillerQueenT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Killer Queen (1st Bomb Final Tier)");
            // Tooltip.SetDefault("Left-click to punch and right-click to trigger any block!\nRange: 16 blocks\nSpecial: Sheer Heart Attack!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 74;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.whoAmI == Main.myPlayer)
            {
                if (mPlayer.canRevertFromKQBTD)
                {
                    if (Main.mouseRight && mPlayer.revertTimer <= 0)
                    {
                        Item.type = ModContent.ItemType<KillerQueenBTD>();
                        Item.SetDefaults(ModContent.ItemType<KillerQueenBTD>());
                        SoundEngine.PlaySound(SoundID.Grab, player.Center);
                        mPlayer.revertTimer += 30;
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<KillerQueenT3>())
                .AddIngredient(ItemID.ChlorophyteBar, 7)
                .AddIngredient(ItemID.SoulofNight, 15)
                .AddIngredient(ModContent.ItemType<Hand>(), 2)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 3)
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 3)
                .AddIngredient(ModContent.ItemType<TaintedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}