using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroMoving : MonoBehaviour
{
    [SerializeField] EnemyAI enemy;
    [SerializeField] Transform spawnPoint;


    public void ActivateEnemy(Transform player) {
        enemy.target = player;
        enemy.attacking = true;
    }

    public void DeactivateEnemy() {
        enemy.target = spawnPoint;
        enemy.attacking = false;
    }
}
