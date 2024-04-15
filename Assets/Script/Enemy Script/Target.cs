using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour, ISTarget, IDataPersistence
{
    [SerializeField] private string id;
    [ContextMenu("Generate guid for id")]
    private void GenerateGUID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    #region Target Health
    [SerializeField] private float health = 50f;
    public float GetZombieHealth() => health;
    #endregion

    private bool isDead = false;

    public bool GetIsDead() {  return isDead; }

    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private ZombieCountManager ZombieCountManager;
    

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if(animator == null)
        {
            Debug.LogWarning("animator not found");
        }
        navMeshAgent = GetComponent<NavMeshAgent>();

        // search count manager in parent
        ZombieCountManager = GetComponentInParent<ZombieCountManager>();
        if(ZombieCountManager == null)
        {
            Debug.LogWarning("Zombie Count Manager not found.");
        }
    }

    IEnumerator WaitAndHideObject(float len)
    {
        // Wait for the duration of the animation
        yield return new WaitForSeconds(len);
       
        // hide the GameObject
        this.gameObject.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {   
            // death
            navMeshAgent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            isDead = true;
            animator.SetTrigger("dead");
            ZombieCountManager.SetZombieAlive(this.gameObject);
            StartCoroutine(WaitAndHideObject(3.0f));
        }
        else
        {
            animator.Play("Reaction");
            isDead = false;
        }
    }

    public void LoadData(GameData data)
    {
        data.enemy.isZombieDead.TryGetValue(id, out isDead);
        if(isDead)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        
        if (data.enemy.isZombieDead.ContainsKey(id))
        {
            data.enemy.isZombieDead.Remove(id);
        }

        data.enemy.isZombieDead.Add(id, isDead);
    }
}
