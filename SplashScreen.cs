using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public Image ProgressPanel;  // Панель завантаж
    public Text ContinueText;     // Текст "Натисніть, щоб продовжити"
    public float ShrinkDuration = 5f;  // Тривалість завантадж (5 сек)

    private float elapsedTime = 0f;
    private bool isPanelShrinking = true;
    private AudioSource nimSound;// добвляєм звук

    void Start()
    {
        nimSound = GetComponent<AudioSource>(); //звук
        ContinueText.gameObject.SetActive(false);  // Спочатку текст схований
    }

    void Update()
    {
        if (isPanelShrinking)
        {
            // Збільшуємо час, що пройшов
            elapsedTime += Time.deltaTime;

            // зменшуємо панель загрузки  
            float shrinkAmount = Mathf.Lerp(1f, 0f, elapsedTime / ShrinkDuration);

            // Оновлення розміру панелі
            if (ProgressPanel != null)
            {
                ProgressPanel.transform.localScale = new Vector3(shrinkAmount, 1, 1);
            }

            // Коли панелі нема показуєм те що приховано 
            if (elapsedTime >= ShrinkDuration)
            {
                isPanelShrinking = false;  // Зупиняємо панель
                ProgressPanel.gameObject.SetActive(false);  // Приховуємо панель
                ContinueText.gameObject.SetActive(true);  // Показуємо текст
            }
        }

        // Очікуємо натискання клавіш
        if (!isPanelShrinking && Input.anyKeyDown)
        {
            // Завантажуємо меню
            SceneManager.LoadScene("Menu"); 
        }
    }
}
