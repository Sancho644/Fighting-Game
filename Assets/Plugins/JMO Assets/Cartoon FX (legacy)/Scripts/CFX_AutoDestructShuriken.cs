using UnityEngine;
using System.Collections;

// Cartoon FX  - (c) 2015 Jean Moreno

// Automatically destructs an object when it has stopped emitting particles and when they have all disappeared from the screen.
// Check is performed every 0.5 seconds to not query the particle system's state every frame.
// (only deactivates the object if the OnlyDeactivate flag is set, automatically used with CFX Spawn System)

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	// If true, deactivate the object instead of destroying it
	[SerializeField] private bool _onlyDeactivate;
	[SerializeField] private ParticleSystem _particleSystem;
	
	
	void OnEnable()
	{
		StartCoroutine(CheckIfAlive());
	}
	
	IEnumerator CheckIfAlive ()
	{
		while(_particleSystem != null)
		{
			yield return new WaitForSeconds(0.5f);
			
			if(!_particleSystem.IsAlive(true))
			{
				if(_onlyDeactivate)
				{
					gameObject.SetActive(false);
				}
				
				break;
			}
		}
	}
}
