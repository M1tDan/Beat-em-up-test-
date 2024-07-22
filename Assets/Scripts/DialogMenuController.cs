using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DialogMenuController : MonoBehaviour
{
    // Ссылки на различные триггеры для точек следования
    public GameObject TriggerPoint1, TriggerPoint2, TriggerGaragePoint, TriggerPoint3, GoToGarageTrigger, OutOfGarageTrigger;

    // Ссылки на игровые объекты UI
    public GameObject joystick, dialogMenu, portret1, portret2, PlayerHP, EnemyHP, GoNext;
    public GameObject[] FightingButtons; // Массив кнопок для боевого интерфейса
    public TMP_Text CharacterName, DialogText; // Текстовые поля для имени персонажа и текста диалога
    public Animator dialogAnimator; // Аниматор для управления анимацией диалога
    private int currentPartOfDialog = 0; // Текущая часть диалога

    // Кнопка для паузы и текст паузы
    public Button pause;
    public GameObject pauseText;

    private void Update()
    {
        // Если триггер точки 3 не активен и игрок нажимает пробел или левую кнопку мыши
        if (TriggerPoint3.activeSelf == false && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            PlayNextDialog(); // Воспроизводим следующую часть диалога
        }
    }

    // Воспроизведение следующей части диалога
    public void PlayNextDialog()
    {
        if (currentPartOfDialog == 1)
        {
            NextDialog(); // Переходим к следующей части диалога
        }
        else if (currentPartOfDialog == 2)
        {
            EndDialog(); // Завершаем диалог
        }
        else
        {
            StartDialog(); // Начинаем диалог
        }
    }

    // Подготовка к диалогу перед запуском, что бы выровнять камеру
    public void PrepareToDialog()
    {
        Vector3 targetPosition = new Vector3(51f, 2.9f, -4f);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, 3 * Time.deltaTime);
        transform.position = smoothedPosition;

        Invoke("StartDialog", 0.1f); // Запускаем диалог через задержку
    }

    // Начало диалога
    public void StartDialog()
    {
        // Скрываем элементы боевого интерфейса
        foreach (var button in FightingButtons)
        {
            button.SetActive(false);
        }

        joystick.gameObject.SetActive(false); // Скрываем джойстик
        PlayerHP.SetActive(false); // Скрываем панель здоровья игрока
        EnemyHP.SetActive(false); // Скрываем панель здоровья врага
        GoNext.SetActive(false); // Скрываем кнопку следующего шага

        dialogMenu.SetActive(true); // Активируем диалоговое меню

        portret1.SetActive(true); // Активируем первый портрет
        portret2.SetActive(false); // Деактивируем второй портрет
        CharacterName.text = "Игрок"; // Устанавливаем имя персонажа
        DialogText.text = "Здесь водится какое-то сообщение, а пока этот текст рыба."; // Устанавливаем текст диалога
        currentPartOfDialog = 1; // Устанавливаем текущую часть диалога
    }

    // Следующая часть диалога
    public void NextDialog()
    {
        portret1.SetActive(false); // Деактивируем первый портрет
        portret2.SetActive(true); // Активируем второй портрет
        CharacterName.text = "NPC"; // Устанавливаем имя персонажа
        DialogText.text = "Здесь тоже есть какой-то текст рыба."; // Устанавливаем текст диалога
        currentPartOfDialog = 2; // Устанавливаем текущую часть диалога
    }

    // Завершение диалога
    public void EndDialog()
    {
        portret1.SetActive(true); // Активируем первый портрет
        portret2.SetActive(false); // Деактивируем второй портрет
        CharacterName.text = "Игрок"; // Устанавливаем имя персонажа
        DialogText.text = "Диалог окончен."; // Устанавливаем текст диалога
        dialogAnimator.Play("Dark"); // Запускаем анимацию окончания диалога
        Invoke("BackToMenu", 3f); // Ждем 2 секунды и возвращаемся в меню
    }

    // Возвращение в главное меню
    private void BackToMenu()
    {
        SceneManager.LoadScene(0); // Загружаем сцену главного меню
    }
}
