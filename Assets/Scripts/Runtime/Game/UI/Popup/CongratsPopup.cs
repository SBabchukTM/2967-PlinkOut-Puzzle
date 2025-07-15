using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using Runtime.Game.UI;
using Runtime.Game.UI.Popup.Data;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class CongratsPopup : BasePopup
    {
        [SerializeField] private SimpleButton _backgroundButton;
        [SerializeField] private TextMeshProUGUI _amount;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            CongratsPopupData congratsPopupData = data as CongratsPopupData;

            _amount.text = congratsPopupData.Amount.ToString();

            _backgroundButton.Button.onClick.AddListener(DestroyPopup);

            return base.Show(data, cancellationToken);
        }
    }
}