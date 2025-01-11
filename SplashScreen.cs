using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public Image ProgressPanel;  // ������ ��������
    public Text ContinueText;     // ����� "��������, ��� ����������"
    public float ShrinkDuration = 5f;  // ��������� ��������� (5 ���)

    private float elapsedTime = 0f;
    private bool isPanelShrinking = true;
    private AudioSource nimSound;// �������� ����

    void Start()
    {
        nimSound = GetComponent<AudioSource>(); //����
        ContinueText.gameObject.SetActive(false);  // �������� ����� ��������
    }

    void Update()
    {
        if (isPanelShrinking)
        {
            // �������� ���, �� �������
            elapsedTime += Time.deltaTime;

            // �������� ������ ��������  
            float shrinkAmount = Mathf.Lerp(1f, 0f, elapsedTime / ShrinkDuration);

            // ��������� ������ �����
            if (ProgressPanel != null)
            {
                ProgressPanel.transform.localScale = new Vector3(shrinkAmount, 1, 1);
            }

            // ���� ����� ���� ������� �� �� ��������� 
            if (elapsedTime >= ShrinkDuration)
            {
                isPanelShrinking = false;  // ��������� ������
                ProgressPanel.gameObject.SetActive(false);  // ��������� ������
                ContinueText.gameObject.SetActive(true);  // �������� �����
            }
        }

        // ������� ���������� �����
        if (!isPanelShrinking && Input.anyKeyDown)
        {
            // ����������� ����
            SceneManager.LoadScene("Menu"); 
        }
    }
}
