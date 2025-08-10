using Project._Scripts.ECS.Components;
using Project._Scripts.UI.Models;
using Project._Scripts.UI.Views;
using UnityEngine;

namespace Project._Scripts.UI.Presenters
{
    public class BusinessItemPresenter
    {
        private readonly BusinessItemView _view;
        private readonly BusinessItemModel _model;
        private readonly BusinessModel _businessModel;

        public BusinessItemPresenter(BusinessItemView view, BusinessItemModel model, BusinessModel businessModel)
        {
            _view = view;
            _model = model;
            _businessModel = businessModel;
            _model.OnLevelUp += _view.SetLevel;
            _model.OnUpgradeBought += _view.SetUpgradeBought;
            _model.OnLevelUpPriceChanged += _view.SetLevelUpPrice;
            _model.OnIncomeChanged += _view.SetIncome;
        }

        public void TryLevelUp()
        {
            if (_businessModel.Balance >= _model.LevelUpPrice)
            {
                _businessModel.Balance -= _model.LevelUpPrice;
                _model.BusinessLevel++;
            }
        }

        public void TryBuyUpgrade(int upgradeIndex)
        {
            if (upgradeIndex == 0)
            {
                if(_model.FirstUpgradeBought) return;
                if (_businessModel.Balance >= _model.FirstUpgradePrice)
                {
                    _businessModel.Balance -= _model.FirstUpgradePrice;
                    _model.FirstUpgradeBought = true;
                }
            }
            
            if (upgradeIndex == 1)
            {
                if (_model.SecondUpgradeBought) return;
                if (_businessModel.Balance >= _model.SecondUpgradePrice)
                {
                    _businessModel.Balance -= _model.SecondUpgradePrice;
                    _model.SecondUpgradeBought = true;
                }
            }
        }

        public void UpdateProgress(float progress)
        {
            _model.BusinessProgress = progress;
            _view.SetProgress(progress);
        }
        
        public void SyncWithECS(ref BusinessComponents.BusinessComponent ecsComponent)
        {
            ecsComponent.level = _model.BusinessLevel;
            ecsComponent.income = _model.BusinessIncome;
        }

        public BusinessItemModel GetModel()
        {
            return _model;
        }
    }
}