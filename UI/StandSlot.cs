using JoJoStands.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using TerraUI;
using TerraUI.Objects;

namespace JoJoStands.UI
{
    public class StandSlot : UIItemSlot
    {
        public StandSlot(Vector2 position, int size = 52, int context = ItemSlot.Context.EquipAccessory,
            string hoverText = "", UIObject parent = null, ConditionHandler conditions = null,
            DrawHandler drawBackground = null, DrawHandler drawItem = null, DrawHandler postDrawItem = null,
            bool drawAsNormalSlot = false, bool scaleToInventory = false)
            : base(position, size, context, hoverText, parent, conditions,
                  drawBackground, drawItem, postDrawItem, drawAsNormalSlot, scaleToInventory)
        {
        }

        public override void OnLeftClick()
        {
            if (Main.mouseItem.IsAir && (Item == null || Item.IsAir))
                return;
            if (!Main.mouseItem.IsAir && Conditions != null && !Conditions(Main.mouseItem))
                return;
            Player player = Main.player[Main.myPlayer];
            StandItemClass outgoing = (!Item.IsAir)
                ? Item.ModItem as StandItemClass
                : null;
            StandItemClass incoming = (!Main.mouseItem.IsAir)
                ? Main.mouseItem.ModItem as StandItemClass
                : null;
            outgoing?.OnUnequip(player);
            Swap(ref Item, ref Main.mouseItem);
            incoming?.OnEquip(player);
        }
    }
}