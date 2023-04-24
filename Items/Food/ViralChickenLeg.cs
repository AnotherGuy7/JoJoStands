using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Items.CraftingMaterials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ViralChickenLeg : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("A dry, breaded (chicken?) leg that, when eaten, makes you feel much better.\nBoosts Stand Speed and player defense for 3m.");
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.maxStack = 99;
            Item.value = Item.buyPrice(silver: 12);
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
            player.AddBuff(ModContent.BuffType<QuickThinking>(), (3 * 60) * 60);
            player.AddBuff(ModContent.BuffType<MentalFortitude>(), (3 * 60) * 60);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralPowder>(), 3)
                .AddIngredient(ItemID.Bunny)
                .AddIngredient(ItemID.Bone, 2)
                .AddTile(TileID.Campfire)
                .Register();
        }
    }
}
