using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CastleManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject[] LeftFlames;
    [SerializeField] GameObject[] MiddleFlames;
    [SerializeField] GameObject[] RightFlames;

    [SerializeField] GameObject[] highlights;
    
    [SerializeField] GameObject[] stars;
    [SerializeField] Image[] starIcons;

    [SerializeField] GameObject returnDoor;

    private int selection = 9;
    private bool puzzleSetup = false;

    //bottom to top ->
    private bool[] leftTriggers = new bool[3] {true, false, false};
    private bool[] middleTriggers = new bool[3] {false, true, false};
    private bool[] rightTriggers = new bool[3] {false, false, true};

    private bool[] starsSpawned = new bool[3] {false, false, false};
    private bool[] starsCollected = new bool[3] {false, false, false};

    private async void Start() {
        if (levelManager.progressManager.levelsDone[1]) {
            returnDoor.SetActive(true);
        }
        await Task.Delay(1000);
        levelManager.circleTransition.OpenBlackScreen();
        if (!levelManager.progressManager.alreadyBegunLevel) {
            await Task.Delay(500);
            levelManager.dialogueManager.triggers[0].TriggerDialogue();
            levelManager.progressManager.alreadyBegunLevel = true;
        } else {
            SetupPuzzle();
        }
    }

    public void SetupPuzzle() {
        if (puzzleSetup) return;
        levelManager.audioManager.PlaySound("CastleSetup");
        LeftFlames[1].SetActive(false);
        LeftFlames[2].SetActive(false);
        MiddleFlames[0].SetActive(false);
        MiddleFlames[2].SetActive(false);
        RightFlames[0].SetActive(false);
        RightFlames[1].SetActive(false);

        foreach(GameObject s in stars) {
            s.SetActive(false);
        }
        puzzleSetup = true;
    }

    public void UpdateSelection(int index) {
        int target = CheckMovable(index);
        if(selection == 9) {
            if (target == -1 || selection == index) return;
        } else if(selection == index) {
            return;
        }

        ChangeHighlighColors(index, Color.yellow);
        highlights[index].SetActive(true);
    }

    public void LeaveSelection(int index) {
        if (selection == index) return;
        highlights[index].SetActive(false);
    }

    public void MakeSelection(int index) {
        levelManager.audioManager.PlaySound("Action");
        if (selection != 9) {
            TryMoveFire(index);
        } else {
            int target = CheckMovable(index);
            if(target == -1) return;
            selection = index;
            ChangeHighlighColors(index, Color.green);
            highlights[index].SetActive(true);
        }
    }

    public void CancelSelection(int index) {
        selection = 9;
        highlights[index].SetActive(false);
    }

    private int CheckMovable(int index) {
        int rIndex = -1;
        switch (index) {
            case 0:
                for (int i = 0; i < leftTriggers.Length; i++) {
                    if (leftTriggers[i]) rIndex = i;
                }
                break;
            case 1:
                for (int i = 0; i < middleTriggers.Length; i++) {
                    if (middleTriggers[i]) rIndex = i;
                }
                break;
            case 2:
                for (int i = 0; i < rightTriggers.Length; i++) {
                    if (rightTriggers[i]) rIndex = i;
                }
                break;
        }
        return rIndex;
    }

    //private int CheckMoving(int index) {
    //    int rIndex = 0;
    //    switch (index) {
    //        case 0:
    //            for (int i = 0; i < leftTriggers.Length; i++) {
    //                if (!leftTriggers[i]) rIndex = i;
    //            }
    //            break;
    //        case 1:
    //            for (int i = 0; i < MiddleTriggers.Length; i++) {
    //                if (!MiddleTriggers[i]) rIndex = i;
    //            }
    //            break;
    //        case 2:
    //            for (int i = 0; i < RightTriggers.Length; i++) {
    //                if (!RightTriggers[i]) rIndex = i;
    //            }
    //            break;
    //    }
    //    return rIndex;
    //}

    private void ChangeHighlighColors(int index, Color c) {
        SpriteRenderer[] sr = highlights[index].GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sr) {
            s.color = c;
        }
    }

    private void TryMoveFire(int index) {
        if(selection == index) {
            CancelSelection(index);
        } else {
            int r = CheckMovable(index);
            int t = CheckMovable(selection);
            if (r >= t) {
                FlashRed(index);
            } else {
                //int t = CheckMoving(index);
                switch (index) {
                    case 0:
                        LeftFlames[t].SetActive(true);
                        leftTriggers[t] = true;
                        break;
                    case 1:
                        MiddleFlames[t].SetActive(true);
                        middleTriggers[t] = true;
                        break;
                    case 2:
                        RightFlames[t].SetActive(true);
                        rightTriggers[t] = true;
                        break;
                    
                }
                //int s = selection;
                //selection = 9;
                switch (selection) {
                    case 0:
                        LeftFlames[t].SetActive(false);
                        leftTriggers[t] = false;
                        break;
                    case 1:
                        MiddleFlames[t].SetActive(false);
                        middleTriggers[t] = false;
                        break;
                    case 2:
                        RightFlames[t].SetActive(false);
                        rightTriggers[t] = false;
                        break;
                }
                CheckColumnCompletion(index);
                CancelSelection(selection);
            }
        }
    }

    private async void FlashRed(int index) {
        ChangeHighlighColors(selection, Color.red);
        await Task.Delay(500);
        CancelSelection(selection);
    }

    private void CheckColumnCompletion(int index) {
        if (starsSpawned[index]) return;
        switch (index) {
            case 0:
                foreach(bool b in leftTriggers) {
                    if (!b) return;
                }
                break;
            case 1:
                foreach (bool b in middleTriggers) {
                    if (!b) return;
                }
                break;
            case 2:
                foreach (bool b in rightTriggers) {
                    if (!b) return;
                }
                break;
        }
        levelManager.audioManager.PlaySound("Collect");
        starsSpawned[index] = true;
        stars[index].SetActive(true);
    }

    public void CollectStar(int index) {
        levelManager.audioManager.PlaySound("Pickup");
        starIcons[index].gameObject.SetActive(true);
        starsCollected[index] = true;
        stars[index].SetActive(false);
        CheckLevelCompletion();
    }

    private void CheckLevelCompletion() {
        foreach(bool b in starsCollected) {
            if(!b) return;
        }
        //Hurray you win!!!
        levelManager.audioManager.PlaySound("Victory");
        levelManager.progressManager.levelsDone[1] = true;
        levelManager.dialogueManager.triggers[1].TriggerDialogue();
        returnDoor.SetActive(true);
    }
}
