﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: UI Controller                                    |
    // Script Author: James Smale                                    |
    // Purpose: Handles updates of the User Interface                |
    //***************************************************************|

    //Scripts
    public PlayerBase playerController;
    public build_a_weapon activeWeaponController;
    //Ammo UI Elements
    public Text totalAmmoText;
    public Text currentMagazineAmmoText;
    //Prototype Weapon UI Elements
    public Slider prototypeChargeSlider;
    public Text prototypeChargePercent;
    //Ability UI Elements
    public Text blinkTimerText;
    //Wave Information UI Elements
    public Text waveTimerText;
    public Text waveNumberText;
    public Image timerBar;
    public float initialTime;
    //Health UI Elements
    public Slider healthBar;

    public void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBase>();

        totalAmmoText = GameObject.Find("TotalAmmoValue").GetComponent<Text>();
        currentMagazineAmmoText = GameObject.Find("CurrentMagazineAmmo").GetComponent<Text>();

        prototypeChargeSlider = GameObject.Find("PrototypeCharge").GetComponent<Slider>();
        prototypeChargePercent = GameObject.Find("PrototypeChargeValue").GetComponent<Text>();

        blinkTimerText = GameObject.Find("BlinkCooldownTimer").GetComponent<Text>();

        waveTimerText = GameObject.Find("WaveTimer").GetComponent<Text>();
        waveNumberText = GameObject.Find("WaveNumber").GetComponent<Text>();
        timerBar = GameObject.Find("TimerBar").GetComponent<Image>();

        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
    }

    public void GetChangedWeapon()
    {
        //Assigns the script of the currently equipped weapon to the variable activeWeaponController so that the correct ammo values can be pulled from it
        activeWeaponController = playerController.activeWeapon.GetComponent<build_a_weapon>();
    }

    public void UpdateBlinkTimer()
    {
        //If the blink timer reaches 5, the timer stops and is replaced with "Blink Ready" to show the player they can use their ability again
        if (playerController.leftAbilityCooldown == 5)
        {
            blinkTimerText.text = "Blink Ready";
        }
        //If the blink timer is still running, the value of the timer is updated on the UI
        else
        {
            blinkTimerText.text = playerController.leftAbilityCooldownRounded.ToString();
        }
    }

    public void UpdateAmmoText()
    {
        //Updates the UI values of the ammo currently loaded in the weapon and the total ammo the player is carrying for the weapon
        currentMagazineAmmoText.text = activeWeaponController.currentBullets.ToString() + "/" + activeWeaponController.magazineCapacity.ToString();
        totalAmmoText.text = activeWeaponController.totalBullets.ToString();
    }

    //Sets the initial value of the wave timer
    public void SetBaseTimerValue(float totalTime) { initialTime = totalTime; }

    public void UpdateWaveTimer(float timeRemaining)
    {
        //Calculates the number of minutes and seconds remaining in the current wave to display the timer in a more readable manner
        int minutes = (int)timeRemaining / 60;
        int seconds = (int)timeRemaining % 60;
        //Updates the text of the wave timer with the newly calculated values
        waveTimerText.text = (minutes + ":" + seconds);
        timerBar.rectTransform.localScale = new Vector2(timerBar.rectTransform.localScale.x - Time.deltaTime / initialTime, timerBar.rectTransform.localScale.y);
    }

    public void UpdateIntermissionTimer(int intermissionTime)
    {
        //During the intermission, the wave timer text is set to the value of the intermission timer so the player know how long of a break there is
        waveTimerText.text = intermissionTime.ToString();
        //Sets the wave number text to "Wave Complete" to inform the player that the wave has ended
        waveNumberText.text = "Wave Complete";
    }

    public void UpdateWaveNumber(int waveNumber)
    {
        waveNumberText.text = "Wave " + waveNumber; 
    }

    public void UpdatePrototypeCharge(int charge)
    {
        //Updates the text representation of Starstone charge to the current value
        prototypeChargePercent.text = charge.ToString() + "%";
        //Updates the value of the slider to reflect the charge of the Starstone
        prototypeChargeSlider.value = charge;
    }

    public void UpdatePrototypeSliderColour(Color newColour)
    {
        GameObject.Find("ChargeFill").GetComponent<Image>().color = newColour;
    }

    public void UpdateHealthbar()
    {
        //Updates the value of the slider to reflect the amount of health the player has left
        healthBar.value = playerController.currentHealth / 100;
    }

}
