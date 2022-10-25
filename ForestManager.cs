using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject timePickup;
    [SerializeField] GameObject healthPickup;
    [SerializeField] TMP_Text remainingTime;
    [SerializeField] TMP_Text coinCounter;

    [SerializeField] GameObject[] allCoins;
    [SerializeField] GameObject[] leftCoins;
    [SerializeField] GameObject[] rightCoins;

    [SerializeField] GameObject returnDoor;

    [SerializeField] AgroMoving fireSkull;

    [SerializeField] int requiredCoins;

    private Camera cam;

    //private bool[] timersAvailable = new bool[2] { false, false };

    private float timeSincePickup = 0f;
    private float timeRemaning = 48f;

    private int coinsCollected = 0;
    private bool lost = false;
    private bool won = false;

    private bool ToggleSelection = false;
    private GameObject currentCoin = null;


    private async void Start() {
        cam = Camera.main;
        if (levelManager.progressManager.levelsDone[0]) {
            returnDoor.SetActive(true);
        }
        await Task.Delay(1000);
        levelManager.circleTransition.OpenBlackScreen();
        SpawnCoin();
        if (!levelManager.progressManager.alreadyBegunLevel) {
            await Task.Delay(500);
            levelManager.dialogueManager.triggers[0].TriggerDialogue();
            levelManager.progressManager.alreadyBegunLevel = true;
        }
    }

    private void Update() {
        if (won || lost) return;
        timeRemaning -= Time.deltaTime;
        if(timeRemaning < 0) {
            OutOfTime();
            return;
        }
        remainingTime.text = $"Time Remaining: {Mathf.Floor(timeRemaning)}";

        timeSincePickup += Time.deltaTime;
        if(timeSincePickup > 10) {
            AddTimePickup();
            timeSincePickup = 0;
        }
    }

    private void AddTimePickup() {
        if (timeRemaning > 25f) return;
        //foreach(bool b in timersAvailable) {
        //    if (b) return;
        //}
        timePickup.SetActive(true);
        healthPickup.SetActive(true);
    }

    public void TimePickup() {
        levelManager.audioManager.PlaySound("Collect");
        timePickup.SetActive(false);
        //timersAvailable[index] = false;
        timeRemaning += 10;
    }

    public void HealthPickup() {
        levelManager.audioManager.PlaySound("Collect");
        healthPickup.SetActive(false);
        levelManager.player.GetComponent<PlayerMovement>().RestoreHealth();
    }

    private async void OutOfTime() {
        lost = true;
        levelManager.audioManager.PlaySound("Death");
        Time.timeScale = 1f;
        levelManager.circleTransition.CloseBlackScreen();
        await Task.Delay(1000);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SpawnCoin() {
        if (ToggleSelection) {
            int random = Random.Range(0, allCoins.Length);
            if(allCoins[random] == currentCoin) {
                random -= 5;
                if(random < 0) random = 0;
            }
            allCoins[random].SetActive(true);
            currentCoin = allCoins[random];
            ToggleSelection = false;
        } else {
            Vector3 viewPos = cam.WorldToViewportPoint(levelManager.player.transform.position);
            if(viewPos.x > .5f) {
                int random = Random.Range(0, leftCoins.Length);
                leftCoins[random].SetActive(true);
                currentCoin = leftCoins[random];
                ToggleSelection = true;
            } else {
                int random = Random.Range(0, rightCoins.Length);
                rightCoins[random].SetActive(true);
                currentCoin = rightCoins[random];
                ToggleSelection = true;
            }
        }
    }

    public async void CollectCoin() {
        if(lost || levelManager.respawning) return;
        levelManager.audioManager.PlaySound("Pickup");
        coinsCollected++;
        coinCounter.text = $"Coins Collected: {coinsCollected}/{requiredCoins}";
        if(coinsCollected >= requiredCoins) {
            fireSkull.DeactivateEnemy();
            won = true;
            await Task.Delay(500);
            levelManager.dialogueManager.triggers[1].TriggerDialogue();
            levelManager.audioManager.PlaySound("Victory");
            returnDoor.SetActive(true);
            levelManager.progressManager.levelsDone[0] = true;
        } else {
            SpawnCoin();
        }
    }
}
