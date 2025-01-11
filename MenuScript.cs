using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject levelChanger;//зміна уровня
    public GameObject exitPanel; //панель виходу
    public Image[] cars;
    public GameObject garagePanel;//панель гаражу
    public Button[] bttns;//кнопки
    public Text[] carsText;
    public string[] levels;


    public Text moneyText;

    public int[] carPrices = { 0, 50, 100 }; // стара ціна за маш
    private bool[] carPurchased; // стежити яка машина зараз куплена
    bool anyCarPurchased = false;
    public CarScript currentCar; // машина зараз

    void Start()
    {
        if (!PlayerPrefs.HasKey("money"))//гроші зберігаються в цей ключ
        {
            PlayerPrefs.SetInt("money", 0);
        }

        // всі машини чорні 
        foreach (var c in cars)
        {
            c.color = Color.black;
        }

        carPurchased = new bool[cars.Length];

        // перевірка купл машин і чи вони зберігаються
        for (int i = 0; i < cars.Length; i++)
        {
            carPurchased[i] = PlayerPrefs.GetInt("carPurchased" + i, i == 0 ? 1 : 0) == 1;
            if (carPurchased[i])
            {
                anyCarPurchased = true;
            }
        }

        UpdateMoneyUI();
        HighlightSelectedCar(); // оновлюєм якщо машина вибрана біла а не чорна
    }

    void Update()
    {
        // натискання клавіші Escape
        if (levelChanger.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            levelChanger.SetActive(false);
        }
        else if (garagePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            garagePanel.SetActive(false);
        }
        else if (!exitPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            exitPanel.SetActive(true); // Відкриваємо exitPanel
        }
        else if (exitPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            exitPanel.SetActive(false); // Закриваємо exitPanel
        }

        moneyText.text = PlayerPrefs.GetInt("money").ToString();
    }

