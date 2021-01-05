using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] private List<Unit> unitList = new List<Unit>();

    public List<Unit> GetUnitList() 
    {
        return unitList;
    }

    #region Server
    public override void OnStartServer() {
        Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
        Unit.ServerOnUnitDeSpawned += ServerHandleUnitDeSpawned;
    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDeSpawned -= ServerHandleUnitDeSpawned;
    }

    private void ServerHandleUnitSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }
        unitList.Add(unit);
    }

    private void ServerHandleUnitDeSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }
        unitList.Remove(unit);
    }
    #endregion

    #region Client

    public override void OnStartClient()
    {
		if (!isClientOnly) { return; }
        Unit.AuthorityOnUnitSpawned += AuthorityUnitSpawnHandler;
        Unit.AuthorityOnUnitDeSpawned += AuthorityUnitDeSpawnHandler;
    }

    public override void OnStopClient()
    {
		if (!isClientOnly) { return; }
        Unit.AuthorityOnUnitSpawned -= AuthorityUnitSpawnHandler;
        Unit.AuthorityOnUnitDeSpawned -= AuthorityUnitDeSpawnHandler;
    }

    private void AuthorityUnitSpawnHandler(Unit unit)
    {
        if (!hasAuthority) { return; }
        unitList.Add(unit);
    }

    private void AuthorityUnitDeSpawnHandler(Unit unit)
    {
        if (!hasAuthority) { return; }
        unitList.Remove(unit);
    }


    #endregion


}
