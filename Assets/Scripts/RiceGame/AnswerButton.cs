using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    [SerializeField] TMP_Text label;

    private int value;
    private RiceManager manager;

    public void Setup(int answerValue, RiceManager gameManager)
    {
        value = answerValue;
        manager = gameManager;

        label.text = value.ToString();
    }

    public void OnPressed()
    {
        manager.SubmitAnswer(value);
    }
}