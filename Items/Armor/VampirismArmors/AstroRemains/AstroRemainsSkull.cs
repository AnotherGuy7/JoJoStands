using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.VampirismArmors.AstroRemains
{
    [AutoloadEquip(EquipType.Head)]
    public class AstroRemainsSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A deer skull with a surprisingly sturdy exterior.");
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 44;
            Item.value = Item.buyPrice(silver: 70);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 8;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AstroRemainsChestplate>() && legs.type == ModContent.ItemType<AstroRemainsGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            player.setBonus = "80% Sunburn Damage Reduction\nWhile using Vampiric Abilities: 20% to dodge incoming attacks";
            vPlayer.sunburnDamageMultiplier *= 0.2f;
            vPlayer.wearingAstroRemainsSet = true;
            Lighting.AddLight(player.Center, 1f, 0.5f, 0.4f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 6)
                .AddIngredient(ItemID.Bone, 45)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}