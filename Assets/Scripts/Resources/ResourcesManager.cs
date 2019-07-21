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

[Serializable]
public struct ResourceAmount
{
    public ResourceType resourceType;
    public int amount;
}

public class ResourcesManager : MonoBehaviour
{
    public Dictionary<ResourceType, int> amounts { get; private set; }


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

        foreach(ResourceAmount resourceAmount in GameManager.instance.resources)
        {
            if(IsNotNone(resourceAmount.resourceType))
            {
                amounts[resourceAmount.resourceType] += resourceAmount.amount;
            }
        }

        onResourceChange?.Invoke(amounts);
    }

    private bool IsNotNone(ResourceType resourceType)
    {
        return resourceType != ResourceType.None;
    }

    private void Decrease(ResourceType resourceType, int amount)
    {
        if (IsNotNone(resourceType))
        {
            amounts[resourceType] -= amount;
        }
        onResourceChange?.Invoke(amounts);
    }

    public void Increase(ResourceType resourceType, int amount)
    {
        if (IsNotNone(resourceType))
        {
            amounts[resourceType] += amount;
        }
        onResourceChange?.Invoke(amounts);
    }

    public bool HasEnough(ResourceAmount[] price)
    {
        foreach(ResourceAmount item in price)
        {
            if (IsNotNone(item.resourceType) && item.amount > amounts[item.resourceType])
                return false;
        }
        return true;
    }

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
            Debug.Log("There are not enough resources");
        }
    }
}
