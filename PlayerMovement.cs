
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CarScript carScript;

    void Update()
    {
        if (carScript != null) // �������� �� null
        {
            if (Input.GetKey(KeyCode.D))
            {
                carScript.Accelerate(-1); // ������ 
            }
            else if (Input.GetKey(KeyCode.A))
            {
                carScript.Accelerate(1); // ����� 
            }
            else
            {
                carScript.Accelerate(0); // �������
            }
        }
    }
}
