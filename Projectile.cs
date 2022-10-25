using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;

    private bool hit = false;

    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) { 
        if (collision.name == "AgroZone") return;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Hit");
        hit = true;

        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if(player != null) {
            player.OnHit();
        }
    }

    private void Update() {
        if (hit) {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Projectile_Explosion") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))) {
                Destroy(gameObject);
            }
            return;
        }
    }
}
