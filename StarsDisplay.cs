using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsDisplayHandler : MonoBehaviour 
{
    private Image[] stars;
    public string keyName = "K"; //наш ключ для рівнів який ми юзаєм

    void Awake()
    {
        stars = GetComponentsInChildren<Image>();//беремо зірки з нашого зображення
    }

    void Start()
    {
        int starCount = PlayerPrefs.GetInt(keyName, 0); // 0 зірок з самого початку 

        if (stars.Length < 3)
        {
            Debug.LogError("Not enough stars assigned to the StarsDisplayHandler object!");
            return;
        }
        //якщо є три зірки вони стає видимою
        if (starCount == 3)
        {

            stars[1].color = new Color(stars[1].color.r, stars[1].color.g, stars[1].color.b, 1);
            stars[2].color = new Color(stars[2].color.r, stars[2].color.g, stars[2].color.b, 1);
            stars[3].color = new Color(stars[3].color.r, stars[3].color.g, stars[3].color.b, 1);

        }//якщо є дві зірки вони стає видимою
        else if (starCount == 2)
        {
            stars[1].color = new Color(stars[1].color.r, stars[1].color.g, stars[1].color.b, 1);
            stars[2].color = new Color(stars[2].color.r, stars[2].color.g, stars[2].color.b, 1);
            stars[3].color = new Color(stars[3].color.r, stars[3].color.g, stars[3].color.b, 0.5f);

        }//якщо є одна зірка вона стає видимою
        else if (starCount == 1)
        {
            stars[1].color = new Color(stars[1].color.r, stars[1].color.g, stars[1].color.b, 1);
            stars[2].color = new Color(stars[2].color.r, stars[2].color.g, stars[2].color.b, 0.5f);
            stars[3].color = new Color(stars[3].color.r, stars[3].color.g, stars[3].color.b, 0.5f);

        }//якщо нема зірочок то зірки сірі
        else
        {

            stars[1].color = new Color(stars[1].color.r, stars[1].color.g, stars[1].color.b, 0.5f);
            stars[2].color = new Color(stars[2].color.r, stars[2].color.g, stars[2].color.b, 0.5f);
            stars[3].color = new Color(stars[3].color.r, stars[3].color.g, stars[3].color.b, 0.5f);
        }
    }
}