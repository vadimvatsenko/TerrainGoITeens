using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private Button button;

    private void OnEnable()
    {
        button.onClick.AddListener(ChangeLevel);
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }

    public void ChangeLevel()
    {
        SceneManager.LoadScene(level);
    }
}
