using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPower : MonoBehaviour
{
    //Identificar si el jugador golpeó al rival por arriba
    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyHitbox enemyHitbox = other.transform.GetComponent<EnemyHitbox>();
        if (enemyHitbox != null)
        {
            GameManager.Instance.PlayerPower(enemyHitbox.power);
        }
    }
}
