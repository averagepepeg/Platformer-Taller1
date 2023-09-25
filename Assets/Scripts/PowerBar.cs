using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        // Registro como Observer
        GameManager.Instance.AddObserver2(AddPower);
    }

    private void AddPower(float power)
    {
        slider.value += power;
    }
}
