using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class NavigateWindow : MonoBehaviour
{
    public Action<int> onYesClick;
    public Action<int> onNoClick;
    public Button yesBtn;
    public Button noBtn;
    public TextMeshProUGUI message;
    public GameObject window;

    private void OnEnable()
    {
        yesBtn.onClick.AddListener(YesBtnClick);
        noBtn.onClick.AddListener(NoBtnClick);
    }

    public void ShowWindow(string text)
    {
        window.SetActive(true);
        message.text = text;
    }

    public void YesBtnClick()
    {
        onYesClick?.Invoke(1);
        Hide();
    }

    public void NoBtnClick()
    {
        onNoClick?.Invoke(1);
        Hide();
    }

    private void OnDisable()
    {
        yesBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();
    }

    public void Hide()
    {
        window.SetActive(false);
    }
}
