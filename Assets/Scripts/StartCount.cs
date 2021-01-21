using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCount : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] AudioClip start;

    GameObject rocket;
    public static StartCount Instance { get; private set; }

    bool stopping = true;

    // Start is called before the first frame update
    void Start()
    {
        rocket = GameObject.Find("Rocket");
        audioSource = GetComponent<AudioSource>();
        rocket.GetComponent<Rocket>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && stopping)
        {
            stopping = false;
            audioSource.PlayOneShot(start);

            Invoke("TurnRocket", 2.5f);
        }
    }

    void Awake() 
    {
        if (Instance) {
            Destroy (gameObject);
        }
        else {
            DontDestroyOnLoad (gameObject);
            Instance = this;
        }
    }

    void TurnRocket()
    {
        rocket.GetComponent<Rocket>().enabled = true;
    }
}
