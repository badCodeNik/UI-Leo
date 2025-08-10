using TMPro;
using UnityEngine;

namespace Project._Scripts.UI.Views
{
    public class BusinessView : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private TMP_Text _balanceText;

        public Transform Container => _container;

        public void SetBalance(float newBalance)
        {
            _balanceText.text = $"Баланс : {newBalance}" ;
        }
    }
}