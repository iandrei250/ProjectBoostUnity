using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;
    private Rigidbody rigidBody;
    private AudioSource audioSource;

    enum State{Alive,Dying, Transcending};
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
      if(state == State.Alive){
        RespondToThrustInput();
        RespondToRotateInput();
      }
    }

    private void OnCollisionEnter(Collision collision){
      Scene currentScene = SceneManager.GetActiveScene ();
      string sceneName = currentScene.name;

        if(state != State.Alive){
          return;
        }
      switch(collision.gameObject.tag){
        case "Friendly": 
          break;

        case "Finished":
          onSuccess();
          break;
          
        default: 
          onDeath(sceneName);
          break;
      }
    }
    private void RespondToThrustInput(){
      if(Input.GetKey(KeyCode.Space)){
             ApplyThrust();
      } else {
            audioSource.Stop();
            mainEngineParticles.Stop();
      }
  }

    private void RespondToRotateInput(){

    rigidBody.freezeRotation = true;//take manual control of rotation
     float rotationThisFrame = Time.deltaTime * rcsThrust;

    if(Input.GetKey(KeyCode.A)){

     
      transform.Rotate(Vector3.forward * rotationThisFrame);

    } else if(Input.GetKey(KeyCode.D)){
      
      transform.Rotate(Vector3.back * rotationThisFrame);
    }
      rigidBody.freezeRotation = false; // resume physics control of rotation
      
  }

  private void LoadNextScene(){
      SceneManager.LoadScene(1);
  }

  private void LoadFirstLevel(){
    SceneManager.LoadScene(0);
  }
  private void LoadSecondLevel(){
    SceneManager.LoadScene(1);
  }

  public void ApplyThrust(){
     rigidBody.AddRelativeForce(Vector3.up * mainThrust);

      if(!audioSource.isPlaying){

        audioSource.PlayOneShot(mainEngine);
      }
      mainEngineParticles.Play();
  }

  public void onDeath(string sceneName)
  {
    state = State.Dying;
    audioSource.Stop();
    audioSource.PlayOneShot(death);

    if(mainEngineParticles.isPlaying){
      mainEngineParticles.Stop();
    }

    deathParticles.Play();
    if(sceneName.Equals("Level 2")){
      Invoke("LoadSecondLevel", 1f);
    } else {
      Invoke("LoadFirstLevel", 1f);
    }
  }
  public void onSuccess()
  {
    state = State.Transcending;
    audioSource.Stop();
    audioSource.PlayOneShot(success);
    successParticles.Play();
    Invoke("LoadNextScene", 1f);
  }
} //end of class
