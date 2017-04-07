using UnityEngine;
using System.Collections;

/*
 * The input controller is a singleton class which interperates the input (from a keyboard or touch) and passes
 * it to the player controller
 */
public class InputController : MonoBehaviour
{

	public static InputController ins;

	#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
	// change slots with a swipe (true) or the accelerometer (false)
	public bool swipeToChangeSlots = false;
	// The number of pixels you must swipe in order to register a horizontal or vertical swipe
	public Vector2 swipeDistance = new Vector2(40, 40);
	// How sensitive the horizontal and vertical swipe are. The higher the value the more it takes to activate a swipe
	public float swipeSensitivty = 2;
	// More than this value and the player will move into the rightmost slot.
	// Less than the negative of this value and the player will move into the leftmost slot.
	// The accelerometer value in between these two values equals the middle slot.
	public float accelerometerRightSlotValue = 0.25f;
	// the higher the value the less likely the player will switch slots
	public float accelerometerSensitivity = 0.1f;
	private Vector2 touchStartPosition;
	private bool acceptInput; // ensure that only one action is performed per touch gesture
	#endif

	private PlayerController playerController;

	private void Awake ()
	{
		if (ins == null)
			ins = this;
		else
			Destroy (gameObject);

	}

	private void Start ()
	{
		StartGame ();
	}

	public void StartGame ()
	{
		playerController = PlayerController.ins;

		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
		touchStartPosition = Vector2.zero;

		#endif

		enabled = true;
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
		acceptInput = true;
		#endif
	}

	public void gameOver ()
	{
		enabled = false;
	}

	public void Update ()
	{
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
		if (Input.touchCount == 1) {
		Touch touch = Input.GetTouch(0);
		if (touch.phase == TouchPhase.Began) {
		touchStartPosition = touch.position;
		} else if (touch.phase == TouchPhase.Moved && acceptInput) {
		Vector2 diff = touch.position - touchStartPosition;
		if (diff.x == 0f)
		diff.x = 1f; // avoid divide by zero
		float verticalPercent = Mathf.Abs(diff.y / diff.x);

		if (verticalPercent > swipeSensitivty && Mathf.Abs(diff.y) > swipeDistance.y) {
		if (diff.y > 0) {
		playerController.jump(false);
		acceptInput = false;
		} else if (diff.y < 0) {
		playerController.slide();
		acceptInput = false;
		}
		touchStartPosition = touch.position;
		} else if (verticalPercent < (1 / swipeSensitivty) && Mathf.Abs(diff.x) > swipeDistance.x) {
		playerController.changeSlots(diff.x > 0 ? true : false);
		acceptInput = false;
		}
		} else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
		acceptInput = true;
		}
		}

		if (!swipeToChangeSlots)
		checkSlotPosition(Input.acceleration.x);
		#else
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			playerController.ChangeSlots (false);
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			playerController.ChangeSlots (true);
		} else if (Input.GetButtonDown("Jump")) {
			playerController.Jump ();
		} /* else if (Input.GetButtonUp("Slide")) {
			playerController.slide();
		}*/


		#endif
	}

	#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
	private void checkSlotPosition(float tiltValue)
	{
	SlotPosition currentSlot = playerController.getCurrentSlotPosition();
	switch (currentSlot) {
	case SlotPosition.Center:
	if (tiltValue < -accelerometerRightSlotValue) {
	playerController.changeSlots(SlotPosition.Left);
	} else if (tiltValue > accelerometerRightSlotValue) {
	playerController.changeSlots(SlotPosition.Right);
	}
	break;
	case SlotPosition.Left:
	if (tiltValue > -accelerometerRightSlotValue + accelerometerSensitivity) {
	playerController.changeSlots(SlotPosition.Center);
	}
	break;
	case SlotPosition.Right:
	if (tiltValue < accelerometerRightSlotValue - accelerometerSensitivity) {
	playerController.changeSlots(SlotPosition.Center);
	}
	break;
	}
	}
	#endif

}

