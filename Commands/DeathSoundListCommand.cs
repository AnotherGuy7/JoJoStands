using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Commands
{
    public class DeathSoundCommandList : ModCommand
    {
        public override CommandType Type { get { return CommandType.Chat; } }

        public override string Command { get { return "deathsoundlist"; } }

        public override string Usage { get { return "/deathsoundlist <number>"; } }

        public override string Description { get { return "Change the sound that plays when you die! Pick the sound with a number"; } }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args[0].Length <= 1)
            {
                int page = int.Parse(args[0]);
                if (page == 1)
                {
                    Main.NewTextMultiline("0. None \n1. Roundabout \n2. CAESAAARRR \n3. Kono me amareri maroreri merare maro \n4. Last Train Home \n5. KORE GA... WAGA KING CRIMSON NO NORIO KU");
                }
            }
            else
            {
                if (args[0].Length == 7 && JoJoStands.testStandPassword.Count == 7)
                {
                    string password = "";
                    for (int i = 0; i < 7; i++)
                    {
                        password += JoJoStands.testStandPassword[i];
                    }
                    Main.NewText(password);
                    if (args[0] == password)
                    {
                        MyPlayer.testStandUnlocked = true;
                        Main.NewText("Worthiness has been achieved.");
                        Main.PlaySound(SoundID.MoonLord);
                    }
                }
            }
        }
    }
}