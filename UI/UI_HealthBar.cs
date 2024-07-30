using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    Entity entity => GetComponentInParent<Entity>();
    RectTransform myTransform;
    Slider slider;

    CharacterStats myStats => GetComponentInParent<CharacterStats>();

    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        myTransform = GetComponent<RectTransform>();

        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    void OnEnable()
    {
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;
    }

    void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;

        if (myStats != null)
            myStats.onHealthChanged -= UpdateHealthUI;
    }
    void FlipUI() => myTransform.Rotate(0, 180, 0);
}
