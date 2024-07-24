using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Items.CraftingMaterials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ViralPunch : ModItem
    {
        private SoundStyle drinkSound;

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("A golden, sour, yet refreshing drink that boosts your mental capabilities somehow.\nBoosts Stand Damage and Stand Speed for 2m.");
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            drinkSound = SoundID.Item3;
            drinkSound.Pitch = 0.8f;

            Item.width = 16;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.maxStack = 99;
            Item.value = Item.buyPrice(silver: 10, copper: 50);
            Item.UseSound = drinkSound;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<StrongWill>();
            Item.buffTime = (2 * 60) * 60;
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
            player.AddBuff(ModContent.BuffType<QuickThinking>(), (2 * 60) * 60);
        }

        public override void AddRecipes()
        {
            CreateRecipe(3)
                .AddIngredient(ModContent.ItemType<ViralPowder>(), 3)
                .AddIngredient(ItemID.HoneyBlock, 2)
                .AddIngredient(ItemID.BottledWater, 2)
                .AddIngredient(ItemID.BlueBerries, 3)
                .AddTile(TileID.Kegs)
                .Register();
        }
    }
}
