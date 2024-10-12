
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class FirstPersonControls : MonoBehaviour
{

    [Header("MOVEMENT SETTINGS")]
    [Space(5)]
    // Public variables to set movement and look speed, and the player camera
    public float moveSpeed; // Speed at which the player moves
    public float lookSpeed; // Sensitivity of the camera movement
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera

    // Private variables to store input values and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player
    private CharacterController characterController; // Reference to the CharacterController component

    [Header("SHOOTING SETTINGS")]
    [Space(5)]
    public GameObject projectilePrefab; // Projectile prefab for shooting
    public Transform firePoint; // Point from which the projectile is fired
    public float projectileSpeed = 20f; // Speed at which the projectile is fired

    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition; // Position where the picked-up object will be held
    private GameObject heldObject; // Reference to the currently held object
    public float pickUpRange = 3f; // Range within which objects can be picked up
    private bool holdingGun = false;
    
    [Header("PULLING SETTINGS")]
    [Space(5)]
    public Transform grabPosition; // Position where the picked-up object will be held
    private GameObject grabbedObject; // Reference to the object currently being grabbed
    public float grabRange = 3f; // Range within which objects can be grabbed from
    private bool isGrabbing = false;
    private float pullForce = 10.0f;

    [Header("ROTATE SETTINGS")] 
    [Space(5)] 
    private GameObject objectRotate;
    private bool isRotating;
    public float rotateRange = 5f;

    [Header("CROUCH HEIGHT SETTINGS")]
    [Space(5)]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchSpeed = 0.5f;
    private bool isCrouching = false;
    
    [Header("INTERACT SETTINGS")]
    [Space(5)]
    public Material switchMaterial; // Material to apply when switch is activated
    public GameObject[] objectsToChangeColor; // Array of objects to change color
    public LayerMask interactLayers;
    public ObjectInteraction objectInteraction;
    public bool isShowing = false;
    public bool isInputEnabled = true;
    public ThoughtCount thoughtCount;
    public GameObject[] frontDoor;
        
    [Header("UI SETTINGS")]
    [Space(5)]
    public TextMeshProUGUI objectInfoText;
    public TextMeshProUGUI pickUpText;
    public Image healthBar;
    public float damageAmount = 0.25f; // Reduce the health bar by this amount
    private float healAmount = 0.5f;// Fill the health bar by this amount
    public TextMeshProUGUI doorOpenText;
    public TextMeshProUGUI doorCloseText;
    public TextMeshProUGUI doorLockedText;
    public TextMeshProUGUI thoughtText;
    public TextMeshProUGUI keyText;
    public TextMeshProUGUI writtenThoughtText;
    public TextMeshProUGUI myThoughtText;
    public GameObject ThoughtBackground;
    public float duration = 10f;
    private bool hasShownMessage = false;
    public float objectRange = 20f;

    [Header("CHANGE VIEWS")] 
    [Space(5)]
    public SecondView secondView;
    public bool isNormalView = true;
    public GameObject SecondViewCanvas;     // The UI panel to display item details
    public GameObject NormalViewCanvas;
    public GameObject NormalViewVolume;
    public GameObject SecondViewVolume;

    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        // Create a new instance of the input actions
        var playerInput = new NewControls();

        // Enable the input actions
        playerInput.Player.Enable();

        // Subscribe to the movement input events
        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero; // Reset moveInput when movement input is canceled

        // Subscribe to the look input events
        playerInput.Player.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>(); // Update lookInput when look input is performed
        playerInput.Player.LookAround.canceled += ctx => lookInput = Vector2.zero; // Reset lookInput when look input is canceled

        // Subscribe to the jump input event
        playerInput.Player.Jump.performed += ctx => Jump(); // Call the Jump method when jump input is performed

        // Subscribe to the shoot input event
        playerInput.Player.Shoot.performed += ctx => Shoot(); // Call the Shoot method when shoot input is performed

        // Subscribe to the pick-up input event
        playerInput.Player.PickUp.performed += ctx => PickUpObject(); // Call the PickUpObject method when pick-up input is performed
        
        // Subscribe to the pick-up input event
        playerInput.Player.Pull.performed += ctx => PullObject(); // Call the PullObject method when grab input is performed
        
        // Subscribe to the pick-up input event
        playerInput.Player.Rotate.performed += ctx => RotateObject(); // Call the RotateObject method when rotate input is performed

        // Subscribe to the crouch input event
        playerInput.Player.Crouch.performed += ctx => ToggleCrouch(); // Call the Crouch method when crouch input is performed
        
        // Subscribe to the interact input event
        playerInput.Player.Interact.performed += ctx => Interact(); // Interact with switch
        
        // Subscribe to the change views inout event
        playerInput.Player.ChangeView.performed += ctx => ChangeView(); // Interact with switch

    }

    private void Update()
    {
        if (isInputEnabled == true)
        {
            // Call Move and LookAround methods every frame to handle player movement and camera rotation
            Move();
            LookAround();
            ApplyGravity();
            CheckForObject();
        }
    }

    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        float currentSpeed;
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // Move the character controller based on the movement vector and speed
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }

    public void LookAround()
    {
        // Get horizontal and vertical look inputs and adjust based on sensitivity
        float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;

        // Horizontal rotation: Rotate the player object around the y-axis
        transform.Rotate(0, LookX, 0);

        // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // Apply the clamped vertical rotation to the player camera
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
    }

    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small value to keep the player grounded
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to the velocity
        characterController.Move(velocity * Time.deltaTime); // Apply the velocity to the character
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            // Calculate the jump velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void Shoot()
    {
        if (holdingGun == true)
        {
            // Instantiate the projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Get the Rigidbody component of the projectile and set its velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * projectileSpeed;

            // Destroy the projectile after 3 seconds
            Destroy(projectile, 3f);
        }
    }

    public void PickUpObject()
    {
        // Check if we are already holding an object
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false; // Enable physics
            heldObject.transform.parent = null;
            holdingGun = false;
        }

        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);


        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.CompareTag("pickUp"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
            }
            else if (hit.collider.CompareTag("Gun"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                holdingGun = true;
            }
        }
    }

    /*public void PullObject()
    {
        Debug.Log("Grabbing");
        if (grabbedObject != null)
        {
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false; // Enable physics
            grabbedObject.transform.parent = null;
            //grabbedObject = null;
            //return;
        }

        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * grabRange, Color.red, 2f);


        if (Physics.Raycast(ray, out hit, grabRange))
        {
            // Check if the hit object has the tag "Movable"
            if (hit.collider.CompareTag("Movable"))
            {
                // Grab the object
                grabbedObject = hit.collider.gameObject;
                //grabbedObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                grabbedObject.transform.position = grabPosition.position;
                grabbedObject.transform.rotation = grabPosition.rotation;
                grabbedObject.transform.parent = grabPosition;
            }
        }
    }*/

    public void PullObject()
    {
        
    }

    public void RotateObject()
    {
        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * rotateRange, Color.red, 2f);


        if (Physics.Raycast(ray, out hit, rotateRange))
        {
            // Check if the hit object has the tag "Movable"
            if (hit.collider.CompareTag("Movable"))
            {
                objectRotate = hit.collider.gameObject;
                if (objectRotate != null)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        // Rotate 90 degrees around the x-axis
                        objectRotate.transform.Rotate(Vector3.up * -90f);
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        // Rotate 90 degrees around the x-axis
                        objectRotate.transform.Rotate(Vector3.up * 90f);
                    }
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        // Rotate 90 degrees around the y-axis
                        objectRotate.transform.Rotate(Vector3.right * 90f);
                    }
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        // Rotate 90 degrees around the y-axis
                        objectRotate.transform.Rotate(Vector3.right * -90f);
                    }
                }
            }
        }
    }



    public void ToggleCrouch()
    {
        if(isCrouching)
        {
            //Stand up
            characterController.height = standingHeight;
            isCrouching = false;
        }
        else
        {
            //Crouch down
            characterController.height = crouchHeight;
            isCrouching = true;
        }
    }
    
    public void Interact()
    {
        // Perform a raycast to detect the lightswitch
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, objectRange)) //, interactLayers))
        {
            if (hit.collider.CompareTag("Switch")) // Assuming the switch has this tag
            {
                // Change the material color of the objects in the array
                foreach (GameObject obj in objectsToChangeColor)
                {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = switchMaterial.color; // Set the color to match the switch material color
                    }
                }
            }

            else if (hit.collider.CompareTag("Door") || hit.collider.CompareTag("Drawer")) // Check if the object is a door
            {
                // Start moving the door upwards
                //StartCoroutine(RaiseDoor(hit.collider.gameObject));
                
                Door DoorOpen = hit.collider.GetComponent<Door>();
                
                if (DoorOpen.needsKey && !DoorOpen.hasUnlocked)
                    doorLockedText.gameObject.SetActive(true);
                
                DoorOpen.ToggleDoor(transform);
                doorLockedText.gameObject.SetActive(false);
            }
            else if (hit.collider.CompareTag("Info"))
            {
                //string objectName = hit.collider.name;
                Debug.Log("Showing info");
                ObjectData currentObjectData = hit.collider.GetComponent<ObjectDataHolder>()?.objectData;
                if (currentObjectData != null)
                {
                    if (isShowing == false)
                    {
                        isInputEnabled = false;
                        isShowing = true;
                        objectInfoText.gameObject.SetActive(false);
                        objectInteraction.ShowObjectDetails(currentObjectData);
                    }
                    else
                    {
                        isInputEnabled = true;
                        isShowing = false;
                        objectInfoText.gameObject.SetActive(true);
                        objectInteraction.HideObjectDetails();
                    
                    }
                }
            }
            else if (hit.collider.CompareTag("Thought"))
            {
                thoughtCount.AddThought();
                Destroy(hit.collider.gameObject);
                
                Thoughts thoughts = hit.collider.gameObject.GetComponent<Thoughts>();
                writtenThoughtText.text = thoughts.writtenThought;
                myThoughtText.text = thoughts.myThought;
                
                StartCoroutine(ActivateForDuration());
                
            }
            else if (hit.collider.CompareTag("Key"))
            {
                foreach (var door in frontDoor)
                {
                    Door openDoor = door.GetComponent<Door>();
                    openDoor.hasKey = true;
                    Destroy(hit.collider.gameObject);
                }
                
            }
        }
    }
    
    private IEnumerator ActivateForDuration()
    {
        // Set the object to active
        //writtenThoughtText.gameObject.SetActive(true);
        //myThoughtText.gameObject.SetActive(true);
        ThoughtBackground.SetActive(true);

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Set the object to inactive
        //writtenThoughtText.gameObject.SetActive(false);
        //myThoughtText.gameObject.SetActive(false);
        ThoughtBackground.SetActive(false);
    }

    private IEnumerator RaiseDoor(GameObject door)
    {
        float raiseAmount =7f; // The total distance the door will be raised
        float raiseSpeed = 2f; // The speed at which the door will be raised
        Vector3 startPosition = door.transform.position; // Store the initial position of the door
        Vector3 endPosition = startPosition + Vector3.up * raiseAmount; // Calculate the final position of the door after raising

        // Continue raising the door until it reaches the target height
        while (door.transform.position.y < endPosition.y)
        {
            // Move the door towards the target position at the specified speed
            door.transform.position = Vector3.MoveTowards(door.transform.position, endPosition, raiseSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame before continuing the loop
        }
    }
    
    private void CheckForObject()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit; 
        // Perform raycast to detect objects
        if (Physics.Raycast(ray, out hit, objectRange))
        {
            // Check if the object has the "pickUp" tag
            if (hit.collider.CompareTag("pickUp"))
            { 
                // Display the pick-up text
                pickUpText.gameObject.SetActive(true);
                pickUpText.text = hit.collider.gameObject.name;
            }
            // Check if the object has the "Info" tag
            else if (hit.collider.CompareTag("Info"))
            { 
                // Display the info text
                objectInfoText.gameObject.SetActive(true);
                //objectInfoText.text = hit.collider.gameObject.name;
            }
            // Check if the object has the "Door" or "Drawer" tag
            else if (hit.collider.CompareTag("Door") || hit.collider.CompareTag("Drawer"))
            {
                Door DoorOpen = hit.collider.GetComponent<Door>();
                /*if (DoorOpen.isOpen == false)
                {
                    doorOpenText.gameObject.SetActive(true);
                    doorCloseText.gameObject.SetActive(false);
                }
                else if (DoorOpen.isOpen == true)
                {
                    doorCloseText.gameObject.SetActive(true);
                    doorOpenText.gameObject.SetActive(false);
                }*/
                if (DoorOpen != null)
                {
                    if (DoorOpen.isOpen == false)
                    {
                        doorOpenText.gameObject.SetActive(true);
                        doorCloseText.gameObject.SetActive(false);
                    }
                    else if (DoorOpen.isOpen == true)
                    {
                        doorCloseText.gameObject.SetActive(true);
                        doorOpenText.gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogError("The object tagged as 'Drawer' or 'Door' is missing the 'Door' component.");
                }
            }
            else if (hit.collider.CompareTag("Thought"))
            {
                thoughtText.gameObject.SetActive(true);
            }
            else if (hit.collider.CompareTag("Key"))
                keyText.gameObject.SetActive(true);
            else
            { 
                // Hide the pick-up text if not looking at an object with info
                pickUpText.gameObject.SetActive(false);
                doorOpenText.gameObject.SetActive(false);
                doorCloseText.gameObject.SetActive(false);
                objectInfoText.gameObject.SetActive(false);
                thoughtText.gameObject.SetActive(false);
                keyText.gameObject.SetActive(false);
            }
        }
        else
        { 
            // Hide the text if not looking at any object
            pickUpText.gameObject.SetActive(false);
            doorOpenText.gameObject.SetActive(false);
            doorCloseText.gameObject.SetActive(false);
            objectInfoText.gameObject.SetActive(false);
        }
    }

    private void ChangeView()
    {
        if (isNormalView)
        {
            //NormalViewCanvas.SetActive(false);
            //SecondViewCanvas.SetActive(true);
            isNormalView = false;
            NormalViewVolume.SetActive(false);
            SecondViewVolume.SetActive(true);
            secondView.StartView();
        }
        else
        {
            //SecondViewCanvas.SetActive(false);
            //NormalViewCanvas.SetActive(true);
            isNormalView = true;
            SecondViewVolume.SetActive(false);
            NormalViewVolume.SetActive(true);
            secondView.StopView();
        }
    }

}
