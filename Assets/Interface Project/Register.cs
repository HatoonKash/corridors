using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{

    public InputField usernameInput;
    public InputField passwordInput;
    public InputField instructornameInput;
    public InputField officeNumInput;
    public Button signUpButton;
    public Button goTosigninButton;
    public Button backButton;

    ArrayList credentials;

    // Start is called before the first frame update
    void Start()
    {
        signUpButton.onClick.AddListener(writeStuffToFile);
        goTosigninButton.onClick.AddListener(goToSignInScene);
        backButton.onClick.AddListener(back);

        if (File.Exists(Application.dataPath + "/credentials.txt"))
        {
            credentials = new ArrayList(File.ReadAllLines(Application.dataPath + "/credentials.txt"));
        }
        else
        {
            File.WriteAllText(Application.dataPath + "/credentials.txt", "");
        }

    }

    void goToSignInScene()
    {
        SceneManager.LoadScene("signin");
    }


    void writeStuffToFile()
    {
        bool isExists = false;

        credentials = new ArrayList(File.ReadAllLines(Application.dataPath + "/credentials.txt"));
        foreach (var i in credentials)
        {
            if (i.ToString().Contains(usernameInput.text))
            {
                isExists = true;
                break;
            }
        }

        if (isExists)
        {
            Debug.Log($"Username '{usernameInput.text}' already exists");
        }
        else
        {
            credentials.Add(usernameInput.text + ":" + passwordInput.text);
            File.WriteAllLines(Application.dataPath + "/credentials.txt", (String[])credentials.ToArray(typeof(string)));
            Debug.Log("Account Registered");
        }

    }
    void back(){
        SceneManager.LoadScene("firstPage1");
    }



}