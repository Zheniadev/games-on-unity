
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CarScript carScript;

    void Update()
    {
        if (carScript != null) // Перевірка на null
        {
            if (Input.GetKey(KeyCode.D))
            {
                carScript.Accelerate(-1); // Вперед 
            }
            else if (Input.GetKey(KeyCode.A))
            {
                carScript.Accelerate(1); // Назад 
            }
            else
            {
                carScript.Accelerate(0); // Зупинка
            }
        }
    }
}
