using System;
using Project._Scripts.Infrastructure;
using Project._Scripts.UI.Configs;
using UnityEngine;

namespace Project._Scripts.UI.Models
{
    public class BusinessItemModel
    {
        private int _businessLevel;
        private bool _firstUpgradeBought;
        private bool _secondUpgradeBought;
        private float _businessProgress;
        private float _levelUpPrice;
        private float _businessIncome;

        public int BusinessLevel
        {
            get => _businessLevel;
            set
            {
                _businessLevel = value;
                OnLevelUp?.Invoke(_businessLevel);
                LevelUpPrice = CalculateLevelUpCost();
                BusinessIncome = CalculateBusinessIncome();
            }
        }

        public bool FirstUpgradeBought
        {
            get => _firstUpgradeBought;
            set
            {
                _firstUpgradeBought = value;
                OnUpgradeBought?.Invoke(0);
                BusinessIncome = CalculateBusinessIncome();
            }
        }

        public float BusinessIncome
        {
            get => _businessIncome;
            private set
            {
                _businessIncome = value;
                OnIncomeChanged?.Invoke(_businessIncome);
            }
        }

        public bool SecondUpgradeBought
        {
            get => _secondUpgradeBought;
            set
            {
                _secondUpgradeBought = value;
                OnUpgradeBought?.Invoke(1);
                BusinessIncome = CalculateBusinessIncome();
            }
        }

        public Action<int> OnLevelUp;
        public Action<int> OnUpgradeBought;
        public Action<float> OnProgressChanged;
        public Action<float> OnLevelUpPriceChanged;
        public Action<float> OnIncomeChanged;
        public int BusinessId;
        public float FirstUpgradePrice;
        public float SecondUpgradePrice;


        public float LevelUpPrice
        {
            get => _levelUpPrice;
            set
            {
                _levelUpPrice = value;
                OnLevelUpPriceChanged?.Invoke(_levelUpPrice);
            }
        }

        public float BusinessProgress
        {
            get => _businessProgress;
            set
            {
                _businessProgress = value;
                OnProgressChanged?.Invoke(_businessProgress);
            }
        }

        private readonly BusinessConfig _config;

        public BusinessItemModel(BusinessConfig config)
        {
            _config = config;
            BusinessLevel = config.businessLevel;
            BusinessId = config.businessId;
            BusinessIncome = config.baseIncome;
            FirstUpgradePrice = config.upgrade1.price;
            SecondUpgradePrice = config.upgrade2.price;
            FirstUpgradeBought = config.upgrade1.isOwned;
            SecondUpgradeBought = config.upgrade2.isOwned;
            BusinessProgress = 0f;
            
            LevelUpPrice = CalculateLevelUpCost();
            BusinessIncome = CalculateBusinessIncome();
        }


        private float CalculateLevelUpCost()
        {
            return (_businessLevel + 1) * _config.baseCost;
        }

        private float CalculateBusinessIncome()
        {
            if (_businessLevel == 0) return 0f;

            float multiplier = 1f;
            if (_firstUpgradeBought) multiplier += _config.upgrade1.incomeMultiplier;
            if (_secondUpgradeBought) multiplier += _config.upgrade2.incomeMultiplier;

            return _businessLevel * _config.baseIncome * multiplier;
        }
    }
}