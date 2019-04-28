using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class WrappedPicture : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 6));
            DisplayName.SetDefault("Wrapped Picture");
            Tooltip.SetDefault("This wrapped photo vibrates and small sounds come from it...");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.noUseGraphic = true;
            item.consumable = true;
            item.useStyle = 1;
            item.useTime = 4;
            item.useAnimation = 4;
            item.height = 20;
            item.maxStack = 1;
            item.value = 11;
            item.rare = 8;
            item.maxStack = 1;
        }

        public override bool UseItem(Player player)
        {
            NPC.NewNPC((int)(player.Center.X), (int)player.Center.Y - 10, mod.NPCType("Yoshihiro"));
            player.ConsumeItem(mod.ItemType("WrappedPicture"), true);
            return true;
        }
    }
}