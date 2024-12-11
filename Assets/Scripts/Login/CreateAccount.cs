using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAccount : MonoBehaviour
{
    private DatabaseReference dbReference;

    [SerializeField] private InputField name;
    [SerializeField] private InputField userName;
    [SerializeField] private InputField password;
    [SerializeField] private InputField officeNumber;
    [SerializeField] private TMP_Dropdown officeDropDown;
    [SerializeField] private Button singUpBtn;

    [SerializeField] private WindowsHandler windowsHandler;

    private bool isSingUp = false;
    
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        
        singUpBtn.onClick.AddListener(GetData);

    }
    public void GetData()
    {
        char[] symbols = password.text.ToCharArray();
        if (symbols.Length < 8)
        {
            windowsHandler.ShowWindow("Password must have minimum 8 symbols");
            return;
        }

        StartCoroutine(GetDataCor());
        StartCoroutine(SingUpSucces());
    }

    IEnumerator GetDataCor()
    {
        var task = dbReference.Child("Teachers").GetValueAsync();
     
            while(!task.IsCompleted)
            {
                yield return null;
            }
            DataSnapshot snapshot = task.Result;
            Dictionary<string, object> dictionary = snapshot.Value as Dictionary<string, object>;

            if (CheckUserNameExist(dictionary))
            {
                SingUp();
            }
      
    }

    public void SingUp()
    {
        if (CheckEmptyInput())
        {
            return;
        }
        StartCoroutine(SetDataCor());
       
        
        
    }
    private bool CheckUserNameExist(Dictionary<string, object> dictionary)
    {
        foreach (var name in dictionary.Values)
        {
            Dictionary<string, object> values = name as Dictionary<string, object>;
            string currentUserName = values["UserName"].ToString();

            if (currentUserName == userName.text)
            {
                windowsHandler.ShowWindow("User name already exist. You need sign in");
                return false;
            }
        }

        return true;
    }

    public bool CheckEmptyInput()
    {
        if (userName.text == String.Empty || name.text == String.Empty ||
            password.text == String.Empty)
        {
            return true;
        }

        return false;
    }

    IEnumerator SingUpSucces()
    {
        yield return new WaitForSeconds(1f);
        
    }

    IEnumerator SetDataCor()
    {
        Teachers teacher = new Teachers();
        teacher.Name = name.text;
        teacher.UserName = userName.text;
        teacher.Password = password.text;
        teacher.Office = officeDropDown.options[officeDropDown.value].text;
        teacher.Online = false;

        string json = JsonUtility.ToJson(teacher);
        var task = dbReference.Child("Teachers").Child(teacher.Name).SetRawJsonValueAsync(json);
        while (!task.IsCompleted)
        {
            yield return null;
        }
        Debug.Log("Succes");
        CurrentTeacher.Name = name.text;
        windowsHandler.ShowWindow("Successful Sign Up", () => SceneManager.LoadScene("account"));
    }
    private void SetCurrentTeacher()
    {
        CurrentTeacher.Name = name.text;
        CurrentTeacher.UserName = userName.text;
        CurrentTeacher.Password = password.text;
        CurrentTeacher.Office = officeDropDown.options[officeDropDown.value].text;
        CurrentTeacher.Online = false;
    }
}
