using UnityEngine;
using UnityEngine.UI;
using System;
using Runtime.Game.Services.UserData;

[RequireComponent(typeof(Collider2D))]
public class LuckyWheelController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider slider;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float minX = -3f;
    [SerializeField] private float maxX = 3f;

    [Header("Fill Settings")]
    [SerializeField] private int maxValue = 25;

    private UserDataService _userDataService;

    public void Init(UserDataService userDataService)
    {
        _userDataService = userDataService;
        slider.maxValue = maxValue;
        slider.value = _userDataService.GetUserData().UserInventory.LuckyBar;
    }

    public void MoveLeft()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x - moveSpeed * Time.deltaTime, minX, maxX);
        transform.position = pos;
    }

    public void MoveRight()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x + moveSpeed * Time.deltaTime, minX, maxX);
        transform.position = pos;
    }

    public bool IsWheelFilled()
    {
        if (slider.value >= maxValue)
        {
            ResetWheel();
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ball")) return;

        FillProgress();
    }

    private void FillProgress()
    {
        slider.value++;
        _userDataService.GetUserData().UserInventory.LuckyBar = (int)slider.value;
    }

    private void ResetWheel()
    {
        slider.value = 0;
    }
}