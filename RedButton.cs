using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] MenuSceneManager menuSceneManager;

    private bool inDoor = false;
    private bool buttonpressed = false;

    private void Update() {
        if (levelManager.dialogueManager.inConversation || levelManager.dialogueManager.timeSinceEndOfConversation < 2 || buttonpressed) return;
        if (Input.GetKeyDown(KeyCode.Space) && inDoor) {
            buttonpressed = true;
            PRESSTHEBUTTON();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            inDoor = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            inDoor = false;
        }
    }

    private void PRESSTHEBUTTON() {
        levelManager.audioManager.PlaySound("Alarm");
        menuSceneManager.TurnOffDoor();
        levelManager.progressManager.pushedTheButton = true;
        levelManager.dialogueManager.triggers[4].TriggerDialogue();
    }
}
