using UnityEngine;
using System;

public class BuildingView : MonoBehaviour
{
    [SerializeField] private int score = 1;

    public event Action<int> OnScoreFlyOut;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnScoreFlyOut?.Invoke(score);
    }
}