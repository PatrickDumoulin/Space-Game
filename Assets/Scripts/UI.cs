using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    Rigidbody shipRb;
    float altitude;
    TMP_Text altitudeText;
    bool isEngineStarted = false;
    bool isThrustStarted = false;
    // Start is called before the first frame update
    void Start()
    {
		GameObject ship = GameObject.Find("Ship3");
        if (ship != null)
        {
            shipRb = ship.GetComponent<Rigidbody>();
        }

		altitudeText = GetComponent<TMP_Text>();
        

    }

    // Update is called once per frame
    void Update()
    {
        CalculateAltitude();
        DisplayStartEngine();

		if (Input.GetKeyDown(KeyCode.E))
		{
			isEngineStarted = true;
		}
		if (isEngineStarted)
		{
			isThrustStarted = true;
			DisplayThrustInstruction();
		}

		if (isEngineStarted && isThrustStarted && Time.time >= 25f )
        {
			DisplayAltitude();
		}
		
	}
        
	private void DisplayAltitude()
	{
		altitudeText.text = $"Reach 18000m Altitude = {Mathf.RoundToInt(altitude) - 14}";
	}

	private void DisplayStartEngine()
	{
		altitudeText.text = "Press E to Start the engine";
	}

	private void DisplayThrustInstruction()
	{
		altitudeText.text = "Press and hold SPACE after countdown to launch";
	}

	void CalculateAltitude()
    {
        altitude = shipRb.transform.position.y * 10; 
    }
}
