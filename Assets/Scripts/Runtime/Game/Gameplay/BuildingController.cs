using DG.Tweening;
using Runtime.Game.Services.AchievementSystem;
using Runtime.Game.Services.UserData;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float moveY = 110f;
    [SerializeField] private float duration = 1f;

    private Sequence _sequence;

    public int Level = 1;
    public BuildingType BuildingType;
    public List<BuildingView> BuildingViews;

    public event Action<int> OnScoreFlyOut;

    private void OnDisable()
    {
        foreach (var view in BuildingViews)
        {
            view.OnScoreFlyOut -= ScoreFlyOut;
        }
    }

    private void SubscribeToEvent()
    {
        foreach (var view in BuildingViews)
        {
            view.OnScoreFlyOut += ScoreFlyOut;
        }
    }

    private void ScoreFlyOut(int score)
    {
        OnScoreFlyOut?.Invoke(score);
        Play(score);
    }

    public void Init()
    {
        SubscribeToEvent();
    }

    public void Play(int score)
    {
        _sequence?.Kill();

        text.gameObject.SetActive(true);
        text.text = $"+{score}";
        var rect = text.rectTransform;

        rect.localScale = Vector3.zero;
        rect.anchoredPosition = Vector2.zero;
        text.alpha = 1f;

        _sequence = DOTween.Sequence();
        _sequence.Join(rect.DOScale(Vector3.one, duration * 0.3f).SetEase(Ease.OutBack))
                 .Join(rect.DOAnchorPosY(moveY, duration).SetEase(Ease.OutCubic))
                 .Join(text.DOFade(0f, duration).SetEase(Ease.InQuad))
                 .OnComplete(() => text.gameObject.SetActive(false));
    }

    public void SetBuildingLevel(int level)
    {
        Level = level;
        foreach (var view in BuildingViews)
        {
            view.gameObject.SetActive(false);
        }
        BuildingViews[Level - 1].gameObject.SetActive(true);
    }
}