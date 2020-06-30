
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 240f;
    [SerializeField] float mainThrust = 5f;
    [SerializeField] float levelLoadDelay = 2f;


    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip dyingSound;
    [SerializeField] AudioClip finishLevel;
    


    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem dyingParticles;
    [SerializeField] ParticleSystem finishLevelParticles;
    

    
    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (state == State.Alive) {
         RespondToThrustInput();
         RespondToRotate();
        }
    }

    void RespondToThrustInput() {
        if (Input.GetKey(KeyCode.Space)) {
            
            ApplyThruster();

            } else {
                audioSource.Stop();
                mainEngineParticles.Stop();
            }
        }
    

    void ApplyThruster() {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if(!audioSource.isPlaying) {

            audioSource.PlayOneShot(mainEngine);
    }
        mainEngineParticles.Play();
    }

    void RespondToRotate() {

        rigidBody.freezeRotation = true;  // Manual control of rotation
        
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationThisFrame);

        } else if (Input.GetKey(KeyCode.D)) {

            transform.Rotate(-Vector3.forward * rotationThisFrame);

        }

       rigidBody.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision) {

        if (state != State.Alive ) { return; } // Ignores collisions when dead

        switch (collision.gameObject.tag) {
            case "Friendly":
                print("OK");
            break;

            case "Finish":
                Success();
            break;

            default: 
                Death();
            break;
        }
    }

    void Success() {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(finishLevel);
        finishLevelParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    void Death() {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(dyingSound);
        dyingParticles.Play();
        Invoke("LoadFirstScene", levelLoadDelay); 
    }

     void LoadNextScene() {
        int currrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currrentSceneIndex + 1;
        
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
            SceneManager.LoadScene(nextSceneIndex);
        } else {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    void LoadFirstScene() {
        SceneManager.LoadScene(0);
    }
    
}
