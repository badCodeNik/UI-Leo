using System;
using UnityEngine;

namespace Project._Scripts.UI.Configs
{
    [CreateAssetMenu(fileName = "BusinessNameConfig", menuName = "Clicker/BusinessName Config")]
    public class BusinessNameConfig : ScriptableObject
    {
        [Header("Business Names")]
        public string[] businessNames = new string[5];
    
        [Header("Upgrade Names")]
        public UpgradeNamesConfig[] upgradeNames = new UpgradeNamesConfig[5];
    }
    
    [Serializable]
    public class UpgradeNamesConfig
    {
        public string upgrade1Name;
        public string upgrade2Name;
    }
}