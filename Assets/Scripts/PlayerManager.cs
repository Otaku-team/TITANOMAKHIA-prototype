using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputSystems;
using Player;

public class PlayerManager : CharacterManager
{
    #region Scripts
    InputHandler inputHandler;
    Animator anim;
    CameraHandler _cameraHandler;
    PlayerLocomotion playerLocomotion;
    #endregion
    #region Variables
    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    #endregion

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        _cameraHandler = FindObjectOfType<CameraHandler>();
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        anim.SetBool("isInAir", isInAir);
        
        inputHandler.TickInput(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);

        CheckForInteractbleObject();
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        //playerLocomotion.HandleJumping();
    }

    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.rb_Input = false;
        inputHandler.rt_Input = false;
        inputHandler.a_Input = false;
        inputHandler.jump_Input = false;
        //inputHandler.inventory_Input = false;

        float delta = Time.deltaTime;
        _cameraHandler ??= CameraHandler.instance;
        _cameraHandler.FollowTarget(delta);
        _cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);

        if (isInAir)
        {
            playerLocomotion.inAirTime = playerLocomotion.inAirTime + Time.deltaTime;
        }
    }

    public void CheckForInteractbleObject()
    {
        if (Physics.SphereCast(transform.position, .3f, transform.forward, out RaycastHit hit, 1f, _cameraHandler.ignoreLayers))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject)
                {
                    string interactableText = interactableObject.interactbleText;

                    if (inputHandler.a_Input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
    }
}
