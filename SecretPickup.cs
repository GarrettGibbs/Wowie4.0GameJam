using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretPickup : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            if (!levelManager.progressManager.gotSecretPickup) {
                levelManager.audioManager.PlaySound("Victory");
                levelManager.dialogueManager.triggers[8].TriggerDialogue();
                levelManager.progressManager.gotSecretPickup = true;
            }
            gameObject.SetActive(false);
        }
    }
}
