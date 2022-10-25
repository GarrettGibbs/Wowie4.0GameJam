using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleStars : MonoBehaviour
{
    [SerializeField] CastleManager castleManager;
    [SerializeField] int starIndex;

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            castleManager.CollectStar(starIndex);
        }
    }
}
