using System;
using UnityEngine;

namespace Project._Scripts.UI.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Clicker/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Starting Balance")]
        public float startingBalance = 0f;
    
        [Header("Business Configurations")]
        public BusinessConfig[] businesses = new BusinessConfig[5];
    }
    
    
    [Serializable]
    public class BusinessConfig
    {
        [Header("Basic Settings")]
        public int businessId;
        public float incomeDelay;
        public int baseCost;
        public float baseIncome;
        public int businessLevel;
        
        [Header("Upgrades")]
        public UpgradeConfig upgrade1;
        public UpgradeConfig upgrade2;
    
        [Header("Starting State")]
        public bool startOwned = false;
    }
    
    [Serializable]
    public class UpgradeConfig
    {
        public float price;
        public float incomeMultiplier;
        public bool isOwned;
    }
}