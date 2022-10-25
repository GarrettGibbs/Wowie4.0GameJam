using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastlePuzzleSelection : MonoBehaviour
{
    [SerializeField] int SelectionIndex;
    [SerializeField] CastleManager castleManager;
    [SerializeField] LevelManager levelManager;

    private bool inSelection = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (inSelection && !levelManager.inSettings) {
                castleManager.MakeSelection(SelectionIndex);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            inSelection = true;
            castleManager.UpdateSelection(SelectionIndex);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            inSelection = false;
            castleManager.LeaveSelection(SelectionIndex);
        }
    }
}
