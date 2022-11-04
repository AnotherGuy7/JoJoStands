using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace JoJoStands.DropConditions
{
    public class JoJoStandsCorruptionCondition : IItemDropRuleCondition
    {
        public string GetConditionDescription()
        {
            return "Drops while in the Corruption";
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
                return info.player.ZoneCorrupt;

            return false;
        }
    }
}
