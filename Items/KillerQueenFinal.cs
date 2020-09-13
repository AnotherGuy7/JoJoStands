using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace JoJoStands.Items
{
	public class KillerQueenFinal : StandItemClass
	{
        public override int standSpeed => 9;
        public override int standType => 1;

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

        public override bool AltFunctionUse(Player player)
        {
            return player.GetModPlayer<MyPlayer>().canRevertFromKQBTD;
        }

        public override bool CanUseItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.altFunctionUse == 2 && mPlayer.revertTimer <= 0)
            {
                item.TurnToAir();
                Item.NewItem(player.Center, mod.ItemType("KillerQueenBTD"));
                mPlayer.revertTimer += 30;
            }
            return true;
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