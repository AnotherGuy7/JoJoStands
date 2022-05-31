using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Items.CraftingMaterials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ViralCupcake : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A well-made cupcake on a tiny plate, all with a tiny viral structure on top!\nBoosts Stand Damage and Stand Crit Chances for 2m.");
            SacrificeTotal = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.maxStack = 99;
            Item.value = Item.buyPrice(silver: 12, copper: 50);
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
            player.AddBuff(ModContent.BuffType<StrongWill>(), (2 * 60) * 60);
            player.AddBuff(ModContent.BuffType<SharpMind>(), (2 * 60) * 60);
        }

        public override void AddRecipes()
        {
            CreateRecipe(6)
                .AddIngredient(ModContent.ItemType<ViralPowder>(), 4)
                .AddIngredient(ItemID.HoneyBlock)
                .AddIngredient(ItemID.Hay)
                .Register();
        }
    }
}
