using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using Runtime.Core.UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Popup
{
    public class InfoPopup : BasePopup
    {
        [SerializeField] private SimpleButton _closeButton;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _closeButton.Button.onClick.AddListener(DestroyPopup);
            return base.Show(data, cancellationToken);
        }
    }
}