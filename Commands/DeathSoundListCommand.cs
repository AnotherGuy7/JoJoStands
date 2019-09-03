using Terraria;
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
            int page = int.Parse(args[0]);
            if (page == 1)
            {
                Main.NewTextMultiline("0. None \n1. Roundabout \n2. CAESAAARRR \n3. Kono me amareri maroreri merare maro \n4. Last Train Home \n5. KORE GA... WAGA KING CRIMSON NO NORIO KU");
            }
        }
    }
}