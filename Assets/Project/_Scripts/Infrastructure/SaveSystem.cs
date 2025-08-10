using System;
using System.Collections.Generic;
using Project._Scripts.UI.Configs;
using Project._Scripts.UI.Models;
using Project._Scripts.UI.Presenters;
using UnityEngine;

namespace Project._Scripts.Infrastructure
{
    public class SaveSystem
    {
        private const string BALANCE_KEY = "PlayerBalance";
        private const string BUSINESS_LEVEL_KEY = "BusinessLevel_";
        private const string BUSINESS_UPGRADE1_KEY = "BusinessUpgrade1_";
        private const string BUSINESS_UPGRADE2_KEY = "BusinessUpgrade2_";
        private const string BUSINESS_PROGRESS_KEY = "BusinessProgress_";
        private const string LAST_SAVE_TIME_KEY = "LastSaveTime";

        [System.Serializable]
        public class SaveData
        {
            public float playerBalance;
            public BusinessSaveData[] businesses;
            public long lastSaveTimeTicks;
        }

        [System.Serializable]
        public class BusinessSaveData
        {
            public int businessId;
            public int level;
            public bool upgrade1Bought;
            public bool upgrade2Bought;
            public float progress;
        }

        public static void SaveGame(float playerBalance, IEnumerable<BusinessItemPresenter> presenters)
        {
            PlayerPrefs.SetFloat(BALANCE_KEY, playerBalance);
            PlayerPrefs.SetString(LAST_SAVE_TIME_KEY, DateTime.Now.ToBinary().ToString());
        
            foreach (var presenter in presenters)
            {
                var business = presenter.GetModel();
                SaveBusiness(business);
            }
        
            PlayerPrefs.Save();
            Debug.Log("Game saved successfully");
        }

        private static void SaveBusiness(BusinessItemModel business)
        {
            string prefix = business.BusinessId.ToString();

            PlayerPrefs.SetInt(BUSINESS_LEVEL_KEY + prefix, business.BusinessLevel);
            PlayerPrefs.SetInt(BUSINESS_UPGRADE1_KEY + prefix, business.FirstUpgradeBought ? 1 : 0);
            PlayerPrefs.SetInt(BUSINESS_UPGRADE2_KEY + prefix, business.SecondUpgradeBought ? 1 : 0);
            PlayerPrefs.SetFloat(BUSINESS_PROGRESS_KEY + prefix, business.BusinessProgress);
        }

        public static SaveData LoadGame(GameConfig gameConfig)
        {
            var saveData = new SaveData();

            saveData.playerBalance = PlayerPrefs.GetFloat(BALANCE_KEY, gameConfig.startingBalance);

            string timeString = PlayerPrefs.GetString(LAST_SAVE_TIME_KEY, "");
            if (!string.IsNullOrEmpty(timeString) && long.TryParse(timeString, out long timeBinary))
            {
                saveData.lastSaveTimeTicks = timeBinary;
            }
            else
            {
                saveData.lastSaveTimeTicks = DateTime.Now.ToBinary();
            }

            saveData.businesses = new BusinessSaveData[gameConfig.businesses.Length];
            for (int i = 0; i < gameConfig.businesses.Length; i++)
            {
                saveData.businesses[i] = LoadBusiness(i, gameConfig.businesses[i]);
            }

            return saveData;
        }

        private static BusinessSaveData LoadBusiness(int businessId, BusinessConfig config)
        {
            string prefix = businessId.ToString();

            return new BusinessSaveData
            {
                businessId = businessId,
                level = PlayerPrefs.GetInt(BUSINESS_LEVEL_KEY + prefix, config.businessLevel),
                upgrade1Bought = PlayerPrefs.GetInt(BUSINESS_UPGRADE1_KEY + prefix, config.upgrade1.isOwned ? 1 : 0) ==
                                 1,
                upgrade2Bought = PlayerPrefs.GetInt(BUSINESS_UPGRADE2_KEY + prefix, config.upgrade2.isOwned ? 1 : 0) ==
                                 1,
                progress = PlayerPrefs.GetFloat(BUSINESS_PROGRESS_KEY + prefix, 0f)
            };
        }

        public static void ClearSave()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Save cleared");
        }

        // Для оффлайн дохода
        public static TimeSpan GetOfflineTime()
        {
            string timeString = PlayerPrefs.GetString(LAST_SAVE_TIME_KEY, "");
            if (!string.IsNullOrEmpty(timeString) && long.TryParse(timeString, out long timeBinary))
            {
                DateTime lastSaveTime = DateTime.FromBinary(timeBinary);
                return DateTime.Now - lastSaveTime;
            }

            return TimeSpan.Zero;
        }
    }
}