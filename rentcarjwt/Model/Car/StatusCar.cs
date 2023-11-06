namespace rentcarjwt.Model.Car_
{
    public enum StatusCar
    {
        free,                 // Вільний 0
        busy,                 // Зданий
        awaitingСonfirmation, // Очікує підтвердження знаходиться на карті (готовий до здачі в оренду)
        Order,                // Замовник очікує підтвердження від Арендодавця
    }                         
}
