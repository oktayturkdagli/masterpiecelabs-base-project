using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int speed = 2;
        [SerializeField] private bool canMove = false;
        [SerializeField] private Vector3 direction = Vector3.forward;
        [SerializeField] private float horizontalBorderSize = 5;
        private Animator playerAnimator;
        private static readonly int AnimatorParameterWalk = Animator.StringToHash("Walk");
        private static readonly int AnimatorParameterDance = Animator.StringToHash("Dance");
        private static readonly int AnimatorParameterBeSad = Animator.StringToHash("BeSad");

        private void Start()
        {
            playerAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            EventManager.Instance.onPressPlayButton += OnPressPlayButton;
            InputManager.Instance.onMove += OnFingerMove;
            InputManager.Instance.onStationary += OnFingerUp;
            InputManager.Instance.onUp += OnFingerUp;
            EventManager.Instance.onWinLevel += OnWinLevel;
            EventManager.Instance.onLoseLevel += OnLoseLevel;
        }

        private void OnDisable()
        {
            EventManager.Instance.onPressPlayButton -= OnPressPlayButton;
            InputManager.Instance.onMove -= OnFingerMove;
            InputManager.Instance.onStationary -= OnFingerUp;
            InputManager.Instance.onUp -= OnFingerUp;
            EventManager.Instance.onWinLevel -= OnWinLevel;
            EventManager.Instance.onLoseLevel -= OnLoseLevel;
        }
        
        private void OnFingerMove(Vector2 delta)
        {
            if (!canMove) return;
            var value = Math.Clamp(delta.x, -1f, 1f);
            direction.x = 20 * value;
        }
        
        private void OnFingerUp()
        {
            if (!canMove) return;
            direction.x = 0;
        }
        
        private void OnPressPlayButton()
        {
            StartCoroutine(ChangeCanMoveDelayed(0.3f, true));
        }
        
        private void OnWinLevel()
        {
            Dance();
        }
        
        private void OnLoseLevel()
        {
            BeSad();
        }

        private void Update()
        {
            Walk();
        }

        private void Walk()
        {
            if (!canMove) return;
            
            // Borders
            switch (direction.x)
            {
                case > 0 when transform.position.x > (horizontalBorderSize/2 - 0.1f):
                case < 0 when transform.position.x < -(horizontalBorderSize/2 - 0.1f):
                    direction.x = 0;
                    break;
            }

            playerAnimator.SetBool(AnimatorParameterWalk, true);
            transform.Translate(direction * (speed * Time.deltaTime), Space.World);
        }

        private void Rise()
        {
            Debug.Log("Rise");
        }

        private void Fall()
        {
            Debug.Log("Fall");
        }

        private void BeSad()
        {
            canMove = false;
            playerAnimator.SetTrigger(AnimatorParameterBeSad);
        }

        private void Dance()
        {
            canMove = false;
            playerAnimator.SetTrigger(AnimatorParameterDance);
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "Finish":
                    other.enabled = false;
                    EventManager.Instance.OnWinLevel();
                    break;
                case "Shoe":
                    Destroy(other.gameObject);
                    Debug.Log("A Shoe was taken.");
                    break;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            switch (collision.gameObject.tag)
            {
                case "CubeRed":
                    EventManager.Instance.OnLoseLevel();
                    break;
            }
        }

        private IEnumerator ChangeCanMoveDelayed(float time, bool enable) 
        {
            yield return new WaitForSecondsRealtime(time); //Wait time second
            canMove = enable;
        }
        
    }
}