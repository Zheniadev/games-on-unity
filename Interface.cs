using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour
{
    public GameObject[] controlCar;
    public CarScript cs;
    public Text moneyCount; //виводим скільки в нас монет в рівні
    public string menuSceneName = "Menu"; //менє сцена
    public int moneyTarget3 = 18;//3 зірки якщо 18
    public int moneyTarget2 = 17;//тут дві
    public int moneyTarget1 = 15;//тут3
    public Image[] stars;
    public string keyName = "K";//ключ для рівнів
    public GameObject[] cars;//масив машин
    public CameraAndBackgroundFollow sc;
    private bool isPaused = false;
    public GameObject pp;
    private bool moneyAded = false; // щоб додавня було 1 раз

    void Start()
    {//зберігаєм наші машини у ключ с, дивимось чи він активний
        cs = cars[PlayerPrefs.GetInt("c")].GetComponent<CarScript>();
        cars[PlayerPrefs.GetInt("c")].SetActive(true);
    }

    void Update()
    {
        if (cs != null && cs.fp.activeSelf) // Перевірка на null
        {
            PlayerPrefs.SetInt(keyName, cs.moneyInt);//зберігає зібрані монети
            PlayerPrefs.Save();
            //включаєм управління машиною
            foreach (var car in controlCar)
            {
                var playerMovement = car.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.enabled = false;
                }
                car.SetActive(false);
            }
            //оновлюєм текст з кількістю монет
            if (moneyCount != null) // Перевірка на null
            {
                moneyCount.text = "Зібрано монет: " + cs.moneyInt.ToString();
            }

            UpdateStars();
            //загружаєм сцену меню коли натиснем по екрану
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(menuSceneName);
            }
        }

        // коли натискаєм esc включ пауза 
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && !cs.fp.activeSelf)
        {
            pauseOn();
        }//також виключається коли повторно
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            _continue();
        }
    }

    public void pauseOn()
    {//коли паузу увімкнено
        pp.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
    //коли вимкнено
    public void _continue()
    {
        pp.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
    //заходим в меню наший час відмирає
    public void gmenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(menuSceneName);
    }

    public void SelectCar(int carIndex)
    {//зберігаєм вибраний нами автомобіль
        PlayerPrefs.SetInt("SelectedCar", carIndex);
        PlayerPrefs.Save();

        Debug.Log("Selected Car Saved Index: " + carIndex);
        //всі машини вимкнені
        foreach (var car in cars)
        {
            car.SetActive(false);
        }
        //активуєм вибрану машину
        if (carIndex >= 0 && carIndex < cars.Length)
        {
            cars[carIndex].SetActive(true);
        }
    }

    private void UpdateStars()
    {
        if (cs == null) return; // Перевірка на null

        // Оновлення зірок відповідно до кількості монет
        if (cs.moneyInt >= moneyTarget3)
        {//якщо 18 монет зберігаєм 3 зірки
            SetStarsAlpha(1, 1, 1);
            PlayerPrefs.SetInt(keyName, 3);
        }//якщо 17 монет зберігаєм 2 зірки
        else if (cs.moneyInt >= moneyTarget2)
        {
            SetStarsAlpha(1, 1, 0.5f);
            PlayerPrefs.SetInt(keyName, 2);
        }//якщо 16 монет зберігаєм 11 зірку
        else if (cs.moneyInt >= moneyTarget1)
        {
            SetStarsAlpha(1, 0.5f, 0.5f);
            PlayerPrefs.SetInt(keyName, 1);
        }//якщо >16 монет зберігаєм 0 зірjr
        else
        {
            SetStarsAlpha(0.5f, 0.5f, 0.5f);
            PlayerPrefs.SetInt(keyName, 0);
        }

        // Додавання монет один раз
        if (!moneyAded)
        {
            int totalMoney = PlayerPrefs.GetInt("money", 0); // поточна кількість монет
            totalMoney += cs.moneyInt; // Додаємо монети
            PlayerPrefs.SetInt("money", totalMoney); // Оновлюємо збережену кількість монет
            PlayerPrefs.Save(); // Зберігаємо зміни
            moneyAded = true; // щоб монети додавались раз
        }
    }

    private void SetStarsAlpha(float alpha1, float alpha2, float alpha3)
    {
        if (stars.Length >= 3) // Перевірка на кількість зірок
        {
            stars[0].color = new Color(stars[0].color.r, stars[0].color.g, stars[0].color.b, alpha1);
            stars[1].color = new Color(stars[1].color.r, stars[1].color.g, stars[1].color.b, alpha2);
            stars[2].color = new Color(stars[2].color.r, stars[2].color.g, stars[2].color.b, alpha3);
        }
    }
}
