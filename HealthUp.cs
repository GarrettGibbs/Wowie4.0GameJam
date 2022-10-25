using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    [SerializeField] ForestManager forestManager;
    //[SerializeField] int index;

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            forestManager.HealthPickup();
        }
    }
}
