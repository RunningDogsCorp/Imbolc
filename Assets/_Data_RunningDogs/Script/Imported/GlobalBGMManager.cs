using UnityEngine;
using System.Collections;

public class GlobalBGMManager : MonoBehaviour {

    AudioSource audioSource;
    AudioClip[] tracks;
    int trackCount;

    private static GlobalBGMManager _instance;
    public static GlobalBGMManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("BGM Manager");
                _instance = obj.AddComponent<GlobalBGMManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        Initiate();
    }

    // Use this for initialization
    void Initiate() {

        //트랙 수
        trackCount = 3;

        //트랙 수에 따라 배열생성
        tracks = new AudioClip[trackCount];
        
        //트랙 배정
        tracks[0] = Resources.Load("Audio/BGM/StoppingEquipment") as AudioClip;
        tracks[1] = Resources.Load("Audio/BGM/100 - MEGALOVANIA") as AudioClip;
        tracks[2] = Resources.Load("Earthbound - Final Boss") as AudioClip;

        //로그확인
        Debug.Log(tracks.Length);

        //오디오소스 붙이기
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlayTrack(int index)
    {
        audioSource.clip = tracks[index];
        audioSource.Play();
    }
}
