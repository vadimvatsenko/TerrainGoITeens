using UnityEngine;
using UnityEngine.UI;

// новый учень Святослав, що вмієЮ на чому зупинився.

// продовжуємо верстати UI, зробимо перемикач рівнів, розглянемо Домашку
// Кахут по UI буде завтра
// Input manager розглянемо завтра

public class MenuSwitcher : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;
    //[SerializeField] private GameObject levelSelectMenuPanel;
    
    [Header("Buttons")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button backMenumenuButton;
    [SerializeField] private Button settingsButton;
    //[SerializeField] private Button levelSelectButton;

    private void OnEnable()
    {
        exitButton.onClick.AddListener(ExitGame);
        backMenumenuButton.onClick.AddListener(ShowMainMenu);
        settingsButton.onClick.AddListener(ShowSettingsMenu);
        //levelSelectButton.onClick.AddListener(ShowLevelSelectMenu);
    }

    private void OnDisable()
    {
        exitButton.onClick.RemoveListener(ExitGame);
        backMenumenuButton.onClick.RemoveListener(ShowMainMenu);
        settingsButton.onClick.RemoveListener(ShowSettingsMenu);
        //levelSelectButton.onClick.RemoveListener(ShowLevelSelectMenu);
    }

    private void Start()
    {
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        HideAllMenus();
        mainMenuPanel.SetActive(true);
    }

    private void ShowSettingsMenu()
    {
        HideAllMenus();
        settingsMenuPanel.SetActive(true);
    }

    private void ShowLevelSelectMenu()
    {
        HideAllMenus();
        //levelSelectMenuPanel.SetActive(true);
    }

    private void HideAllMenus()
    {
        mainMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        //levelSelectMenuPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }
}
