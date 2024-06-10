using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceInventory
{
    ResourceBlock resources = new ResourceBlock(200, 50);

    private Text metalAmountDisplay;
    private Text woodAmountDisplay;

    public void initialize()
    {
        metalAmountDisplay = GameObject.Find("MetalAmount").GetComponent<Text>();
        woodAmountDisplay = GameObject.Find("WoodAmount").GetComponent<Text>();

        metalAmountDisplay.text = resources.metal.ToString();
        woodAmountDisplay.text = resources.wood.ToString();
    }

    public void addResources(ResourceBlock amount)
    {
        resources += amount;
        updateResources();
        ResourceChangePopup.instance.queueResourceChange(amount);
    }

    public bool subtractResources(ResourceBlock amount)
    {
        if (amount.metal > resources.metal ||
            amount.wood  > resources.wood) return false;

        resources -= amount;
        updateResources();
        ResourceChangePopup.instance.queueResourceChange(amount * -1);
        return true;
    }

    private void updateResources() 
    {
        metalAmountDisplay.text = resources.metal.ToString();
        woodAmountDisplay.text = resources.wood.ToString();
    }
}
