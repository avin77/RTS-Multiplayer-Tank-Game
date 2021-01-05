﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    [SerializeField] private Targetable target;

    public Targetable GetTarGetable() {
        return target;
    }

    [Command]
    public void CmdSetTarget(GameObject targetGameObject ) {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable newTarget)){ return; }
         target = newTarget;
        }

    [Server]
    public void ClearTarget() {
        target = null;
    }
}


