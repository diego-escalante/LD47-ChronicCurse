using UnityEngine;

[RequireComponent (typeof (CollisionController))]
public class PlayerMovement : MonoBehaviour {

    [Header("Basic Jumping")]
    // Jump Height and timeToJumpApex define gravity and jump strength.
    [SerializeField]
    float jumpHeight = 3.25f;
    [SerializeField]
    float timeToJumpApex = 0.33f;
    float gravity, maxJumpVelocity;

    [Header("Basic Running")]
    // MoveSpeed and timeToTopSpeed.
    [SerializeField]
    float runSpeed = 10f;
    [SerializeField]
    float timeToTopSpeed = 0.15f;
    float acceleration;

    [Header("Terminal Velocity")]
    // terminalVelocityFactor defines terminalVelocity as a multiplier of jumpVelocity.
    [SerializeField]
    float terminalVelocityFactor = 3f;
    float terminalVelocity;

    [Header("Multijumping")]
    // Jumps describes the number of jumps the player can perform before having to be grounded again.
    // The first "main" jump is can only be done when grounded (or within coyote time.)
    // MultiJumpFactor describes how strong subsequent jumps are relative to the first jump.
    [SerializeField]
    int jumps = 1;
    [SerializeField]
    float multijumpHeight = 2f;
    float maxMultiJumpVelocity;
    int jumpsLeft;

    [Header("Variable Jumping")]
    // Ability to let the player modify jump height. minJumpHeight defines minJumpVelocity.
    [SerializeField]
    bool VariableJumpEnabled = true;
    [SerializeField]
    float minJumpHeight = 1.25f;
    [SerializeField]
    float minMultiJumpHeight = 1f;
    float minJumpVelocity;
    float minMultiJumpVelocity;

    [Header("Coyote Time")]
    // Coyote Time describes the amount of time AFTER not being grounded that the player can still jump.
    [SerializeField]
    bool coyoteTimeEnabled = true;
    [SerializeField]
    float coyoteTime = 0.1f;
    float coyoteTimeLeft;

    [Header("Jump Buffering")]
    // The Jump Buffer adds a small time buffer for the jump input, so if you hit it a little too early before
    // touching the ground, the character will still automatically jump on the frame it is grounded.
    [SerializeField]
    float jumpBufferTime = 0.09f;
    float jumpBufferTimeLeft;

    [Header("Walking")]
    // Walking changes the default moving speed to a slower pace, and makes it so that holding
    // the shift key causes the player to run at top speed.
    [SerializeField]
    bool walkingEnabled = false;
    [SerializeField]
    float walkSpeed = 5f;
    bool isRunning;

    [Header("Wall Sliding")]
    [SerializeField]
    bool wallSlideEnabled = true;
    [SerializeField]
    float wallSlideSpeed = 1f;

    [Header("Wall Jumping")]
    [SerializeField]
    bool wallJumpingEnabled = true;
    [SerializeField]
    float wallStickTime = 0.33f;
    WallStickingInfo wallStickingInfo = new WallStickingInfo();
    [SerializeField]
    float wallJumpLaunchAngle = 30f;
    [SerializeField]
    float wallJumpLaunchSpeed = 30f;
    [SerializeField]
    float wallJumpHopAngle = 40f;
    [SerializeField]
    float wallJumpHopSpeed = 20f;
    [SerializeField]
    float wallJumpClimbAngle = 65f;
    [SerializeField]
    float wallJumpClimbSpeed = 20f;
    [SerializeField]
    float toLaunchGracePeriod = 0.1f;
    float toLaunchGracePeriodLeft;
    float wallDirection;

    // The internal velocity of the player.
    Vector2 velocity = Vector2.zero;

    // Reference to CollisionController and its CollisionInfo struct for collision checking.
    CollisionController collisionController;
    CollisionController.CollisionInfo collisionInfo;

