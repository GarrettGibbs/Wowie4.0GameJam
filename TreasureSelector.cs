using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureSelector : MonoBehaviour
{
    [SerializeField] DesertManager desertManager;
    [SerializeField] CaveManager caveManager;
    [SerializeField] int index;
    [SerializeField] bool isDesertScene = true;

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            if (isDesertScene) {
                desertManager.selection = index;
            } else {
                caveManager.selection = index;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            if (!isDesertScene) {
                caveManager.selection = -1;
            }
        }
    }
}
