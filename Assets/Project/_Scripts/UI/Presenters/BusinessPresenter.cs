using Project._Scripts.UI.Models;
using Project._Scripts.UI.Views;
using UnityEngine;

namespace Project._Scripts.UI.Presenters
{
    public class BusinessPresenter
    {
        private BusinessView _view;
        private readonly BusinessModel _model;

        public BusinessPresenter(BusinessView businessView,BusinessModel model)
        {
            _view = businessView;
            _model = model;
            _model.OnBalanceChanged += _view.SetBalance;
        }
        
        public void SetBalance(float balance)
        {
            _model.Balance = balance;
        }

        public void AddIncome(float newBalance)
        {
            _model.Balance += newBalance;
        }
    }
}