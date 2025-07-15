using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using Runtime.Game.UI;
using System.Threading;
using UnityEngine;

namespace Runtime.Core.UI.Popup
{
    public class TermsPopup : BasePopup
    {
        [SerializeField] private SimpleButton _closeButton;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _closeButton.Button.onClick.AddListener(DestroyPopup);
            return base.Show(data, cancellationToken);
        }
    }
}