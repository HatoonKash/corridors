using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using TMPro;

public class TeacherDataController : MonoBehaviour
{
  [SerializeField] private TMP_Dropdown dropdown;
  [SerializeField] private NavigateWindow windowNavigate;
  [SerializeField] private TargetHandler targetHendler;

  private DatabaseReference dbReference;
  private List<string> teachersList;

  private void Start()
  {
    teachersList = new List<string>();
    dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    windowNavigate.onYesClick += AgreeToNavigeteWithOffline;
    windowNavigate.onNoClick += DisagreeNavigeteWithOffline;
    StartCoroutine(GetData());
    StartCoroutine(Timer());
  }

  private void ResetTeachersList()
  {
    dropdown.ClearOptions();
    dropdown.AddOptions(teachersList);
    teachersList.Clear();
  }
  
  IEnumerator GetData()
  {
   teachersList.Add("");
    var task = dbReference.Child("Teachers").GetValueAsync();
    while (!task.IsCompleted)
    {
      yield return null;
    }

    DataSnapshot snapShot = task.Result;

    Dictionary<string, object> dict = snapShot.Value as Dictionary<string, object>;

    foreach (var items in dict.Values)
    {
      Dictionary<string, object> values = items as Dictionary<string, object>;
      string currentName = values["Name"].ToString();
      string office = values["Office"].ToString();
      teachersList.Add($"{currentName}-{office}");
    }
    ResetTeachersList();
    
  }

  IEnumerator Timer()
  {
    while (true)
    {
      yield return new WaitForSeconds(120);
      StartCoroutine(GetData());
    }
  }

  public void CheckOnlineTeacher(string curentTeacherName)
  {
    StartCoroutine(OnlineGet(curentTeacherName));
  }

  IEnumerator OnlineGet(string curentTeacheName)
  {
    var task = dbReference.Child("Teachers").GetValueAsync();
    while (!task.IsCompleted)
    {
      yield return null;
    }

    DataSnapshot snapShot = task.Result;
    Dictionary<string, object> dict = snapShot.Value as Dictionary<string, object>;
    OnlineCheck(dict, curentTeacheName);
  }

  private void OnlineCheck(Dictionary<string, object> dict, string curretnTeacherName)
  {
    foreach (var items in dict.Values)
    {
      Dictionary<string, object> values = items as Dictionary<string, object>;

      if (values["Name"].ToString() == curretnTeacherName)
      {
        if ((bool) values["Online"])
        {
          targetHendler.SetNavigation();
          return;
        }
        else
        {
          string text = $"Dr. {values["Name"]} is currently unavailable, do you still want to navigate to the office?";
          windowNavigate.ShowWindow(text);
          return;
        }
      }
    }
  }

  public void AgreeToNavigeteWithOffline(int i)
  {
    targetHendler.SetNavigation();
  }

  public void DisagreeNavigeteWithOffline(int i)
  {
    targetHendler.ResetNavigation();
    windowNavigate.Hide();
  }
}
