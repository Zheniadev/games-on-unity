using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject levelChanger;//���� ������
    public GameObject exitPanel; //������ ������
    public Image[] cars;
    public GameObject garagePanel;//������ ������
    public Button[] bttns;//������
    public Text[] carsText;
    public string[] levels;


    public Text moneyText;

    public int[] carPrices = { 0, 50, 100 }; // ����� ���� �� ���
    private bool[] carPurchased; // ������� ��� ������ ����� �������
    bool anyCarPurchased = false;
    public CarScript currentCar; // ������ �����

    void Start()
    {
        if (!PlayerPrefs.HasKey("money"))//����� ����������� � ��� ����
        {
            PlayerPrefs.SetInt("money", 0);
        }

        // �� ������ ���� 
        foreach (var c in cars)
        {
            c.color = Color.black;
        }

        carPurchased = new bool[cars.Length];

        // �������� ���� ����� � �� ���� �����������
        for (int i = 0; i < cars.Length; i++)
        {
            carPurchased[i] = PlayerPrefs.GetInt("carPurchased" + i, i == 0 ? 1 : 0) == 1;
            if (carPurchased[i])
            {
                anyCarPurchased = true;
            }
        }

        UpdateMoneyUI();
        HighlightSelectedCar(); // �������� ���� ������ ������� ��� � �� �����
    }

    void Update()
    {
        // ���������� ������ Escape
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
            exitPanel.SetActive(true); // ³�������� exitPanel
        }
        else if (exitPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            exitPanel.SetActive(false); // ��������� exitPanel
        }

        moneyText.text = PlayerPrefs.GetInt("money").ToString();
    }

