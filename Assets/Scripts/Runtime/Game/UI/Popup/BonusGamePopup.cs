using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using Runtime.Core.UI.Popup;
using System.Threading;
using UnityEngine;

public class BonusGamePopup : BasePopup
{
    public override async UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
    {
        await base.Show(data, cancellationToken);
        Debug.Log("Show");
        await UniTask.Delay(3000, cancellationToken: cancellationToken);

        DestroyPopup();
        Debug.Log("Hide");
    }
}