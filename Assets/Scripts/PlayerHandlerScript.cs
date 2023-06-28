using MoreMountains.Feedbacks;
using MoreMountains.InventoryEngine;
using System.Collections;
using UnityEngine;
using static SwitchCreateMode;

public class PlayerHandlerScript : MonoBehaviour
{
    private static string ANIMATION_WALK = "walking";
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
    [SerializeField] 
    private SwitchCreateMode createMode;

    private Camera _camera;
    private GameObject _player;
    private GameObject _character;
    private Animator _playerAnimator;
    private bool _canMove;
    [HideInInspector]
    public Vector3 Target;

    void Start()
    {
        _camera = _storyEngineScript.Camera;
        _player = _storyEngineScript.Player;
        _character = _player.transform.Find(_storyEngineScript.getCharacterSkin()).gameObject;
        //_character.SetActive(true);
        _playerAnimator = _character.GetComponent<Animator>();
        Target = _player.transform.position;
        _canMove = false;
    }

    void Update()
    {
        if (createMode.currentMode == SwitchCreateMode.CreateMode.TestingMode)
        {
            _character.SetActive(true);
            _canMove = true;
            if (Input.GetMouseButton(1))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    _playerAnimator.SetBool(ANIMATION_WALK, true);
                    Target = hit.point;
                }
            }
        } else
        {
            _canMove= false;
            _character.SetActive(false);
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
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            
        }*/
        if (_canMove)
        {
            if (Vector3.Distance(_player.transform.position, Target) > 0.2)
            {
                _playerAnimator.SetBool(ANIMATION_WALK, true);
                _player.transform.position = Vector3.MoveTowards(_player.transform.position, Target, _speed * Time.deltaTime);
                _player.transform.LookAt(new Vector3(Target.x, _player.transform.position.y, Target.z));
            }
            else
                _playerAnimator.SetBool(ANIMATION_WALK, false);
        }
    }

    private void OnTriggerEnter(Collider other)
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
    }

    IEnumerator ExecuteAfter(float time, Animator animator, string name)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool(name, false);
        _canMove = true;
    }
}
