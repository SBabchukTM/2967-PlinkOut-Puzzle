using Runtime.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] public HoleWithSymbol HoleWithSymbol;

    private void Awake()
    {
        var holeView = GetComponentInChildren<HoleView>();
        var symbolView = GetComponentInChildren<SymbolView>();
        HoleWithSymbol = new HoleWithSymbol()
        {
            HoleView = holeView,
            SymbolView = symbolView
        };
    }

    public void SetSymbolViewActive(bool active)
    {
        HoleWithSymbol.SymbolView.gameObject.SetActive(active);
    }
}

[System.Serializable]
public class HoleWithSymbol
{
    public HoleView HoleView;
    public SymbolView SymbolView;
}