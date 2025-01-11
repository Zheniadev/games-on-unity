using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour
{
    public GameObject[] controlCar;
    public CarScript cs;
    public Text moneyCount; //������� ������ � ��� ����� � ���
    public string menuSceneName = "Menu"; //��� �����
    public int moneyTarget3 = 18;//3 ���� ���� 18
    public int moneyTarget2 = 17;//��� ��
    public int moneyTarget1 = 15;//���3
    public Image[] stars;
    public string keyName = "K";//���� ��� ����
    public GameObject[] cars;//����� �����
    public CameraAndBackgroundFollow sc;
    private bool isPaused = false;
    public GameObject pp;
    private bool moneyAded = false; // ��� ������� ���� 1 ���

    void Start()
    {//������� ���� ������ � ���� �, �������� �� �� ��������
        cs = cars[PlayerPrefs.GetInt("c")].GetComponent<CarScript>();
        cars[PlayerPrefs.GetInt("c")].SetActive(true);
    }

    void Update()
    {
        if (cs != null && cs.fp.activeSelf) // �������� �� null
        {
            PlayerPrefs.SetInt(keyName, cs.moneyInt);//������ ����� ������
            PlayerPrefs.Save();
            //������� ��������� �������
            foreach (var car in controlCar)
            {
                var playerMovement = car.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.enabled = false;
                }
                car.SetActive(false);
            }
            //�������� ����� � ������� �����
            if (moneyCount != null) // �������� �� null
            {
                moneyCount.text = "ǳ����� �����: " + cs.moneyInt.ToString();
            }

            UpdateStars();
            //�������� ����� ���� ���� �������� �� ������
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(menuSceneName);
            }
        }

        // ���� �������� esc ����� ����� 
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && !cs.fp.activeSelf)
        {
            pauseOn();
        }//����� ����������� ���� ��������
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            _continue();
        }
    }

    public void pauseOn()
    {//���� ����� ��������
        pp.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
    //���� ��������
    public void _continue()
    {
        pp.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
    //������� � ���� ����� ��� ������
    public void gmenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(menuSceneName);
    }

    public void SelectCar(int carIndex)
    {//������� �������� ���� ���������
        PlayerPrefs.SetInt("SelectedCar", carIndex);
        PlayerPrefs.Save();

        Debug.Log("Selected Car Saved Index: " + carIndex);
        //�� ������ �������
        foreach (var car in cars)
        {
            car.SetActive(false);
        }
        //������� ������� ������
        if (carIndex >= 0 && carIndex < cars.Length)
        {
            cars[carIndex].SetActive(true);
        }
    }

    private void UpdateStars()
    {
        if (cs == null) return; // �������� �� null

        // ��������� ���� �������� �� ������� �����
        if (cs.moneyInt >= moneyTarget3)
        {//���� 18 ����� ������� 3 ����
            SetStarsAlpha(1, 1, 1);
            PlayerPrefs.SetInt(keyName, 3);
        }//���� 17 ����� ������� 2 ����
        else if (cs.moneyInt >= moneyTarget2)
        {
            SetStarsAlpha(1, 1, 0.5f);
            PlayerPrefs.SetInt(keyName, 2);
        }//���� 16 ����� ������� 11 ����
        else if (cs.moneyInt >= moneyTarget1)
        {
            SetStarsAlpha(1, 0.5f, 0.5f);
            PlayerPrefs.SetInt(keyName, 1);
        }//���� >16 ����� ������� 0 ��jr
        else
        {
            SetStarsAlpha(0.5f, 0.5f, 0.5f);
            PlayerPrefs.SetInt(keyName, 0);
        }

        // ��������� ����� ���� ���
        if (!moneyAded)
        {
            int totalMoney = PlayerPrefs.GetInt("money", 0); // ������� ������� �����
            totalMoney += cs.moneyInt; // ������ ������
            PlayerPrefs.SetInt("money", totalMoney); // ��������� ��������� ������� �����
            PlayerPrefs.Save(); // �������� ����
            moneyAded = true; // ��� ������ ���������� ���
        }
    }

    private void SetStarsAlpha(float alpha1, float alpha2, float alpha3)
    {
        if (stars.Length >= 3) // �������� �� ������� ����
        {
            stars[0].color = new Color(stars[0].color.r, stars[0].color.g, stars[0].color.b, alpha1);
            stars[1].color = new Color(stars[1].color.r, stars[1].color.g, stars[1].color.b, alpha2);
            stars[2].color = new Color(stars[2].color.r, stars[2].color.g, stars[2].color.b, alpha3);
        }
    }
}
