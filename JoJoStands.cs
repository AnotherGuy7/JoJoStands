using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands
{
	public class JoJoStands : Mod
	{
        public static ModHotKey ItemHotKey;
        public static ModHotKey AccessoryHotKey;
        internal static JoJoStands Instance;

        internal UserInterface Bet;     //for later use

        public override void Load()
		{
			// Registers a new hotkey
			ItemHotKey = RegisterHotKey("Item Special", "P"); // See https://docs.microsoft.com/en-us/previous-versions/windows/xna/bb197781(v%3dxnagamestudio.41) for special keys
            AccessoryHotKey = RegisterHotKey("Accessory Special", "k");
        }
    }
}