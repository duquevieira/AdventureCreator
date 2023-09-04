using MoreMountains.Feedbacks;
using MoreMountains.InventoryEngine;
using System.Collections;
using UnityEngine;

public class PlayerHandlerScript : MonoBehaviour
{
    protected static string ANIMATION_WALK = "walking";
    private static string ITEMS_HIGH = "pickupHigh";
    private static string ITEMS_LOW = "pickupGround";

    /*private static string DOOR = "Door";
    private static string ANIMATION_DOOR = "clicked";
    private static string KEY = "KeyExample";
    private static string EXAMPLE = "ExampleItem";*/

    [SerializeField]
    private float _speed;
    [SerializeField]
    private StoryEngineScript _storyEngineScript;

    protected Camera _camera;
    private GameObject _player;
    protected GameObject _character;
    protected Animator _playerAnimator;
    protected bool _canMove;
    [HideInInspector]
    public Vector3 Target;

    //TODO DELETE
    private Animator NPCAnimator0;
    private Animator NPCAnimator1;

    void Start()
    {
        _camera = _storyEngineScript.Camera;
        _player = _storyEngineScript.Player;
        _character = _player.transform.Find(_storyEngineScript.getCharacterSkin()).gameObject;
        _character.SetActive(true);
        _playerAnimator = _character.GetComponent<Animator>();
        Target = _player.transform.position;
        _canMove = true;
        //TODO DELETE
        /*GameObject aux = GameObject.Find("NPC").transform.Find("MHunter").gameObject;
        aux.SetActive(true);
        NPCAnimator0 = aux.GetComponent<Animator>();
        aux = GameObject.Find("NPC (1)").transform.Find("MWorker").gameObject;
        aux.SetActive(true);
        NPCAnimator1 = aux.GetComponent<Animator>();
        NPCAnimator0.SetInteger("targetAnimation", 8);
        NPCAnimator1.SetInteger("targetAnimation", 8);*/
    }

    public virtual void Update()
    {
        //TODO DELETE
       /*if(Input.GetKeyDown(KeyCode.Q))
       {
            int aux = NPCAnimator0.GetInteger("targetAnimation");
            if(aux == 16)
            {
                aux = 0;
            }
            else
            {
                aux++;
            }
            NPCAnimator0.SetInteger("targetAnimation", aux);
       }
       if(Input.GetKeyDown(KeyCode.W))
       {
            int aux = NPCAnimator1.GetInteger("targetAnimation");
            if (aux == 16)
            {
                aux = 0;
            }
            else
            {
                aux++;
            }
            NPCAnimator1.SetInteger("targetAnimation", aux);
        }*/
       if (Input.GetMouseButtonDown(1))
       {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                ProcessEntry(hit, null);
            }
        }
        /*if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.name == DOOR &&
                Vector3.Distance(Player.transform.position, hit.collider.transform.position) < 3 &&
                Inventory.GetQuantity(KEY) > 0)
                {
                    Animator doorAnimator = hit.collider.GetComponent<Animator>();
                    MMFeedbacks clickFeedBack = hit.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<MMFeedbacks>();
                    doorAnimator.SetBool(ANIMATION_DOOR, true);
                    clickFeedBack?.PlayFeedbacks();
                    StartCoroutine(ExecuteAfter(3, doorAnimator, ANIMATION_DOOR));
                }
            }
        }*/
        if (_canMove)
        {
            Vector3 goal = new Vector3(Target.x, _player.transform.position.y, Target.z);
            if (Vector3.Distance(_player.transform.position, goal) > 0.2)
            {
                _playerAnimator.SetBool(ANIMATION_WALK, true);
                _player.transform.position = Vector3.MoveTowards(_player.transform.position, goal, _speed * Time.deltaTime);
                _player.transform.LookAt(goal);
            }
            else
                _playerAnimator.SetBool(ANIMATION_WALK, false);
        }
    }

    public void ProcessEntry(RaycastHit hit, string itemName)
    {
        if (_storyEngineScript.IsStoryStep(hit.transform.name))
        {
            float distance = Vector3.Distance(_player.transform.position, hit.collider.transform.position);
            if (distance < 3)
            {
                if (_storyEngineScript.ProcessEntry(hit.collider, itemName))
                {
                    _playerAnimator.SetBool(ITEMS_HIGH, true);
                    StartCoroutine(ExecuteAfter(1.5f, _playerAnimator, ITEMS_HIGH));
                }
                else
                {
                    _playerAnimator.SetBool(ITEMS_LOW, true);
                    StartCoroutine(ExecuteAfter(1.5f, _playerAnimator, ITEMS_LOW));
                }
                _canMove = false;
            }
            else
            {
                Target = Vector3.Lerp(hit.point, _player.transform.position, 2 / distance);
            }
        }
        else
        {
            Target = hit.point;
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.position.y > (gameObject.transform.position.y + 0.5))
        {
            _playerAnimator.SetBool(ITEMS_HIGH, true);
            StartCoroutine(ExecuteAfter(1.5f, _playerAnimator, ITEMS_HIGH));
        }
        else
        {
            _playerAnimator.SetBool(ITEMS_LOW, true);
            StartCoroutine(ExecuteAfter(1.5f, _playerAnimator, ITEMS_LOW));
        }
        _canMove = false;
        _storyEngineScript.ProcessEntry(other.gameObject.name);
    }*/

    IEnumerator ExecuteAfter(float time, Animator animator, string name)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool(name, false);
        _canMove = true;
        Target = _player.transform.position;
    }
}