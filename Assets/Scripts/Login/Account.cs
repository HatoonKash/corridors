using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Account : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI teacherName;
    [SerializeField] private Toggle toggle;
   
    private DatabaseReference dbReference;

    private bool isOnline = false;
    public Button backButton;

    private void OnEnable()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        backButton.onClick.AddListener(GoToTheWelcomePage);
        int indexOnline = PlayerPrefs.GetInt("Online", 0);

        if (indexOnline == 0)
        {
            isOnline = false;
            toggle.isOn = false;
        }
        else
        {
            isOnline = true;
            toggle.isOn = true;
        }


    }

    private void OnDisable()
    {
        backButton.onClick.RemoveAllListeners();
    }

    void Start()
    {
       
        teacherName.text = $"Welcome {CurrentTeacher.Name}";
    }
    
    public void OnToogleValueChange()
    {
        isOnline = toggle.isOn;
        if (isOnline)
        {
            PlayerPrefs.SetInt("Online", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Online", 0);
        }

        GetData();
    }

    public void ChangeOnlineStatus(Teachers teacher)
    {
        
        string json = JsonUtility.ToJson(teacher);
        
        dbReference.Child("Teachers").Child(teacher.Name).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Succes");
            }
            else
            {
                Debug.Log("Faild");
            }
        });
    }

    public void GetData()
    {
        dbReference.Child("Teachers").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Dictionary<string, object> dictionary = snapshot.Value as Dictionary<string, object>;
                CheckUserName(dictionary);
            }
        });
    }
    private void CheckUserName(Dictionary<string, object> dictionary)
    {
        foreach (var name in dictionary.Values)
        {
            Dictionary<string, object> values = name as Dictionary<string, object>;
            string currentUserName = values["UserName"].ToString();

            if (currentUserName == CurrentTeacher.UserName)
            {
                Teachers teacher = new Teachers();
                teacher.Name = CurrentTeacher.Name;
                teacher.UserName = CurrentTeacher.UserName;
                teacher.Password = CurrentTeacher.Password;
                teacher.Office = CurrentTeacher.Office;
                teacher.Online = isOnline;
                ChangeOnlineStatus(teacher);
            }
        }
        
    }

    public void GoToTheWelcomePage()
    {
        SceneManager.LoadScene("firstPage1");
    }
}
