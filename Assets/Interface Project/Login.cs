
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{

    public InputField usernameInput;
    public InputField passwordInput;
    public Button signInButton;
    public Button goToSignUpButton;
    public Button backButton;
    public WindowsHandler windowsHandler;

    ArrayList credentials;

   private DatabaseReference dbreference;

    private bool isLoginSucces = false;

    // Start is called before the first frame update
    void Start()
    {
        signInButton.onClick.AddListener(signIn);
        goToSignUpButton.onClick.AddListener(moveToSignUp);
        backButton.onClick.AddListener(back);
        dbreference = FirebaseDatabase.DefaultInstance.RootReference;

        // if (File.Exists(Application.dataPath + "/credentials.txt"))
        // {
        //     credentials = new ArrayList(File.ReadAllLines(Application.dataPath + "/credentials.txt"));
        // }
        // else
        // {
        //     Debug.Log("Credential file doesn't exist");
        // }

    }



    // Update is called once per frame
    void signIn()
    {
       GetDatabaseDate();
       // bool isExists = false;
        //
        // credentials = new ArrayList(File.ReadAllLines(Application.dataPath + "/credentials.txt"));
        //
        // foreach (var i in credentials)
        // {
        //     string line = i.ToString();
        //     //Debug.Log(line);
        //     //Debug.Log(line.Substring(11));
        //     //substring 0-indexof(:) - indexof(:)+1 - i.length-1
        //     if (i.ToString().Substring(0, i.ToString().IndexOf(":")).Equals(usernameInput.text) &&
        //         i.ToString().Substring(i.ToString().IndexOf(":") + 1).Equals(passwordInput.text))
        //     {
        //         isExists = true;
        //         break;
        //     }
        // }
        //
        // if (isExists)
        // {
        //     Debug.Log($"Logging in '{usernameInput.text}'");
        //     loadWelcomeScreen();
        // }
        // else
        // {
        //     Debug.Log("Incorrect credentials");
        // }
    }

    void moveToSignUp()
    {
        SceneManager.LoadScene("signup");
    }

    void LoadWelcomeScreen()
    {
        SceneManager.LoadScene("account");
    }

    void back()
    {
        SceneManager.LoadScene("firstPage1");
    }

    private void GetDatabaseDate()
    {
        StartCoroutine(GetDataCor());
    }

    IEnumerator GetDataCor()
    {
        var task = dbreference.Child("Teachers").GetValueAsync();

        while (!task.IsCompleted)
        {
            yield return null;
        }
        
        DataSnapshot snapshot = task.Result;
        Dictionary<string, object> dict = snapshot.Value as Dictionary<string, object>;
        CheckCoorectLogin(dict, usernameInput.text, passwordInput.text);
        
}

    private void CheckCoorectLogin(Dictionary<string, object> allData,string usernName, string pass)
    {
        foreach (var allUsers in allData)
        {
            Dictionary<string, object> user = allUsers.Value as Dictionary<string, object>;

            string userName = user["UserName"].ToString();
            string userPass = user["Password"].ToString();
            string name = user["Name"].ToString();

            if (userName == usernameInput.text && userPass == passwordInput.text)
            {
                windowsHandler.ShowWindow("Log in Successful", () => LoadWelcomeScreen());
                isLoginSucces = true;
                CurrentTeacher.Name = name;
                CurrentTeacher.UserName = userName;
                CurrentTeacher.Password = pass;
                CurrentTeacher.Office = user["Office"].ToString();
                CurrentTeacher.Online = false;
                return;
            }
        }
        windowsHandler.ShowWindow("Incorrect username or password");
       
    }
}