using UnityEngine;

// Класс для управления камерой и точками следования
public class CameraFollowingAndPoints : MonoBehaviour
{
    public Transform player; // Ссылка на объект персонажа
    public float yOffset = 0f; // Смещение по оси Y
    public float onPosition = 0; // Значение не равно нулю по достижении тригеров
    private float smoothSpeed = 2f; // Скорость плавного перехода

    public GameObject GoNext, Player; // Ссылки на  игрока и маркер следования

    // Барьеры на различных стадиях
    public GameObject FirstStageBariers, SecondStageBariers, GarageBarier, ThirdStageBariers;
    // Триггеры для точек следования и активации
    public GameObject TriggerPoint1, TriggerPoint2, TriggerGaragePoint, TriggerPoint3, GoToGarageTrigger, OutOfGarageTrigger;

    void LateUpdate()
    {
        // Если игрок существует, то камера следует за игроком
        if (player != null && onPosition == 0)
        {
            Vector3 targetPosition = new Vector3(player.position.x, 2.9f, -4f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        // Позиция 1 - для первой точки (по достижении тригера камера занимает определёную позицию на арене и перестаёт следовать за игроком)
        else if (onPosition == 1)
        {
            Vector3 targetPosition = new Vector3(3f, 2.9f, -4f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        // Позиция 2 - для второй точки
        else if (onPosition == 2)
        {
            Vector3 targetPosition = new Vector3(21f, 2.9f, -4f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        // Позиция 3 - для третьей точки
        else if (onPosition == 3)
        {
            Vector3 targetPosition = new Vector3(39f, 2.9f, -4f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        // Позиция 4 - для четвертой точки (для диалога)
        else if (onPosition == 4)
        {
            Vector3 targetPosition = new Vector3(49f, 2.9f, -4f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        // Позиция 5 - для пятой точки
        else if (onPosition == 5)
        {
            Vector3 targetPosition = new Vector3(78f, 2.9f, -4f);
            transform.position = targetPosition;
        }
        // Позиция 6 - для шестой точки
        else if (onPosition == 6)
        {
            Vector3 targetPosition = new Vector3(30f, 2.9f, -4f);
            transform.position = targetPosition;
            NormalFollowing(); // Возвращаем обычное следование
        }
    }

    // Активация метки следования
    public void GoNextActivate()
    {
        Debug.Log("Activating now.");
        GoNext.SetActive(true);
    }

    // Деактивация метки следования (по достижении контрльных точек)
    public void GoNextDisactivate()
    {
        Debug.Log("Disactivating now.");
        GoNext.SetActive(false);
    }

    // Обычное следование за игроком
    public void NormalFollowing()
    {
        onPosition = 0;
    }

    // Установка точки следования 1
    public void ControllPoint1()
    {
        onPosition = 1;
    }

    // Установка точки следования 2
    public void ControllPoint2()
    {
        onPosition = 2;
    }

    // Установка точки следования 3
    public void ControllPoint3()
    {
        onPosition = 3;
    }

    /*
    // Закомментированный метод для управления точкой следования для катсцены
    public void CutsceneControllPoint()
    {
        onPosition = 4;
    }
    */

    // Обработка столкновения игрока с триггером
    public void HandlePlayerCollision(string triggerName)
    {
        // В зависимости от имени триггера выполняем определенные действия
        if (triggerName == "FirstTrigger")
        {
            GoNextDisactivate(); // Деактивируем метку следования этапа
            FirstStageBariers.SetActive(true); // Активируем барьеры на первой стадии
            ControllPoint1(); // Устанавливаем точку следования 1
        }
        else if (triggerName == "SecondTrigger")
        {
            GoNextDisactivate(); // Деактивируем метку следования этапа
            SecondStageBariers.SetActive(true); // Активируем барьеры на второй стадии
            ControllPoint2(); // Устанавливаем точку следования 2
        }
        else if (triggerName == "ThirdTrigger")
        {
            GoNextDisactivate(); // Деактивируем метку следования этапа
            ThirdStageBariers.SetActive(true); // Активируем барьеры на третьей стадии
            ControllPoint3(); // Устанавливаем точку следования 3
        }
        else if (triggerName == "GarageTrigger")
        {
            GarageBarier.SetActive(false); // Деактивируем барьер у гаража
            OutOfGarageTrigger.SetActive(true); // Активируем триггер выхода из гаража
            TriggerGaragePoint.SetActive(false); // Скрывваем тригер что бы не допустить повторного использования
        }
        else if (triggerName == "DialogTrigger")
        {
            //dialogMenuController.StartDialog(); // Запускаем диалоговое меню (закомментировано)
        }
        else if (triggerName == "GoToGarage")
        {
            GoNextDisactivate(); // Деактивируем метку следования этапа
            onPosition = 5; // Устанавливаем точку следования 5
            player.transform.position = new Vector3(78f, 1.05f, -1f); // Перемещаем игрока в гараж
        }
        else if (triggerName == "OutOfGarage")
        {
            GoNextActivate(); // Деактивируем метку следования этапа
            onPosition = 6; // Устанавливаем точку следования 6
            player.transform.position = new Vector3(30f, 1.05f, 1f); // Перемещаем игрока из гаража
        }
    }
}
