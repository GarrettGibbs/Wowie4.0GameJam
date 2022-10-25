using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CaveManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] Tommy tommy;
    [SerializeField] GameObject[] teleportPoints;
    [SerializeField] GameObject[] tommyHearts;
    [SerializeField] GameObject returnDoor;
    [SerializeField] TMP_Text counterText;

    [SerializeField] Image ruleIcon;
    [SerializeField] Image indicatorIcon;

    [SerializeField] Sprite[] rules;
    [SerializeField] Sprite[] indicators;

    private int tommyHealth = 3;
    private bool choosing = false;
    public int selection = -1;
    private int totalTreasures = 0;
    [SerializeField] int requiredTreasures;

    private Vector3 tommyPosition;

    private bool levelStarted = false;

    List<int> acceptableChoices = new List<int>();
    private int rule; //exclamation, check, x-out, question mark, plus, minus, 
    private int indicator; //crystal, crown, key

    private async void Start() {
        tommyPosition = tommy.transform.position;
        if (levelManager.progressManager.levelsDone[4]) {
            returnDoor.SetActive(true);
        }
        await Task.Delay(1000);
        levelManager.circleTransition.OpenBlackScreen();
        if (!levelManager.progressManager.alreadyBegunLevel) {
            await Task.Delay(500);
            if (levelManager.progressManager.hasMetTommy) {
                levelManager.dialogueManager.triggers[1].TriggerDialogue();
            } else {
                levelManager.dialogueManager.triggers[0].TriggerDialogue();
            }
            levelManager.progressManager.alreadyBegunLevel = true;
        } else {
            StartLevel();
        }
    }

    public void StartLevel() {
        if (!levelStarted) {
            NewTreasureRound();
        }
    }

    private void Update() {
        if (selection < 0 || !levelStarted || totalTreasures >= requiredTreasures || 
            levelManager.dialogueManager.inConversation || levelManager.dialogueManager.timeSinceEndOfConversation < 2) return;
        if (choosing) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                MakeSelection();
            }
        }
    }

    private async void MakeSelection() {
        int pick = selection;
        choosing = false;
        levelManager.audioManager.PlaySound("Action");
        tommy.TeleportTommy(teleportPoints[pick].transform.position);
        await Task.Delay(500);
        if (!acceptableChoices.Contains(pick)) {
            await Task.Delay(200);
            TommyTakeDamage();
            await Task.Delay(700);
            NewTreasureRound();
        } else {
            levelManager.audioManager.PlaySound("Victory");
            totalTreasures++;
            counterText.text = $"Treasure Collected: {totalTreasures}/{requiredTreasures}";
            if (totalTreasures == requiredTreasures) {
                OnSuccess();
            } else {
                await Task.Delay(700);
                NewTreasureRound();
            }
        }
    }

    private async void OnSuccess() {
        await Task.Delay(500);
        if (levelManager.progressManager.hasMetTommy) {
            levelManager.dialogueManager.triggers[3].TriggerDialogue();
        } else {
            levelManager.dialogueManager.triggers[2].TriggerDialogue();
        }
        levelManager.progressManager.hasMetTommy = true;
        levelManager.progressManager.levelsDone[4] = true;
        returnDoor.SetActive(true);
    }

    private async void NewTreasureRound() {
        if (levelStarted) {
            tommy.TeleportTommy(tommyPosition);
        }
        await Task.Delay(500);
        levelManager.audioManager.PlaySound("CastleSetup");

        int ran1 = Random.Range(0, rules.Length);
        rule = ran1;
        ruleIcon.sprite = rules[ran1];

        int ran2 = Random.Range(0, indicators.Length);
        indicator = ran2;
        indicatorIcon.sprite = indicators[ran2];

        acceptableChoices = new List<int>();
        PopulateChoices();

        choosing = true;
        levelStarted = true;
    }

    private void PopulateChoices() {
        switch (rule) {
            case 0: //select that one
                acceptableChoices.Add(indicator);
                break;
            case 1: //also select that one
                acceptableChoices.Add(indicator);
                break;
            case 2: //not that one
                for (int i = 0; i < indicators.Length; i++) {
                    if(i != indicator) {
                        acceptableChoices.Add(i);
                    }
                }
                break;
            case 3: //any of them work
                for (int i = 0; i < indicators.Length; i++) {
                    acceptableChoices.Add(i);
                }
                break;
            case 4: //add +1 to index
                int temp = indicator + 1;
                if(temp > 2) {
                    temp = 0;
                }
                acceptableChoices.Add(temp);
                break;
            case 5: //add -1 to index
                int temp2 = indicator - 1;
                if (temp2 < 0) {
                    temp2 = 2;
                }
                acceptableChoices.Add(temp2);
                break;
        }
    }

    public async void TommyTakeDamage() {
        tommyHealth--;
        tommy.TakeDamage(tommyHealth);

        for (int i = 0; i < tommyHearts.Length; i++) {
            if (tommyHealth > i) {
                tommyHearts[i].SetActive(true);
            } else {
                tommyHearts[i].SetActive(false);
            }
        }
        if (tommyHealth <= 0) {
            levelManager.respawning = true;
            await Task.Delay(500);
            levelManager.audioManager.PlaySound("Death");
            Time.timeScale = 1f;
            levelManager.circleTransition.CloseBlackScreen();
            await Task.Delay(500);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
