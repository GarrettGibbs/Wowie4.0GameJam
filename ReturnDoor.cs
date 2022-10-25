using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnDoor : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    private bool inDoor = false;
    private bool leaving = false;

    private void Update() {
        if (levelManager.dialogueManager.inConversation || levelManager.dialogueManager.timeSinceEndOfConversation < 2 || leaving || levelManager.inSettings) return;
        if(Input.GetKeyDown(KeyCode.Space) && inDoor) {
            leaving = true;
            ReturnToMenu();
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

    private async void ReturnToMenu() {
        Time.timeScale = 1f;
        levelManager.circleTransition.CloseBlackScreen();
        await Task.Delay(1000);
        SceneManager.LoadScene(0);
    }
}
