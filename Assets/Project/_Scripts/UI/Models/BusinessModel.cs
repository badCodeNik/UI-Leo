using System;

namespace Project._Scripts.UI.Models
{
    public class BusinessModel
    {
        private float _balance;
        public Action<float> OnBalanceChanged;

        public float Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnBalanceChanged?.Invoke(_balance);
            }
        }

        public BusinessModel(float gameConfigStartingBalance)
        {
            Balance = gameConfigStartingBalance;
        }
    }
}