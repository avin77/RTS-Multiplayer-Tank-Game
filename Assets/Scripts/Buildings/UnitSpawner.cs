using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour,IPointerClickHandler
{

    [SerializeField] private GameObject unitPrefab = null;
    [SerializeField] private Transform unitSpawnPoint = null;

    #region server
    [Command]
    private void CmdSpawnUnit() 
    {
        Debug.Log("inside cmdspawn click");
        GameObject unitInstance = Instantiate(unitPrefab, unitSpawnPoint.position, unitSpawnPoint.rotation);
        NetworkServer.Spawn(unitInstance,connectionToClient);
    }

    #endregion

    #region client
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("inside mouse click");
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        if (!hasAuthority) { return; }

        CmdSpawnUnit();
    }
    #endregion
}
