using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Portal Key", fileName = "Item effect data - PortalKey")]
public class ItemEffect_PortalKey : ItemEffect_DataSO
{
    public override void ExcuteEffect()
    {
        if (SceneManager.GetActiveScene().name == "Level_Rainbow 's place")
        {
            Debug.Log("Cannot open portal in this place");
            return;
        }

        Object_Portal.instance.ActivatePortal(Vector3.zero);
    }
}