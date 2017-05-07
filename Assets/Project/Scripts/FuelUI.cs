using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelUI : MonoBehaviour 
{

	public Gradient scaleColors;
	public Gradient recharging;

	private Image scale;
	void Start () 
	{
		scale = GetComponent<Image>();
	}
	

	void LateUpdate () 
	{
		float fuelprogress = VectorExtras.ReverseLerp( PlayerController.currentFuel, 0f, PlayerController.singleton.maxFuelTime );
		scale.fillAmount = fuelprogress;

		fuelprogress = VectorExtras.MirrorValue( fuelprogress, 0f, 1f );
		if( PlayerController.fuelDepleted )
			scale.color = recharging.Evaluate( fuelprogress );
		else
			scale.color = scaleColors.Evaluate( fuelprogress );
		

	}
}
