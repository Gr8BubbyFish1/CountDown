using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RiceManager : MonoBehaviour
{
    [SerializeField] private Vector2 spawnAreaCenter;
    [SerializeField] private Vector2 spawnAreaSize;

    private int riceCountMean;
    private float riceCountStd;
    private int riceCountMax = 50;
    private int riceCountMin = 5;
    private int riceCount;
    private int correctCount;

    [SerializeField] private GameObject ricePrefab;
    [SerializeField] private float minDistance;
    private List<Vector2> ricePositions = new();

    private List<int> answerChoices = new();
    [SerializeField] private AnswerButton[] answerButtons;

    private bool riceEnd;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DifficultySelect(StaticManager.totalRoundsPlayed);

        riceEnd = false;
        riceCount = GetRandomNormal(riceCountMean, riceCountStd, riceCountMin, riceCountMax);

        ricePositions.Clear();

        // Find spawn positions for each rice grain
        for (int i = 0; i < riceCount; i++)
        {
            bool found = false;

            // Keep trying until it doesn't fall too close to another rice, if it takes too many attempts
            for (int attempt = 0; attempt < 100; attempt++)
            {
                Vector2 p = RandomPoint();

                if (IsValid(p))
                {
                    ricePositions.Add(p);
                    Instantiate(ricePrefab, p, Quaternion.Euler(0f, 0f, Random.Range(0f, 180f)));
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Debug.Log("Couldn't find room for another object.");
                break;
            }
        }

        riceCount = ricePositions.Count;
        correctCount = riceCount;
        answerChoices.Add(correctCount);

        // Adds believable incorrect options to possible guesses
        // "believable" means within ~4 of the correct count
        while (answerChoices.Count < 4)
        {
            int offset = Random.Range(-4, 4);

            if (offset == 0)
                continue;

            int guess = Mathf.Max(0, correctCount + offset);

            if (!answerChoices.Contains(guess))
                answerChoices.Add(guess);
        }

        // Shuffle answer choices
        for (int i = answerChoices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            (answerChoices[i], answerChoices[j]) = (answerChoices[j], answerChoices[i]);
        }

        // Assign answer choices to the buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].Setup(answerChoices[i], this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (StaticManager.hasTimeRunOut() && !riceEnd)
        {
            riceEnd = true;
            Debug.Log("Time up");
            StaticManager.removeLife();
            StaticManager.EndMiniGame();
        }
    }

    private int GetRandomNormal(int mean, float std, int min, int max)
    {
        float sum = 0f;

        for (int i = 0; i < 6; i++)
        {
            sum += Random.value;
        }

        float normal = (sum - 3f) / 0.707f;

        int result = Mathf.RoundToInt(mean + normal * std);

        return Mathf.Clamp(result, min, max);
    }

    private Vector2 RandomPoint()
    {
        return new Vector2(
            Random.Range(
                spawnAreaCenter.x - spawnAreaSize.x / 2,
                spawnAreaCenter.x + spawnAreaSize.x / 2),
            Random.Range(
                spawnAreaCenter.y - spawnAreaSize.y / 2,
                spawnAreaCenter.y + spawnAreaSize.y / 2));
    }

    private bool IsValid(Vector2 point)
    {
        foreach (Vector2 other in ricePositions)
        {
            if (Vector2.Distance(point, other) < minDistance)
                return false;
        }

        return true;
    }

    public void SubmitAnswer(int guess)
    {
        if (guess == correctCount && !riceEnd)
        {
            //Debug.Log("Correct!");
            // Win
            riceEnd = true;
            audioManager.PlaySFX(audioManager.winSFX);
            StaticManager.EndMiniGame();
        }
        else if (!riceEnd)
        {
            //Debug.Log("Wrong!");
            // Lose
            riceEnd = true;
            StaticManager.removeLife();
            StaticManager.EndMiniGame();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spawnAreaCenter + spawnAreaSize / 4, new Vector2(minDistance, minDistance));
    }

    private void DifficultySelect(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                riceCountMean = 10;
                riceCountStd = 1f;
                StaticManager.setTimer(15);
                break;
            case 1:
                riceCountMean = 15;
                riceCountStd = 1.5f;
                StaticManager.setTimer(13);
                break;
            case 2:
                riceCountMean = 20;
                riceCountStd = 2f;
                StaticManager.setTimer(12);
                break;
            case 3:
                riceCountMean = 25;
                riceCountStd = 2f;
                StaticManager.setTimer(11);
                break;
            default:
                riceCountMean = 25;
                riceCountStd = 2.5f;
                StaticManager.setTimer(10);
                break;
        }
    }
}
