using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    static SoundController instance;
    public static SoundController GetInstance() {
        return instance;
    }

    [Header("AudioSource")]
    [SerializeField] public AudioSource bgmAudio;
    [SerializeField] public AudioSource sfxAudio;
    
    [Header("AudioClip")]
    [SerializeField] AudioClip buttonClick_clip;
    [SerializeField] AudioClip correctAnswer_clip;
    [SerializeField] AudioClip wrongAnswer_clip;
    [SerializeField] AudioClip win_clip;
    [SerializeField] AudioClip charging_clip;
    [SerializeField] AudioClip bgmMenu_clip;
    [SerializeField] AudioClip bgmGameplay_clip;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void PlayBGM(BGMType type) {
        if((type==BGMType.Menu && bgmAudio.clip==bgmMenu_clip)
        ||(type==BGMType.Gameplay && bgmAudio.clip==bgmGameplay_clip)) return;

        if(bgmAudio.isPlaying) bgmAudio.Stop();

        if(type==BGMType.Menu) bgmAudio.clip=bgmMenu_clip;
        else bgmAudio.clip=bgmGameplay_clip;

        bgmAudio.Play();
    }

    public void PlayButtonClick() { sfxAudio.PlayOneShot(buttonClick_clip);Debug.Log("PlayButtonClick"); }
    public void PlayCorrect() { sfxAudio.PlayOneShot(correctAnswer_clip);Debug.Log("PlayCorrect"); }
    public void PlayWrong() { sfxAudio.PlayOneShot(wrongAnswer_clip);Debug.Log("PlayWrong"); }
    public void PlayWin() { sfxAudio.PlayOneShot(win_clip);Debug.Log("PlayWin"); }
    public void PlayCharging() { sfxAudio.PlayOneShot(charging_clip);Debug.Log("PlayCharging"); }

    public void SetBGMVolume(float _value) => bgmAudio.volume = _value;
    public void MuteBGM() => bgmAudio.mute = true;
    public void UnmuteBGM() => bgmAudio.mute = false;

    public void SetSFXVolume(float _value) => sfxAudio.volume = _value;
    public void MuteSFX() => sfxAudio.mute = true;
    public void UnmuteSFX() => sfxAudio.mute = false;
}

public enum BGMType {
    Menu,
    Gameplay
}