
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    ArrayList credentials;

    // Start is called before the first frame update
    void Start()
    {
        signInButton.onClick.AddListener(signIn);
        goToSignUpButton.onClick.AddListener(moveToSignUp);
        backButton.onClick.AddListener(back);


        if (File.Exists(Application.dataPath + "/credentials.txt"))
        {
            credentials = new ArrayList(File.ReadAllLines(Application.dataPath + "/credentials.txt"));
        }
        else
        {
            Debug.Log("Credential file doesn't exist");
        }

    }



    // Update is called once per frame
    void signIn()
    {
        bool isExists = false;

        credentials = new ArrayList(File.ReadAllLines(Application.dataPath + "/credentials.txt"));

        foreach (var i in credentials)
        {
            string line = i.ToString();
            //Debug.Log(line);
            //Debug.Log(line.Substring(11));
            //substring 0-indexof(:) - indexof(:)+1 - i.length-1
            if (i.ToString().Substring(0, i.ToString().IndexOf(":")).Equals(usernameInput.text) &&
                i.ToString().Substring(i.ToString().IndexOf(":") + 1).Equals(passwordInput.text))
            {
                isExists = true;
                break;
            }
        }

        if (isExists)
        {
            Debug.Log($"Logging in '{usernameInput.text}'");
            loadWelcomeScreen();
        }
        else
        {
            Debug.Log("Incorrect credentials");
        }
    }

    void moveToSignUp()
    {
        SceneManager.LoadScene("signup");
    }

    void loadWelcomeScreen()
    {
        SceneManager.LoadScene("account");
    }

    void back(){
        SceneManager.LoadScene("firstPage1");
    }
}