public void BuyCar(int carIndex)
{
    int currentMoney = PlayerPrefs.GetInt("money");

    // чи маш купл. чи ні і чи хватає money
    if (!carPurchased[carIndex] && currentMoney >= carPrices[carIndex])
    {
        currentMoney -= carPrices[carIndex];
        PlayerPrefs.SetInt("money", currentMoney);
        PlayerPrefs.SetInt("carPurchased" + carIndex, 1);
        PlayerPrefs.Save();

        carPurchased[carIndex] = true;
        anyCarPurchased = true; // Оновлюємо
        UpdateMoneyUI();
        Debug.Log("Автомобіль " + carIndex + " куплено.");
    }
    else if (carPurchased[carIndex])
    {
        Debug.Log("Цей автомобіль вже куплений.");
    }
    else
    {
        Debug.Log("Недостатньо монет для купівлі.");
    }
}


    public void OnClickStart()
    {
        levelChanger.SetActive(true); // Відкриваємо levelchanger щоб вибирати рів.
    }

    public void OnClickExit()
    {
        exitPanel.SetActive(true); // Відкриваємо Exit panel коли натиснуто
    }

    public void OnConfirmExit()
    {
        Application.Quit(); // закриваємо гру

#if UNITY_EDITOR
        // Якщо в редакторі Unity, зупиняємо гру під час розробки
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnCancelExit()
    {
        exitPanel.SetActive(false); // Закриваємо пан. виходу
    }

    public void LevelBttns(int level)
    {
        if (anyCarPurchased == true)
        {
        SceneManager.LoadScene(level); // Завантаження сцени з вибраним рівнем
        }
        else
        {
            Debug.Log("Спершу купіть автомобіль.");
        }
    }

    public void carChanger(int car)
    {
        // перевірка чи ти купив машину
        if (carPurchased[car])
        {
            PlayerPrefs.SetInt("c", car);
            PlayerPrefs.Save();

            // на всі машини поставляний чорний колір
            foreach (var c in cars)
            {
                c.color = Color.black;
            }

            // якщо машина куплена вона біла
            if (car >= 0 && car < cars.Length)
            {
                cars[car].color = Color.white;
            }
        }
        else
        {
            Debug.Log("Спершу купіть цей автомобіль.");//якщо машина не купленннна
        }
    }
    //фунцція для того щоб виб рати машину і вивод. інформ
    public void SelectCar(int carIndex)
    {
        if (carPurchased[carIndex])
        {
            PlayerPrefs.SetInt("c", carIndex);
            PlayerPrefs.Save();
            HighlightSelectedCar();
            Debug.Log("Вибрано автомобіль " + carIndex);
        }
        else
        {
            Debug.Log("Спершу купіть цей автомобіль.");
        }
    }
    public void UpgradeSpeedForCar(int carIndex)
    {
        int cost = 10; // Вартість тюнингу
        float speedIncrease = 50f; // на скільки скорость зростає

        // Перевірка чи ти не бєдний
        if (PlayerPrefs.GetInt("money") >= cost)
        {
            // нова поточна швидкість передня для маш
            float currentMaxSpeed = PlayerPrefs.GetFloat("maxSpeed" + carIndex, 1000f); // передня швидкість.перша
            float newMaxSpeed = currentMaxSpeed + speedIncrease;
            PlayerPrefs.SetFloat("maxSpeed" + carIndex, newMaxSpeed); // Зберігаємо нову передню швидкість

            // отрим поточ швик для маш задн 
            float currentMaxBackSpeed = PlayerPrefs.GetFloat("maxbackSpeed" + carIndex, -1000f); // задня швидкість,перша
            float newMaxBackSpeed = currentMaxBackSpeed - speedIncrease; 
            PlayerPrefs.SetFloat("maxbackSpeed" + carIndex, newMaxBackSpeed); // Зберігаємо нову задню швидкість

            PlayerPrefs.Save(); // збереження

            // Віднімаємо витрачені гроші
            SpendMoney(cost); 
            //вивід на екран тюн нгу
            Debug.Log($"Швидкість машини {carIndex} покращена: передня {newMaxSpeed}, задня {newMaxBackSpeed}");
        }
        else
        {
            Debug.Log("Недостатньо монет для покращення швидкості!");
        }
    }
    //функція щоб тратитти гроші
    void SpendMoney(int amount)
    {
        int currentMoney = PlayerPrefs.GetInt("money");
        currentMoney -= amount;
        PlayerPrefs.SetInt("money", currentMoney);
        PlayerPrefs.Save();
        UpdateMoneyUI();
    }

    //вивід монет на екран
    private void UpdateMoneyUI()
    {
        moneyText.text = PlayerPrefs.GetInt("money").ToString();
    }

    private void HighlightSelectedCar()
    {
        // ставляєм машину білою коли її вибирем
        foreach (var c in cars)
        {
            c.color = Color.black;
        }

        int selectedCar = PlayerPrefs.GetInt("c", 0);

        // чи машина куплена
        if (carPurchased[selectedCar])
        {
            cars[selectedCar].color = Color.white;//якщо купленна машина стає білою
        }
    }
    public void ResetCarSpeed(int carIndex)
    {
        // Видал передньої та задньої швидкості
        PlayerPrefs.SetFloat("maxSpeed" + carIndex, 1000f); //передня швидкість
        PlayerPrefs.SetFloat("maxbackSpeed" + carIndex, -1000f); //задня швидкість

        PlayerPrefs.Save(); // Збереження змін

        Debug.Log($"Швидкість машини {carIndex} скинута: передня {1000f}, задня {-1000f}");//вивід на екран
    }

    public void OnResetSpeedButton(int carIndex)
    {
        ResetCarSpeed(carIndex); // скидаєм тюнинг
    }
    // функція щоб видалити машини які купленні
    public void ResetCarPurchases()
    {
        for (int i = 0; i < carPurchased.Length; i++)
        {
            PlayerPrefs.SetInt("carPurchased" + i, 0); // видаляєм машини
        }
        PlayerPrefs.Save();
        Debug.Log("Машини скинуті");
    }

    // Зміна цін на автомобілі
    public void OnPriceUpdate()
    {
        carPrices[1] = 20; // ціна маш. 1
        carPrices[2] = 40; // цін маш. 2
        SetCarPrices(); // метод для зберігання нових цін
    }

    // Оновлення цін автомобілів
    private void SetCarPrices()
    {
        for (int i = 0; i < carPrices.Length; i++)
        {
            PlayerPrefs.SetInt("carPrice" + i, carPrices[i]); // нові ціни зберігаються   в PlayerPrefs
        }
        PlayerPrefs.Save(); // Збереження змін
        Debug.Log("Ціни оновлено");
    }

    // Зміна кількості монет
    public void OnMoneyUpdate(int newMoneyAmount)
    {
        PlayerPrefs.SetInt("money", newMoneyAmount); // новлення монет
        PlayerPrefs.Save(); // Збереження змін
        UpdateMoneyUI(); // UI нова кількість монет
        Debug.Log("Нова кількість монет: " + newMoneyAmount); //перевірка
    }

public void ResetLevels()
    {
        // Перебираємо всі рівні
        foreach (var level in levels)
        {
            PlayerPrefs.SetInt(level, 0); // Скидаємо прогрес для кожного рівня
        }

        PlayerPrefs.Save(); // Зберігаємо зміни
        Debug.Log("Прогрес рівнів скинуто.");
    }
}
