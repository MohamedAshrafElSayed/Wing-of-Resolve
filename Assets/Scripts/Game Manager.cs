using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public EagleController eagleController;

    [Header("UI")]
    public GameObject flyerLayer;
    public GameObject useImage;
    public Text takeOffOrLandText;
    public Text speedText;
    public Slider powerProgressBar, weightProgressBar;
    public GameObject takeOffImage, landImage;
    public GameObject boostImage, normalSpeedImage;

    void Start()
    {
        eagleController.activated = true;
        InitializeUI();
        LockMouse();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {
            HideOrUnhideUI();
        }

        bool result = false;
        
        result = eagleController.takeOff;
        takeOffImage.SetActive(!result);
        landImage.SetActive(result);

        result = eagleController.boosting;
        boostImage.SetActive(result);
        normalSpeedImage.SetActive(!result);

        speedText.text = (int)eagleController.GetFlyingSpeed() + "";
        powerProgressBar.value = eagleController.GetStaminaPercentage();
        weightProgressBar.value = eagleController.GetWeightPercentage();
    }

    public void SetUseImageVisibility(bool enabled)
    {
        useImage.SetActive(enabled);
    }

    void InitializeUI()
    {
        useImage.SetActive(false);
        flyerLayer.SetActive(true);
        powerProgressBar.gameObject.SetActive(true);
        takeOffOrLandText.text = "[Space]";
    }

    void LockMouse()
    {
        // Lock mouse cursor in the view
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void HideOrUnhideUI()
    {
        flyerLayer.SetActive(!flyerLayer.activeSelf);
    }
}
