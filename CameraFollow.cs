
using UnityEngine;

public class CameraAndBackgroundFollow : MonoBehaviour
{
    public Transform target1;  // ����� ������
    public Transform target2;  // ����� ������
    public Transform target3;  // ����� ������
    public Vector3 offset = new Vector3(0, 0, -10); // ������������ ������ �� �� �����
    public float smoothSpeed = 0.125f; // ��������

    private Transform currentTarget;

    void Start()
    {
        // ������ �� ������� ������
        UpdateTarget();
    }

    void LateUpdate()
    {
        // �������� �� ������ ������� � ������
        UpdateTarget();

        // ������ �� ������� ���� ���� �
        if (currentTarget != null)
        {
            Vector3 desiredPosition = currentTarget.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
    //������� ��� ����� ����� �� ����
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
