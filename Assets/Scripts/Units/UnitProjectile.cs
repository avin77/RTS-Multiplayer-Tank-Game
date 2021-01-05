﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField]private Rigidbody rb = null;
    [SerializeField]private float launchForce = 10f;
    [SerializeField] private int damageToDeal = 20;
    [SerializeField] private float destroyAfterSeconds = 5f;

    void Start() {
        rb.velocity = transform.forward * launchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroyProjectile), destroyAfterSeconds);

    }

    [Server]
    private void DestroyProjectile() {
        NetworkServer.Destroy(gameObject);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkIdentity)){
           if( networkIdentity.connectionToClient == connectionToClient){ return; }
        }
        if (other.TryGetComponent<Health>(out Health health)) {
            health.DealDamage(damageToDeal);
        }

        DestroyProjectile();
    }


}
