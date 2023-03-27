using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class PlayerHandlerScript : MonoBehaviour
{
    //String constants
    private static string FLOOR = "floor";
    private static string ANIMATION_WALK = "walking";
    private static string ITEMS_TAG = "items";
    private static string ITEMS_HIGH = "pickupHigh";
    private static string ITEMS_LOW = "pickupGround";

    //Default value should be 2.5
    [SerializeField]
    private float speed;
    //Script responsible for processing the events triggered
    [SerializeField]
    private StoryEngineScript storyEngineScript;

    //Game camera
    private new Camera camera;
    //Player object
    private GameObject player;
    //Player Skin
    private GameObject character;
    //Player Animator
    private Animator playerAnimator;
    //Used to check if user is locked in an animation
    private bool canMove;
    //Target coordinates of Player movement
    private Vector3 target;

    //Public function that sets up the Player from StoryEngine
    public void Setup(Camera camera, GameObject player, string name)
    {
        this.camera = camera;
        this.player = player;
        character = player.transform.Find(name).gameObject;
        character.SetActive(true);
        playerAnimator = character.GetComponent<Animator>();
        target = player.transform.position;
        canMove = true;
    }

    void Update()
    {
        //Register Player right-click to define new target coordinates
        if (Input.GetMouseButton(1))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag(FLOOR))
                {
                    playerAnimator.SetBool(ANIMATION_WALK, true);
                    target = hit.point;
                }
            }
        }
        //Player Movement and Animation
        if (canMove)
        {
            if (Vector3.Distance(player.transform.position, target) > 0.2)
            {
                playerAnimator.SetBool(ANIMATION_WALK, true);
                player.transform.position = Vector3.MoveTowards(player.transform.position, target, speed * Time.deltaTime);
                player.transform.LookAt(new Vector3(target.x, player.transform.position.y, target.z));
            }
            else
            {
                playerAnimator.SetBool(ANIMATION_WALK, false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if Player is colliding with items to display the correct animation
        if(other.CompareTag(ITEMS_TAG))
        {
            //Check if item has higher Y than Player
            if (other.gameObject.transform.position.y > (gameObject.transform.position.y + 0.5))
            {
                playerAnimator.SetBool(ITEMS_HIGH, true);
                StartCoroutine(ExecuteAfter(1.5f, playerAnimator, ITEMS_HIGH));
            }
            else
            {
                playerAnimator.SetBool(ITEMS_LOW, true);
                StartCoroutine(ExecuteAfter(1.5f, playerAnimator, ITEMS_LOW));
            }
            //Lock Player Movement
            canMove = false;
        }
        //If not an item send information to StoryEngine to process
        else
        {
            storyEngineScript.ProcessEntry(other);
        }
    }

    //Auxiliary function to update Animator after the animation is over
    IEnumerator ExecuteAfter(float time, Animator animator, string name)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool(name, false);
        canMove = true;
    }
}
