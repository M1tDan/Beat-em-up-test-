using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DialogMenuController : MonoBehaviour
{
    // ������ �� ��������� �������� ��� ����� ����������
    public GameObject TriggerPoint1, TriggerPoint2, TriggerGaragePoint, TriggerPoint3, GoToGarageTrigger, OutOfGarageTrigger;

    // ������ �� ������� ������� UI
    public GameObject joystick, dialogMenu, portret1, portret2, PlayerHP, EnemyHP, GoNext;
    public GameObject[] FightingButtons; // ������ ������ ��� ������� ����������
    public TMP_Text CharacterName, DialogText; // ��������� ���� ��� ����� ��������� � ������ �������
    public Animator dialogAnimator; // �������� ��� ���������� ��������� �������
    private int currentPartOfDialog = 0; // ������� ����� �������

    // ������ ��� ����� � ����� �����
    public Button pause;
    public GameObject pauseText;

    private void Update()
    {
        // ���� ������� ����� 3 �� ������� � ����� �������� ������ ��� ����� ������ ����
        if (TriggerPoint3.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            PlayNextDialog(); // ������������� ��������� ����� �������
        }
    }

    // ��������������� ��������� ����� �������
    public void PlayNextDialog()
    {
        if (currentPartOfDialog == 1)
        {
            NextDialog(); // ��������� � ��������� ����� �������
        }
        else if (currentPartOfDialog == 2)
        {
            EndDialog(); // ��������� ������
        }
        else
        {
            StartDialog(); // �������� ������
        }
    }

    // ���������� � ������� ����� ��������, ��� �� ��������� ������
    public void PrepareToDialog()
    {
        Vector3 targetPosition = new Vector3(51f, 2.9f, -4f);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, 3 * Time.deltaTime);
        transform.position = smoothedPosition;

        Invoke("StartDialog", 0.1f); // ��������� ������ ����� ��������
    }

    // ������ �������
    public void StartDialog()
    {
        // �������� �������� ������� ����������
        foreach (var button in FightingButtons)
        {
            button.SetActive(false);
        }

        joystick.gameObject.SetActive(false); // �������� ��������
        PlayerHP.SetActive(false); // �������� ������ �������� ������
        EnemyHP.SetActive(false); // �������� ������ �������� �����
        GoNext.SetActive(false); // �������� ������ ���������� ����

        dialogMenu.SetActive(true); // ���������� ���������� ����

        portret1.SetActive(true); // ���������� ������ �������
        portret2.SetActive(false); // ������������ ������ �������
        CharacterName.text = "�����"; // ������������� ��� ���������
        DialogText.text = "����� ������� �����-�� ���������, � ���� ���� ����� ����."; // ������������� ����� �������
        currentPartOfDialog = 1; // ������������� ������� ����� �������
    }

    // ��������� ����� �������
    public void NextDialog()
    {
        portret1.SetActive(false); // ������������ ������ �������
        portret2.SetActive(true); // ���������� ������ �������
        CharacterName.text = "NPC"; // ������������� ��� ���������
        DialogText.text = "����� ���� ���� �����-�� ����� ����."; // ������������� ����� �������
        currentPartOfDialog = 2; // ������������� ������� ����� �������
    }

    // ���������� �������
    public void EndDialog()
    {
        portret1.SetActive(true); // ���������� ������ �������
        portret2.SetActive(false); // ������������ ������ �������
        CharacterName.text = "�����"; // ������������� ��� ���������
        DialogText.text = "������ �������."; // ������������� ����� �������
        dialogAnimator.Play("Dark"); // ��������� �������� ��������� �������
        Invoke("BackToMenu", 3f); // ���� 2 ������� � ������������ � ����
    }

    // ����������� � ������� ����
    private void BackToMenu()
    {
        SceneManager.LoadScene(0); // ��������� ����� �������� ����
    }
}
