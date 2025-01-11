using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarScript : MonoBehaviour
{
    WheelJoint2D[] wheelJoints;

    public float maxSpeed = 1000f;//швидкість вперед
    public float maxbackSpeed = -1000f;//назад
    public float acceleration = 250f;//прискорення
    public float brakeForce = 1000f;//сила гальмування
    public LayerMask map;
    public Transform bwheel;//посилання на заднє колесо
    private float motorSpeed = 0f;//швидкість двигуна
    private bool grounded;//чи машина на землі

    public int moneyInt = 0;//кількість монет зібраних
    public Text moneyText;//текст для монет
    public GameObject fp;//фінішна панель
    public float wheelSize = 0.078f;//початковий розмір колеса
    private AudioSource carSound;//звук для машини
    public AudioSource moneySound;//звук для монет

    public float fuelSize = 25f;//повний бак
    public float fuelUsage = 0.5f;//скільки машина їсть
    private float currentFuel;//поточна кількість топлива
    public GameObject fuelProgressBar;//наша панель яка зменшується
    private Slider fuelSlider;
    public float fuelAdd = 17f; //скільки добавляється бензину коли підбираєм каністру

    void Start()
    {
        LoadCarStats();
        wheelJoints = gameObject.GetComponents<WheelJoint2D>();//всі колеса
        if (wheelJoints.Length == 0) Debug.LogWarning("Не знайдено жодного WheelJoint2D на цьому об'єкті.");//якщо нема колес

        carSound = GetComponent<AudioSource>();//добавляєм звук машин
        if (carSound == null) Debug.LogWarning("AudioSource не призначено!");

        currentFuel = fuelSize;//поточний рівень пального рівний початковому
        if (fuelProgressBar != null)
        {
            fuelProgressBar.transform.localScale = new Vector3(currentFuel / fuelSize, 1, 1);//налаштуванання панелі пального
        }
        else
        {//панель не призначено
            Debug.LogWarning("fuelProgressBar не призначено!");
        }
    }

    void Update()
    {//оновлюєм текст кількості монет
        if (moneyText != null)
        {
            moneyText.text = moneyInt.ToString();
        }
        else
        {//якщо тексту нема
            Debug.LogWarning("moneyText не призначено!");
        }

        if (bwheel != null)//чи колесо на землі
        {
            grounded = Physics2D.OverlapCircle(bwheel.transform.position, wheelSize, map);
        }
        else
        {//колесо не призначено
            Debug.LogWarning("bwheel не призначено!");
        }
        //якщо кінчився бензин машина зупиняється і не їде а в консолі відображається текст
        if (currentFuel <= 0)
        {
            motorSpeed = 0f;
            Debug.Log("Бензин закінчився!");
        }
    }

    void FixedUpdate()
    {
        foreach (var wheelJoint in wheelJoints)
        {
            JointMotor2D motor = wheelJoint.motor;//налаштування мотору
            motor.motorSpeed = motorSpeed;//настройки мотора
            motor.maxMotorTorque = brakeForce;//сила тормозіння
            wheelJoint.motor = motor;//Застосовуєм наші налаштування
        }

        if (carSound != null)//зміна висоти звуку машини від швидкості
        {
            carSound.pitch = Mathf.Clamp(-motorSpeed / 1000f, 0.3f, 3f);
        }

        // Споживання пального під час руху
        if (motorSpeed != 0 && currentFuel > 0)
        {
            ConsumeFuel(Time.fixedDeltaTime * fuelUsage);
        }
    }

    public void Accelerate(int direction)
    {
        if (currentFuel > 0)
        {
            if (direction == 1)
            {
                motorSpeed += acceleration * Time.fixedDeltaTime;//швидкіть вперед
                motorSpeed = Mathf.Clamp(motorSpeed, 0, maxSpeed);//обмеження швидкості 
            }
            else if (direction == -1)
            {
                motorSpeed -= acceleration * Time.fixedDeltaTime;//швидкість назад
                motorSpeed = Mathf.Clamp(motorSpeed, maxbackSpeed, 0);//обмеження швидкосі
            }
            else
            {
                if (motorSpeed > 0)
                {
                    motorSpeed -= brakeForce * Time.fixedDeltaTime;//гальмування
                    if (motorSpeed < 0) motorSpeed = 0;
                }
                else if (motorSpeed < 0)
                {
                    motorSpeed += brakeForce * Time.fixedDeltaTime;//гальмування
                    if (motorSpeed > 0) motorSpeed = 0;
                }
            }
        }
    }

    void ConsumeFuel(float amount)
    {
        currentFuel -= amount;//зменшення кількості пального
        if (currentFuel < 0) currentFuel = 0;

        // Оновлення розміру панелі
        if (fuelProgressBar != null)
        {
            fuelProgressBar.transform.localScale = new Vector3(currentFuel / fuelSize, 1, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag("money"))
        {
            Destroy(trigger.gameObject);//видаляєм монету на яку наїхали 
            moneyInt++;//монети додаються
            moneySound.pitch = Mathf.Clamp(moneySound.pitch + 0.1f, 0.5f, 3f);//звук моонти стає іншого тону коли наї
            moneySound.Play();
            if (moneyText != null) moneyText.text = moneyInt.ToString();//оновлення тексту
        }
        else if (trigger.CompareTag("Finish"))//якщо трігер нажатий
        {
            if (fp != null) fp.SetActive(true);
        }
        else if (trigger.gameObject.CompareTag("fuel"))//пальне додається коли находим трігер пальне
        {
            currentFuel += fuelAdd;//додаавння пального
            currentFuel = Mathf.Min(currentFuel, fuelSize); 
            Destroy(trigger.gameObject);//знищується каністра
        }
    }

    void OnDrawGizmos()
    {
        if (bwheel != null)
        {
            Gizmos.DrawWireSphere(bwheel.transform.position, wheelSize);//візуалізація радіусу колеса
        }
    }
    void LoadCarStats()
    {
        int selectedCar = PlayerPrefs.GetInt("c", 0); // Отримання індексу вибраної машини

        maxSpeed = PlayerPrefs.GetFloat("maxSpeed" + selectedCar, 1000f); // Передня
        maxbackSpeed = PlayerPrefs.GetFloat("maxbackSpeed" + selectedCar, -1000f); // Задня

        Debug.Log($"Максимальна швидкість машини {selectedCar}: Передня {maxSpeed}, Задня {maxbackSpeed}");
    }

    public void ImproveSpeed(float increaseAmount, int carIndex)
    {
        if (PlayerPrefs.GetInt("c") == carIndex)
        {
            maxSpeed += increaseAmount;  // Збільшити передню
            maxbackSpeed -= increaseAmount;  // Робимо задню ще більш від'ємною

            PlayerPrefs.SetFloat("maxSpeed" + carIndex, maxSpeed);
            PlayerPrefs.SetFloat("maxbackSpeed" + carIndex, maxbackSpeed);
            // PlayerPrefs.Save();
            PlayerPrefs.DeleteAll(); // Зітріть всі дані перед тестами

            Debug.Log($"Покращення: Передня {maxSpeed}, Задня {maxbackSpeed}");//вивід тюнингу машини в консоль
        }
        else
        {
            Debug.Log("Це не вибрана машина.");//якщо машина не вибрана
        }
    }


}
