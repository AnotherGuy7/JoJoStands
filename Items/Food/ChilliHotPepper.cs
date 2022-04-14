using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ChilliHotPepper : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A stand that absorbs electricity to become stronger and faster. It was caught and crushed into a fine powder, which was then sprinkled on this red hot chili pepper.");
        }

        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 28;
            item.useTime = 20;
            item.useAnimation = 20;
            item.value = Item.buyPrice(copper: 2);
            item.UseSound = SoundID.Item2;
            item.rare = ItemRarityID.Green;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            return true;
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffID.OnFire, 60 * 60 * 60);
            player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " couldn't handle the SPICE!!!"), 800, player.direction);
        }
    }
}
