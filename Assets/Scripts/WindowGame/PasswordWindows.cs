using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;


public class PasswordWindows : MonoBehaviour
{
    private int passwordLength;
    private String password;
    private int currentIndex;

    [SerializeField] private TextMeshProUGUI backText;
    [SerializeField] private TextMeshProUGUI frontText;
    
    public bool done;
    public Animator drapes;
    
    void OnEnable()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    void OnDisable()
    {
        Keyboard.current.onTextInput -= OnTextInput;
    }
    private void Start()
    {
        frontText.maxVisibleCharacters = 0;
        currentIndex = 0;
    }

    private void OnTextInput(char c)
    {
        if (char.IsLetter(c) && !done && password.Length > 0)
        {
            if (char.ToUpper(c) == password[currentIndex])
            {
                currentIndex++;
                frontText.maxVisibleCharacters = currentIndex;
                if (currentIndex >= passwordLength)
                {
                    done = true;
                    drapes.Play("TestDrapesAnimationClip");
                    //Debug.Log("Complete");
                }
            }
            else
            {
                currentIndex = 0;
                frontText.maxVisibleCharacters = currentIndex;
            }
        }
    }

    public void GeneratePassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        StringBuilder word = new StringBuilder(passwordLength);

        for (int i = 0; i < passwordLength; i++)
        {
            word.Append(chars[Random.Range(0, chars.Length)]);
        }
        password = word.ToString();
        backText.text = password;
        frontText.text = password;
    }

    public void setPasswordLength(int length)
    {
        passwordLength = length;
    }
    
}
