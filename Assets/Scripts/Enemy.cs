using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float damage = 10f;

    private void Start()
    {
        GameManager.Instance.AddObserver(PlayerDamage);
    }

    private void PlayerDamage(float damage)
    {
        Debug.Log("Enemigo reacciona al danho del jugador.");
    }
}
