using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;

    // Observer

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        // Registro como Observer
        GameManager.Instance.AddObserver(ReduceHealth);
    }

    private void ReduceHealth(float damage)
    {
        slider.value -= damage;
    }
}