    public void OnValidate() {
        jumpHeight = Mathf.Max(0, jumpHeight);
        minJumpHeight = Mathf.Clamp(minJumpHeight, 0, jumpHeight);
        timeToJumpApex = Mathf.Max(0.01f, timeToJumpApex);
        terminalVelocityFactor = Mathf.Max(1, terminalVelocityFactor);
        runSpeed = Mathf.Max(0, runSpeed);
        walkSpeed = Mathf.Clamp(walkSpeed, 0, runSpeed);
        timeToTopSpeed = Mathf.Max(0.01f, timeToTopSpeed);
        coyoteTime = Mathf.Max(0, coyoteTime);
        jumps = Mathf.Max(0, jumps);
        multijumpHeight = Mathf.Max(0, multijumpHeight);
        minMultiJumpHeight = Mathf.Clamp(minMultiJumpHeight, 0, multijumpHeight);
        jumpBufferTime = Mathf.Max(0, jumpBufferTime);
        wallSlideSpeed = Mathf.Max(0, wallSlideSpeed);
        wallStickTime = Mathf.Max(0, wallStickTime);
        wallJumpLaunchAngle = Mathf.Clamp(wallJumpLaunchAngle, 0, 90);
        wallJumpHopAngle = Mathf.Clamp(wallJumpHopAngle, 0, 90);
        wallJumpClimbAngle = Mathf.Clamp(wallJumpClimbAngle, 0, 90);
        wallJumpLaunchSpeed = Mathf.Max(0, wallJumpLaunchSpeed);
        wallJumpHopSpeed = Mathf.Max(0, wallJumpHopSpeed);
        wallJumpClimbSpeed = Mathf.Max(0, wallJumpClimbSpeed);
        toLaunchGracePeriod = Mathf.Max(0, toLaunchGracePeriod);
        UpdateKinematics();
    }

	public void Start() {
        collisionController = GetComponent<CollisionController>();
        UpdateKinematics();
	}
	
