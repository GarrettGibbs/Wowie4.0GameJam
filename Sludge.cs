using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Sludge : MonoBehaviour
{
    [SerializeField] Animator animator;

    //private void Update() {
    //    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sludge_Exploding") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))) {
    //        Destroy(gameObject);
    //    }
    //}

    private async void OnTriggerEnter2D(Collider2D collision) {
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
        } else {
            Goblin goblin = collision.GetComponent<Goblin>();
            if(goblin != null) {
                if (goblin.hitting) {
                    goblin.OnHitSludge();
                    Destroy(gameObject);
                } else {
                    goblin.OnMissSludge();
                    animator.SetTrigger("Explode");
                    GetComponent<Rigidbody2D>().gravityScale = 0;
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    await Task.Delay(200);
                    Destroy(gameObject);
                }
            }
        }
    }
}
