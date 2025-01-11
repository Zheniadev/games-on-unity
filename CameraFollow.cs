
using UnityEngine;

public class CameraAndBackgroundFollow : MonoBehaviour
{
    public Transform target1;  // Перша машина
    public Transform target2;  // Друга машина
    public Transform target3;  // Третя машина
    public Vector3 offset = new Vector3(0, 0, -10); // налуштування камери що до обєкта
    public float smoothSpeed = 0.125f; // плавність

    private Transform currentTarget;

    void Start()
    {
        // слідкуєм за ативним обєктом
        UpdateTarget();
    }

    void LateUpdate()
    {
        // Перевірка чи машина активна і слідкуєм
        UpdateTarget();

        // Слідуємо за машиною якщо вона є
        if (currentTarget != null)
        {
            Vector3 desiredPosition = currentTarget.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
    //функція для нових машин на сцені
    void UpdateTarget()
    {
        if (target1 != null && target1.gameObject.activeInHierarchy)
        {
            currentTarget = target1;

        }
        else if (target2 != null && target2.gameObject.activeInHierarchy)
        {
            currentTarget = target2;

        }
        else if (target3 != null && target3.gameObject.activeInHierarchy)
        {
            currentTarget = target3;

        }
        else
        {
            currentTarget = null;
            Debug.Log("No active target to follow");
        }
    }

}
