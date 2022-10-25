using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroStationary : MonoBehaviour
{
    [SerializeField] StationaryEnemy enemy;

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            enemy.target = player.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null) {
            enemy.target = null;
        }
    }
}
