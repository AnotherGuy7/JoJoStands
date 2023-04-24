using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class DollyDaggerT1 : StandItemClass
    {
        public override int StandTierDisplayOffset => 1;
        public override string StandProjectileName => "DollyDagger";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dolly Dagger (Tier 1)");
            // Tooltip.SetDefault("As an Item: Left-click to use this as a dagger to stab enemies!\nIn the Stand Slot: Equip it to nullify and reflect 35% of all damage!");
        }

        public override void SetDefaults()
        {
            Item.damage = 34;
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.maxStack = 1;
            Item.noUseGraphic = false;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standType = 1;
            mPlayer.standAccessory = true;
            player.AddBuff(ModContent.BuffType<DollyDaggerActiveBuff>(), 10);
            JoJoStands.testStandPassword.Add(Convert.ToChar((363 / 5) + -7));
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 4)
                .AddIngredient(ItemID.Wood, 3)
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}