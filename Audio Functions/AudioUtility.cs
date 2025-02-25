using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioUtility
{
    public static float minimum = -81f;
    // ### MMN BE MORE SPECIFIC IN THIS FUNCTION NAME, WHAT'S A? 'CONVERTAMPLITUDETODB' ###
    public static float ConvertAtoDb(float amp)
    {
        amp = Mathf.Clamp(amp, ConvertDbtoA(minimum), 1f);
        return 20 * Mathf.Log(amp) / Mathf.Log(10);
    }
    // ### MMN BE MORE SPECIFIC IN THIS FUNCTION NAME, WHAT'S A? 'CONVERTDBTOAMPLITUDE' ###
    public static float ConvertDbtoA(float db)
    {
        return Mathf.Pow(10, db / 20);
    }

    public static AudioClip RandomClipFromArray(AudioClip[] cliplist)
    {
        return cliplist[Mathf.Clamp(0, Random.Range(0, cliplist.Length), cliplist.Length)];
    }     // ### MMN LINE BREAK HERE ###
    public static AudioClip RandomClipFromList(List<AudioClip> cliplist)
    {
        return cliplist[Mathf.Clamp(0, Random.Range(0, cliplist.Count), cliplist.Count)];
    }

    // ### MMN REMOVE WHITESPACE LINE HERE ###
    // ### MMN REMOVE WHITESPACE LINE HERE ###
    // ### MMN THIS SHOULD PROBABLY BE CALLED 'MAP TO RANGE', SCALE VALUE IS VAGUE. ###
    public static float ScaleValue(float value, float originalStart, float originalEnd, float newStart, float newEnd)
    {
        // credit to Wim Coenen, https://stackoverflow.com/questions/4229662/convert-numbers-within-a-range-to-numbers-within-another-range //
        double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
        return (float)(newStart + ((value - originalStart) * scale));
    }

    // ### MMN REMOVE WHITESPACE LINE HERE ###
    // ### MMN REMOVE WHITESPACE LINE HERE ###
    // ### MMN REMOVE WHITESPACE LINE HERE ###
    // ### MMN REMOVE WHITESPACE LINE HERE ###
    public static AudioMixerGroup GetMixerGroup(string groupName)
    {
        AudioMixer masterMixer = Resources.Load("Master") as AudioMixer;
        AudioMixerGroup mixerGroup = masterMixer.FindMatchingGroups(groupName)[0];
        return mixerGroup;
    }

    // ### MMN REMOVE WHITESPACE LINE HERE ###
    private static IEnumerator FadeAudioSource(AudioSource source, float fadetime, float targetVol, float curveShape, bool stopAfterFade)
    {
        Keyframe[] keys = new Keyframe[2];
        keys[0] = new Keyframe(0, 0, 0, 1f - curveShape, 0, 1f - curveShape);
        keys[1] = new Keyframe(1, 1, 1f - curveShape, 0f, curveShape, 0);
        AnimationCurve animcur = new AnimationCurve(keys);
    // ### MMN STATEMENT BRACKETS ###
        if (source.gameObject.GetComponent<AudioSource>())
            source = source.gameObject.GetComponent<AudioSource>();

        float startVol = ConvertAtoDb(source.volume);
        float currentFadeVolume = startVol;
        float currentTime = 0f;

        while (currentTime < fadetime)
        {
            currentTime += Time.deltaTime;
            currentFadeVolume = Mathf.Lerp(startVol, targetVol, animcur.Evaluate(currentTime / fadetime));
            source.volume = ConvertDbtoA(currentFadeVolume);
            yield return null;
        }

        if (stopAfterFade)
        {
            yield return new WaitForSeconds(fadetime);
            source.Stop();
        }

        yield break;
    }

}
    // ### MMN REMOVE WHITESPACE LINE HERE ###
    // ### MMN POTENTIALLY ADD A DEBUG UTILITY FUNCTION HERE IF YOU'RE CONSISTENTLY USING THE SAME FORMAT, INSTEAD OF IN THE AUDIO CONTROLLER/EVERY OTHER CLASS ###
