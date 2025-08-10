namespace Project._Scripts.ECS.Components
{
    public class BusinessRequests
    {
        public struct BusinessUpgradeRequest
        {
            public int businessId;
            public int upgradeIndex;
        }

        public struct LevelUpRequest
        {
            public int businessId;
        }
    }
}