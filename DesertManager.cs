using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DesertManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject treasure;
    [SerializeField] GameObject mimic;
    [SerializeField] Tommy tommy;
    [SerializeField] GameObject[] teleportPoints;
    [SerializeField] GameObject[] treasurePoints;
    [SerializeField] GameObject[] tommyHearts;
    [SerializeField] GameObject returnDoor;
    [SerializeField] TMP_Text counterText;

    private List<GameObject> treasures = new List<GameObject>();
    private Mimic hiddenMimic = null;
    private int hiddenMimicIndex = 0;
    private bool[] taken = new bool[5] {false,false,false,false,false};

    private int tommyHealth = 3;
    private bool choosing = false;
    public int selection = 0;
    private int treasuresFound = 0;

    private int totalTreasures = 0;
    [SerializeField] int requiredTreasures;

    private Vector3 tommyPosition;

    private bool levelStarted = false;

    private async void Start() {
        tommyPosition = tommy.transform.position;
        if (levelManager.progressManager.levelsDone[2]) {
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
        if (!levelStarted ||totalTreasures >= requiredTreasures || levelManager.dialogueManager.inConversation || levelManager.dialogueManager.timeSinceEndOfConversation < 2) return;
        if (choosing) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                MakeSelection();
                return;
            }
            for (int i = 0; i < treasures.Count; i++) {
                if (selection == i) {
                    treasures[i].transform.GetChild(0).gameObject.SetActive(true);
                } else {
                    treasures[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        } else {
            for (int i = 0; i < treasures.Count; i++) {
                treasures[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private async void MakeSelection() {
        if (taken[selection]) return;
        int pick = selection;
        choosing = false;
        levelManager.audioManager.PlaySound("Action");
        tommy.TeleportTommy(teleportPoints[pick].transform.position);
        taken[pick] = true;
        await Task.Delay(500);
        if(pick == hiddenMimicIndex) {
            hiddenMimic.animator.SetTrigger("Attack");
            await Task.Delay(200);
            TommyTakeDamage();
            await Task.Delay(700);
            NewTreasureRound();
        } else {
            levelManager.audioManager.PlaySound("Victory");
            treasures[pick].SetActive(false);
            treasuresFound++;
            totalTreasures++;
            counterText.text = $"Treasure Collected: {totalTreasures}/{requiredTreasures}";
            if (totalTreasures == requiredTreasures) {
                OnSuccess();
            } else if(treasuresFound == 4) {
                NewTreasureRound();
            } else {
                choosing = true;
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
        levelManager.progressManager.levelsDone[2] = true;
        returnDoor.SetActive(true);
    }

    private async void NewTreasureRound() {
        for (int i = 0; i < taken.Length; i++) {
            taken[i] = false;
        }
        treasuresFound = 0;
        if (levelStarted) {
            tommy.TeleportTommy(tommyPosition);
        }
        await Task.Delay(500);
        levelManager.audioManager.PlaySound("CastleSetup");
        GameObject[] tempt = treasures.ToArray();
        for (int i = 0; i < tempt.Length; i++) {
            Destroy(tempt[i]);
        }
        treasures = new List<GameObject>();
        int random = Random.Range(0, 5);
        for (int i = 0; i < 5; i++) {
            if(i == random) {
                GameObject m = Instantiate(mimic, treasurePoints[i].transform.position, Quaternion.identity);
                hiddenMimic = m.GetComponent<Mimic>();
                hiddenMimicIndex = i;
                treasures.Add(m);
            } else {
                GameObject t = Instantiate(treasure, treasurePoints[i].transform.position, Quaternion.identity);
                treasures.Add(t);
            }
        }
        choosing = true;
        levelStarted = true;
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
        if(tommyHealth <= 0) {
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
