using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    private Rigidbody rigidBody;
    private AudioSource rocketThrusting;

    enum State{Alive,Dying, Transcending};
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rocketThrusting = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
      //todo stop sound on death
      if(state == State.Alive){
        Thrust();
        Rotate();
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
          state = State.Transcending;
          Invoke("LoadNextScene", 1f);
          break;
        default: print("Dead");
          state = State.Dying;
          if(sceneName.Equals("Level 2")){
            Invoke("LoadSecondLevel", 1f);
          } else {
            Invoke("LoadFirstLevel", 1f);
          }
          break;
      }
    }
    private void Thrust(){
      if(Input.GetKey(KeyCode.Space)){
             
              rigidBody.AddRelativeForce(Vector3.up * mainThrust);

              if(!rocketThrusting.isPlaying){

                  rocketThrusting.Play();
              }
          }else {
            rocketThrusting.Stop();
          }
  }

    private void Rotate(){

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
} //end of class
