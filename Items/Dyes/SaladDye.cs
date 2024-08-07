﻿using Terraria;
using Terraria.ID;

namespace JoJoStands.Items.Dyes
{
    public class SaladDye : StandDye
    {
        public override string DyePath => "/Salad";

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 25);
        }

        public override void OnEquipDye(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.usingStandTextureDye = true;
            mPlayer.currentTextureDye = MyPlayer.StandTextureDye.Salad;
            base.OnEquipDye(player);
        }

        public override void UpdateEquippedDye(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.usingStandTextureDye = true;
            mPlayer.currentTextureDye = MyPlayer.StandTextureDye.Salad;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Waterleaf)
            .AddIngredient(ItemID.Deathweed)
            .AddIngredient(ItemID.Shiverthorn)
            .AddIngredient(ItemID.Fireblossom)
            .AddTile(TileID.DyeVat)
            .Register();
        }
    }
}
