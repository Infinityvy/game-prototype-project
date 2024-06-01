using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceInventory
{
    ResourceBlock resources = new ResourceBlock(20, 5);

    private TextMeshProUGUI metalAmountDisplay;
    private TextMeshProUGUI woodAmountDisplay;

    public void initialize()
    {
        metalAmountDisplay = GameObject.Find("MetalAmount").GetComponent<TextMeshProUGUI>();
        woodAmountDisplay = GameObject.Find("WoodAmount").GetComponent<TextMeshProUGUI>();

        metalAmountDisplay.text = resources.metal.ToString();
        woodAmountDisplay.text = resources.wood.ToString();
    }

    public void addResources(ResourceBlock amount)
    {
        resources += amount;
        updateResources();
    }

    public bool subtractResources(ResourceBlock amount)
    {
        if (amount.metal > resources.metal ||
            amount.wood  > resources.wood) return false;

        resources -= amount;
        updateResources();
        return true;
    }

    private void updateResources() 
    {
        metalAmountDisplay.text = resources.metal.ToString();
        woodAmountDisplay.text = resources.wood.ToString();
    }
}
