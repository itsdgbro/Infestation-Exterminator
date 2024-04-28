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
        id = System.Guid.NewGuid().ToString() + gameObject.transform.GetSiblingIndex();
    }

    #region Target Health
    [SerializeField] private float health = 50f;
    public float GetZombieHealth() => health;
    #endregion

    private bool isDead = false;

    public bool GetIsDead() { return isDead; }

    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private ZombieCountManager ZombieCountManager;

    public bool isZombieArgo { get; set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("animator not found");
        }
        navMeshAgent = GetComponent<NavMeshAgent>();

        // search count manager in parent
        ZombieCountManager = GetComponentInParent<ZombieCountManager>();
        if (ZombieCountManager == null)
        {
            Debug.LogWarning("Zombie Count Manager not found.");
        }

        isZombieArgo = false;
    }

    public void TakeDamage(float amount)
    {
        isZombieArgo = true;
        health -= amount;
        if (health <= 0)
        {
            // death
            navMeshAgent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            isDead = true;
            animator.SetTrigger("dead");
            ZombieCountManager.SetZombieAlive(this.gameObject);
            Destroy(this.gameObject, 15.0f);
        }
        else
        {
            animator.Play("Reaction");
            isDead = false;
        }
    }

    public void LoadData(GameData data)
    {
        //data.enemy.isZombieDead.TryGetValue(id, out isDead);
        //if(isDead)
        //{
        //    this.gameObject.SetActive(false);

        if (data.enemy.isZombieDead.ContainsKey(id))
        {
            isDead = data.enemy.isZombieDead[id];
        }
        else
        {
            isDead = true && DataPersistenceManager.instance.GetIsLoad();
        }
        if (isDead)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
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
