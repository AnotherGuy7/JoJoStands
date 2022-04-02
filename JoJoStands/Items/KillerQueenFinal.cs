using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KillerQueenFinal : StandItemClass
	{
        public override int standSpeed => 9;
        public override int standType => 1;
        public override string standProjectileName => "KillerQueen";
        public override int standTier => 4;

        public override string Texture
        {
            get { return mod.Name + "/Items/KillerQueenT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Killer Queen (Final)");
			Tooltip.SetDefault("Left-click to punch and right-click to trigger any block! \nRange: 16 blocks \nSpecial: Sheer Heart Attack!\nUsed in Stand Slot");
		}

        public override void SetDefaults()
        {
            item.damage = 74;
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
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
                        item.type = mod.ItemType("KillerQueenBTD");
                        item.SetDefaults(mod.ItemType("KillerQueenBTD"));
                        Main.PlaySound(SoundID.Grab);
                        mPlayer.revertTimer += 30;
                    }
                }
            }
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("KillerQueenT3"));
            recipe.AddIngredient(ItemID.ChlorophyteBar, 7);
            recipe.AddIngredient(ItemID.SoulofNight, 15);
            recipe.AddIngredient(mod.ItemType("Hand"), 2);
            recipe.AddIngredient(mod.ItemType("WillToDestroy"), 3);
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 3);
            recipe.AddIngredient(mod.ItemType("TaintedLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}