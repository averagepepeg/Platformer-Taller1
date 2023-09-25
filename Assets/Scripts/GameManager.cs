using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance;

    public UnityAction<float> OnPlayerDamage;
    public UnityAction<float> OnPlayerPower;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerDamage(float damage)
    {
        // Notificar a todos que el jugador recibió un danho
        OnPlayerDamage.Invoke(damage);
    }

    public void PlayerPower(float power)
    {
        //Notificar a todos cuanto poder tiene el jugador
        OnPlayerPower.Invoke(power);
    }

    public void AddObserver(UnityAction<float> action)
    {
        OnPlayerDamage += action;
    }

    public void AddObserver2(UnityAction<float> action)
    {
        OnPlayerPower += action;
    }
}
