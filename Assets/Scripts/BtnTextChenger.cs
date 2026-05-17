using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BtnTextChenger : MonoBehaviour
{
    public Button buttonToChange; // кнопка, текст якої треба змінити
    private string[] randomTexts = { "Привіт", "Unity", "Кнопка", "Текст змінено" }; // масив випадкових рядків

    // Метод для зміни тексту кнопки
    public void ChangeText()
    {
        string randomText = randomTexts[Random.Range(0, randomTexts.Length)]; // вибираємо випадковий рядок
        buttonToChange.GetComponentInChildren<TextMeshProUGUI>().text = randomText; // змінюємо текст кнопки
    }
}
