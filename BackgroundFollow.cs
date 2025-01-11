using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    // Налаштуванння для камери
    public Vector3 offset;   // Відстань між камероюі машиною
    public float smoothSpeed = 0.125f; //руху камери

    // Налаштування для фону
    public Transform background; // силкка на задній фон
    public float parallaxEffect = 0.05f; // Сила паралак

    private Transform target; // ціль для кам
    private Vector3 lastCameraPosition; //як камера розташована до маш 

    void Start()
    {
        // Знаходимо перший активний об'єкт з тегом "Car"
        FindCarTarget();
        lastCameraPosition = transform.position;
    }

    void LateUpdate()
    {
        // Перевіряємо, чи є ціль; якщо ні, шукаємо знову
        if (target == null)
        {
            FindCarTarget();
            return;
        }

        // Оновлення місцезнаходження  камери
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z) + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Оновлення місцезнаходженння фону де є паралакса
        Vector3 deltaMovement = transform.position - lastCameraPosition;
        background.position += new Vector3(deltaMovement.x * parallaxEffect, deltaMovement.y * parallaxEffect, 0);

        // Оновлюємо останню позицію камери до наступного кадру
        lastCameraPosition = transform.position;
    }

    // знаходження активного об'єкта з тегом "Car"
    void FindCarTarget()
    {
        GameObject car = GameObject.FindGameObjectWithTag("Car");
        if (car != null)
        {
            target = car.transform;
        }
    }
}
