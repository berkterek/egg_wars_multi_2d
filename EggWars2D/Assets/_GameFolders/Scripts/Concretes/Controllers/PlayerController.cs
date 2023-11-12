using EggWars2D.Inputs;
using UnityEngine;

namespace EggWars2D.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        InputReader _inputReader;
        float _clickedScreenX;

        void Awake()
        {
            _inputReader = new InputReader();
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
            }
            else if (_inputReader.IsTouch)
            {
                float xDifference = _inputReader.ScreenTouchPosition.x - _clickedScreenX;

                Debug.Log("X difference => " + xDifference);
            }
        }
    }
}