using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelection : MonoBehaviour
{ 
    [SerializeField]
    private LayerMask layerMask = new LayerMask();

    [SerializeField] private RectTransform unitSelectionArea=null;

    private RTSPlayer player;
    private Camera mainCamera;
    private Vector2 startPosition;
    public List<Unit> selectedUnits { get; } = new List<Unit>();
    private void Start()
    {
        mainCamera = Camera.main;
        //player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        Invoke("GetPlayer", 0.1f);
        //player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
    }

    private void GetPlayer()

    {

        player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

    }

    // Update is called once per frame
    private void Update()
    {

        /*if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }*/

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed) 
        {
            UpdateSelectionArea();        
        }
    }

    private void UpdateSelectionArea()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        float areaWidth = mousePosition.x - startPosition.x;
        float areaHeight = mousePosition.y - startPosition.y;
        unitSelectionArea.sizeDelta = new Vector2(Math.Abs(areaWidth), Math.Abs(areaHeight));
        unitSelectionArea.anchoredPosition = startPosition + new Vector2(areaWidth / 2, areaHeight / 2);
    }

    private void StartSelectionArea()
    {
        if (!Keyboard.current.shiftKey.isPressed)
        {
            foreach (Unit selectedUnit in selectedUnits)
            {
                selectedUnit.DeSelect();
            }
            selectedUnits.Clear();
        }
        unitSelectionArea.gameObject.SetActive(true);
        startPosition = Mouse.current.position.ReadValue();
        UpdateSelectionArea();
    }

    private void ClearSelectionArea() 
    {
        unitSelectionArea.gameObject.SetActive(false);
        if (unitSelectionArea.sizeDelta.magnitude == 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }
            Debug.Log("inside raycast");
            if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) { return; }
            Debug.Log("inside hit collider");
            if (!unit.hasAuthority) { return; }
            selectedUnits.Add(unit);
            foreach (Unit selectedUnit in selectedUnits)
            {
                selectedUnit.Select();
            }
            return;
        }
        Debug.Log("anchoredposition" + unitSelectionArea.anchoredPosition);
        Vector2 min = unitSelectionArea.anchoredPosition - (unitSelectionArea.sizeDelta / 2);
        Vector2 max = unitSelectionArea.anchoredPosition + (unitSelectionArea.sizeDelta / 2);
        foreach (Unit unit in player.GetUnitList())
        {
            if (selectedUnits.Contains(unit)) { continue; }
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(unit.transform.position);
            if (screenPosition.x > min.x && screenPosition.x < max.x && screenPosition.y > min.y && screenPosition.y < max.y)
            {
                selectedUnits.Add(unit);
                unit.Select();
            }
        }
    }
}
