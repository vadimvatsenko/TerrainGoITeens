using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button button;
    

    private void OnEnable()
    {
        button.onClick.AddListener(OnClickBtn);
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
    
    private void OnClickBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Clicked on button");
    }
}
