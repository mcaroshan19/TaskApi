using APITask.Interface;

namespace APITask.Implimention
{
    public class FoodService: IIFoodService
    {

        public List<string> GetAvailableFoods()
        {
            return new List<string> { "Pizza", "Burger", "Pasta", "Salad" };
        }
    }
}
