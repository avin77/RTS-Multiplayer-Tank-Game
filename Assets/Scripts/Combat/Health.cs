using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxhealth = 100;

    public event Action ServerOnDie;

    [SyncVar]
    private int currentHealth;

    #region Server
    public override void OnStartServer()
    {
        currentHealth = maxhealth;
    }

    [Server]
    public void DealDamage(int damageAmount)
    {
        if (currentHealth == 0) { return; }
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        if (currentHealth != 0) { return; }

        ServerOnDie?.Invoke();
        Debug.Log("We died");
    }
    #endregion



}
