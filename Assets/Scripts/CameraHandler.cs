using InputSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler instance;

    #region Variables
    private PlayerManager playerManager;
    private InputHandler inputHandler;
    public Transform targetTranform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Transform myTransform;
    private Vector3 cameraTransformPosition;

    public LayerMask ignoreLayers;
    public LayerMask enviromentLayer;

    private Vector3 cameraFollowVelocity;

    public float lookspeed = .1f;
    public float followSpeed = .1f;
    public float pivotSpeed = .03f;

    private float targetPosition;
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;
    public float minimumPivot = -35;
    public float maximumPivot = 35;

    public float cameraSphereRadius = .2f;
    public float cameraCollisionOffset = .2f;
    public float minimumCollisionOffset = .2f;
    public float lockedPivotPosition = 2.25f;
    public float unlockedPivotPosition = 1.65f;

    public Transform currentLockOnTarget;
    public List<CharacterManager> availableTargets = new List<CharacterManager>();
    public Transform nearestLockOnTarget;
    public Transform leftLockTransform;
    public Transform rightLockTransform;
    public float maxlockOnDistance = 30;
    #endregion

    private void Awake()
    {
        instance = this;
        myTransform = transform;
        defaultPosition = cameraPivotTransform.localPosition.z;
        ignoreLayers = ~(1 << 9 | 1 << 10);
        targetTranform = FindObjectOfType<PlayerManager>().transform;
        inputHandler = FindObjectOfType<InputHandler>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {
        enviromentLayer = LayerMask.NameToLayer("Enviroment");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTranform.position, 
            ref cameraFollowVelocity, delta / followSpeed);
        myTransform.position = targetPosition;

        HandleCameraCollisions(delta);
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
        {
            lookAngle += (mouseXInput * lookspeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
        else
        {
            float velocity = 0;

            Vector3 dir = currentLockOnTarget.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = currentLockOnTarget.position - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
        }
    }

    private void HandleCameraCollisions(float delta)
    {
        targetPosition = defaultPosition;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(
            cameraPivotTransform.position, cameraSphereRadius, direction, out RaycastHit hit, Mathf.Abs(targetPosition)
            , ignoreLayers))
        {
            float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(dis - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = -minimumCollisionOffset;
        }

        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / .2f);
        cameraTransform.localPosition = cameraTransformPosition;
    }

    public void HandleLockOn()
    {
        float shorestDistance = Mathf.Infinity;
        float shortestDistanceOfLeftTarget = Mathf.Infinity;
        float shortestDistanceOfRightTarget = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(targetTranform.position, 26);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            if (character)
            {
                Vector3 lockTargetDirection = character.transform.position - targetTranform.position;
                float distanceFromTarget = Vector3.Distance(targetTranform.position, character.transform.position);
                float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                if (character.transform.root != targetTranform.transform.root
                    && viewableAngle > -50 && viewableAngle < 50 
                    && distanceFromTarget <= maxlockOnDistance)
                {
                    if (Physics.Linecast(playerManager.lockOnTransform.position, character.transform.position, out RaycastHit hit))
                    {
                        Debug.DrawLine(playerManager.lockOnTransform.position, character.transform.position);

                        if (hit.transform.gameObject.layer == enviromentLayer)
                        {
                            // Cannot lock onto target, object in the way

                        }
                        else
                        {
                            availableTargets.Add(character);
                        }
                    }
                }
            }
        }

        for (int k = 0; k < availableTargets.Count; k++)
        {
            float distanceFromTarget = Vector3.Distance(targetTranform.position, availableTargets[k].transform.position);

            if (distanceFromTarget < shorestDistance)
            {
                shorestDistance = distanceFromTarget;
                nearestLockOnTarget = availableTargets[k].lockOnTransform;
            }

            if (inputHandler.lockOnFlag)
            {
                Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(availableTargets[k].transform.position);
                var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
                var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;

                if (relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                {
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    leftLockTransform = availableTargets[k].lockOnTransform;
                }

                if (relativeEnemyPosition.x < 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                {
                    shortestDistanceOfRightTarget = distanceFromRightTarget;
                    rightLockTransform = availableTargets[k].lockOnTransform;
                }
            }
        }
    }

    public void ClearLockOnTarget()
    {
        availableTargets.Clear();
        currentLockOnTarget = null;
        nearestLockOnTarget = null;
    }

    public void SetCameraHeight(float delta)
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
        Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

        if (currentLockOnTarget != null)
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, delta);
        }
        else
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, delta);
        }

    }
}
