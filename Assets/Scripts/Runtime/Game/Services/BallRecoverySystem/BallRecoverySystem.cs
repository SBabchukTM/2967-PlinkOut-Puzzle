using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Runtime.Game.Services.UserData.Data;

public class BallRecoverySystem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider _ballSlider;
    [SerializeField] private TextMeshProUGUI _ballsCountText;
    [SerializeField] private TextMeshProUGUI _recoveryTimerText;

    [Header("Settings")]
    [SerializeField] private int _maxBalls = 10;
    [SerializeField] private float _recoveryInterval = 5f;

    private BallRecoveryData _ballRecoveryData;
    [SerializeField] private int _currentBalls = 0;
    private float _timer;
    private bool _isBonusGame = false;

    public event Action OnBallsChanged;

    public void Init(BallRecoveryData data)
    {
        _ballRecoveryData = data;
        int balls = data.CurrentBalls;
        float timer = data.Timer;
        _currentBalls = balls;
        _timer = timer;
        UpdateUI();
    }

    private void Update()
    {
        if (_ballRecoveryData == null)
            return;

        if (_isBonusGame)
        {
            UpdateBonusGameUI();
            return;
        }

        if (_currentBalls < _maxBalls)
        {
            _timer += Time.deltaTime;

            if (_timer >= _recoveryInterval)
            {
                _timer = 0f;
                AddBalls(5);
            }
        }

        UpdateTimerUI();
    }

    public bool TryUseBall()
    {
        if (_currentBalls > 0)
        {
            _ballRecoveryData.CurrentBalls--;

            if (_isBonusGame)
            {
                _currentBalls--;
                UpdateBonusGameUI();
            }
            else
                UpdateUI();

            return true;
        }
        return false;
    }

    public bool HasBalls()
    {
        return _currentBalls > 0;
    }

    public void AddBalls(int amount)
    {
        _ballRecoveryData.CurrentBalls = Mathf.Clamp(_currentBalls + amount, 0, _maxBalls);
        UpdateUI();
    }

    public void StartBonusGame()
    {
        _isBonusGame = true;
        _recoveryTimerText.gameObject.SetActive(false);

        _maxBalls = 3;
        _currentBalls = 3;

        UpdateBonusGameUI();
    }

    public void EndBonusGame()
    {
        _recoveryTimerText.gameObject.SetActive(true);
        _isBonusGame = false;
    }

    public int GetCurrentBalls() => _currentBalls;

    private void UpdateUI()
    {
        _currentBalls = _ballRecoveryData.CurrentBalls;

        if (_ballSlider != null)
        {
            _ballSlider.maxValue = _maxBalls;
            _ballSlider.value = _currentBalls;
        }

        if (_ballsCountText != null)
            _ballsCountText.text = $"{_currentBalls}/{_maxBalls}";

        UpdateTimerUI();

        OnBallsChanged?.Invoke();
    }

    private void UpdateBonusGameUI()
    {
        if (_ballSlider != null)
        {
            _ballSlider.maxValue = _maxBalls;
            _ballSlider.value = _currentBalls;
        }

        if (_ballsCountText != null)
            _ballsCountText.text = $"{_currentBalls}/{_maxBalls}";
    }

    private void UpdateTimerUI()
    {
        if (_recoveryTimerText != null)
        {
            if (_currentBalls < _maxBalls)
            {
                float timeLeft = Mathf.Clamp(_recoveryInterval - _timer, 0f, _recoveryInterval);
                _ballRecoveryData.Timer = timeLeft;
                _recoveryTimerText.text = $"5 balls in {timeLeft:F1}s";
            }
            else
            {
                _recoveryTimerText.text = "Full";
            }
        }
    }
}