using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputSystems;
using Player;

public class AnimatorHandler : MonoBehaviour
{
    #region Scripts
    PlayerManager playerManager;
    InputHandler inputHandler;
    PlayerLocomotion locomotion;
    #endregion
    #region Variables
    [HideInInspector]
    public Animator anim;
    public bool canRotate;
    int vertical, horizontal;
    #endregion

    public void Initialize()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        anim = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>();
        locomotion = GetComponentInParent<PlayerLocomotion>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovements, float horizontalMovements, bool isSprinting)
    {
        #region Vertical
        float v = 0;

        if (verticalMovements > 0 && verticalMovements < .55f)
        {
            v = .5f;
        }
        else if (verticalMovements > .55f)
        {
            v = 1;
        }
        else if (verticalMovements < 0 && verticalMovements > -.55f)
        {
            v = -.5f;
        }
        else if (verticalMovements < -.55f)
        {
            v = -1;
        }
        #endregion
        #region Horizontal
        float h = 0;

        if (horizontalMovements > 0 && horizontalMovements < .55f)
        {
            h = .5f;
        }
        else if (horizontalMovements > .55f)
        {
            h = 1;
        }
        else if (horizontalMovements < 0 && horizontalMovements > -.55f)
        {
            h = -.5f;
        }
        else if (horizontalMovements < -.55f)
        {
            h = -1;
        }
        #endregion

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovements;
        }

        anim.SetFloat(vertical, v, .1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, .1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, .2f);
    }

    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }

    public void EnableCombo()
    {
        anim.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        anim.SetBool("canDoCombo", false);
    }

    private void OnAnimatorMove()
    {
        if (playerManager.isInteracting == false) return;

        float delta = Time.deltaTime;
        locomotion.rigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        locomotion.rigidbody.velocity = velocity;
    }
}
