using UnityEngine;

// ����� ��� ���������� ������� � ������� ����������
public class CameraFollowingAndPoints : MonoBehaviour
{
    public Transform player; // ������ �� ������ ���������
    public float yOffset = 0f; // �������� �� ��� Y
    public float onPosition = 0; // �������� �� ����� ���� �� ���������� ��������
    private float smoothSpeed = 2f; // �������� �������� ��������

    public GameObject GoNext, Player; // ������ ��  ������ � ������ ����������

    // ������� �� ��������� �������
    public GameObject FirstStageBariers, SecondStageBariers, GarageBarier, ThirdStageBariers;
    // �������� ��� ����� ���������� � ���������
    public GameObject TriggerPoint1, TriggerPoint2, TriggerGaragePoint, TriggerPoint3, GoToGarageTrigger, OutOfGarageTrigger;

    void LateUpdate()
    {
        // ���� ����� ����������, �� ������ ������� �� �������
        if (player != null && onPosition == 0)
        {
            Vector3 targetPosition = new Vector3(player.position.x, 2.9f, -4f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        // ������� 1 - ��� ������ ����� (�� ���������� ������� ������ �������� ���������� ������� �� ����� � �������� ��������� �� �������)
        else if (onPosition == 1)
        {
            Vector3 targetPosition = new Vector3(3f, 2.9f, -4f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        // ������� 2 - ��� ������ �����
        else if (onPosition == 2)
        {
            Vector3 targetPosition = new Vector3(21f, 2.9f, -4f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        // ������� 3 - ��� ������� �����
        else if (onPosition == 3)
        {
            Vector3 targetPosition = new Vector3(39f, 2.9f, -4f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        // ������� 4 - ��� ��������� ����� (��� �������)
        else if (onPosition == 4)
        {
            Vector3 targetPosition = new Vector3(49f, 2.9f, -4f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        // ������� 5 - ��� ����� �����
        else if (onPosition == 5)
        {
            Vector3 targetPosition = new Vector3(78f, 2.9f, -4f);
            transform.position = targetPosition;
        }
        // ������� 6 - ��� ������ �����
        else if (onPosition == 6)
        {
            Vector3 targetPosition = new Vector3(30f, 2.9f, -4f);
            transform.position = targetPosition;
            NormalFollowing(); // ���������� ������� ����������
        }
    }

    // ��������� ����� ����������
    public void GoNextActivate()
    {
        Debug.Log("Activating now.");
        GoNext.SetActive(true);
    }

    // ����������� ����� ���������� (�� ���������� ���������� �����)
    public void GoNextDisactivate()
    {
        Debug.Log("Disactivating now.");
        GoNext.SetActive(false);
    }

    // ������� ���������� �� �������
    public void NormalFollowing()
    {
        onPosition = 0;
    }

    // ��������� ����� ���������� 1
    public void ControllPoint1()
    {
        onPosition = 1;
    }

    // ��������� ����� ���������� 2
    public void ControllPoint2()
    {
        onPosition = 2;
    }

    // ��������� ����� ���������� 3
    public void ControllPoint3()
    {
        onPosition = 3;
    }

    /*
    // ������������������ ����� ��� ���������� ������ ���������� ��� ��������
    public void CutsceneControllPoint()
    {
        onPosition = 4;
    }
    */

    // ��������� ������������ ������ � ���������
    public void HandlePlayerCollision(string triggerName)
    {
        // � ����������� �� ����� �������� ��������� ������������ ��������
        if (triggerName == "FirstTrigger")
        {
            GoNextDisactivate(); // ������������ ����� ���������� �����
            FirstStageBariers.SetActive(true); // ���������� ������� �� ������ ������
            ControllPoint1(); // ������������� ����� ���������� 1
        }
        else if (triggerName == "SecondTrigger")
        {
            GoNextDisactivate(); // ������������ ����� ���������� �����
            SecondStageBariers.SetActive(true); // ���������� ������� �� ������ ������
            ControllPoint2(); // ������������� ����� ���������� 2
        }
        else if (triggerName == "ThirdTrigger")
        {
            GoNextDisactivate(); // ������������ ����� ���������� �����
            ThirdStageBariers.SetActive(true); // ���������� ������� �� ������� ������
            ControllPoint3(); // ������������� ����� ���������� 3
        }
        else if (triggerName == "GarageTrigger")
        {
            GarageBarier.SetActive(false); // ������������ ������ � ������
            OutOfGarageTrigger.SetActive(true); // ���������� ������� ������ �� ������
            TriggerGaragePoint.SetActive(false); // ��������� ������ ��� �� �� ��������� ���������� �������������
        }
        else if (triggerName == "DialogTrigger")
        {
            //dialogMenuController.StartDialog(); // ��������� ���������� ���� (����������������)
        }
        else if (triggerName == "GoToGarage")
        {
            GoNextDisactivate(); // ������������ ����� ���������� �����
            onPosition = 5; // ������������� ����� ���������� 5
            player.transform.position = new Vector3(78f, 1.05f, -1f); // ���������� ������ � �����
        }
        else if (triggerName == "OutOfGarage")
        {
            GoNextActivate(); // ������������ ����� ���������� �����
            onPosition = 6; // ������������� ����� ���������� 6
            player.transform.position = new Vector3(30f, 1.05f, 1f); // ���������� ������ �� ������
        }
    }
}
