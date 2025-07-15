using UnityEngine;

public class SymbolView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public bool IsActive;

    public void SetStatus(bool active)
    {
        IsActive = active;
        if (IsActive)
        {
            _spriteRenderer.color = Color.white;
        }
        else
        {
            _spriteRenderer.color = Color.green;
        }
    }
}