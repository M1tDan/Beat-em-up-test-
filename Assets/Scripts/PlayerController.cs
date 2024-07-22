using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Этот класс управляет основным персонажем игрока
[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    public GameObject player, knife; // Ссылки на объекты игрока и ножа
    public Slider PlayerHPSlider, EnemyHPSlider; // Ссылки на слайдеры здоровья игрока и врага
    public GameObject PlayerHP, EnemyHP; // Ссылки на панели здоровья игрока и врага
    public TMP_Text PlayerHPText, EnemyHPText; // Ссылки на тексты для отображения здоровья игрока и врага

    public DialogMenuController dialogMenuController; // Ссылка на контроллер диалогового меню
    public CameraFollowingAndPoints cameraFollowingAndPoints; // Ссылка на скрипт следования камеры
    public GameObject GoNext; // Ссылка на объект кнопки перехода к следующему действию

    private int playerHealth = 10; // Начальное здоровье игрока
    public bool isAttacking = false; // Флаг, указывающий на то, идет ли атака в данный момент
    private EnemyHealth currentEnemy; // Ссылка на текущего врага

    [SerializeField] private Rigidbody _rigidbody; // Ссылка на Rigidbody игрока
    [SerializeField] private FixedJoystick _joystick; // Ссылка на виртуальный джойстик

    [SerializeField] public float _moveSpeed = 4; // Скорость движения игрока
    private Vector3 moveDirection; // Направление движения игрока

    public Animator animator; // Ссылка на компонент Animator игрока

    public GameObject[] FightingButtons; // Массив кнопок управления боевыми действиями

    private void Start()
    {
        // Устанавливаем начальные параметры анимации и тени игрока
        animator.SetInteger("IsMoving", 0);
        animator.SetFloat("AnimSpeed", 1);
        animator.SetFloat("lastHorizontalMove", 1);
        player.GetComponent<SpriteRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        // Установите начальное значение слайдера здоровья игрока
        PlayerHPSlider.maxValue = playerHealth;
        PlayerHPSlider.value = playerHealth;

        PlayerHPText.text = PlayerHPSlider.value + "/" + PlayerHPSlider.maxValue;

        // Находим камеру и диалоговое меню в сцене
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

        // Обновляем скорость игрока
        _rigidbody.velocity = new Vector3(moveDirection.x * _moveSpeed, _rigidbody.velocity.y, moveDirection.z * _moveSpeed);

        // Управление анимацией движения игрока
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

        // Управление скоростью анимации при ускорении
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

        // Управление атаками и ножом
        HandleAttacks();
    }

    //Расчёт движения через джойстик
    private void HandleJoystickInput()
    {
        if (isAttacking == false && (_joystick != null && (_joystick.Horizontal != 0f || _joystick.Vertical != 0f)))
            moveDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
    }

    //Расчёт движения через кливиатуру
    private void HandleKeyboardInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (isAttacking == false && (moveX != 0f || moveY != 0f))
            moveDirection = new Vector3(moveX, 0, moveY);
    }

    private void HandleAttacks()
    {
        // Управление атаками через клавиши Q, E, R, Space
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

    // Метод для атаки 1
    public void Attack1()
    {
        StartAttack();
        animator.SetTrigger("Attack1");
        Invoke("EndAttack", 0.4f);
    }

    // Метод для атаки 2
    public void Attack2()
    {
        StartAttack();
        animator.SetTrigger("Attack2");
        Invoke("EndAttack", 0.4f);
    }

    // Метод для атаки 3
    public void Attack3()
    {
        StartAttack();
        animator.SetTrigger("Attack3");
        Invoke("EndAttack", 0.4f);
    }

    // Метод для атаки ножом
    public void KnifeAttack()
    {
        StartAttack();
        animator.SetTrigger("KnifeAttack");
        Invoke("EndAttack", 0.4f);
    }

    // Обработчик столкновений с триггерами
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, столкновение с предметом
        if (other.CompareTag("Item"))
            FightingButtons[4].SetActive(true);

        // Проверяем, столкновение с врагом
        if (other.CompareTag("enemy"))
        {
            EnemyHP.SetActive(true);
            EnemyHPText.text = PlayerHPSlider.value + "/" + PlayerHPSlider.maxValue;
            currentEnemy = other.GetComponent<EnemyHealth>();
            if (currentEnemy != null)
            {
                // Устанавливаем максимальное и текущее значение слайдера здоровья врага
                EnemyHPSlider.maxValue = currentEnemy.startingHealth;
                EnemyHPSlider.value = currentEnemy.currentHealth;
                EnemyHPText.text = EnemyHPSlider.value + "/" + EnemyHPSlider.maxValue;
            }
        }

        // Обработка различных триггеров по их имени
        HandleTriggerByName(other.gameObject.name);
    }

    // Обработчик выхода из триггера
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

    // Метод для взятия ножа
    public void TakeKnife()
    {
        knife.SetActive(false);

        // Управление кнопками в зависимости от наличия ножа
        FightingButtons[0].SetActive(false);
        FightingButtons[1].SetActive(false);
        FightingButtons[2].SetActive(false);
        FightingButtons[3].SetActive(true);
        FightingButtons[4].SetActive(false);
        FightingButtons[5].SetActive(true);

        // Управление анимацией взятия ножа
        if (animator.GetFloat("lastHorizontalMove") == 1)
            animator.Play("PlayerKnife_Idle_Right");
        else if (animator.GetFloat("lastHorizontalMove") == -1)
            animator.Play("PlayerKnife_Idle_Left");
        else
            animator.Play("PlayerKnife_Idle_Right");

        animator.SetBool("HaveKnife", true);
    }

    // Метод для сброса ножа
    public void DropKnife()
    {
        // Управление кнопками после сброса ножа
        FightingButtons[0].SetActive(true);
        FightingButtons[1].SetActive(true);
        FightingButtons[2].SetActive(true);
        FightingButtons[3].SetActive(false);
        FightingButtons[4].SetActive(false);
        FightingButtons[5].SetActive(false);

        // Управление анимацией после сброса ножа
        if (animator.GetFloat("lastHorizontalMove") == 1)
            animator.Play("Player_Idle_Right");
        else if (animator.GetFloat("lastHorizontalMove") == -1)
            animator.Play("Player_Idle_Left");
        else
            animator.Play("Player_Idle_Right");

        animator.SetBool("HaveKnife", false);
    }

    // Метод для нанесения урона игроку
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        PlayerHPSlider.value = playerHealth; // Обновляем слайдер здоровья игрока

        if (playerHealth <= 0)
            Debug.Log("Player Died");
    }

    // Метод для нанесения урона врагу
    private void DealDamageToEnemy()
    {
        if (currentEnemy != null)
        {
            if (animator.GetBool("HaveKnife") == true)
                currentEnemy.TakeDamage(2); // Наносим повышенный урон врагу
            else
                currentEnemy.TakeDamage(1); // Наносим урон врагу

            EnemyHPSlider.value = currentEnemy.currentHealth; // Обновляем слайдер здоровья врага
            EnemyHPText.text = EnemyHPSlider.value + "/" + EnemyHPSlider.maxValue;
        }

        // Проверка смерти врага
        if (EnemyHPSlider.value <= 0)
        {
            EnemyHP.SetActive(false);
            cameraFollowingAndPoints.GoNextActivate();
        }
    }

    // Метод для начала атаки
    public void StartAttack()
    {
        isAttacking = true;
    }

    // Метод для окончания атаки
    public void EndAttack()
    {
        isAttacking = false;
    }

    // Обработчик столкновений с триггерами по имени
    private void HandleTriggerByName(string triggerName)
    {
        // Проверяем имя триггера и отправляем соответствующее сообщение камере
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

    // Обработчик триггеров, взаимодействующих с камерой
    private void HandleCameraTrigger(string triggerName)
    {
        // Ищем скрипт камеры
        CameraFollowingAndPoints cameraScript = Object.FindFirstObjectByType<CameraFollowingAndPoints>();

        // Проверяем, что камера найдена
        if (cameraScript != null)
        {
            // Отправляем сообщение методу на камере
            cameraScript.SendMessage("HandlePlayerCollision", triggerName, SendMessageOptions.DontRequireReceiver);
        }

        // Деактивируем триггер после использования
        GameObject triggerObject = GameObject.Find(triggerName);
        if (triggerObject != null)
        {
            triggerObject.SetActive(false);
        }
    }

    // Обработчик триггера диалога
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
        isAttacking = true; // Устанавливаем состояние атаки в true при начале диалога
    }
}
