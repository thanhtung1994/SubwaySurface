using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public enum SlotPosition
{
	Left = -1,
	Center,
	Right,
}

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	public static PlayerController ins;
	public Transform groundCheck;
	public LayerMask groundLayer;

	private CharacterController character;
	private Animator anim;
	private Rigidbody rigid;
	private SlotPosition currentSlotPosition;
	private Vector3 targetPosition;
	private Transform thisTransform;
	private Vector3 startPosition;
	private float targetSlotValue;
	private float speed = 8;
	private float distanceLane = 2;
	private float animationDuration = 2;


	private float jumpForce =150f;
	private Collider[] groundCollision;
	private bool isGround = false;
	private float groundCheckRadius = 0.5f;


	private void Awake ()
	{
		if (ins == null)
			ins = this;
		else
			Destroy (gameObject);
		
	}

	private void Start ()
	{
		character = GetComponent<CharacterController> ();
		rigid = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		thisTransform = transform;
		startPosition = thisTransform.position;
		targetPosition = startPosition;
		targetPosition.y = thisTransform.position.y;
	}

	private void Update ()
	{
		groundCollision = Physics.OverlapSphere (groundCheck.position, groundCheckRadius, groundLayer);
		if (groundCollision.Length > 0) {
			isGround = true;
		} else {
			isGround = false;

		}
		targetPosition.y = thisTransform.position.y;
		targetPosition.z += speed * Time.deltaTime;
		if (thisTransform.position != targetPosition) {
			transform.position = Vector3.Lerp (transform.position, targetPosition, (speed + 2) * Time.deltaTime);
		}
	}

	private void UpdateTargetPosition ()
	{
		if (Time.timeSinceLevelLoad < animationDuration)
			return;
		targetPosition.x = startPosition.x + targetSlotValue;
	}

	public void ChangeSlots (bool right)
	{
		SlotPosition targetSlot = (SlotPosition)Mathf.Clamp ((int)currentSlotPosition + (right ? 1 : -1), (int)SlotPosition.Left, (int)SlotPosition.Right);

		ChangeSlots (targetSlot);
	}

	public void ChangeSlots (SlotPosition targetSlot)
	{
		if (targetSlot == currentSlotPosition)
			return;
		currentSlotPosition = targetSlot;
		targetSlotValue = (int)currentSlotPosition * distanceLane;
		UpdateTargetPosition ();
	}

	public void Jump ()
	{
		Debug.Log ("test1");
		if (isGround) {
			rigid.AddForce (new Vector3(0,jumpForce,0));
			anim.SetTrigger ("Jump");
			Debug.Log ("test");
		}
	}
}
