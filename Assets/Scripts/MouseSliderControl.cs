using UnityEngine;                  // Підключаємо базові класи Unity
using UnityEngine.UI;               // Підключаємо UI, щоб працювати зі Slider
using UnityEngine.EventSystems;     // Підключаємо інтерфейси для кліку та перетягування мишкою
using TMPro;                        // Підключаємо TextMeshPro для виводу відсотків

public class MouseSliderControl : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private Slider slider;               // Сюди підключаємо Slider з Inspector

    [SerializeField] private TextMeshProUGUI percentText; // Сюди підключаємо TMP Text для показу відсотків

    private void Awake()
    {
        // Якщо ми забули перетягнути Slider в Inspector
        if (slider == null)
        {
            // Тоді пробуємо знайти Slider на тому ж об'єкті, де висить цей скрипт
            slider = GetComponent<Slider>();
        }

        // Одразу оновлюємо текст відсотків при запуску сцени
        UpdatePercentText();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Цей метод викликається, коли ми натиснули мишкою по Slider

        // Встановлюємо значення слайдера залежно від позиції мишки
        SetSliderValue(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Цей метод викликається, коли ми рухаємо мишку із затиснутою кнопкою

        // Встановлюємо значення слайдера залежно від позиції мишки
        SetSliderValue(eventData);
    }

    private void SetSliderValue(PointerEventData eventData)
    {
        // Отримуємо RectTransform у Slider
        // RectTransform відповідає за розмір і позицію UI-елемента
        RectTransform rect = slider.GetComponent<RectTransform>();

        // Переводимо позицію мишки з координат екрана
        // у локальні координати всередині Slider
        bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,                      // Усередині якого UI-об'єкта шукаємо позицію
            eventData.position,         // Позиція мишки на екрані
            eventData.pressEventCamera, // Камера, через яку був клік
            out Vector2 localPoint      // Сюди Unity запише локальну позицію мишки
        );

        // Якщо позицію мишки вдалося перевести у координати Slider
        if (isInside)
        {
            // Рахуємо значення від 0 до 1
            // 0 — це лівий край Slider
            // 1 — це правий край Slider
            float normalizedValue = Mathf.InverseLerp(
                rect.rect.xMin,         // Ліва межа Slider
                rect.rect.xMax,         // Права межа Slider
                localPoint.x            // Поточна позиція мишки по X
            );

            // Обмежуємо значення від 0 до 1
            // Щоб слайдер не виходив за межі 0% і 100%
            normalizedValue = Mathf.Clamp01(normalizedValue);

            // Переводимо значення 0..1 у реальне значення Slider
            // Наприклад, якщо maxValue = 100, то 0.5 стане 50
            slider.value = normalizedValue * slider.maxValue;

            // Після зміни Slider оновлюємо текст з відсотками
            UpdatePercentText();
        }
    }

    private void UpdatePercentText()
    {
        // Якщо TMP Text не підключений в Inspector, нічого не робимо
        if (percentText == null)
        {
            return;
        }

        // Рахуємо відсоток заповнення Slider
        // slider.value — поточне значення
        // slider.maxValue — максимальне значення
        float percent = slider.value / slider.maxValue * 100f;

        // Округлюємо відсотки до цілого числа
        int roundedPercent = Mathf.RoundToInt(percent);

        // Записуємо текст, наприклад: 75%
        percentText.text = roundedPercent + "%";
    }
}