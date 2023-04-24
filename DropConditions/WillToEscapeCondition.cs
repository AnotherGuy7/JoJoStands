using Terraria.GameContent.ItemDropRules;

namespace JoJoStands.DropConditions
{
    public class WillToEscapeCondition : IItemDropRuleCondition
    {
        public string GetConditionDescription()
        {
            return "Drops while in the Caverns";
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
                return info.player.ZoneNormalCaverns;

            return false;
        }
    }
}
