using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Tommy : MonoBehaviour
{
    [SerializeField] GameObject teleportSkull;
    [SerializeField] Animator animator;
    [SerializeField] LevelManager levelManager;
    //[SerializeField] DesertManager desertManager;

    //[SerializeField] bool isDesertScene = true;

    //private int health = 3;

    public void TakeDamage(int health) {
        levelManager.audioManager.PlaySound("Hit");
        animator.SetTrigger("Hit");
        animator.SetInteger("HitPoints", health);
    }

    public async void TeleportTommy(Vector3 location) {
        teleportSkull.SetActive(true);
        transform.position = location;
        levelManager.audioManager.PlaySound("TransitionIn");
        await Task.Delay(500);
        teleportSkull.SetActive(false);
    }
}
