using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using Runtime.Game.UI;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class GameOverPopup : BasePopup
    {
        [SerializeField] private TextMeshProUGUI _winAmountText;

        [SerializeField] private SimpleButton _continueButton;

        public event Action OnContinuePressed;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            var gameOverPopupData = data as GameOverPopupData;
            SetData(gameOverPopupData);

            _continueButton.Button.onClick.AddListener(() => { OnContinuePressed?.Invoke(); DestroyPopup(); });
            return base.Show(data, cancellationToken);
        }

        private void SetData(GameOverPopupData gameOverPopupData)
        {
            var win = gameOverPopupData.Win;

            _winAmountText.text = win.ToString();
        }
    }
}