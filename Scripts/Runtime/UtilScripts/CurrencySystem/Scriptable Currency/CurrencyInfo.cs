using GlebSherzhukov.Scripts.Game.CurrencySystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_dev.Scripts.Runtime.UtilScripts.CurrencySystem
{
    [CreateAssetMenu(menuName = "D-Dev/Currencies/CurrencyData")]
    public class CurrencyInfo : ScriptableObject
    {
        #region Fields

        [Title("Currency Settings")]
        [SerializeField] private Currency _currency;

        #endregion

        #region Properties

        public Currency Currency => _currency;

        #endregion
        
        #region Debug

        [PropertySpace(10)]
        [Button]
        private void DebugDepositValue(int depositValue) => _currency.TryDeposit(depositValue);

        [Button]
        private void DebugWithdrawValue(int withdrawValue) => _currency.TryWithdraw(withdrawValue);

        #endregion
    }
}