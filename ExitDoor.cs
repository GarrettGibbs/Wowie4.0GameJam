using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject MidEndPanel;
    [SerializeField] GameObject leaveButton;

    private bool inDoor = false;
    private bool ending = false;

    private void Update() {
        if (levelManager.dialogueManager.inConversation || levelManager.dialogueManager.timeSinceEndOfConversation < 2 || ending) return;
        if (Input.GetKeyDown(KeyCode.Space) && inDoor) {
            levelManager.gameEnd = true;
            ending = true;
            MidGameEnd();
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

    private async void MidGameEnd() {
        MidEndPanel.SetActive(true);
        levelManager.audioManager.PlaySound("GameEnd");
        await Task.Delay(3000);
        levelManager.readyToLeave = true;
        leaveButton.SetActive(true);
        CanvasGroup cg = leaveButton.GetComponent<CanvasGroup>();
        cg.interactable = true;
        LeanTween.value(leaveButton, .01f, 1, .8f).setOnUpdate((float val) => {
            cg.alpha = val;
        });
    } 


}
