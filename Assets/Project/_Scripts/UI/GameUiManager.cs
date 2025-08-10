using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Project._Scripts.ECS.Components;
using Project._Scripts.Infrastructure;
using Project._Scripts.UI.Configs;
using Project._Scripts.UI.Models;
using Project._Scripts.UI.Presenters;
using Project._Scripts.UI.Views;
using Project._Scripts.Utils;
using UnityEngine;

namespace Project._Scripts.UI
{
    public class GameUiManager : MonoBehaviour
    {
        [SerializeField] private BusinessView _businessView;
        private BusinessPresenter _businessPresenter;
        private readonly Dictionary<int, BusinessItemPresenter> _businessItemPresenters = new();
        private BusinessItemModel[] _businessModels;
        private BusinessModel _businessModel;

        public void Initialize(GameConfig gameConfig, BusinessNameConfig businessNameConfig,
            EcsWorld world)
        {
            var saveData = SaveSystem.LoadGame(gameConfig);
            _businessModel = new BusinessModel(saveData.playerBalance);
            var businessPresenter = new BusinessPresenter(_businessView, _businessModel);
            businessPresenter.SetBalance(saveData.playerBalance);
            _businessModels = new BusinessItemModel[gameConfig.businesses.Length];
            for (var index = 0; index < gameConfig.businesses.Length; index++)
            {
                var businessConfig = gameConfig.businesses[index];
                var entity = world.NewEntity();

                var businessItemModel = CreateBusinessModel(businessConfig, saveData.businesses[index]);
                _businessModels[index] = businessItemModel;

                InitEcsUi(world, entity, businessItemModel, businessConfig);

                var prefab = Resources.Load<BusinessItemView>(Constants.ResourcePaths.BusinessItemPrefab);
                var view = Instantiate(prefab, parent: _businessView.Container);
                SetViewData(businessNameConfig, world, view, businessConfig, entity, index, businessItemModel);

                var businessItemPresenter = new BusinessItemPresenter(view, businessItemModel, _businessModel);
                _businessItemPresenters.Add(businessConfig.businessId, businessItemPresenter);
            }

            _businessPresenter = businessPresenter;
        }

        private BusinessItemModel CreateBusinessModel(BusinessConfig config, SaveSystem.BusinessSaveData saveData)
        {
            var model = new BusinessItemModel(config);

            if (saveData != null)
            {
                model.BusinessLevel = saveData.level;
                model.FirstUpgradeBought = saveData.upgrade1Bought;
                model.SecondUpgradeBought = saveData.upgrade2Bought;
                model.BusinessProgress = saveData.progress;

                Debug.Log(
                    $"Loaded business {config.businessId}: level={saveData.level}, progress={saveData.progress:F2}");
            }

            return model;
        }

        private static void SetViewData(BusinessNameConfig businessNameConfig, EcsWorld world, BusinessItemView view,
            BusinessConfig businessConfig, int entity, int index, BusinessItemModel businessItemModel)
        {
            view.Init(world, businessConfig.businessId, entity);
            view.SetName(businessNameConfig.businessNames[index]);
            view.SetFirstUpgradeName(businessNameConfig.upgradeNames[index]);
            view.SetIncome(businessItemModel.BusinessIncome);
            view.SetLevel(businessItemModel.BusinessLevel);
            view.SetLevelUpPrice(businessItemModel.LevelUpPrice);
            view.SetFirstUpgradePercent(businessConfig.upgrade1.incomeMultiplier);
            view.SetSecondUpgradePercent(businessConfig.upgrade2.incomeMultiplier);
            if (businessItemModel.FirstUpgradeBought)
            {
                view.SetUpgradeBought(0);
            }
            else
            {
                view.SetUpgradePrice(businessItemModel.FirstUpgradePrice, 0);
            }

            if (businessItemModel.SecondUpgradeBought)
            {
                view.SetUpgradeBought(1);
            }
            else
            {
                view.SetUpgradePrice(businessItemModel.SecondUpgradePrice, 1);
            }


            view.SetProgress(businessItemModel.BusinessProgress);
        }

        private static void InitEcsUi(EcsWorld world, int entity, BusinessItemModel businessItemModel,
            BusinessConfig businessConfig)
        {
            ref var businessComponent = ref world.GetPool<BusinessComponents.BusinessComponent>().Add(entity);
            businessComponent.incomeDelay = businessConfig.incomeDelay;
            businessComponent.businessId = businessConfig.businessId;
            businessComponent.income = businessItemModel.BusinessIncome;
            businessComponent.level = businessItemModel.BusinessLevel;
            businessComponent.incomeTimer = businessItemModel.BusinessProgress * businessConfig.incomeDelay;
        }

        public bool TryGetPresenterById(int businessId, out BusinessItemPresenter presenter)
        {
            return _businessItemPresenters.TryGetValue(businessId, out presenter);
        }

        public void TryLevelUp(int businessId)
        {
            if (_businessItemPresenters.TryGetValue(businessId, out var businessItemPresenter))
            {
                businessItemPresenter.TryLevelUp();
            }
        }

        public void TryBuyUpgrade(int businessId, int upgradeIndex)
        {
            if (_businessItemPresenters.TryGetValue(businessId, out var businessItemPresenter))
            {
                businessItemPresenter.TryBuyUpgrade(upgradeIndex);
            }
        }

        public void AddIncome(float newBalance)
        {
            _businessPresenter.AddIncome(newBalance);
        }


        private void OnDestroy()
        {
            SaveSystem.SaveGame(_businessModel.Balance, _businessItemPresenters.Values);
        }
    }
}