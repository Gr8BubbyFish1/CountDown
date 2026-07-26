using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;

    void Update()
    {
        highScoreText.text = $"High Score:\n {StaticManager.highScore} Games";
    }
}