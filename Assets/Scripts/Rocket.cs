using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Rocket : MonoBehaviour
{
    [SerializeField] Text energyText;
    [SerializeField] int energyTotal = 1000;
    [SerializeField] int expense = 100;
    [SerializeField] float rotSpeed = 150f;
    [SerializeField] float flySpeed = 3f;
    new Rigidbody rigidbody;
    AudioSource audioSource;

    [SerializeField] AudioClip fly;
    [SerializeField] AudioClip finish;
    [SerializeField] AudioClip dead;
    [SerializeField] AudioClip cheat;

    [SerializeField] ParticleSystem flyPartical;
    [SerializeField] ParticleSystem boomPartical;
    [SerializeField] ParticleSystem finishPartical;


    enum State
    {
        Playing,
        Dead,
        NextLevel,
    }
    bool collisionOff = false;

    [SerializeField] State state = State.Playing;

    // Start is called before the first frame update
    void Start()
    {
        energyText.text = energyTotal.ToString();
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Q))
        {
            SceneManager.LoadScene(0); 
        }

        if(state == State.Playing && energyTotal > 0){
            Rotation();
            Launch();
        }

        if(Debug.isDebugBuild)
        {
            DebugKey();
        }
    }
    
    void DebugKey()
    {
        if(Input.GetKeyUp(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyUp(KeyCode.K))
        {
            LoadLastLevel();
        }
        else if(Input.GetKeyUp(KeyCode.C))
        {
            audioSource.PlayOneShot(cheat);
            Invoke("audioSource.Stop", 1.3f);

            collisionOff = !collisionOff;
        }
    }
    

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Playing || collisionOff)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                audioSource.Stop();
                finishPartical.Play();
                audioSource.PlayOneShot(finish);
                state = State.NextLevel;

                Invoke("LoadNextLevel", 3f);
                break;
            case "Battery":
                GetEnergy(1000, collision.gameObject);
                break;
            default:
                audioSource.Stop();
                boomPartical.Play();
                audioSource.PlayOneShot(dead);
                state = State.Dead;   

                Invoke("LoadLastLevel", 3f);          
                break;
        }
    }

    void LoadNextLevel()
    {
        state = State.Playing;
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if(nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIndex = 1;
        }

        SceneManager.LoadScene(nextLevelIndex);
    }

    void LoadLastLevel()
    {
        state = State.Playing;
        int lastLevelIndex = SceneManager.GetActiveScene().buildIndex - 1;

        if(lastLevelIndex < 1)
        {
            lastLevelIndex = 1;
        }

        SceneManager.LoadScene(lastLevelIndex);       
    }

    void Launch()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            energyTotal -= (int)(expense * Time.deltaTime);
            energyText.text = energyTotal.ToString();

            if(energyTotal <= 1)           
            {
                energyTotal = 0;
                flyPartical.Stop();
                audioSource.Stop();
            
                Invoke("LoadLastLevel", 2f);
                return;
            }

            rigidbody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(fly);
            }
            
            if(!flyPartical.isPlaying)
            {
                flyPartical.Play();
            }
        }
        else
        {
            flyPartical.Stop();
            audioSource.Stop();
        }
    }

    void Rotation()
    {
        rigidbody.freezeRotation = true;
        float rotate = 1f;
        float rotationSpeed = rotSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate((new Vector3(-rotate, 0, 0)) * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate((new Vector3(rotate, 0, 0)) * rotationSpeed);
        }
        
        rigidbody.freezeRotation = false;
    }

    void GetEnergy(int energyToAdd, GameObject batteryObj)
    {
        Destroy(batteryObj);
        energyTotal += energyToAdd;
        energyText.text = energyTotal.ToString();
    }
}