public void BuyCar(int carIndex)
{
    int currentMoney = PlayerPrefs.GetInt("money");

    // �� ��� ����. �� � � �� ����� money
    if (!carPurchased[carIndex] && currentMoney >= carPrices[carIndex])
    {
        currentMoney -= carPrices[carIndex];
        PlayerPrefs.SetInt("money", currentMoney);
        PlayerPrefs.SetInt("carPurchased" + carIndex, 1);
        PlayerPrefs.Save();

        carPurchased[carIndex] = true;
        anyCarPurchased = true; // ���������
        UpdateMoneyUI();
        Debug.Log("��������� " + carIndex + " �������.");
    }
    else if (carPurchased[carIndex])
    {
        Debug.Log("��� ��������� ��� ��������.");
    }
    else
    {
        Debug.Log("����������� ����� ��� �����.");
    }
}


    public void OnClickStart()
    {
        levelChanger.SetActive(true); // ³�������� levelchanger ��� �������� ��.
    }

    public void OnClickExit()
    {
        exitPanel.SetActive(true); // ³�������� Exit panel ���� ���������
    }

    public void OnConfirmExit()
    {
        Application.Quit(); // ��������� ���

#if UNITY_EDITOR
        // ���� � �������� Unity, ��������� ��� �� ��� ��������
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnCancelExit()
    {
        exitPanel.SetActive(false); // ��������� ���. ������
    }

    public void LevelBttns(int level)
    {
        if (anyCarPurchased == true)
        {
        SceneManager.LoadScene(level); // ������������ ����� � �������� �����
        }
        else
        {
            Debug.Log("������ ����� ���������.");
        }
    }

    public void carChanger(int car)
    {
        // �������� �� �� ����� ������
        if (carPurchased[car])
        {
            PlayerPrefs.SetInt("c", car);
            PlayerPrefs.Save();

            // �� �� ������ ����������� ������ ����
            foreach (var c in cars)
            {
                c.color = Color.black;
            }

            // ���� ������ ������� ���� ���
            if (car >= 0 && car < cars.Length)
            {
                cars[car].color = Color.white;
            }
        }
        else
        {
            Debug.Log("������ ����� ��� ���������.");//���� ������ �� ����������
        }
    }
    //������� ��� ���� ��� ��� ���� ������ � �����. ������
    public void SelectCar(int carIndex)
    {
        if (carPurchased[carIndex])
        {
            PlayerPrefs.SetInt("c", carIndex);
            PlayerPrefs.Save();
            HighlightSelectedCar();
            Debug.Log("������� ��������� " + carIndex);
        }
        else
        {
            Debug.Log("������ ����� ��� ���������.");
        }
    }
    public void UpgradeSpeedForCar(int carIndex)
    {
        int cost = 10; // ������� �������
        float speedIncrease = 50f; // �� ������ �������� ������

        // �������� �� �� �� �����
        if (PlayerPrefs.GetInt("money") >= cost)
        {
            // ���� ������� �������� ������� ��� ���
            float currentMaxSpeed = PlayerPrefs.GetFloat("maxSpeed" + carIndex, 1000f); // ������� ��������.�����
            float newMaxSpeed = currentMaxSpeed + speedIncrease;
            PlayerPrefs.SetFloat("maxSpeed" + carIndex, newMaxSpeed); // �������� ���� ������� ��������

            // ����� ����� ���� ��� ��� ���� 
            float currentMaxBackSpeed = PlayerPrefs.GetFloat("maxbackSpeed" + carIndex, -1000f); // ����� ��������,�����
            float newMaxBackSpeed = currentMaxBackSpeed - speedIncrease; 
            PlayerPrefs.SetFloat("maxbackSpeed" + carIndex, newMaxBackSpeed); // �������� ���� ����� ��������

            PlayerPrefs.Save(); // ����������

            // ³������ �������� �����
            SpendMoney(cost); 
            //���� �� ����� ��� ���
            Debug.Log($"�������� ������ {carIndex} ���������: ������� {newMaxSpeed}, ����� {newMaxBackSpeed}");
        }
        else
        {
            Debug.Log("����������� ����� ��� ���������� ��������!");
        }
    }
    //������� ��� �������� �����
    void SpendMoney(int amount)
    {
        int currentMoney = PlayerPrefs.GetInt("money");
        currentMoney -= amount;
        PlayerPrefs.SetInt("money", currentMoney);
        PlayerPrefs.Save();
        UpdateMoneyUI();
    }

    //���� ����� �� �����
    private void UpdateMoneyUI()
    {
        moneyText.text = PlayerPrefs.GetInt("money").ToString();
    }

    private void HighlightSelectedCar()
    {
        // �������� ������ ���� ���� �� �������
        foreach (var c in cars)
        {
            c.color = Color.black;
        }

        int selectedCar = PlayerPrefs.GetInt("c", 0);

        // �� ������ �������
        if (carPurchased[selectedCar])
        {
            cars[selectedCar].color = Color.white;//���� �������� ������ ��� ����
        }
    }
    public void ResetCarSpeed(int carIndex)
    {
        // ����� �������� �� ������ ��������
        PlayerPrefs.SetFloat("maxSpeed" + carIndex, 1000f); //������� ��������
        PlayerPrefs.SetFloat("maxbackSpeed" + carIndex, -1000f); //����� ��������

        PlayerPrefs.Save(); // ���������� ���

        Debug.Log($"�������� ������ {carIndex} �������: ������� {1000f}, ����� {-1000f}");//���� �� �����
    }

    public void OnResetSpeedButton(int carIndex)
    {
        ResetCarSpeed(carIndex); // ������ ������
    }
    // ������� ��� �������� ������ �� �������
    public void ResetCarPurchases()
    {
        for (int i = 0; i < carPurchased.Length; i++)
        {
            PlayerPrefs.SetInt("carPurchased" + i, 0); // �������� ������
        }
        PlayerPrefs.Save();
        Debug.Log("������ ������");
    }

    // ���� ��� �� ��������
    public void OnPriceUpdate()
    {
        carPrices[1] = 20; // ���� ���. 1
        carPrices[2] = 40; // ��� ���. 2
        SetCarPrices(); // ����� ��� ��������� ����� ���
    }

    // ��������� ��� ���������
    private void SetCarPrices()
    {
        for (int i = 0; i < carPrices.Length; i++)
        {
            PlayerPrefs.SetInt("carPrice" + i, carPrices[i]); // ��� ���� �����������   � PlayerPrefs
        }
        PlayerPrefs.Save(); // ���������� ���
        Debug.Log("ֳ�� ��������");
    }

    // ���� ������� �����
    public void OnMoneyUpdate(int newMoneyAmount)
    {
        PlayerPrefs.SetInt("money", newMoneyAmount); // �������� �����
        PlayerPrefs.Save(); // ���������� ���
        UpdateMoneyUI(); // UI ���� ������� �����
        Debug.Log("���� ������� �����: " + newMoneyAmount); //��������
    }

public void ResetLevels()
    {
        // ���������� �� ���
        foreach (var level in levels)
        {
            PlayerPrefs.SetInt(level, 0); // ������� ������� ��� ������� ����
        }

        PlayerPrefs.Save(); // �������� ����
        Debug.Log("������� ���� �������.");
    }
}
