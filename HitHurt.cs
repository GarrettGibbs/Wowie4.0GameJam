using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHurt : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            if (player.levelManager.respawning) return;
            player.OnHit();
            float force = 6000;
            Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();
            //var opposite = -rigidbody.velocity;
            Vector3 dir = (Vector3)rigidbody.position - transform.position;
            rigidbody.AddForce(dir * force);
            //dir = -dir.normalized;
            //GetComponent<Rigidbody2D>().AddForce(dir * force);
        }
    }
}
