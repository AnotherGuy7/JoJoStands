using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class IceCream : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ice Cream");
            /* Tooltip.SetDefault("Every eigth hit on enemies with Stand Attacks is a guaranteed critical strike hit." +
                "70% increased stand crit damage. Stand Crit chances are reduced by 10%."); */
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 36;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 9);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().iceCreamEquipped = true;
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts -= 0.1f;
            player.AddBuff(ModContent.BuffType<CooledOut>(), 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<UnderbossPhone>())
                .AddIngredient(ModContent.ItemType<ManifestedWillEmblem>())
                .AddIngredient(ModContent.ItemType<IceCubes>(), 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}