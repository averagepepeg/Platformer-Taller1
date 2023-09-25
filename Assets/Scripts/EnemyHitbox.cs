using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public float power = 5f;
    //Cuanto se llenar� la barra de poder
    //del jugador con cada golpe

    private void Start()
    {
        GameManager.Instance.AddObserver2(PlayerPower);
    }

    private void PlayerPower(float power)
    {
        Debug.Log("Lo est� golpeando.");
    }
}
