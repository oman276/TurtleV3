using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    
    public Sound[] boings;
    int boingCount = 0;
    
    public Sound[] hits;
    int hitCount = 0;
    
    public Sound[] enemyHits;
    int enemyHitCount = 0;

    public Sound[] splashes;
    int splashCount = 0;

    public static AudioManager instance;
    public bool waterPlaying = false;
    public bool lavaPlaying = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = UnityEngine.Random.Range(s.pitchMin, s.pitchMax);
            s.source.loop = s.loop;

        }
        foreach (Sound s in boings)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = UnityEngine.Random.Range(s.pitchMin, s.pitchMax);
            s.source.loop = s.loop;

        }
        foreach (Sound s in hits)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = UnityEngine.Random.Range(s.pitchMin, s.pitchMax);
            s.source.loop = s.loop;

        }
        foreach (Sound s in enemyHits)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = UnityEngine.Random.Range(s.pitchMin, s.pitchMax);
            s.source.loop = s.loop;

        }
        foreach (Sound s in splashes)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = UnityEngine.Random.Range(s.pitchMin, s.pitchMax);
            s.source.loop = s.loop;

        }
        //Play("running_water");

        Play("main_theme");
    }


    public void Play(string name)
    {
        Sound s;
        if (name == "boing")
        {
            s = boings[boingCount];
            boingCount++;
            if (boingCount >= 5) boingCount = 0;
        }
        else if (name == "hit") {
            s = hits[hitCount];
            hitCount++;
            if (hitCount >= 4) hitCount = 0;
        }
        else if (name == "enemy_hit")
        {
            s = enemyHits[enemyHitCount];
            enemyHitCount++;
            if (enemyHitCount >= 3) enemyHitCount = 0;
        }
        else if (name == "splash")
        {
            //Debug.Log("splash");
            s = splashes[splashCount];
            splashCount++;
            if (splashCount >= 12) splashCount = 0;
        }
        else
        {
            s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                print("Sound " + name + "not found");
                return;
            }
        }
        s.source.pitch = UnityEngine.Random.Range(s.pitchMin, s.pitchMax);
        s.source.Play();

    }

    public bool isPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            print("Sound " + name + "not found");
            return false;
        }
        return s.source.isPlaying;
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            print("Sound " + name + " not found");
            return;
        }

        s.source.Stop();
    }
    public void muteSound(bool muted)
    {
        if (muted)
        {
            foreach (Sound s in sounds)
            {
                s.source.volume = 0;

            }
        }
        else
        {
            foreach (Sound s in sounds)
            {
                s.source.volume = 0.3f;
                s.volume = 0.3f;
            }
        }
    }
}