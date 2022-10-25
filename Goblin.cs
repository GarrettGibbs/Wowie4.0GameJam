using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] LevelManager levelManager;
    [SerializeField] DungeonManager dungeonManager;

    private bool playerPresent = false;
    public bool hitting = false;
    private float timeSinceHittingStarting = 0f;

    private void Start() {
        animator.SetBool("Hitting", false);
    }

    private void Update() {
        if (levelManager.dialogueManager.inConversation) return;
        if(Input.GetKeyDown(KeyCode.Space) && playerPresent) {
            StartHitting();
        } else if(hitting) {
            timeSinceHittingStarting += Time.deltaTime;
            CheckStopHitting();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            playerPresent = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            playerPresent = false;
        }
    }

    private void StartHitting() {
        animator.SetBool("Hitting", true);
        hitting = true;
        timeSinceHittingStarting = 0f;
        levelManager.audioManager.PlaySound("ShortHit");
    }

    private void CheckStopHitting() {
        if(timeSinceHittingStarting >= 5f) {
            animator.SetBool("Hitting", false);
            hitting = false;
        }
    }

    public void OnHitSludge() {
        levelManager.audioManager.PlaySound("CastleSetup");
        dungeonManager.CollectSludge();
    }

    public void OnMissSludge() {
        levelManager.audioManager.PlaySound("Hit");
        animator.SetTrigger("Hit");
        dungeonManager.GoblinHit();
    }
}
