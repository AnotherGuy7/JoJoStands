using Microsoft.Xna.Framework;
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
            Item.width = 36;
            Item.height = 28;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.value = Item.buyPrice(copper: 2);
            Item.UseSound = SoundID.Item2;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
            Item.consumable = true;
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 offset = new Vector2((Item.width / 2) * player.direction, -Item.height / 2);
            offset += new Vector2(Main.rand.Next(-2, 2 + 1), Main.rand.Next(-2, 2 + 1));
            player.itemLocation -= offset;
        }

        public override bool? UseItem(Player player)
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
