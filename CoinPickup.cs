using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] ForestManager forestManager;

    private bool pickedUp = false;

    private void OnEnable() {
        pickedUp = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (pickedUp) return;
        EnemyAI skull = collision.gameObject.GetComponent<EnemyAI>();
        if (skull != null) {
            pickedUp = true;
            forestManager.CollectCoin();
            gameObject.SetActive(false);
        }
    }
}
