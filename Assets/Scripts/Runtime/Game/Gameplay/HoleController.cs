using UnityEngine;
using System.Collections.Generic;
using Runtime.Game;
using Runtime.Game.Services.UserData.Data;
using System;
using Runtime.Game.Services.UserData;

public class HoleController : MonoBehaviour
{
    [SerializeField] private List<Slot> _slots;
    [SerializeField] private HoleSetup _holeSetup;

    private bool _isBonusGame = false;

    public event Action<int> OnBallEnter;
    public event Action OnBonusGameActive;

    private UserDataService _userDataService;
    private UserHoleProgress _userHoleProgress;

    private void OnDisable()
    {
        foreach (var slot in _slots)
        {
            slot.HoleWithSymbol.HoleView.OnBallEnter -= OnHoleBallEnter;
        }
    }

    public void Init(UserDataService userDataService, bool isBonusGame)
    {
        _userDataService = userDataService;
        _userHoleProgress = userDataService.GetUserData().UserHoleProgress;
        _isBonusGame = isBonusGame;

        for (int i = 0; i < _slots.Count; i++)
        {
            var holeView = _slots[i].HoleWithSymbol.HoleView;
            var symbolView = _slots[i].HoleWithSymbol.SymbolView;

            holeView.SetID(i);
            holeView.OnBallEnter += OnHoleBallEnter;

            if (_isBonusGame)
            {
                _slots[i].SetSymbolViewActive(_isBonusGame);

                var cost = GetCostList()[i];
                ApplyBonusGameHoleConfig(holeView, cost);
                _slots[i].SetSymbolViewActive(false);
            }
            else
            {
                int level = _userHoleProgress.GetHoleLevel(i);
                bool isActive = _userHoleProgress.GetActivatedSymbol(i);

                ApplyHoleConfig(holeView, level);
                ApplySymbolConfig(symbolView, isActive);
            }
        }
    }

    private void OnHoleBallEnter(int holeIndex, int cost)
    {
        OnBallEnter?.Invoke(cost);

        if (!_isBonusGame)
        {
            int currentLevel = _userHoleProgress.GetHoleLevel(holeIndex);

            int nextLevel = 1;
            if (currentLevel + 1 < _holeSetup.HoleConfigs.Count)
            {
                nextLevel = currentLevel + 1;
            }
            if (_holeSetup.HoleConfigs[nextLevel].IsGolden && HasOtherGolden(holeIndex))
            {
                nextLevel = currentLevel;
            }

            _userHoleProgress.HoleLevels[holeIndex] = nextLevel;

            var holeView = _slots[holeIndex].HoleWithSymbol.HoleView;
            var symbolView = _slots[holeIndex].HoleWithSymbol.SymbolView;
            ApplyHoleConfig(holeView, nextLevel);


            ApplySymbolConfig(symbolView, true);
            SaveHoles();
        }
    }

    private void ApplyHoleConfig(HoleView view, int level)
    {
        HoleConfig config = _holeSetup.HoleConfigs[level];
        view.SetCost(config.Score);
        view.SetColor(config.HoleColor);
        view.SetText(config.IsGolden ? "GOLD" : config.Score.ToString());
        view.SetLevel(level);
    }

    private void ApplySymbolConfig(SymbolView symbolView, bool active)
    {
        symbolView.SetStatus(active);
        /*        if (AreAllSymbolsActivated())
                {
                    DeactivateAllSymbols();
                    OnBonusGameActive?.Invoke();
                }*/
        SaveHoles();
    }

    private List<int> GetCostList()
    {
        List<int> scores = new List<int>()
        {
            50000,
            10000,
            20000,
            50000,
            100000,
            100000,
            20000,
            35000,
            10000,
            50000
        };
        return scores;
    }

    public void ApplyBonusGameHoleConfig(HoleView view, int cost)
    {
        HoleConfig config = _holeSetup.HoleConfigs[5];
        view.SetCost(cost);
        view.SetColor(config.HoleColor);
        cost /= 1000;
        view.SetText(cost.ToString() + "K");
    }

    public bool AreAllSymbolsActivated()
    {
        foreach (var slot in _slots)
        {
            var symbolView = slot.HoleWithSymbol.SymbolView;
            if (!symbolView.IsActive)
                return false;
        }

        return true;
    }

    public void DeactivateAllSymbols()
    {
        foreach (var slot in _slots)
        {
            var symbolView = slot.HoleWithSymbol.SymbolView;
            symbolView.SetStatus(false);
        }
    }

    private bool HasOtherGolden(int excludeIndex)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (i == excludeIndex) continue;

            int level = _userHoleProgress.GetHoleLevel(i);

            if (_holeSetup.HoleConfigs[level].IsGolden)
                return true;
        }

        return false;
    }

    private void SaveHoles()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            int id = _slots[i].HoleWithSymbol.HoleView.ID;
            int level = _slots[i].HoleWithSymbol.HoleView.CurrentLevel;
            _userHoleProgress.HoleLevels[id] = level;

            bool active = _slots[i].HoleWithSymbol.SymbolView.IsActive;
            _userHoleProgress.ActivatedSymbols[id] = active;

            _userDataService.GetUserData().UserHoleProgress = _userHoleProgress;
            _userDataService.SaveUserData();
        }
    }
}