using UnityEngine;
using UnityEngine.UI;

public class BuildingBuyButton : MonoBehaviour
{
    public Text description;
    public Text price;
    public Button button;

    public void Init(Building building)
    {
        description.text = building.description;
        price.text = GetPriceText(building.price);
        button.onClick.AddListener(() => GameManager.instance.buildingManager.BuyBuilding(building));
    }

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
