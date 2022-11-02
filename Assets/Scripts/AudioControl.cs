using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[System.Serializable]
public class Audio {
    public AudioSource source;
}*/


public class AudioControl : MonoBehaviour
{
    //public List<Audio> tracks; 
    //private AudioSource activeTrack;
    private int i;
    public AudioSource adSource;
    public AudioClip[] adClips;

    // Start is called before the first frame update
    void Start()
    {
        /*foreach (Audio track in tracks) {
            track.source.enabled = false;
            track.source.loop = false;
        }*/
        i = 0;
        //int rand = (int)Random.Range(0,tracks.Count-1f);
        //Debug.Log(rand);
        //tracks[i].source.enabled = true;
        //tracks[i].source.Play();
        //activeTrack = tracks[i].source;
        //Debug.Log(activeTrack.ToString());
    }

    public void Next() {
        //if (i < tracks.Count) i += 1;
        if (i < adClips.Length) i += 1;
        else i = 0;
        adSource.clip = adClips[i];
        adSource.Play();
        //tracks[i--].source.enabled = false;
        //tracks[i].source.enabled = true;
        //tracks[i].source.Play();
        //activeTrack = tracks[i].source;
        Debug.Log(i);
    }

    // Update is called once per frame
    //IEnumerator playAudioSequentually()
    void Update()
    {
        /*
        yield return null;
        while (i < adClips.Length) {
            adSource.clip = adClips[i];
            adSource.Play();
            while(adSource.isPlaying) {
                yield return null;
            }
            i++;
        }*/

        if (!adSource.isPlaying) {
            if (i < adClips.Length) i += 1;
            else i = 0;
            adSource.clip = adClips[i];
            adSource.Play();
            Debug.Log(i);
        }
        /*
        if (!activeTrack.isPlaying) {
            //int rand = (int)Random.Range(0,tracks.Count-1f);
            //if (i < tracks.Count) i += 1;
            if (i < 5) i += 1;
            else i = 0;
            tracks[i--].source.enabled = false;
            tracks[i].source.enabled = true;
            tracks[i].source.Play();
            activeTrack = tracks[i].source;
            Debug.Log(activeTrack.ToString());
        }*/
    }
}
