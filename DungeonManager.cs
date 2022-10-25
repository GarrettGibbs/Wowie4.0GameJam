using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject sludge;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] GameObject[] goblinHearts;
    [SerializeField] TMP_Text sludgeCollected;
    [SerializeField] GameObject returnDoor;

    public int SludgesHit = 0;
    private int SludgesNeeded = 15;

    private int goblinHealth = 5;

    private float timeSinceSludge = 2f;
    private bool spawning = false;

    private bool beganLevel = false;

    private async void Start() {
        if (levelManager.progressManager.levelsDone[3]) {
            returnDoor.SetActive(true);
        }
        await Task.Delay(1000);
        levelManager.circleTransition.OpenBlackScreen();
        if (!levelManager.progressManager.alreadyBegunLevel) {
            await Task.Delay(500);
            levelManager.dialogueManager.triggers[0].TriggerDialogue();
            levelManager.progressManager.alreadyBegunLevel = true;
        } else {
            StartSpawning();
        }
    }

    public void StartSpawning() {
        if (!beganLevel) {
            beganLevel = true;
            spawning = true;
        }
    }

    private void Update() {
        if (spawning) {
            timeSinceSludge += Time.deltaTime;
            if(timeSinceSludge >= 2.5f) {
                SpawnSludge();
            }
        }
    }

    private void SpawnSludge() {
        Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        Instantiate(sludge, spawnPoint, Quaternion.identity);
        timeSinceSludge = 0f;
    }

    public async void CollectSludge() {
        SludgesHit++;
        sludgeCollected.text = $"Sludge Collected {SludgesHit}/{SludgesNeeded}";
        if(SludgesHit >= SludgesNeeded) {
            spawning = false;
            Sludge[] sludges = FindObjectsOfType<Sludge>();
            foreach(Sludge s in sludges) {
                Destroy(s.gameObject);
            }
            await Task.Delay(500);
            levelManager.audioManager.PlaySound("Victory");
            levelManager.dialogueManager.triggers[1].TriggerDialogue();
            levelManager.progressManager.levelsDone[3] = true;
            returnDoor.SetActive(true);
        }
    }

    public async void GoblinHit() {
        goblinHealth--;

        for (int i = 0; i < goblinHearts.Length; i++) {
            if (goblinHealth > i) {
                goblinHearts[i].SetActive(true);
            } else {
                goblinHearts[i].SetActive(false);
            }
        }

        if(goblinHealth <= 0) {
            levelManager.audioManager.PlaySound("Death");
            Time.timeScale = 1f;
            levelManager.circleTransition.CloseBlackScreen();
            await Task.Delay(1000);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
