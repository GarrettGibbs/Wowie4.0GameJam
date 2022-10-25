using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerMovement player;
    public AudioManager audioManager;
    public CircleTransition circleTransition;
    public ProgressManager progressManager;
    public DialogueManager dialogueManager;

    public bool respawning = false;
    public bool gameEnd = false;
    public bool inSettings = false;
    public bool readyToLeave = false;

    private void Awake() {
        audioManager = FindObjectOfType<AudioManager>();
        circleTransition = FindObjectOfType<CircleTransition>();
        progressManager = FindObjectOfType<ProgressManager>();
        circleTransition.player = player.transform;
    }
}
