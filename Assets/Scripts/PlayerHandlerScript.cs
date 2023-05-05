using System.Collections;
using UnityEngine;

public class PlayerHandlerScript : MonoBehaviour
{
    //private static string FLOOR = "floor";
    private static string ANIMATION_WALK = "walking";
    private static string ITEMS_TAG = "items";
    private static string ITEMS_HIGH = "pickupHigh";
    private static string ITEMS_LOW = "pickupGround";

    [SerializeField]
    private float _speed;
    [SerializeField]
    private StoryEngineScript _storyEngineScript;

    private Camera _camera;
    private GameObject _player;
    private GameObject _character;
    private Animator _playerAnimator;
    private bool _canMove;
    private Vector3 _target;

    void Start()
    {
        _camera = _storyEngineScript.Camera;
        _player = _storyEngineScript.Player;
        _character = _player.transform.Find(_storyEngineScript.getCharacterSkin()).gameObject;
        _character.SetActive(true);
        _playerAnimator = _character.GetComponent<Animator>();
        _target = _player.transform.position;
        _canMove = true;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
               _playerAnimator.SetBool(ANIMATION_WALK, true);
                _target = hit.point;
            }
        }
        if (_canMove)
        {
            if (Vector3.Distance(_player.transform.position, _target) > 0.2)
            {
                _playerAnimator.SetBool(ANIMATION_WALK, true);
                _player.transform.position = Vector3.MoveTowards(_player.transform.position, _target, _speed * Time.deltaTime);
                _player.transform.LookAt(new Vector3(_target.x, _player.transform.position.y, _target.z));
            }
            else
                _playerAnimator.SetBool(ANIMATION_WALK, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(ITEMS_TAG))
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
        }
        _storyEngineScript.ProcessEntry(other.gameObject.name);
    }

    IEnumerator ExecuteAfter(float time, Animator animator, string name)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool(name, false);
        _canMove = true;
    }
}
