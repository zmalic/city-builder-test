using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dynamically added button for buying a building
/// </summary>
public class BuildingBuyButton : MonoBehaviour
{
    public Text description;
    public Text price;
    public Button button;

    public void Init(Building building)
    {
        // set description, price and action
        description.text = building.description;
        price.text = GetPriceText(building.price);
        button.onClick.AddListener(() => GameManager.instance.buildingManager.BuyBuilding(building));
    }

    /// <summary>
    /// Creates text for price from ResourceAmount array
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    private string GetPriceText(ResourceAmount[] price)
    {
        string priceString = "";
        foreach(ResourceAmount resourceAmount in price)
        {
            priceString += resourceAmount.resourceType.ToString() + ": " + resourceAmount.amount + ", ";
        }
        priceString = priceString.Substring(0, priceString.Length - 2);
        return string.Format("({0})", priceString);
    }
}