	public void Update() {
        if (coyoteTimeEnabled) {
            coyoteTimeLeft -= Time.deltaTime;
            // Get rid of primary jump if no coyote time is left.
            if (jumpsLeft == jumps && coyoteTimeLeft <= 0) {
                jumpsLeft--;
            }
        } else {
            // Get rid of primary jump if not on the ground.
            if (jumpsLeft == jumps && !collisionInfo.collisionBelow) {
                jumpsLeft--;
            }
        }

        // Register jump input ahead of time for buffer and use it if applicable, keep track of time since jump input.
        if (jumpBufferTimeLeft >= 0) {
            jumpBufferTimeLeft -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            jumpBufferTimeLeft = jumpBufferTime;
        }

        // Calculate horizontal movement.
        float hInput = Input.GetAxisRaw("Horizontal");

        // If we are sticking to wall, there's no horizontal movement to calculate.
        if (!(wallJumpingEnabled && wallStickingInfo.IsSticking())) {
            isRunning = true;
            if (walkingEnabled) {
                isRunning = Input.GetKey(KeyCode.LeftShift);
            }
            float targetSpeed = hInput * (isRunning ? runSpeed : walkSpeed);
            velocity.x = (velocity.x > targetSpeed) ? Mathf.Max(targetSpeed, velocity.x - (acceleration * Time.deltaTime)) 
                                                    : Mathf.Min(velocity.x + (acceleration * Time.deltaTime), targetSpeed);    
        }

        // Special case: Wall jumping requires precise tricky timing of inputs.
        // Give the player a little bit of time to correct their mistake.
        if (toLaunchGracePeriodLeft > 0 && hInput == -wallDirection) {
            velocity.x = -wallDirection * Mathf.Cos(wallJumpLaunchAngle * Mathf.Deg2Rad);
            velocity.y = Mathf.Sin(wallJumpLaunchAngle * Mathf.Deg2Rad);
            velocity *= wallJumpLaunchSpeed;
            toLaunchGracePeriodLeft = -1;
        }
        
        // Calculate vertical movement. (By gravity or by jumping.)
        if (wallJumpingEnabled && toLaunchGracePeriodLeft > 0) {
            toLaunchGracePeriodLeft -= Time.deltaTime;
        }
        velocity.y = Mathf.Clamp(velocity.y + gravity * Time.deltaTime, -terminalVelocity, terminalVelocity);
        if (jumpBufferTimeLeft >= 0) {
            if (wallJumpingEnabled && wallStickingInfo.IsSticking()) {
                // Wall jump
                wallDirection = wallStickingInfo.GetWallDirection();
                if (hInput == wallDirection) {
                    velocity.x = -wallDirection * Mathf.Cos(wallJumpClimbAngle * Mathf.Deg2Rad);
                    velocity.y = Mathf.Sin(wallJumpClimbAngle * Mathf.Deg2Rad);
                    velocity *= wallJumpClimbSpeed;
                    // Set launching grace period.
                    toLaunchGracePeriodLeft = toLaunchGracePeriod;
                } else if (hInput == 0) {
                    velocity.x = -wallDirection * Mathf.Cos(wallJumpHopAngle * Mathf.Deg2Rad);
                    velocity.y =  Mathf.Sin(wallJumpHopAngle * Mathf.Deg2Rad);
                    velocity *= wallJumpHopSpeed;
                    // Set launching grace period.
                    toLaunchGracePeriodLeft = toLaunchGracePeriod;
                } else {
                    velocity.x = -wallDirection * Mathf.Cos(wallJumpLaunchAngle * Mathf.Deg2Rad);
                    velocity.y = Mathf.Sin(wallJumpLaunchAngle * Mathf.Deg2Rad);
                    velocity *= wallJumpLaunchSpeed;
                }
                jumpBufferTimeLeft = -1;
                wallStickingInfo.Reset();
            } else if (jumpsLeft > 0) {
                // Regular jumps
                velocity.y = (jumps == jumpsLeft ? maxJumpVelocity : maxMultiJumpVelocity);
                jumpsLeft--;
                // Just set the jump timer to negative to "consume" input.
                jumpBufferTimeLeft = -1;
            }
        }
        
        // Shortening jumps by releasing space.
        if (VariableJumpEnabled && Input.GetKeyUp(KeyCode.Space)) {
            if (jumps == jumpsLeft+1) {
                if (velocity.y > minJumpVelocity) {
                    velocity.y = minJumpVelocity;
                }
            } else {
                if (velocity.y > minMultiJumpVelocity) {
                    velocity.y = minMultiJumpVelocity;
                }
            }
        }

        // Calculate change in position based on collisions.
        collisionInfo = collisionController.Check(velocity * Time.deltaTime);

        // React to vertical collisions.
        if (collisionInfo.colliderVertical != null) {
            velocity.y = 0;
            // If collision is below, reset jumping and coyote time (if enabled).
            if (collisionInfo.collisionBelow) {
                if (coyoteTimeEnabled) {
                    coyoteTimeLeft = coyoteTime;
                }
                if (wallJumpingEnabled) {
                    // Remove wall sticking if grounded.
                    wallStickingInfo.Reset();
                }
                jumpsLeft = jumps;
            }
        }

        // React to horizontal collisions. 
        if (collisionInfo.colliderHorizontal != null) {
            velocity.x = 0;
        }

        if (wallJumpingEnabled) {
            wallStickingInfo.Tick();
        }
        bool pushingAgainstWall = !collisionInfo.collisionBelow && ((collisionInfo.collisionRight && hInput == 1) || (collisionInfo.collisionLeft && hInput == -1));
        // If we are pushing against a wall without any ground, reset stick.
        if (wallJumpingEnabled && pushingAgainstWall) {
            wallStickingInfo.Reset(collisionInfo.collisionRight ? 1 : -1, wallStickTime);
        }
        // If we are pushing against a wall or stuck to it, slide.
        if ((wallJumpingEnabled && wallStickingInfo.IsSticking()) || pushingAgainstWall) {
            // Sliding
            if (wallSlideEnabled && velocity.y < wallSlideSpeed) {
                velocity.y = -wallSlideSpeed;
            }
        }

        // Special case: If we are sticking but we aren't against a wall, stop sticking. (Usually happens when sliding off a wall.)
        if (wallJumpingEnabled && wallStickingInfo.IsSticking()) {
            if (!collisionController.Check(Vector2.right * 0.01f).collisionRight && !collisionController.Check(Vector2.left * 0.01f).collisionLeft) {
                wallStickingInfo.Reset();
            }        
        }

        // Move the object.
        transform.Translate(collisionInfo.moveVector);
	}

    public bool IsWallSticking() {
        return wallJumpingEnabled && wallStickingInfo.IsSticking();
    }

    void UpdateKinematics(){
        acceleration = runSpeed / timeToTopSpeed;
        gravity = -(2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        maxMultiJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * multijumpHeight);
        minMultiJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minMultiJumpHeight);
        terminalVelocity = maxJumpVelocity * terminalVelocityFactor;
    }

    private struct WallStickingInfo {
        float timeLeft;
        int wallDirection;

        public void Reset() {
            Reset(0, 0);
        }

        public void Reset(int wallDirection, float timeLeft) {
            this.wallDirection = wallDirection;
            this.timeLeft = timeLeft;
        }

        public bool IsSticking() {
            return timeLeft > 0;
        }

        public void Tick() {
            timeLeft -= Time.deltaTime;
        }

        public int GetWallDirection() {
            return wallDirection;
        }
    }
}
