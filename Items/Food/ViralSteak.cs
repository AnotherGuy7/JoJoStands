using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Items.CraftingMaterials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ViralSteak : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("An already warm, juicy, and medium-rare steak.\nBoosts Stand Range and Stand Damage for 3m.");
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 22;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.maxStack = 99;
            Item.value = Item.buyPrice(silver: 15, copper: 25);
            Item.UseSound = SoundID.Item2;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
            Item.consumable = true;
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 offset = new Vector2((Item.width / 2) * player.direction, -Item.height / 2);
            player.itemLocation -= offset;
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
