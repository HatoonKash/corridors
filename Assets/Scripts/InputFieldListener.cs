using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class InputFieldListener : MonoBehaviour
{
    private InputField inputField;

    public bool CanUseSpace;
    private char[] symbols;
    private void Start()
    {
        inputField = GetComponent<InputField>();
        inputField.onValueChanged.AddListener(ChackEnglishWords);

        if (CanUseSpace)
        {
            symbols = "abcdefghijklmnopqrstuvwxyz_1234567890 ".ToCharArray();
        }
        else
        {
            symbols = "abcdefghijklmnopqrstuvwxyz_1234567890".ToCharArray();
        }
    }

    private void ChackEnglishWords(string line)
    {
        inputField.text = CleanInput(line);
    }
    private string CleanInput(string message)
    {
        // Replace invalid characters with empty strings.
        if(message.Length > 0)
        {
            
            char[] chars = message.ToLower().ToCharArray();
            char lastChar = chars[chars.Length-1];
            if(!symbols.Contains(lastChar))
            {
                message = message.Substring(0,message.Length-1);
            }
        }
        return message;
    }
    void OnDisable()
    {
        inputField.onValueChanged.RemoveAllListeners();
    }
}
