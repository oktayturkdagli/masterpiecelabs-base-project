using System;
using UnityEngine;

namespace Game
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private bool isUp, isDown, isMove, isStationary, isSwipe;
        [SerializeField] private SwipeType swipeType;
        [SerializeField] private Vector2 swipeDelta, moveDelta, startTouch, lastTouch;
        [SerializeField]  private float swipeThreshold = 300f;

        public bool IsUp
        {
            get => isUp;
            set => isUp = value;
        }
        public bool IsDown
        {
            get => isDown;
            set => isDown = value;
        }
        public Vector2 SwipeDelta
        {
            get => swipeDelta;
            set => swipeDelta = value;
        }
        public Vector2 StartTouch
        {
            get => startTouch;
            set => startTouch = value;
        }

        public static InputManager Instance { get; set; }

        public event Action onUp;
        public event Action onDown;
        public event Action<Vector2> onMove;
        public event Action onStationary;
        public event Action<SwipeType> onSwipe;

        public void OnUp()
        {
            onUp?.Invoke();
        }
        public void OnMove(Vector2 delta)
        {
            onMove?.Invoke(delta);
        }
        public void OnStationary()
        {
            onStationary?.Invoke();
        }
        public void OnDown()
        {
            onDown?.Invoke();
        }
        public void OnSwipe(SwipeType type)
        {
            onSwipe?.Invoke(type);
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Update()
        {
            ResetVariables();
            ComputerInputs();
            MobileInputs();
            SwipeCheck();
            HandleInputs();
        }

        private void ResetVariables()
        {
            isUp = isDown = isMove = isStationary = isSwipe = false;
            swipeType = SwipeType.Null;
            swipeDelta = Vector2.zero;
        }

        private void ComputerInputs()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDown = true;
                startTouch = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isUp = true;
                startTouch = swipeDelta = Vector2.zero;
            }
        }

        private void MobileInputs()
        {
            if (Input.touches.Length == 0) return;

            switch (Input.touches[0].phase)
            {
                case TouchPhase.Began:
                    isDown = true;
                    startTouch = Input.mousePosition;
                    lastTouch = Input.touches[0].position;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isUp = true;
                    startTouch = swipeDelta = Vector2.zero;
                    lastTouch = Vector2.zero;
                    break;
                case TouchPhase.Moved:
                    moveDelta = Input.touches[0].position - lastTouch;
                    if (moveDelta.magnitude > 0.01f)
                        isMove = true;
                    break;
                case TouchPhase.Stationary:
                    isStationary = true;
                    lastTouch = Input.touches[0].position;
                    break;
                default:
                    break;
            }
        }

        private void SwipeCheck()
        {
            if (startTouch != Vector2.zero)
            {
                // Mobile
                if (Input.touches.Length != 0)
                {
                    swipeDelta = Input.touches[0].position - startTouch;
                }
                // Computer
                else if (Input.GetMouseButton(0))
                {
                    swipeDelta = (Vector2)Input.mousePosition - startTouch;
                }
            }

            //  Threshold
            if (swipeDelta.magnitude <= swipeThreshold) return;

            isSwipe = true;
            var x = swipeDelta.x;
            var y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // Left or Right?
                if (x < 0)
                    swipeType = SwipeType.Left;
                else
                    swipeType = SwipeType.Right;
            }
            else
            {
                // Up or Down?
                if (y < 0)
                    swipeType = SwipeType.Down;
                else
                    swipeType = SwipeType.Up;
            }

            startTouch = Vector2.zero;
        }

        private void HandleInputs()
        {
            if (isUp)
            {
                OnUp();
            }

            if (isDown)
            {
                OnDown();
            }
            
            if (isMove)
            {
                OnMove(moveDelta);
            }
            
            if (isStationary)
            {
                OnStationary();
            }

            if (isSwipe && swipeType != SwipeType.Null)
            {
                OnSwipe(swipeType);
            }
        }

    }

    public enum SwipeType
    {
        Null,
        Left,
        Right,
        Up,
        Down
    }
    
}