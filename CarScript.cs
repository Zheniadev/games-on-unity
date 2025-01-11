using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarScript : MonoBehaviour
{
    WheelJoint2D[] wheelJoints;

    public float maxSpeed = 1000f;//�������� ������
    public float maxbackSpeed = -1000f;//�����
    public float acceleration = 250f;//�����������
    public float brakeForce = 1000f;//���� �����������
    public LayerMask map;
    public Transform bwheel;//��������� �� ���� ������
    private float motorSpeed = 0f;//�������� �������
    private bool grounded;//�� ������ �� ����

    public int moneyInt = 0;//������� ����� �������
    public Text moneyText;//����� ��� �����
    public GameObject fp;//������ ������
    public float wheelSize = 0.078f;//���������� ����� ������
    private AudioSource carSound;//���� ��� ������
    public AudioSource moneySound;//���� ��� �����

    public float fuelSize = 25f;//������ ���
    public float fuelUsage = 0.5f;//������ ������ ����
    private float currentFuel;//������� ������� �������
    public GameObject fuelProgressBar;//���� ������ ��� ����������
    private Slider fuelSlider;
    public float fuelAdd = 17f; //������ ������������ ������� ���� ������� �������

    void Start()
    {
        LoadCarStats();
        wheelJoints = gameObject.GetComponents<WheelJoint2D>();//�� ������
        if (wheelJoints.Length == 0) Debug.LogWarning("�� �������� ������� WheelJoint2D �� ����� ��'���.");//���� ���� �����

        carSound = GetComponent<AudioSource>();//��������� ���� �����
        if (carSound == null) Debug.LogWarning("AudioSource �� ����������!");

        currentFuel = fuelSize;//�������� ����� �������� ����� �����������
        if (fuelProgressBar != null)
        {
            fuelProgressBar.transform.localScale = new Vector3(currentFuel / fuelSize, 1, 1);//�������������� ����� ��������
        }
        else
        {//������ �� ����������
            Debug.LogWarning("fuelProgressBar �� ����������!");
        }
    }

    void Update()
    {//�������� ����� ������� �����
        if (moneyText != null)
        {
            moneyText.text = moneyInt.ToString();
        }
        else
        {//���� ������ ����
            Debug.LogWarning("moneyText �� ����������!");
        }

        if (bwheel != null)//�� ������ �� ����
        {
            grounded = Physics2D.OverlapCircle(bwheel.transform.position, wheelSize, map);
        }
        else
        {//������ �� ����������
            Debug.LogWarning("bwheel �� ����������!");
        }
        //���� ������� ������ ������ ����������� � �� ��� � � ������ ������������ �����
        if (currentFuel <= 0)
        {
            motorSpeed = 0f;
            Debug.Log("������ ���������!");
        }
    }

    void FixedUpdate()
    {
        foreach (var wheelJoint in wheelJoints)
        {
            JointMotor2D motor = wheelJoint.motor;//������������ ������
            motor.motorSpeed = motorSpeed;//��������� ������
            motor.maxMotorTorque = brakeForce;//���� ���������
            wheelJoint.motor = motor;//���������� ���� ������������
        }

        if (carSound != null)//���� ������ ����� ������ �� ��������
        {
            carSound.pitch = Mathf.Clamp(-motorSpeed / 1000f, 0.3f, 3f);
        }

        // ���������� �������� �� ��� ����
        if (motorSpeed != 0 && currentFuel > 0)
        {
            ConsumeFuel(Time.fixedDeltaTime * fuelUsage);
        }
    }

    public void Accelerate(int direction)
    {
        if (currentFuel > 0)
        {
            if (direction == 1)
            {
                motorSpeed += acceleration * Time.fixedDeltaTime;//������� ������
                motorSpeed = Mathf.Clamp(motorSpeed, 0, maxSpeed);//��������� �������� 
            }
            else if (direction == -1)
            {
                motorSpeed -= acceleration * Time.fixedDeltaTime;//�������� �����
                motorSpeed = Mathf.Clamp(motorSpeed, maxbackSpeed, 0);//��������� �������
            }
            else
            {
                if (motorSpeed > 0)
                {
                    motorSpeed -= brakeForce * Time.fixedDeltaTime;//�����������
                    if (motorSpeed < 0) motorSpeed = 0;
                }
                else if (motorSpeed < 0)
                {
                    motorSpeed += brakeForce * Time.fixedDeltaTime;//�����������
                    if (motorSpeed > 0) motorSpeed = 0;
                }
            }
        }
    }

    void ConsumeFuel(float amount)
    {
        currentFuel -= amount;//��������� ������� ��������
        if (currentFuel < 0) currentFuel = 0;

        // ��������� ������ �����
        if (fuelProgressBar != null)
        {
            fuelProgressBar.transform.localScale = new Vector3(currentFuel / fuelSize, 1, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag("money"))
        {
            Destroy(trigger.gameObject);//�������� ������ �� ��� ������ 
            moneyInt++;//������ ���������
            moneySound.pitch = Mathf.Clamp(moneySound.pitch + 0.1f, 0.5f, 3f);//���� ������ ��� ������ ���� ���� ��
            moneySound.Play();
            if (moneyText != null) moneyText.text = moneyInt.ToString();//��������� ������
        }
        else if (trigger.CompareTag("Finish"))//���� ����� �������
        {
            if (fp != null) fp.SetActive(true);
        }
        else if (trigger.gameObject.CompareTag("fuel"))//������ �������� ���� ������� ����� ������
        {
            currentFuel += fuelAdd;//��������� ��������
            currentFuel = Mathf.Min(currentFuel, fuelSize); 
            Destroy(trigger.gameObject);//��������� �������
        }
    }

    void OnDrawGizmos()
    {
        if (bwheel != null)
        {
            Gizmos.DrawWireSphere(bwheel.transform.position, wheelSize);//���������� ������ ������
        }
    }
    void LoadCarStats()
    {
        int selectedCar = PlayerPrefs.GetInt("c", 0); // ��������� ������� ������� ������

        maxSpeed = PlayerPrefs.GetFloat("maxSpeed" + selectedCar, 1000f); // �������
        maxbackSpeed = PlayerPrefs.GetFloat("maxbackSpeed" + selectedCar, -1000f); // �����

        Debug.Log($"����������� �������� ������ {selectedCar}: ������� {maxSpeed}, ����� {maxbackSpeed}");
    }

    public void ImproveSpeed(float increaseAmount, int carIndex)
    {
        if (PlayerPrefs.GetInt("c") == carIndex)
        {
            maxSpeed += increaseAmount;  // �������� �������
            maxbackSpeed -= increaseAmount;  // ������ ����� �� ���� ��'�����

            PlayerPrefs.SetFloat("maxSpeed" + carIndex, maxSpeed);
            PlayerPrefs.SetFloat("maxbackSpeed" + carIndex, maxbackSpeed);
            // PlayerPrefs.Save();
            PlayerPrefs.DeleteAll(); // ǳ���� �� ��� ����� �������

            Debug.Log($"����������: ������� {maxSpeed}, ����� {maxbackSpeed}");//���� ������� ������ � �������
        }
        else
        {
            Debug.Log("�� �� ������� ������.");//���� ������ �� �������
        }
    }


}
