using EggWars2D.Inputs;
using UnityEngine;

namespace EggWars2D.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float _moveSpeed = 10f;
        [SerializeField] float _minMaxX = 5f;
        [SerializeField] Transform _transform;
        
        InputReader _inputReader;
        float _clickedScreenX;
        float _clickedPlayerX;
        float _width;

        void OnValidate()
        {
            if (_transform == null) _transform = GetComponent<Transform>();
        }

        void Awake()
        {
            _inputReader = new InputReader();
        }

        void Start()
        {
            _width = Screen.width;
        }

        void Update()
        {
            ManageControl();
        }

        void ManageControl()
        {
            if (_inputReader.IsTouchDown)
            {
                _clickedScreenX = _inputReader.ScreenTouchPosition.x;
                _clickedPlayerX = _transform.position.x;
            }
            else if (_inputReader.IsTouch)
            {
                float xDifference = _inputReader.ScreenTouchPosition.x - _clickedScreenX;

                xDifference /= _width;
                xDifference *= _moveSpeed;

                float newXPosition = _clickedPlayerX + xDifference;
                newXPosition = Mathf.Clamp(newXPosition, -_minMaxX, _minMaxX);
                
                Vector3 position = _transform.position;
                _transform.position = new Vector3(newXPosition, position.y, position.z);
            }
        }
    }
}