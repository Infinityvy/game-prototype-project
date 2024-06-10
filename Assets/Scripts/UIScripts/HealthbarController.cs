using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    public Text healthBarText;
    public Slider healthBarSlider;

    private PlayerEntity player;

    private void Start()
    {
        player = PlayerEntity.instance;
    }

    void Update()
    {
        healthBarText.text = ((int)player.getHealth()).ToString() + "/" + ((int)player.getMaxHealth()).ToString();
        healthBarSlider.value = player.getHealth();
    }
}
