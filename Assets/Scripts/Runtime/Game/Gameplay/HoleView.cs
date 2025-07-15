using UnityEngine;
using TMPro;
using System;

namespace Runtime.Game
{
    public class HoleView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Collider2D _holeCollider;
        [SerializeField] private SpriteRenderer _holeSprite;
        [SerializeField] private int _id;
        [SerializeField] private int _cost;
        public event Action<int, int> OnBallEnter;
        public int CurrentLevel { get; private set; }

        public void SetLevel(int level)
        {
            CurrentLevel = level;
        }

        public int GetLevel()
        {
            return CurrentLevel;
        }

        public int ID => _id;
        public Collider2D Collider => _holeCollider;
        public SpriteRenderer Sprite => _holeSprite;
        public int Cost => _cost;

        public void SetText(string text)
        {
            if (_scoreText != null)
                _scoreText.text = text;
        }

        public void SetID(int newID)
        {
            _id = newID;
        }

        public void SetCost(int cost)
        {
            _cost = cost;
        }

        public void SetColor(Color color)
        {
            if (_holeSprite != null)
                _holeSprite.color = color;
        }

        public void SetColliderActive(bool active)
        {
            if (_holeCollider != null)
                _holeCollider.enabled = active;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Ball"))
            {
                Destroy(collision.gameObject);
                OnBallEnter?.Invoke(_id, _cost);
            }
        }
    }
}