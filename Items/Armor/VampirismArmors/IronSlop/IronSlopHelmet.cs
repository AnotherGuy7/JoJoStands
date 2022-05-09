using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.VampirismArmors.IronSlop
{
    [AutoloadEquip(EquipType.Head)]
    public class IronSlopHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A scrappily crafted iron(?) helmet. It's dirty with mud everywhere.");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = Item.buyPrice(silver: 5);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<IronSlopChestplate>() && legs.type == ModContent.ItemType<IronSlopShorts>();
        }

        public override void UpdateArmorSet(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            player.setBonus = "+5% Vampiric Damage; 35% Sunburn Damage Reduction.";
            vPlayer.sunburnDamageMultiplier *= 0.65f;
            vPlayer.sunburnRegenTimeMultiplier *= 0.8f;
            vPlayer.vampiricDamageMultiplier += 0.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 9)
                .AddIngredient(ItemID.MudBlock, 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 9)
                .AddIngredient(ItemID.MudBlock, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}