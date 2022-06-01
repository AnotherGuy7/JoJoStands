using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class AntiVampireSerum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anti-Vampirism Serum");
            Tooltip.SetDefault("A serum infused with Hamon made to cure those who have fallen to the power of the masks.");
            SacrificeTotal = 2;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 99;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.value = Item.buyPrice(silver: 2, copper: 50);
            Item.rare = ItemRarityID.LightPurple;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            return true;
        }

        public override void OnConsumeItem(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item3, player.Center);

            if (player.HasBuff(ModContent.BuffType<Vampire>()))
                player.ClearBuff(ModContent.BuffType<Vampire>());
            if (player.HasBuff(ModContent.BuffType<AjaVampire>()))
                player.ClearBuff(ModContent.BuffType<AjaVampire>());
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HealingPotion)
                .AddIngredient(ItemID.Sunflower, 3)
                .AddIngredient(ItemID.Daybloom, 2)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 2)
                .AddTile(TileID.Bottles)
                .AddTile(TileID.AlchemyTable)
                .Register();
        }
    }
}
