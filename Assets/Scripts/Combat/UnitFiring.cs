using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField] private Targeter targeter = null;
    [SerializeField] private GameObject projectilePrefab = null;
    [SerializeField] private Transform projectileSpawnPoint = null;
    [SerializeField] private float fireRange = 5f;
    [SerializeField] private float fireRate = 5f;
    [SerializeField] private float rotationSpeed = 20f;

    private float lastFireTime;

    [ServerCallback]
    private void Update()
    {
        if (targeter.GetTarGetable() == null) { return; }
        if (!CanFireAtTarget()) { return; }
        Quaternion targetRotation = Quaternion.LookRotation(targeter.GetTarGetable().transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (Time.time > 1 / fireRate + lastFireTime) {
            Quaternion projectileRotation = Quaternion.LookRotation(targeter.GetTarGetable().GetAimAtPoint().position-projectileSpawnPoint.position);
            GameObject projectleInstance = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileRotation);
            NetworkServer.Spawn(projectleInstance, connectionToClient);
            lastFireTime = Time.time;
        }
    }

    private bool CanFireAtTarget() {
        return (targeter.GetTarGetable().transform.position - transform.position).sqrMagnitude <= fireRange * fireRange;
    }

}
