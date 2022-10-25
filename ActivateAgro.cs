using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAgro : MonoBehaviour
{
    [SerializeField] AgroMoving agroMoving;
    [SerializeField] bool activate;

    [SerializeField] ActivateAgro otherAcitvatePoint;

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            if (activate) {
                agroMoving.ActivateEnemy(player.transform);
            } else {
                agroMoving.DeactivateEnemy();
            }
            otherAcitvatePoint.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
