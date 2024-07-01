using UnityEngine;

using FlyingSystem;

public class EagleController : MonoBehaviour
{
    private Transform characterTransform;
    public Transform meshRootTransform;

    public Transform springArmTransform;
    public Camera characterCamera;
    private Transform characterCameraTransform;

    public Animator animator;

    public TrailRenderer leftWingTrailRenderer, rightWingTrailRenderer;

    public Renderer speedLineParticleRenderer;

    private CreatureFlyingSystem creatureFlyingSystem;

    private AudioSource audioSource;

    private Airflow airflow;

    public bool activated = false;

    [Header("General Attributes")]
    public bool takeOff;
    public bool boosting;

    public float cameraSpeed = 300.0f;

    [Range(0.0f, 100.0f)]
    public float springArmSmoothingFactor = 0.25f;

    public float normalCameraY = 3.0f, normalCameraZ = -12.0f;
    public float divingZoomOutY = 3.0f, divingZoomOutZ = -15.0f;

    private bool hideWingTrails = false;

    private float targetSpringArmRotationX, targetSpringArmRotationY;

    public bool isGrabbing = false;
    private Transform targetGrabObjectTransform;
    private Rigidbody targetGrabObjectRigidbody;

    void Start()
    {
        characterTransform = this.transform;
        characterCameraTransform = characterCamera.transform;

        speedLineParticleRenderer.enabled = false;

        creatureFlyingSystem = this.GetComponent<CreatureFlyingSystem>();

        audioSource = this.GetComponent<AudioSource>();

        if (activated)
            Activate();
    }

    void Update()
    {
        if (activated)
        {
            PCInputControlLogic();
            CameraControlLogic();
        }
    }

    public void Activate()
    {
        activated = true;
        characterCamera.enabled = true;
        characterCamera.GetComponent<AudioListener>().enabled = true;
    }

    public void Deactivate()
    {
        activated = false;
        characterCamera.enabled = false;
        characterCamera.GetComponent<AudioListener>().enabled = false;
    }

    void PCInputControlLogic()
    {
        // Take off / grab
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (creatureFlyingSystem.inAir)
            {
                if (isGrabbing)
                    Drop();
            }
            else
                TakeOff();
        }

        // Fly forward / stop
        if (Input.GetKey(KeyCode.W))
            creatureFlyingSystem.FlyForward();
        else if (Input.GetKey(KeyCode.S))
            creatureFlyingSystem.SlowDown();
        else if (Input.GetKeyUp(KeyCode.S))
            creatureFlyingSystem.StopSlowingDown();

        // Turn left / right
        creatureFlyingSystem.AddYawInput(Input.GetAxis("Mouse X"));

        DivingLogic();

        // Boost on / off
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            Boost();
    }

    void DivingLogic()
    {
        if (creatureFlyingSystem.inAir && creatureFlyingSystem.diving)
        {
            // Camera zoom out
            characterCameraTransform.localPosition = Vector3.Lerp(characterCameraTransform.localPosition, new Vector3(0.0f, divingZoomOutY, divingZoomOutZ), 0.95f * Time.deltaTime);

            animator.SetBool("FlyToGlide", true);
            animator.SetBool("GlideToFly", false);

            // Enable trails from both wings
            if (!leftWingTrailRenderer.enabled)
            {
                hideWingTrails = false;

                leftWingTrailRenderer.enabled = true;
                rightWingTrailRenderer.enabled = true;
                
                speedLineParticleRenderer.enabled = true;
            }
        }
        else
        {
            // Reset all effects
            characterCameraTransform.localPosition = Vector3.Lerp(characterCameraTransform.localPosition, new Vector3(0.0f, normalCameraY, normalCameraZ), 0.5f * Time.deltaTime);

            animator.SetBool("GlideToFly", true);
            animator.SetBool("FlyToGlide", false);

            if (!hideWingTrails)
            {
                hideWingTrails = true;

                leftWingTrailRenderer.enabled = false;
                rightWingTrailRenderer.enabled = false;

                speedLineParticleRenderer.enabled = false;
            }
        }
    }

    void CameraControlLogic()
    {
        springArmTransform.position = Vector3.Lerp(characterTransform.position, springArmTransform.position, springArmSmoothingFactor * Time.deltaTime);
        springArmTransform.rotation = Quaternion.Euler(springArmTransform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime, springArmTransform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime, 0.0f);
    }

    public void TakeOff()
    {
        if (!creatureFlyingSystem.inAir)
        {
            creatureFlyingSystem.TakeOff();
            takeOff = creatureFlyingSystem.inAir;

            animator.SetBool("FlyToIdle", false);
            animator.SetBool("IdleToFly", true);

            animator.SetBool("GlideToIdle", false);

            audioSource.Play();
        }
    }

    public void Boost()
    {
        creatureFlyingSystem.boosting = !creatureFlyingSystem.boosting;
        boosting = creatureFlyingSystem.boosting;
    }

    public void Drop()
    {
        if (targetGrabObjectTransform != null)
        {
            isGrabbing = false;

            targetGrabObjectTransform.SetParent(null);

            targetGrabObjectRigidbody.useGravity = true;
            targetGrabObjectRigidbody.isKinematic = false;

            creatureFlyingSystem.currentCarryingWeight -= 3.0f;

            targetGrabObjectTransform = null;
        }
    }

    public float GetFlyingSpeed()
    {
        return creatureFlyingSystem.flyingSpeed;
    }

    public float GetStaminaPercentage()
    {
        return creatureFlyingSystem.staminaPercentage;
    }

    public float GetWeightPercentage()
    {
        return creatureFlyingSystem.weightPercentage;
    }

    void OnCollisionEnter(Collision collision)
    {
        // The target collision can be anything like ground, terrain, etc.
        if (collision.collider.name == "Road" || collision.gameObject.CompareTag("Ground"))
        {
            if (creatureFlyingSystem.inAir && !isGrabbing)
            {
                creatureFlyingSystem.Land();
                takeOff = creatureFlyingSystem.inAir;

                animator.SetBool("GlideToIdle", true);

                animator.SetBool("FlyToIdle", true);
                animator.SetBool("IdleToFly", false);

                animator.SetBool("FlyToGlide", false);

                leftWingTrailRenderer.enabled = false;
                rightWingTrailRenderer.enabled = false;

                speedLineParticleRenderer.enabled = false;
            }
        }
        else if (collision.collider.name == "Weight" && !isGrabbing)
        {
            // Grab
            isGrabbing = true;

            targetGrabObjectTransform = collision.transform;

            targetGrabObjectRigidbody = targetGrabObjectTransform.GetComponent<Rigidbody>();
            targetGrabObjectRigidbody.useGravity = false;
            targetGrabObjectRigidbody.isKinematic = true;

            targetGrabObjectTransform.SetParent(meshRootTransform);
            targetGrabObjectTransform.localPosition = new Vector3(0.0f, -2.25f, -2.172f);

            creatureFlyingSystem.currentCarryingWeight += 3.0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Entering the airflow can lift up the flyer
        if (other.name == "Airflow")
        {
            airflow = other.GetComponent<Airflow>();

            creatureFlyingSystem.AddAirflowForce(airflow.intensity, airflow.acceleration, airflow.fadeOutAcceleration);
            creatureFlyingSystem.stopFlying = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Airflow")
        {
            creatureFlyingSystem.EndAirflowForce();
            creatureFlyingSystem.stopFlying = false;
        }
    }
}