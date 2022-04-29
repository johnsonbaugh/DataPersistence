using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private InputField nameInputUI;
    [SerializeField] private Text scoreTextUI;

    const string defaultName = "Joe";

    private void Start()
    {
        var highScores = GameManager.Instance.highScores;

        string scoreText = "";
        foreach (var score in highScores.list)
        {
            scoreText += score.name + " " + score.score + "\n";
        }
        scoreTextUI.text = scoreText;

        nameInputUI.text = GameManager.Instance.playerName;
    }

    public void StartGame()
    {
        if (GameManager.Instance.playerName.Equals(""))
            GameManager.Instance.playerName = defaultName;

        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void OnNameChanged()
    {
        GameManager.Instance.playerName = nameInputUI.text;
    }
}
