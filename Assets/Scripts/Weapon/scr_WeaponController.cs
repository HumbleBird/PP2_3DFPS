using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static scr_Models;

public class scr_WeaponController : MonoBehaviour
{
    private Scr_Character_Controller characterController;

    [Header("Settings")]
    public WeaponSettginsModel settings;

    bool isInitialised;

    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;

    private void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;
    }

    public void Initialise(Scr_Character_Controller controller)
    { 
        characterController = controller; 
        isInitialised = true;
    }

    private void Update()
    {
        if (!isInitialised)
        {
            return;
        }

        newWeaponRotation.y += settings.SwayAmount * (settings.SwayXInverted ? -characterController.input_View.x : characterController.input_View.x) * Time.deltaTime;
        newWeaponRotation.x += settings.SwayAmount * (settings.SwayYInverted ? characterController.input_View.y : -characterController.input_View.y) * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(newWeaponRotation);
    }


}
