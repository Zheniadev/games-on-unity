using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsDisplayHandler : MonoBehaviour 
{
    private Image[] stars;
    public string keyName = "K"; //��� ���� ��� ���� ���� �� ����

    void Awake()
    {
        stars = GetComponentsInChildren<Image>();//������ ���� � ������ ����������
    }

    void Start()
    {
        int starCount = PlayerPrefs.GetInt(keyName, 0); // 0 ���� � ������ ������� 

        if (stars.Length < 3)
        {
            Debug.LogError("Not enough stars assigned to the StarsDisplayHandler object!");
            return;
        }
        //���� � ��� ���� ���� ��� �������
        if (starCount == 3)
        {

            stars[1].color = new Color(stars[1].color.r, stars[1].color.g, stars[1].color.b, 1);
            stars[2].color = new Color(stars[2].color.r, stars[2].color.g, stars[2].color.b, 1);
            stars[3].color = new Color(stars[3].color.r, stars[3].color.g, stars[3].color.b, 1);

        }//���� � �� ���� ���� ��� �������
        else if (starCount == 2)
        {
            stars[1].color = new Color(stars[1].color.r, stars[1].color.g, stars[1].color.b, 1);
            stars[2].color = new Color(stars[2].color.r, stars[2].color.g, stars[2].color.b, 1);
            stars[3].color = new Color(stars[3].color.r, stars[3].color.g, stars[3].color.b, 0.5f);

        }//���� � ���� ���� ���� ��� �������
        else if (starCount == 1)
        {
            stars[1].color = new Color(stars[1].color.r, stars[1].color.g, stars[1].color.b, 1);
            stars[2].color = new Color(stars[2].color.r, stars[2].color.g, stars[2].color.b, 0.5f);
            stars[3].color = new Color(stars[3].color.r, stars[3].color.g, stars[3].color.b, 0.5f);

        }//���� ���� ������ �� ���� ��
        else
        {

            stars[1].color = new Color(stars[1].color.r, stars[1].color.g, stars[1].color.b, 0.5f);
            stars[2].color = new Color(stars[2].color.r, stars[2].color.g, stars[2].color.b, 0.5f);
            stars[3].color = new Color(stars[3].color.r, stars[3].color.g, stars[3].color.b, 0.5f);
        }
    }
}