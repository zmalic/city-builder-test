using System;
using System.Collections.Generic;
using UnityEngine;
public enum ResourceType
{
    None,
    Gold,
    Wood,
    Steel
}

/// <summary>
/// Resource type - amount pair
/// </summary>
[Serializable]
public struct ResourceAmount
{
    public ResourceType resourceType;
    public int amount;
}


/// <summary>
/// Manager for all resources in the game
/// </summary>
public class ResourcesManager : MonoBehaviour
{
    /// <summary>
    /// Main dictionary for all resources
    /// </summary>
    public Dictionary<ResourceType, int> amounts { get; private set; }

    /// <summary>
    /// Resource change event
    /// </summary>
    /// <param name="amounts"></param>
    public delegate void OnResourceChange(Dictionary<ResourceType, int> amounts);
    public static event OnResourceChange onResourceChange;

    public void Init()
    {
        // Set 0 for all types of resource except None
        amounts = new Dictionary<ResourceType, int>();
        for(int i = 1; i < Enum.GetNames(typeof(ResourceType)).Length; i++)
        {
            ResourceType resourceType = (ResourceType)i;
            if (!amounts.ContainsKey(resourceType))
            {
                amounts.Add(resourceType, 0);
            }
        }

        // Populate amounts dictionary using initial values from GameManager
        foreach (ResourceAmount resourceAmount in GameManager.instance.resources)
        {
            if(IsNotNone(resourceAmount.resourceType))
            {
                amounts[resourceAmount.resourceType] += resourceAmount.amount;
            }
        }

        // Invoke event
        onResourceChange?.Invoke(amounts);
    }

    private bool IsNotNone(ResourceType resourceType)
    {
        return resourceType != ResourceType.None;
    }

    /// <summary>
    /// Decrease resourceType resource 
    /// (Buying buildings)
    /// </summary>
    /// <param name="resourceType"></param>
    /// <param name="amount"></param>
    private void Decrease(ResourceType resourceType, int amount)
    {
        if (IsNotNone(resourceType))
        {
            amounts[resourceType] -= amount;
        }
        onResourceChange?.Invoke(amounts);
    }


    /// <summary>
    /// Increase resourceType resource 
    /// (Buildings production)
    /// </summary>
    /// <param name="resourceType"></param>
    /// <param name="amount"></param>
    public void Increase(ResourceType resourceType, int amount)
    {
        if (IsNotNone(resourceType))
        {
            amounts[resourceType] += amount;
        }
        onResourceChange?.Invoke(amounts);
    }

    /// <summary>
    /// Check is there enough resources for array of ResourceAmount
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    public bool HasEnough(ResourceAmount[] price)
    {
        foreach(ResourceAmount item in price)
        {
            if (IsNotNone(item.resourceType) && item.amount > amounts[item.resourceType])
                return false;
        }
        return true;
    }

    /// <summary>
    /// If there is enough resources decrease it
    /// </summary>
    /// <param name="price"></param>
    public void Buy(ResourceAmount[] price)
    {
        if (HasEnough(price))
        {
            foreach (ResourceAmount item in price)
            {
                if (IsNotNone(item.resourceType))
                    Decrease(item.resourceType, item.amount);
            }
        }
        else
        {
            Debug.Log("There is not enough resources");
        }
    }
}
