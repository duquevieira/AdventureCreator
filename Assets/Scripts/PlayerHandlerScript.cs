using MeadowGames.UINodeConnect4;
using MoreMountains.Feedbacks;
using MoreMountains.InventoryEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerHandlerScript : MonoBehaviour
{
    protected static string ANIMATION_WALK = "walking";
    protected static string ANIMATION_EMOTION = "targetAnimation";

    [SerializeField]
    private float _speed;
    [SerializeField]
    private StoryEngineScript _storyEngineScript;
    [SerializeField]
    private Image _dialogBox;

    protected Camera _camera;
    private GameObject _player;
    protected GameObject _character;
    protected Animator _playerAnimator;
    protected bool _canMove;
    [HideInInspector]
    public Vector3 Target;


    void Start()
    {
        _camera = _storyEngineScript.Camera;
        _player = _storyEngineScript.Player;
        //TODO DELETE
        _player.transform.position = new Vector3(0, 2, 0);
        _character = _player.transform.Find(_storyEngineScript.getCharacterSkin()).gameObject;
        _character.SetActive(true);
        _playerAnimator = _character.GetComponent<Animator>();
        Target = _player.transform.position;
        _canMove = true;
    }

    public void CanMove()
    {
        _canMove = true;
    }

    public virtual void Update()
    {
        if(_dialogBox != null && _dialogBox.IsActive())
        {
            _canMove = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                ProcessEntry(hit, null);
            }
        }
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
        float distance = Vector3.Distance(_player.transform.position, hit.collider.transform.position);
        if (_storyEngineScript.IsStoryStep(hit.collider.name))
        {
            if (distance < 3)
            {
                if(transform.position != hit.transform.position)
                    _player.transform.LookAt(new Vector3(hit.transform.position.x, 0, hit.transform.position.z));
                int animation = _storyEngineScript.ProcessEntry(hit.collider, itemName);
                if (animation > -1)
                {
                    _playerAnimator.SetInteger(ANIMATION_EMOTION, animation);
                    StartCoroutine(ExecuteAfter(1.5f, _playerAnimator));
                }
                else
                {
                    _playerAnimator.SetInteger(ANIMATION_EMOTION, 7);
                    StartCoroutine(ExecuteAfter(1.5f, _playerAnimator));
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
            Animator animator = hit.collider.transform.root.GetComponent<Animator>();
            if (animator != null)
            {
                if (distance < 3)
                {
                    _player.transform.LookAt(new Vector3(hit.transform.position.x, 0, hit.transform.position.z));
                    animator.SetBool("On", !animator.GetBool("On"));
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
    }

    IEnumerator ExecuteAfter(float time, Animator animator)
    {
        yield return new WaitForSeconds(time);
        animator.SetInteger(ANIMATION_EMOTION, 0);
        _canMove = true;
        Target = _player.transform.position;
    }
}