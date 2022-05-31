using JoJoStands.Buffs.ItemBuff;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class ArrowShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Stab yourself with this to slowly manifest a stand!");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.maxStack = 1;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.rare = ItemRarityID.Green;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 0, 5, 0);
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " couldn't handle being cut by an arrow shard."), 1, -player.direction);
                player.AddBuff(ModContent.BuffType<Pierced>(), 36000);
                player.ConsumeItem(Item.type);
            }
            return true;
        }
    }
}