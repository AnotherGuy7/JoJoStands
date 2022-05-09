using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using JoJoStands.Networking;

namespace JoJoStands.Items
{
    public class WrappedPicture : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wrapped Picture>();
            Tooltip.SetDefault("This wrapped photo vibrates and small sounds come from it...>();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.useStyle = 1;
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 11;
            Item.rare = 8;
            Item.maxStack = 1;
        }

        /*public override bool? UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int yoshihiro = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 25, Mod.NPCType("Yoshihiro>());
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, yoshihiro);
                }
            }
            return true;
        }*/

        public override bool? UseItem(Player player)
        {
            if (NPC.AnyNPCs(Mod.NPCType("Yoshihiro>()))
            {
                Main.NewText("There is already a Yoshihiro alive!>();
                return false;
            }
            else
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 25, Mod.NPCType("Yoshihiro>());
                }
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModNetHandler.playerSync.SendYoshihiroToSpawn(256, player.whoAmI, Mod.NPCType("Yoshihiro>(), new Vector2(player.Center.X, player.Center.Y - 25));
                }

            }
            /*if (player.whoAmI == Main.myPlayer)
            {
                int yoshihiro = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 25, Mod.NPCType("Yoshihiro>());
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, yoshihiro);
                }
            }*/
            //Main.NewText(yoshihiro);
            return true;
        }
    }
}