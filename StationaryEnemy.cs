using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class StationaryEnemy : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    public Transform target;
    [SerializeField] float speed = 400f;
    [SerializeField] float rotationSpeed = 20f;
    [SerializeField] float rotationModifier = 0f;
    //[SerializeField] float nextWaypointDistance = 3f;

    [SerializeField] Transform enemyGFX;
    [SerializeField] Animator animator;

    private bool m_FacingRight = true;

    //Path path;
    //int currentWaypoint = 0;
    //bool reachedEndOfPath = false;

    [SerializeField] int hitPoints = 3;
    [SerializeField] bool isDead = false;

    //[SerializeField] Seeker seeker;
    //[SerializeField] Rigidbody2D rb;

    //Attack
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectile;
    [SerializeField] float fireRate = 2f;
    [SerializeField] float timeSinceFire = 0f;

    void FixedUpdate()
    {
        if (isDead) {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Skull_Death") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))) {
                Destroy(gameObject);
            }
            return;
        }

        CheckFire();

        if (target == null) return;

        //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector3 vectorToTarget = target.position - enemyGFX.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.fixedDeltaTime * rotationSpeed);

        //print(vectorToTarget);

        //if (vectorToTarget.x > 0 && !m_FacingRight) {
        //    // ... flip the player.
        //    Flip();
        //}
        //else if (vectorToTarget.x < 0 && m_FacingRight) {
        //    // ... flip the player.
        //    Flip();
        //}

        //if (force.x >= .01f) {
        //    enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        //} else if (force.x <= -.01f) {
        //    enemyGFX.localScale = new Vector3(1f, -1f, 1f);
        //}
    }

    //private void Flip() {
    //    m_FacingRight = !m_FacingRight;

    //    transform.Rotate(0f, 180f, 0f);
    //}

    private void CheckFire() {
        if(timeSinceFire >= fireRate && target != null) {
            Instantiate(projectile, firePoint.position, firePoint.rotation);
            animator.SetTrigger("Attack");
            levelManager.audioManager.PlaySound("FireShot");
            timeSinceFire = 0f;
        } else {
            timeSinceFire += Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            player.OnHit();
            float force = 400;
            Vector3 dir = (Vector3)collision.contacts[0].point - transform.position;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dir.normalized * force * 2);
            //dir = -dir.normalized;
            //GetComponent<Rigidbody2D>().AddForce(dir * force);
        }
    }
}
