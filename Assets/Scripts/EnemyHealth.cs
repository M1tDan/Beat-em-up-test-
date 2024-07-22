using UnityEngine;

// Класс для управления здоровьем врага
public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 5; // Начальное здоровье врага
    public int currentHealth; // Текущее здоровье врага

    public GameObject FirstStageBariers, SecondStageBarier, GarageBarier, ThirdStageBarier; // Ссылки на барьеры в различных стадиях уровня

    public CameraFollowingAndPoints cameraFollowingAndPoints; // Ссылка на скрипт следования камеры

    private void Start()
    {
        currentHealth = startingHealth; // Устанавливаем текущее здоровье равным начальному
    }

    // Метод для получения урона
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Уменьшаем текущее здоровье на указанное количество урона
        if (currentHealth <= 0)
        {
            Die(); // Если здоровье меньше или равно нулю, вызываем метод смерти
        }
    }

    // Метод вызывается при смерти врага
    private void Die()
    {
        // Выполняем различные действия в зависимости от имени врага
        if (gameObject.name == "Enemy1")
        {
            FirstStageBariers.SetActive(false); // Отключаем барьеры на первой стадии
            cameraFollowingAndPoints.NormalFollowing(); // Возвращаем обычное следование камеры
            cameraFollowingAndPoints.GoNextActivate(); // Активируем маркер движения
        }
        else if (gameObject.name == "Enemy2")
        {
            SecondStageBarier.SetActive(false); // Отключаем барьеры на второй стадии
            cameraFollowingAndPoints.NormalFollowing(); // Возвращаем обычное следование камеры
            cameraFollowingAndPoints.GoNextActivate(); // Активируем маркер движения
        }
        else if (gameObject.name == "Enemy3")
        {
            ThirdStageBarier.SetActive(false); // Отключаем барьеры на третьей стадии
            cameraFollowingAndPoints.NormalFollowing(); // Возвращаем обычное следование камеры
            cameraFollowingAndPoints.GoNextActivate(); // Активируем маркер движения
        }

        // Уничтожаем врага как объект
        Destroy(gameObject);
    }
}
