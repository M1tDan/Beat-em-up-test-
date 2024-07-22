using UnityEngine;

// ����� ��� ���������� ��������� �����
public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 5; // ��������� �������� �����
    public int currentHealth; // ������� �������� �����

    public GameObject FirstStageBariers, SecondStageBarier, GarageBarier, ThirdStageBarier; // ������ �� ������� � ��������� ������� ������

    public CameraFollowingAndPoints cameraFollowingAndPoints; // ������ �� ������ ���������� ������

    private void Start()
    {
        currentHealth = startingHealth; // ������������� ������� �������� ������ ����������
    }

    // ����� ��� ��������� �����
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // ��������� ������� �������� �� ��������� ���������� �����
        if (currentHealth <= 0)
        {
            Die(); // ���� �������� ������ ��� ����� ����, �������� ����� ������
        }
    }

    // ����� ���������� ��� ������ �����
    private void Die()
    {
        // ��������� ��������� �������� � ����������� �� ����� �����
        if (gameObject.name == "Enemy1")
        {
            FirstStageBariers.SetActive(false); // ��������� ������� �� ������ ������
            cameraFollowingAndPoints.NormalFollowing(); // ���������� ������� ���������� ������
            cameraFollowingAndPoints.GoNextActivate(); // ���������� ������ ��������
        }
        else if (gameObject.name == "Enemy2")
        {
            SecondStageBarier.SetActive(false); // ��������� ������� �� ������ ������
            cameraFollowingAndPoints.NormalFollowing(); // ���������� ������� ���������� ������
            cameraFollowingAndPoints.GoNextActivate(); // ���������� ������ ��������
        }
        else if (gameObject.name == "Enemy3")
        {
            ThirdStageBarier.SetActive(false); // ��������� ������� �� ������� ������
            cameraFollowingAndPoints.NormalFollowing(); // ���������� ������� ���������� ������
            cameraFollowingAndPoints.GoNextActivate(); // ���������� ������ ��������
        }

        // ���������� ����� ��� ������
        Destroy(gameObject);
    }
}
