using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Transferpoint : MonoBehaviour
{
    [SerializeField] private string switchToScene;
    [Space]
    [SerializeField] private RespawnType transferpointType;
    [SerializeField] private RespawnType connectedTransferpoint;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private bool canBeTriggered = true;

    public RespawnType GetTransferpointType() => transferpointType;

    public Vector3 GetPositionAndSetTiggerFalse()
    {
        canBeTriggered = false;
        return respawnPoint == null ? transform.position : respawnPoint.position;
    }

    private void OnValidate()
    {
        gameObject.name = "Object_Trnsferpoint - " + transferpointType.ToString() + " - " + switchToScene;

        if(transferpointType == RespawnType.Enter)
            connectedTransferpoint = RespawnType.Exit;

        if (transferpointType == RespawnType.Exit)
            connectedTransferpoint = RespawnType.Enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeTriggered == false)
            return;

        GameManager.instance.ChangeScene(switchToScene, connectedTransferpoint);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggered = true;
    }
}
