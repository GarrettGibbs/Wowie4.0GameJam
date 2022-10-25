using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{
    public int targetScene = 0;
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject dontPushButton;
    [SerializeField] GameObject exitDoor;
    [SerializeField] GameObject superEndPanel;
    [SerializeField] GameObject[] explosions;
    [SerializeField] GameObject leaveButton;

    private async void Start() {
        levelManager.progressManager.alreadyBegunLevel = false;
        if (!levelManager.progressManager.firstTimeAtMenu) {
            await Task.Delay(1000);
            levelManager.circleTransition.OpenBlackScreen();
            CheckForConversation();
        } else {
            await Task.Delay(500);
            levelManager.dialogueManager.triggers[0].TriggerDialogue();
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && levelManager.readyToLeave) {
            ExitGame();
            return;
        }
        if (levelManager.respawning || levelManager.gameEnd || levelManager.inSettings) return;
        if (Input.GetKeyDown(KeyCode.Space) && targetScene != 0 && !levelManager.dialogueManager.inConversation && levelManager.dialogueManager.timeSinceEndOfConversation > 2) {
            levelManager.respawning = true;
            moveToNewScene();
        }
    }

    private async void moveToNewScene() {
        levelManager.progressManager.firstTimeAtMenu = false;
        levelManager.circleTransition.CloseBlackScreen();
        await Task.Delay(1000);
        SceneManager.LoadScene(targetScene);
    }

    private async void CheckForConversation() {
        int completedLevels = 0;
        foreach(bool b in levelManager.progressManager.levelsDone) {
            if (b) completedLevels++;
        }
        switch (completedLevels) {
            case 1:
                if (!levelManager.progressManager.secondConversation) {
                    await Task.Delay(500);
                    levelManager.dialogueManager.triggers[1].TriggerDialogue();
                    levelManager.progressManager.secondConversation = true;
                }
                break;
            case 2:
                if (!levelManager.progressManager.thirdConversation) {
                    await Task.Delay(500);
                    levelManager.dialogueManager.triggers[2].TriggerDialogue();
                    levelManager.progressManager.thirdConversation = true;
                }
                break;
            case 3:
                if (!levelManager.progressManager.pushedTheButton) {
                    dontPushButton.SetActive(true);
                    exitDoor.SetActive(true);
                }
                if (!levelManager.progressManager.fourthConversation) {
                    await Task.Delay(500);
                    levelManager.dialogueManager.triggers[3].TriggerDialogue();
                    levelManager.progressManager.fourthConversation = true;
                }
                break;
            case 4:
                if(!levelManager.progressManager.pushedTheButton) {
                    exitDoor.SetActive(true);
                }
                if (!levelManager.progressManager.fifthConversation) {
                    await Task.Delay(500);
                    if (!levelManager.progressManager.pushedTheButton) {
                        levelManager.dialogueManager.triggers[5].TriggerDialogue();
                    } else {
                        levelManager.dialogueManager.triggers[6].TriggerDialogue();
                    }
                    levelManager.progressManager.fifthConversation = true;
                }
                break;
            case 5:
                if (!levelManager.progressManager.sixthConversation) {
                    levelManager.audioManager.PlaySound("Cheer");
                    await Task.Delay(500);
                    levelManager.dialogueManager.triggers[7].TriggerDialogue();
                    levelManager.progressManager.sixthConversation = true;
                }
                break;
        }
    }

    public async void CheckForSuperEnding() {
        if (levelManager.progressManager.sixthConversation) {
            levelManager.gameEnd = true;
            superEndPanel.SetActive(true);
            levelManager.audioManager.PlaySound("GameEnd");
            SetOffExplosions();
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

    private async void SetOffExplosions() {
        explosions[0].SetActive(true);
        await Task.Delay(500);
        explosions[1].SetActive(true);
        await Task.Delay(500);
        explosions[2].SetActive(true);
        await Task.Delay(500);
        explosions[3].SetActive(true);
        await Task.Delay(500);
        explosions[4].SetActive(true);
        await Task.Delay(500);
        explosions[5].SetActive(true);
        await Task.Delay(500);
        explosions[6].SetActive(true);
        await Task.Delay(500);
    }

    public void TurnOffDoor() {
        exitDoor.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
