using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	[Header("Character Values")]
	public Vector2 move;
	public Vector2 look;
	public bool jump;
	public bool sprint;
	public bool cursorLocked = true;

	[Header("Gun Values")]
	public bool toggleAiming = false;
	public bool aiming = false;
	public bool isFiring = false;

	public delegate void OnPress();
	public OnPress onReload;
	public OnPress onSwitchFiremode;

	// methods called by player input component
	public void OnMove(InputValue value) => MoveInput(value.Get<Vector2>());
	public void OnLook(InputValue value) => LookInput(value.Get<Vector2>());
	public void OnJump(InputValue value) => JumpInput(value.isPressed);
	public void OnSprint(InputValue value) => SprintInput(value.isPressed);
	public void OnAim(InputValue value) => AimInput(value.isPressed);
	public void OnFire(InputValue value) => FireInput(value.isPressed);
	public void OnReload(InputValue value)
    {
		if (value.isPressed) 
			onReload.Invoke();
    }
	public void OnSwitchFiremode(InputValue value) 
	{ 
		if (value.isPressed) 
			onSwitchFiremode.Invoke(); 
	}

	// publlic methods. potentially used by on screen (mobile) controls
	public void MoveInput(Vector2 newMoveDirection) => move = newMoveDirection;
	public void LookInput(Vector2 newLookDirection) => look = newLookDirection;
	public void JumpInput(bool newJumpState) => jump = newJumpState;
	public void SprintInput(bool newSprintState) => sprint = newSprintState;
	public void FireInput(bool newFireState) => isFiring = newFireState;
	public void AimInput(bool newAimState)
    {
		if (!toggleAiming)
			aiming = newAimState;
		if(toggleAiming && newAimState)
			aiming = !aiming;
    }

	// cursor settings
	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}