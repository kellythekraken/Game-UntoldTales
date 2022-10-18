using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public enum ParamType{ CONTINUOUS, LABEL}
public enum AudioControlType{ PARAMETER,SONGSWITCH}
public enum ParamScope{ LOCAL, GLOBAL}

[System.Serializable]
public struct LabeledEventEmitter
{
	public string _name;
	public StudioEventEmitter eventEmitter;

	public LabeledEventEmitter(string name, StudioEventEmitter emitter)
	{
		_name = name;
		eventEmitter = emitter;
	}
}

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;
	List<FMOD.Studio.EventInstance> unreleasedSounds;
	public List<LabeledEventEmitter> BGMEventEmitters;
	internal string currentPlayingBGM;

	void Awake() => Instance = this;
	void Start()
	{
		unreleasedSounds = new List<FMOD.Studio.EventInstance>();
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

	public void AddToBGMEventList(string name, StudioEventEmitter emitter)
	{
		LabeledEventEmitter newEvent = new LabeledEventEmitter(name, emitter);
		BGMEventEmitters.Add(newEvent);
	}

	public void PlayBGM(string eventName)
	{
		if(currentPlayingBGM == eventName) return;

		StopAllBGM();

		LabeledEventEmitter emitter = BGMEventEmitters.Find(t => t._name == eventName);
		emitter.eventEmitter.Play();
		currentPlayingBGM = eventName;
		return;

		var path = "event:/" + eventName;
		//maybe instantiate an event emitter and add to list.
		FMOD.Studio.EventInstance soundEvent = RuntimeManager.CreateInstance(path);
		soundEvent.start();
		unreleasedSounds.Add(soundEvent);
	}
	public void StopAllBGM()
	{
		foreach(var i in BGMEventEmitters) 
		{ i.eventEmitter.Stop();}
	}
	public void RemoveSound(string levelName)
	{

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
		LabeledEventEmitter emitter = BGMEventEmitters.Find(t => t._name == eventName);
		Debug.Log("stop" + emitter._name +" from playing");

		emitter.eventEmitter.Stop();
		return;
		var instance = GetPlayingInstanceByName(eventName);
		instance.release(); 
		if (!EqualityComparer<FMOD.Studio.EventInstance>.Default.Equals(instance,default(FMOD.Studio.EventInstance)))
		{
			unreleasedSounds.Remove(instance);
		}
	}
	public void SetLocalParam(string eventName, string paramName, float newValue)
	{
		var emitter = FindEmitterByName(eventName);
		Debug.Log("IS playing? " + emitter.IsPlaying());
		foreach(var i in emitter.Params)
		{
			if (i.Name == paramName)
			{
				i.Value = newValue;
				Debug.Log("new value: " + i.Value);
				break;
			}
		}
		//else {Debug.LogWarning("didn't find the emitter in the bgm emitter list!");}
		
		//var instance = GetPlayingInstanceByName(eventName);
		//instance.setParameterByName(paramName, newValue);
		
	}

	public void SetGlobalParam(string paramName, float newValue)
	{
		RuntimeManager.StudioSystem.setParameterByName(paramName,newValue);
	}
	public void FadeParameter(string eventName, ParamScope scope, string paramName, float fadeFrom, float fadeTo)
	{
		switch(scope)
		{
			case ParamScope.LOCAL:
			var instance = GetPlayingInstanceByName(eventName);
			StartCoroutine(FadeLocalParam(instance, paramName, fadeFrom, fadeTo));
			return;

			case ParamScope.GLOBAL:
			StartCoroutine(FadeGlobalParam(paramName,fadeFrom, fadeTo));
			return;
		}
		
	}
    IEnumerator FadeLocalParam(FMOD.Studio.EventInstance instance, string paramName, float fadeFrom, float fadeTo)
    {
        float elapsedTime = 0f;
        while(elapsedTime < 1f)
        {
            var value = Mathf.Lerp(fadeFrom,fadeTo,elapsedTime);
            instance.setParameterByName(paramName, value);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
		instance.setParameterByName(paramName, fadeTo);

    }
    IEnumerator FadeGlobalParam(string paramName, float fadeFrom, float fadeTo)
    {
        float elapsedTime = 0f;
        while(elapsedTime < 1f)
        {
            var value = Mathf.Lerp(fadeFrom,fadeTo,elapsedTime);
            RuntimeManager.StudioSystem.setParameterByName(paramName,value);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        RuntimeManager.StudioSystem.setParameterByName(paramName,fadeTo);
    }

#region HELPERS
	StudioEventEmitter FindEmitterByName(string name)
	{
		foreach(var i in BGMEventEmitters)
		{
			if(i._name == name) return i.eventEmitter;
		}
		return null;
	}
	FMOD.Studio.EventInstance GetPlayingInstanceByName(string eventName)
	{
		var path = "event:/" + eventName;

		foreach(var i in unreleasedSounds)
		{
			string result;
			FMOD.Studio.EventDescription description;
			i.getDescription(out description);
			description.getPath(out result);
			if(result == path) { return i;}
		}
		Debug.LogWarning("Did not find event by name");
		return default(FMOD.Studio.EventInstance);
	}
#endregion
}
