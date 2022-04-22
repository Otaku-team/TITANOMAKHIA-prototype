using UnityEngine;

namespace InputSystems
{
    public class InputHandler : MonoBehaviour
    {
        #region Variables
        PlayerControls _inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        UIManager uiManager;
        CameraHandler cameraHandler;

        public float horizontal, vertical;
        public float moveAmount;
        public float mouseX, mouseY;

        public bool b_Input;
        public bool a_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool jump_Input;
        public bool lockOn_Input;

        public bool inventory_Input;
        public bool inventoryFlag;

        public bool comboFlag;
        public bool rollFlag;
        public bool sprintFlag;
        public bool lockOnFlag;

        public float rollInputTimer;

        Vector2 _movementInput;
        Vector2 _cameraInput;
        #endregion

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        public void OnEnable()
        {
            _inputActions ??= new PlayerControls();

            _inputActions.PlayerMovement.Movement.performed += i => _movementInput = i.ReadValue<Vector2>();
            _inputActions.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();
            _inputActions.PlayerActions.RB.performed += i => rb_Input = true;
            _inputActions.PlayerActions.RT.performed += i => rt_Input = true;
            _inputActions.PlayerMovement.Jump.performed += i => jump_Input = true;
            _inputActions.PlayerActions.A.performed += i => a_Input = true;
            _inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
            _inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;

            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            HandleMoveInput(delta);
            HandleAttackInput(delta);
            HandleRollInput(delta);
            HandleLockOnInput(delta);
            HandleInventoryInput();
        }

        private void HandleMoveInput(float delta)
        {
            horizontal = _movementInput.x;
            vertical = _movementInput.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = _cameraInput.x;
            mouseY = _cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            b_Input = _inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            sprintFlag = b_Input;

            if (b_Input)
            {
                rollInputTimer += delta;
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < .5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {
            if (rb_Input)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;
                    if (playerManager.canDoCombo)
                        return;

                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }

            }

            if (rt_Input)
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;

                playerAttacker.HandleHeaveAttack(playerInventory.rightWeapon);
            }
        }

        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;
                
                if (inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.hudWindow.SetActive(false);
                    inventory_Input = false;
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput(float delta)
        {
            if (lockOn_Input && lockOnFlag == false)
            {
                lockOn_Input = false;
                cameraHandler.ClearLockOnTarget();
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTarget();
            }

            cameraHandler.SetCameraHeight(delta);
        }
    }
}
