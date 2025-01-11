using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    // ������������� ��� ������
    public Vector3 offset;   // ³������ �� �������� �������
    public float smoothSpeed = 0.125f; //���� ������

    // ������������ ��� ����
    public Transform background; // ������ �� ����� ���
    public float parallaxEffect = 0.05f; // ���� �������

    private Transform target; // ���� ��� ���
    private Vector3 lastCameraPosition; //�� ������ ����������� �� ��� 

    void Start()
    {
        // ��������� ������ �������� ��'��� � ����� "Car"
        FindCarTarget();
        lastCameraPosition = transform.position;
    }

    void LateUpdate()
    {
        // ����������, �� � ����; ���� �, ������ �����
        if (target == null)
        {
            FindCarTarget();
            return;
        }

        // ��������� ���������������  ������
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z) + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // ��������� ���������������� ���� �� � ���������
        Vector3 deltaMovement = transform.position - lastCameraPosition;
        background.position += new Vector3(deltaMovement.x * parallaxEffect, deltaMovement.y * parallaxEffect, 0);

        // ��������� ������� ������� ������ �� ���������� �����
        lastCameraPosition = transform.position;
    }

    // ����������� ��������� ��'���� � ����� "Car"
    void FindCarTarget()
    {
        GameObject car = GameObject.FindGameObjectWithTag("Car");
        if (car != null)
        {
            target = car.transform;
        }
    }
}
