using UnityEngine;
using TMPro;

// simple way to show gun ammo and fire mode for testing purposes. this would be overhauled in a finished project
public class GunInfo : MonoBehaviour
{
    public Gun gun;
    public TMP_Text ammo;
    public TMP_Text fireMode;

    void Update()
    {
        // change TMP text to show ammo + firemode from Gun object
        ammo.text = gun.magazineContents + "/" + gun.magazineCapacity + " (R)";
        fireMode.text = gun.GetCurrentFireMode().GetName() + " (X)";
    }
}
