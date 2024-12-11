using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class welcomePage : MonoBehaviour
{
    public Button signInButton;
    public Button signUpButton;
    // Start is called before the first frame update
    public Button navigationButton;
    // Start is called before the first frame update
    void Start()
    {
        signInButton.onClick.AddListener(signIn);
        signUpButton.onClick.AddListener(signUp);
        navigationButton.onClick.AddListener(navigation);
    }

    void signIn()
    {
        SceneManager.LoadScene("signin");
    }

    void signUp(){
        SceneManager.LoadScene("signup");
    }

    void navigation()
    {
        SceneManager.LoadScene("IndoorNavigation");
    }

}
