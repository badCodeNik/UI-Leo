using Leopotam.EcsLite;
using Project._Scripts.ECS.Components;
using Project._Scripts.UI.Configs;
using Project._Scripts.UI.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project._Scripts.UI.Views
{
    public class BusinessItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _businessItemName;
        [SerializeField] private TMP_Text _levelValueText;
        [SerializeField] private TMP_Text _incomeValueText;
        [SerializeField] private TMP_Text _levelUpPriceText;
        [SerializeField] private TMP_Text _firstUpgradePriceText;
        [SerializeField] private TMP_Text _firstUpgradeValueText;
        [SerializeField] private TMP_Text _secondUpgradeValueText;
        [SerializeField] private TMP_Text _secondUpgradePriceText;
        [SerializeField] private TMP_Text _firstUpgradeNameText;
        [SerializeField] private TMP_Text _secondUpgradeNameText;
        [SerializeField] private TMP_Text _isFirstUpgradeBoughtText;
        [SerializeField] private TMP_Text _isSecondUpgradeBoughtText;
        [SerializeField] private Image _incomeFillImage;
        [SerializeField] private Button _levelUpButton;
        [SerializeField] private Button _firstUpgradeButton;
        [SerializeField] private Button _secondUpgradeButton;

        public int BusinessId { get; private set; }
        private int _entity;
        private EcsWorld _world;

        public void Init(EcsWorld world, int id, int entity)
        {
            BusinessId = id;
            _entity = entity;
            _world = world;
            _levelUpButton.onClick.AddListener(OnLevelUpClicked);
            _firstUpgradeButton.onClick.AddListener(OnFirstUpgradeClicked);
            _secondUpgradeButton.onClick.AddListener(OnSecondUpgradeClicked);
        }

        public void SetName(string name)
        {
            _businessItemName.text = name;
        }

        public void SetUpgradePrice(float price, int index)
        {
            switch (index)
            {
                case 0:
                    _firstUpgradePriceText.text = $"Цена : {price}";
                    break;
                case 1:
                    _secondUpgradePriceText.text = $"Цена : {price}";
                    break;
            }
        }

        public void SetIncome(float income)
        {
            _incomeValueText.text = $"{income}";
        }

        public void SetLevel(int level)
        {
            _levelValueText.text = $"{level}";
        }

        public void SetLevelUpPrice(float price)
        {
            _levelUpPriceText.text = $"{price}";
        }

        public void SetFirstUpgradePercent(float percent)
        {
            _firstUpgradeValueText.text = $"Доход + {percent * 100}%";
        }
        
        public void SetSecondUpgradePercent(float percent)
        {
            _secondUpgradeValueText.text = $"Доход + {percent * 100}%";
        }

        private void OnLevelUpClicked()
        {
            ref var levelUpRequest = ref _world.GetPool<BusinessRequests.LevelUpRequest>().Add(_entity);
            levelUpRequest.businessId = BusinessId;
        }

        private void OnFirstUpgradeClicked()
        {
            ref var upgradeRequest = ref _world.GetPool<BusinessRequests.BusinessUpgradeRequest>().Add(_entity);
            upgradeRequest.upgradeIndex = 0;
            upgradeRequest.businessId = BusinessId;
        }

        private void OnSecondUpgradeClicked()
        {
            ref var upgradeRequest = ref _world.GetPool<BusinessRequests.BusinessUpgradeRequest>().Add(_entity);
            upgradeRequest.upgradeIndex = 1;
            upgradeRequest.businessId = BusinessId;
        }

        public void SetProgress(float progress)
        {
            _incomeFillImage.fillAmount = progress;
        }

        public void SetUpgradeBought(int upgradeIndex)
        {
            switch (upgradeIndex)
            {
                case 0:
                    _firstUpgradePriceText.text = "Куплено";
                    break;
                case 1:
                    _secondUpgradePriceText.text = "Куплено";
                    break;
            }
        }

        public void SetFirstUpgradeName(UpgradeNamesConfig upgradeNames)
        {
            _firstUpgradeNameText.text = upgradeNames.upgrade1Name;
            _secondUpgradeNameText.text = upgradeNames.upgrade2Name;
        }
    }
}