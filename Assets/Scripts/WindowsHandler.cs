using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowsHandler : MonoBehaviour
{
   [SerializeField] private Button btnOk;
   [SerializeField] private TextMeshProUGUI message;
   [SerializeField] private GameObject window;

   private Action action;
   private void OnEnable()
   {
      btnOk.onClick.AddListener(HideWindow);
      action = null;
   }

   public void ShowWindow(string text, Action neededAction = null)
   {
      window.SetActive(true);
      message.text = text;
      action = neededAction;
   }

   public void HideWindow()
   {
      window.SetActive(false);
      action?.Invoke();
   }

   private void OnDisable()
   {
      btnOk.onClick.RemoveAllListeners();
   }
}
