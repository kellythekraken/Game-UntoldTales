using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;
	public EventReference eventPath;
	
	List<FMOD.Studio.EventInstance> unreleasedSounds;

	void Awake() => Instance = this;
	void Start()
	{
		unreleasedSounds = new List<FMOD.Studio.EventInstance>();
		//PlaySound("BGM/Home");
		//StopSound("Arrival");
	}

	void OnDisable()
	{
		if(unreleasedSounds.Count>0) ReleaseAllSound();
	}
	public void PlayOneShot(string eventName)
	{
		var path = "event:/" + eventName;
		RuntimeManager.PlayOneShot(path);
	}

	public void PlaySound(string eventName)
	{
		var path = "event:/" + eventName;
		Debug.Log("playing: " + path);
		FMOD.Studio.EventInstance soundEvent = RuntimeManager.CreateInstance(path);
		
		// Optionally you could set parameters here instead of using the supplied FMOD parameter script
		// See the helper function below named "SetParameter" by Liam de Koster-Kjaer
//		SetParameter(soundEvent, "concrete", concrete);
//		SetParameter(soundEvent, "wood", wood);
  
		// Play and release one-shot instance
		soundEvent.start();
		unreleasedSounds.Add(soundEvent);
	}
	public void ReleaseAllSound()
	{
		foreach(var i in unreleasedSounds)
		{
			i.release();
		}
		unreleasedSounds.Clear();
	}
	public void StopSound(string eventName)
	{
		var path = "event:/" + eventName;

		foreach(var i in unreleasedSounds)
		{
			string result;
			FMOD.Studio.EventDescription description;
			i.getDescription(out description);
			description.getPath(out result);
			if(result == path) { i.release(); unreleasedSounds.Remove(i); return;}
		}
	}
	//trigger: use fmod studio parameter trigger
	//instance.setParameterByName("Pitch", 1f);

		//GLOBAL PARAMETER:
	//studio global parameter trigger
	//FMODUnity.RuntimeManager.StudioSystem.setParameterByName("EQ Global", eq);
}
