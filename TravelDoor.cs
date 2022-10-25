using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelDoor : MonoBehaviour
{
    [SerializeField] int DoorIndex;
    [SerializeField] MenuSceneManager msm;
    [SerializeField] GameObject DoneIndicator;
    [SerializeField] GameObject NeededIndicator;
    [SerializeField] GameObject BackgroundIndicator;
    [SerializeField] LevelManager levelManager;

    private void Start() {
        if (levelManager.progressManager.levelsDone[DoorIndex-1]) {
            DoneIndicator.SetActive(true);
        } else {
            NeededIndicator.SetActive(true);
        }
    }

    public void UpdateSceneSelecttion(bool onOff) {
        if (onOff) {
            msm.targetScene = DoorIndex;
        } else {
            msm.targetScene = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            BackgroundIndicator.SetActive(true);
            msm.targetScene = DoorIndex;
        }
    }

    //private void OnTriggerStay2D(Collider2D collision) {
    //    objectInTrigger = true;
    //}

    void OnTriggerExit2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            BackgroundIndicator.SetActive(false);
            msm.targetScene = 0;
        }
    }
}
