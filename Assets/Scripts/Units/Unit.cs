using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeSelected = null;
    [SerializeField] private UnitMovement unitMovement = null;
    [SerializeField] private Targeter target = null;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDeSpawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDeSpawned;

    public UnitMovement GetUnitMovement()
    {
        return unitMovement;
    }

    public Targeter GetTargetMovement()
    {
        return target;
    }

    #region server
    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
    }
    public override void OnStopServer()
    {
        ServerOnUnitDeSpawned?.Invoke(this);
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        if (!isClientOnly || !hasAuthority) { return; }
        AuthorityOnUnitSpawned?.Invoke(this);
    }


    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority) { return; }
        AuthorityOnUnitDeSpawned?.Invoke(this);
    }

    [Client]
    public void Select()
    {
        if (!hasAuthority){ return; }
        onSelected?.Invoke();
    }

    [Client]
    public void DeSelect()
    {
        if (!hasAuthority) { return; }
        onDeSelected?.Invoke();
    }
    #endregion
}
