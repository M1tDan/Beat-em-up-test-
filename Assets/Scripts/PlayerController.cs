using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ���� ����� ��������� �������� ���������� ������
[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    public GameObject player, knife; // ������ �� ������� ������ � ����
    public Slider PlayerHPSlider, EnemyHPSlider; // ������ �� �������� �������� ������ � �����
    public GameObject PlayerHP, EnemyHP; // ������ �� ������ �������� ������ � �����
    public TMP_Text PlayerHPText, EnemyHPText; // ������ �� ������ ��� ����������� �������� ������ � �����

    public DialogMenuController dialogMenuController; // ������ �� ���������� ����������� ����
    public CameraFollowingAndPoints cameraFollowingAndPoints; // ������ �� ������ ���������� ������
    public GameObject GoNext; // ������ �� ������ ������ �������� � ���������� ��������

    private int playerHealth = 10; // ��������� �������� ������
    public bool isAttacking = false; // ����, ����������� �� ��, ���� �� ����� � ������ ������
    private EnemyHealth currentEnemy; // ������ �� �������� �����

    [SerializeField] private Rigidbody _rigidbody; // ������ �� Rigidbody ������
    [SerializeField] private FixedJoystick _joystick; // ������ �� ����������� ��������

    [SerializeField] public float _moveSpeed = 4; // �������� �������� ������
    private Vector3 moveDirection; // ����������� �������� ������

    public Animator animator; // ������ �� ��������� Animator ������

    public GameObject[] FightingButtons; // ������ ������ ���������� ������� ����������

    private void Start()
    {
        // ������������� ��������� ��������� �������� � ���� ������
        animator.SetInteger("IsMoving", 0);
        animator.SetFloat("AnimSpeed", 1);
        animator.SetFloat("lastHorizontalMove", 1);
        player.GetComponent<SpriteRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        // ���������� ��������� �������� �������� �������� ������
        PlayerHPSlider.maxValue = playerHealth;
        PlayerHPSlider.value = playerHealth;

        PlayerHPText.text = PlayerHPSlider.value + "/" + PlayerHPSlider.maxValue;

        // ������� ������ � ���������� ���� � �����
        cameraFollowingAndPoints = Object.FindFirstObjectByType<CameraFollowingAndPoints>();
        if (cameraFollowingAndPoints == null)
        {
            Debug.LogError("CameraFollowingAndPoints not found!");
        }

        dialogMenuController = Object.FindFirstObjectByType<DialogMenuController>();
    }

    private void Update()
    {
        moveDirection = Vector3.zero;

        HandleJoystickInput();
        HandleKeyboardInput();

        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
        }

        // ��������� �������� ������
        _rigidbody.velocity = new Vector3(moveDirection.x * _moveSpeed, _rigidbody.velocity.y, moveDirection.z * _moveSpeed);

        // ���������� ��������� �������� ������
        if (moveDirection.x != 0f || moveDirection.z != 0f)
        {
            animator.SetInteger("IsMoving", 1);
            if (moveDirection.x != 0f)
            {
                animator.SetFloat("lastHorizontalMove", moveDirection.x);
            }
            animator.SetFloat("HorizontalMove", animator.GetFloat("lastHorizontalMove"));
        }
        else
        {
            animator.SetInteger("IsMoving", 0);
        }

        // ���������� ��������� �������� ��� ���������
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _moveSpeed = 8;
            animator.SetFloat("AnimSpeed", 1.5f);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _moveSpeed = 4;
            animator.SetFloat("AnimSpeed", 1f);
        }

        // ���������� ������� � �����
        HandleAttacks();
    }

    //������ �������� ����� ��������
    private void HandleJoystickInput()
    {
        if (isAttacking == false && (_joystick != null && (_joystick.Horizontal != 0f || _joystick.Vertical != 0f)))
            moveDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
    }

    //������ �������� ����� ����������
    private void HandleKeyboardInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (isAttacking == false && (moveX != 0f || moveY != 0f))
            moveDirection = new Vector3(moveX, 0, moveY);
    }

    private void HandleAttacks()
    {
        // ���������� ������� ����� ������� Q, E, R, Space
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (FightingButtons[0].activeSelf == true)
                Attack1();
            else if (FightingButtons[5].activeSelf == true)
                KnifeAttack();
            else
            {
                DropKnife();
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
            Attack2();
        if (Input.GetKeyDown(KeyCode.R))
            Attack3();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (FightingButtons[4].activeSelf == true)
            {
                TakeKnife();
            }
            else if (FightingButtons[5].activeSelf == true)
                DropKnife();
            else
                return;
        }
    }

    // ����� ��� ����� 1
    public void Attack1()
    {
        StartAttack();
        animator.SetTrigger("Attack1");
        Invoke("EndAttack", 0.4f);
    }

    // ����� ��� ����� 2
    public void Attack2()
    {
        StartAttack();
        animator.SetTrigger("Attack2");
        Invoke("EndAttack", 0.4f);
    }

    // ����� ��� ����� 3
    public void Attack3()
    {
        StartAttack();
        animator.SetTrigger("Attack3");
        Invoke("EndAttack", 0.4f);
    }

    // ����� ��� ����� �����
    public void KnifeAttack()
    {
        StartAttack();
        animator.SetTrigger("KnifeAttack");
        Invoke("EndAttack", 0.4f);
    }

    // ���������� ������������ � ����������
    private void OnTriggerEnter(Collider other)
    {
        // ���������, ������������ � ���������
        if (other.CompareTag("Item"))
            FightingButtons[4].SetActive(true);

        // ���������, ������������ � ������
        if (other.CompareTag("enemy"))
        {
            EnemyHP.SetActive(true);
            EnemyHPText.text = PlayerHPSlider.value + "/" + PlayerHPSlider.maxValue;
            currentEnemy = other.GetComponent<EnemyHealth>();
            if (currentEnemy != null)
            {
                // ������������� ������������ � ������� �������� �������� �������� �����
                EnemyHPSlider.maxValue = currentEnemy.startingHealth;
                EnemyHPSlider.value = currentEnemy.currentHealth;
                EnemyHPText.text = EnemyHPSlider.value + "/" + EnemyHPSlider.maxValue;
            }
        }

        // ��������� ��������� ��������� �� �� �����
        HandleTriggerByName(other.gameObject.name);
    }

    // ���������� ������ �� ��������
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
            FightingButtons[4].SetActive(false);
        if (other.CompareTag("enemy"))
        {
            currentEnemy = null;
            EnemyHP.SetActive(false);
        }
    }

    // ����� ��� ������ ����
    public void TakeKnife()
    {
        knife.SetActive(false);

        // ���������� �������� � ����������� �� ������� ����
        FightingButtons[0].SetActive(false);
        FightingButtons[1].SetActive(false);
        FightingButtons[2].SetActive(false);
        FightingButtons[3].SetActive(true);
        FightingButtons[4].SetActive(false);
        FightingButtons[5].SetActive(true);

        // ���������� ��������� ������ ����
        if (animator.GetFloat("lastHorizontalMove") == 1)
            animator.Play("PlayerKnife_Idle_Right");
        else if (animator.GetFloat("lastHorizontalMove") == -1)
            animator.Play("PlayerKnife_Idle_Left");
        else
            animator.Play("PlayerKnife_Idle_Right");

        animator.SetBool("HaveKnife", true);
    }

    // ����� ��� ������ ����
    public void DropKnife()
    {
        // ���������� �������� ����� ������ ����
        FightingButtons[0].SetActive(true);
        FightingButtons[1].SetActive(true);
        FightingButtons[2].SetActive(true);
        FightingButtons[3].SetActive(false);
        FightingButtons[4].SetActive(false);
        FightingButtons[5].SetActive(false);

        // ���������� ��������� ����� ������ ����
        if (animator.GetFloat("lastHorizontalMove") == 1)
            animator.Play("Player_Idle_Right");
        else if (animator.GetFloat("lastHorizontalMove") == -1)
            animator.Play("Player_Idle_Left");
        else
            animator.Play("Player_Idle_Right");

        animator.SetBool("HaveKnife", false);
    }

    // ����� ��� ��������� ����� ������
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        PlayerHPSlider.value = playerHealth; // ��������� ������� �������� ������

        if (playerHealth <= 0)
            Debug.Log("Player Died");
    }

    // ����� ��� ��������� ����� �����
    private void DealDamageToEnemy()
    {
        if (currentEnemy != null)
        {
            if (animator.GetBool("HaveKnife") == true)
                currentEnemy.TakeDamage(2); // ������� ���������� ���� �����
            else
                currentEnemy.TakeDamage(1); // ������� ���� �����

            EnemyHPSlider.value = currentEnemy.currentHealth; // ��������� ������� �������� �����
            EnemyHPText.text = EnemyHPSlider.value + "/" + EnemyHPSlider.maxValue;
        }

        // �������� ������ �����
        if (EnemyHPSlider.value <= 0)
        {
            EnemyHP.SetActive(false);
            cameraFollowingAndPoints.GoNextActivate();
        }
    }

    // ����� ��� ������ �����
    public void StartAttack()
    {
        isAttacking = true;
    }

    // ����� ��� ��������� �����
    public void EndAttack()
    {
        isAttacking = false;
    }

    // ���������� ������������ � ���������� �� �����
    private void HandleTriggerByName(string triggerName)
    {
        // ��������� ��� �������� � ���������� ��������������� ��������� ������
        switch (triggerName)
        {
            case "FirstTrigger":
            case "SecondTrigger":
            case "GarageTrigger":
            case "ThirdTrigger":
            case "GoToGarage":
            case "OutOfGarage":
                HandleCameraTrigger(triggerName);
                break;
            case "DialogTrigger":
                HandleDialogTrigger();
                break;
            default:
                break;
        }
    }

    // ���������� ���������, ����������������� � �������
    private void HandleCameraTrigger(string triggerName)
    {
        // ���� ������ ������
        CameraFollowingAndPoints cameraScript = Object.FindFirstObjectByType<CameraFollowingAndPoints>();

        // ���������, ��� ������ �������
        if (cameraScript != null)
        {
            // ���������� ��������� ������ �� ������
            cameraScript.SendMessage("HandlePlayerCollision", triggerName, SendMessageOptions.DontRequireReceiver);
        }

        // ������������ ������� ����� �������������
        GameObject triggerObject = GameObject.Find(triggerName);
        if (triggerObject != null)
        {
            triggerObject.SetActive(false);
        }
    }

    // ���������� �������� �������
    private void HandleDialogTrigger()
    {
        if (dialogMenuController != null)
        {
            dialogMenuController.PrepareToDialog();
        }
        else
        {
            Debug.LogWarning("DialogMenuController is not assigned!");
        }
        isAttacking = true; // ������������� ��������� ����� � true ��� ������ �������
    }
}
