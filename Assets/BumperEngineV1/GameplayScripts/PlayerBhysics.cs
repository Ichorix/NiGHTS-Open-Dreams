﻿using UnityEngine;
using System.Collections;

public class PlayerBhysics : MonoBehaviour
{
    ActionManager Action;

    [Header("Movement Values")]

    public float MoveAccell = 0.5f;
    public AnimationCurve AccellOverSpeed;
    public float AccellShiftOverSpeed;
    public float MoveDecell = 1.3f;
    public float AirDecell = 1.05f;
    public float TangentialDrag;
    public float TangentialDragShiftSpeed;
    public float TurnSpeed = 16;
    public AnimationCurve TurnRateOverAngle;
    public AnimationCurve TangDragOverAngle;
    public AnimationCurve TangDragOverSpeed;
    public float TopSpeed = 15;
    public float MaxSpeed = 30;
    public float MaxFallingSpeed = 30;
    public float m_JumpPower = 2;
    public float GroundStickingDistance = 1;
    public float GroundStickingPower = -1;
    public float SlopeStandingLimit = 0.8f;
    public float SlopePower = 0.5f;
    public float SlopeRunningAngleLimit = 0.5f;
    public float SlopeSpeedLimit = 10;
    public float UphillMultiplier = 0.5f;
    public float DownhillMultiplier = 2;
    public float StartDownhillMultiplier = -7;
    public AnimationCurve SlopePowerOverSpeed;
    public float SlopePowerShiftSpeed;
    public float LandingConversionFactor = 2;

    [Header("AirMovementExtras")]
    public float AirControlAmmount = 2;
    public float AirSkiddingForce = 10;
    public bool StopAirMovementIfNoInput = false;


    public bool Grounded { get; set; }
    public Vector3 GroundNormal { get; set; }
    public Vector3 CollisionPointsNormal { get; set; }

    public Rigidbody p_rigidbody { get; set; }

    public Vector3 Gravity;
    public Vector3 MoveInput { get; set; }

    [Header("Other Values")]

    public float GroundOffset;
    RaycastHit hit;
    public Transform CollisionPoint;
    public Collider CollisionSphere;
    public Collider CollisionCapsule;
    public Transform MainCamera;
    public Transform Colliders;
    public SonicSoundsControl sounds;

    

    public DebugUI Debui;


    [Header("Rolling Values")]

    public float RollingLandingBoost;
    public float RollingDownhillBoost;
    public float RollingUphillBoost;
    public float RollingStartSpeed;
    public float RollingTurningDecreace;
    public float RollingFlatDecell;
    public float SlopeTakeoverAmount; // This is the normalized slope angle that the player has to be in order to register the land as "flat"
    public bool isRolling { get; set; }

    //Cache

    public float curvePosAcell { get; set; }
    public float curvePosTang { get; set; }
    public float curvePosSlope { get; set; }
    public float b_normalSpeed { get; set; }
    public Vector3 b_normalVelocity { get; set; }
    public Vector3 b_tangentVelocity { get; set; }

    //Etc
    [Header("Etc Values")]

    public bool UseSphereToGetNormal;

    Vector3 KeepNormal;
    float KeepNormalCounter;
    public bool WasOnAir { get; set; }
    public Vector3 PreviousInput { get; set; }
    public Vector3 RawInput { get; set; }
    public Vector3 PreviousRawInput { get; set; }
    public Vector3 PreviousRawInputForAnim { get; set; }
    public float SpeedMagnitude { get; set; }
    public float XZmag { get; set; }


    [Header("Greedy Stick Fix")]
    public bool EnableDebug;
    public float TimeOnGround { get; set; }
    RaycastHit hitSticking, hitRot;
    float RayToGroundDistancecor, RayToGroundRotDistancecor;

    [Tooltip("This is the values of the Lerps when the player encounters a slope , the first one is negative slopes (loops), and the second one is positive Slopes (imagine running on the outside of a loop),This values shouldnt be touched unless yuou want to go absurdly faster. Default values 0.885 and 1.5")]
    public Vector2 StickingLerps = new Vector2(0.885f, 1.5f);
    [Tooltip("This is the limit from 0 to 1 the degrees that the player should be sticking 0 is no angle , 1 is everything bellow 90°, and 0.5 is 45° angles, default 0.4")]
    public float StickingNormalLimit = 0.4f;
    [Tooltip("This is the cast ahead when the player hits a slope, this will be used to predict it's path if it is going on a high speed. too much of this value might send the player flying off before it hits the loop, too little might see micro stutters, default value 1.9")]
    public float StickCastAhead = 1.9f;
    [Tooltip("This is the position above the raycast hit point that the player will be placed if he is loosing grip on positive G turns, this value will snap the player back into the mesh, it shouldnt be moved unless you scale the collider, default value 0.6115")]
    public float negativeGHoverHeight = 0.6115f;
    public float RayToGroundDistance = 0.55f;
    public float RaytoGroundSpeedRatio = 0.01f;
    public float RaytoGroundSpeedMax = 2.4f;
    public float RayToGroundRotDistance = 1.1f;  
    public float RaytoGroundRotSpeedMax = 2.6f;
    public float RotationResetThreshold = -0.1f;

    public LayerMask Playermask;




    private void Start()
    {
        p_rigidbody = GetComponent<Rigidbody>();
        PreviousInput = transform.forward;
        Action = GetComponent<ActionManager>();
    }

    void FixedUpdate()
    {
        TimeOnGround += Time.deltaTime;
        if (!Grounded) TimeOnGround = 0;
        GeneralPhysics();

    }

    void Update()
    {
        SpeedMagnitude = p_rigidbody.velocity.magnitude;
        InputChecks();
    }

    void InputChecks()
    {
        //Rolling
        if (Input.GetKey(KeyCode.N) && p_rigidbody.velocity.sqrMagnitude > RollingStartSpeed)
        {
            isRolling = true;
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            isRolling = false;
        }
    }

    void GeneralPhysics()
    {
        //Set Previous input
        if (RawInput.sqrMagnitude >= 0.03f)
        {
            PreviousRawInputForAnim = RawInput * 90;
            PreviousRawInputForAnim = PreviousRawInputForAnim.normalized;
        }

        if (MoveInput.sqrMagnitude >= 0.9f)
        {
            PreviousInput = MoveInput;
        }
        if (RawInput.sqrMagnitude >= 0.9f)
        {
            PreviousRawInput = RawInput;
        }

        //Set Curve thingies
        curvePosAcell = Mathf.Lerp(curvePosAcell, AccellOverSpeed.Evaluate((p_rigidbody.velocity.sqrMagnitude / MaxSpeed) / MaxSpeed), Time.fixedDeltaTime * AccellShiftOverSpeed);
        curvePosTang = Mathf.Lerp(curvePosTang, TangDragOverSpeed.Evaluate((p_rigidbody.velocity.sqrMagnitude / MaxSpeed) / MaxSpeed), Time.fixedDeltaTime * TangentialDragShiftSpeed);
        curvePosSlope = Mathf.Lerp(curvePosSlope, SlopePowerOverSpeed.Evaluate((p_rigidbody.velocity.sqrMagnitude / MaxSpeed) / MaxSpeed), Time.fixedDeltaTime * SlopePowerShiftSpeed);

        // Apply Max Speed Limit
        XZmag = new Vector3(p_rigidbody.velocity.x, 0, p_rigidbody.velocity.z).magnitude;

        // Do it for X and Z
        if (XZmag > MaxSpeed)
        {
            Vector3 ReducedSpeed = p_rigidbody.velocity;
            float keepY = p_rigidbody.velocity.y;
            ReducedSpeed = Vector3.ClampMagnitude(ReducedSpeed, MaxSpeed);
            ReducedSpeed.y = keepY;
            p_rigidbody.velocity = ReducedSpeed;
        }

        //Do it for Y
        if (Mathf.Abs(p_rigidbody.velocity.y) > MaxFallingSpeed)
        {
            Vector3 ReducedSpeed = p_rigidbody.velocity;
            float keepX = p_rigidbody.velocity.x;
            float keepZ = p_rigidbody.velocity.z;
            ReducedSpeed = Vector3.ClampMagnitude(ReducedSpeed, MaxSpeed);
            ReducedSpeed.x = keepX;
            ReducedSpeed.z = keepZ;
            p_rigidbody.velocity = ReducedSpeed;
        }

        //Rotate Colliders     
        if (EnableDebug)
        {
            Debug.DrawRay(transform.position + (transform.up * 2) + transform.right, -transform.up * (2f + RayToGroundRotDistancecor), Color.red);
        }
        if ((Physics.Raycast(transform.position + (transform.up * 2), -transform.up, out hitRot, 2f + RayToGroundRotDistancecor, Playermask)))
        {
            //GroundNormal = hit.normal;
            GroundNormal = hitRot.normal;
            transform.rotation = Quaternion.FromToRotation(transform.up, GroundNormal) * transform.rotation;


            KeepNormal = GroundNormal;
            KeepNormalCounter = 0;
        }
        else
        {
            //Keep the rotation after exiting the ground for a while, to avoid collision issues.

            KeepNormalCounter += Time.deltaTime;
            if (KeepNormalCounter < 0.083f)
            {
                transform.rotation = Quaternion.FromToRotation(transform.up, KeepNormal) * transform.rotation;
            }
            else
            {
                if (transform.up.y < RotationResetThreshold)
                {
                    transform.rotation = Quaternion.identity;
                    if (EnableDebug)
                    {
                        Debug.Log("reset");
                    }
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                }
            }
        }
        CheckForGround();
    }

    void HandleGroundControl(float deltaTime, Vector3 input)
    {
        //By Damizean

        // We assume input is already in the Player's local frame...
        // Fetch velocity in the Player's local frame, decompose into lateral and vertical
        // components.

        Vector3 velocity = p_rigidbody.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        Vector3 lateralVelocity = new Vector3(localVelocity.x, 0.0f, localVelocity.z);
        Vector3 verticalVelocity = new Vector3(0.0f, localVelocity.y, 0.0f);

        // If there is some input...

        if (input.sqrMagnitude != 0.0f)
        {
            // Normalize to get input direction.

            Vector3 inputDirection = input.normalized;
            float inputMagnitude = input.magnitude;

            // Step 1) Determine angle and rotation between current lateral velocity and desired direction.
            //         Prevent invalid rotations if no lateral velocity component exists.

            float deviationFromInput = Vector3.Angle(lateralVelocity, inputDirection) / 180.0f;
            Quaternion lateralToInput = Mathf.Approximately(lateralVelocity.sqrMagnitude, 0.0f)
                                      ? Quaternion.identity
                                      : Quaternion.FromToRotation(lateralVelocity.normalized, inputDirection);

            // Step 2) Let the user retain some component of the velocity if it's trying to move in
            //         nearby directions from the current one. This should improve controlability.

            float turnRate = TurnRateOverAngle.Evaluate(deviationFromInput);
            lateralVelocity = Vector3.RotateTowards(lateralVelocity, lateralToInput * lateralVelocity,
                                                    Mathf.Deg2Rad * TurnSpeed * turnRate * Time.deltaTime, 0.0f);

            // Step 3) Further lateral velocity into normal (in the input direction) and tangential
            //         components. Note: normalSpeed is the magnitude of normalVelocity, with the added
            //         bonus that it's signed. If positive, the speed goes towards the same
            //         direction than the input :)

            var normalDot = Vector3.Dot(lateralVelocity.normalized, inputDirection.normalized);

            if (Mathf.Abs(normalDot) <= 0.6f && normalDot > -0.6f)
            {
                inputDirection = Vector3.Slerp(lateralVelocity.normalized, inputDirection, 0.075f);
            }

            float normalSpeed = Vector3.Dot(lateralVelocity, inputDirection);
            Vector3 normalVelocity = inputDirection * normalSpeed;
            Vector3 tangentVelocity = lateralVelocity - normalVelocity;
            float tangentSpeed = tangentVelocity.magnitude;

            // Step 4) Apply user control in this direction.

            if (normalSpeed < TopSpeed)
            {
                // Accelerate towards the input direction.
                normalSpeed += (isRolling ? 0 : MoveAccell) * deltaTime * inputMagnitude;

                normalSpeed = Mathf.Min(normalSpeed, TopSpeed);

                // Rebuild back the normal velocity with the correct modulus.

                normalVelocity = inputDirection * normalSpeed;
            }

            // Step 5) Dampen tangential components.

            float dragRate = TangDragOverAngle.Evaluate(deviationFromInput)
                           * TangDragOverSpeed.Evaluate((tangentSpeed * tangentSpeed) / (MaxSpeed * MaxSpeed));

            tangentVelocity = Vector3.MoveTowards(tangentVelocity, Vector3.zero,
                                                  TangentialDrag * dragRate * deltaTime);

            /*
            float tangentDrag = ;

            if (!isRolling)
            {
                tangentVelocity = Vector3.MoveTowards(tangentVelocity, Vector3.zero,
                    TangentialDrag * tangentDrag * deltaTime * inputMagnitude);
            }
            else
            {
                tangentVelocity = Vector3.MoveTowards(tangentVelocity, Vector3.zero,
                    TangentialDrag * RollingTurningDecreace * tangentDrag * deltaTime * inputMagnitude);

            }*/

            // Recompose lateral velocity from both components.

            lateralVelocity = normalVelocity + tangentVelocity;

            //Export nescessary variables

            b_normalSpeed = normalSpeed;
            b_normalVelocity = normalVelocity;
            b_tangentVelocity = tangentVelocity;

            //DEBUG VARIABLES
            /*
            Debui.inputDirection = inputDirection;
            Debui.inputMagnitude = inputMagnitude;
            Debui.velocity = rigidbody.velocity;
            Debui.localVelocity = localVelocity;
            Debui.normalSpeed = normalSpeed;
            Debui.normalVelocity = normalVelocity;
            Debui.tangentVelocity = tangentVelocity;
*/
        }

        // Otherwise, apply some damping as to decelerate Sonic.

        if (input.sqrMagnitude == 0 && !isRolling && Grounded)
        {
            lateralVelocity /= MoveDecell;
        }
        if (isRolling && GroundNormal.y > SlopeTakeoverAmount)
        {
            lateralVelocity /= RollingFlatDecell;
        }

        // Compose local velocity back and compute velocity back into the Global frame.

        localVelocity = lateralVelocity + verticalVelocity;

        //new line for the stick to ground from GREEDY


        velocity = transform.TransformDirection(localVelocity);

        velocity = NewStickToGround(velocity);
        p_rigidbody.velocity = velocity;
    }

    void GroundMovement()
    {
        //Stop Rolling
        if (p_rigidbody.velocity.sqrMagnitude < 20)
        {
            isRolling = false;
        }

        //Slope Physics
        SlopePlysics();

        // Call Ground Control
        HandleGroundControl(1, MoveInput * curvePosAcell);

        
    }

    void SlopePlysics()
    {
        //ApplyLandingSpeed
        if (WasOnAir && Grounded)
        {
            Vector3 Addsped;

            if (!isRolling)
            {
                Addsped = GroundNormal * LandingConversionFactor;
                //StickToGround(GroundStickingPower);
            }
            else
            {
                Addsped = (GroundNormal * LandingConversionFactor) * RollingLandingBoost;
                //StickToGround(GroundStickingPower * RollingLandingBoost);
                sounds.SpinningSound();
            }

            Addsped.y = 0;
            AddVelocity(Addsped);
            WasOnAir = false;
        }

        //Get out of slope if speed it too low
        if (p_rigidbody.velocity.sqrMagnitude < SlopeSpeedLimit && SlopeRunningAngleLimit > GroundNormal.y)
        {
            transform.rotation = Quaternion.identity;
            AddVelocity(GroundNormal * 3);
        }
        else
        {
            //Sticking to ground power
            //StickToGround(GroundStickingPower);
        }

        //Apply slope power
        if (Grounded && GroundNormal.y < SlopeStandingLimit)
        {
            if (p_rigidbody.velocity.y > StartDownhillMultiplier)
            {
                if (!isRolling)
                {
                    Vector3 force = new Vector3(0, (SlopePower * curvePosSlope) * UphillMultiplier, 0);
                    AddVelocity(force);
                }
                else
                {
                    Vector3 force = new Vector3(0, (SlopePower * curvePosSlope) * UphillMultiplier, 0) * RollingUphillBoost;
                    AddVelocity(force);
                }
            }
            else
            {
                if (MoveInput != Vector3.zero && b_normalSpeed > 0)
                {
                    if (!isRolling)
                    {
                        Vector3 force = new Vector3(0, (SlopePower * curvePosSlope) * DownhillMultiplier, 0);
                        AddVelocity(force);
                    }
                    else
                    {
                        Vector3 force = new Vector3(0, (SlopePower * curvePosSlope) * DownhillMultiplier, 0) * RollingDownhillBoost;
                        AddVelocity(force);
                    }

                }
                else
                {
                    Vector3 force = new Vector3(0, SlopePower * curvePosSlope, 0);
                    AddVelocity(force);
                }
            }
        }

    }

    public void StickToGround(float StickingPower)
    {
        /* NULLIFIED BY NEW Sticking;
		float StickingSpeedFactor = (p_rigidbody.velocity.magnitude * 0.075f);

        CollisionPoint.LookAt(transform.position);
		if (Physics.Raycast(CollisionPoint.position, -Colliders.up, out hit, GroundStickingDistance*StickingSpeedFactor) && !Input.GetButton("A"))
        {
			Vector3 force = hit.normal * StickingPower * StickingSpeedFactor;
            AddVelocity(force);
        }
        */
    }

    public Vector3 NewStickToGround(Vector3 Velocity)
    {
        Vector3 result = Velocity;

        if (Grounded && TimeOnGround > 0.1f && SpeedMagnitude > 1)
        {
            if (EnableDebug)
            {
                Debug.Log("Before: " + result + "speed " + result.magnitude);
            }
            float DirectionDot = Vector3.Dot(p_rigidbody.velocity.normalized, hit.normal);
            Vector3 normal = hit.normal;
            Vector3 Raycasterpos = p_rigidbody.position + (hit.normal * -0.12f);

            if (EnableDebug)
            {
                Debug.Log("Speed: " + SpeedMagnitude + "\n Direction DOT: " + DirectionDot + " \n Velocity Normal:" + p_rigidbody.velocity.normalized + " \n  Ground normal : " + hit.normal);
                Debug.DrawRay(hit.point + (transform.right * 0.2F), hit.normal * 3, Color.yellow, 1);
            }

            //If the Raycast Hits something, it adds it's normal to the ground normal making an inbetween value the interpolates the direction;
            Debug.DrawRay(Raycasterpos, p_rigidbody.velocity * StickCastAhead * Time.deltaTime, Color.black, 1);
            if (Physics.Raycast(Raycasterpos, p_rigidbody.velocity.normalized, out hitSticking, SpeedMagnitude * StickCastAhead * Time.deltaTime, Playermask))
            {
                if (EnableDebug) Debug.Log("AvoidingGroundCollision");

                if (Vector3.Dot(normal, hitSticking.normal) > 0.15f) //avoid flying off Walls
                {
                    normal = hitSticking.normal.normalized;
                    Vector3 Dir = Align(Velocity, normal.normalized);
                    result = Vector3.Lerp(Velocity, Dir, StickingLerps.x);
                    transform.position = hit.point + normal * negativeGHoverHeight;
                    if (EnableDebug)
                    {
                        Debug.DrawRay(hit.point, normal * 3, Color.red, 1);
                        Debug.DrawRay(transform.position, Dir.normalized * 3, Color.yellow, 1);
                        Debug.DrawRay(transform.position + transform.right, Dir.normalized * 3, Color.cyan + Color.black, 1);
                    }
                }
            }
            else
            {
                if (Mathf.Abs(DirectionDot) < StickingNormalLimit) //avoid SuperSticking
                {
                    Vector3 Dir = Align(Velocity, normal.normalized);
                    float lerp = StickingLerps.y;
                    if (Physics.Raycast(Raycasterpos + (p_rigidbody.velocity * StickCastAhead * Time.deltaTime), -hit.normal, out hitSticking, 2.5f, Playermask))
                    {
                        float dist = hitSticking.distance;
                        if (EnableDebug)
                        {
                            Debug.Log("PlacedDown" + dist);
                            Debug.DrawRay(Raycasterpos + (p_rigidbody.velocity * StickCastAhead * Time.deltaTime), -hit.normal * 3, Color.cyan, 2);
                        }
                        if (dist > 1.5f)
                        {
                            if (EnableDebug) Debug.Log("ForceDown");
                            lerp = 5;
                            result += (-hit.normal * 10);
                            transform.position = hit.point + normal * negativeGHoverHeight;
                        }
                    }

                    result = Vector3.LerpUnclamped(Velocity, Dir, lerp);

                    if (EnableDebug)
                    {
                        Debug.Log("Lerp " + lerp + " Result " + result);
                        Debug.DrawRay(hit.point, normal * 3, Color.green, 0.6f);
                        Debug.DrawRay(transform.position, result.normalized * 3, Color.grey, 0.6f);
                        Debug.DrawRay(transform.position + transform.right, result.normalized * 3, Color.cyan + Color.black, 0.6f);
                    }
                }

            }

            result += (-hit.normal * 2); // traction addition
            if (EnableDebug)
            {
                Debug.Log("After: " + result + "speed " + result.magnitude);
            }
        }

        return result;
    }

    Vector3 Align(Vector3 vector, Vector3 normal)
    {
        //typically used to rotate a movement vector by a surface normal
        Vector3 tangent = Vector3.Cross(normal, vector);
        Vector3 newVector = -Vector3.Cross(normal, tangent);
        vector = newVector.normalized * vector.magnitude;
        return vector;
    }

    public void AddVelocity(Vector3 force)
    {
        p_rigidbody.velocity += force;
    }

    void AirMovement()
    {
        //AddSpeed
        HandleGroundControl(AirControlAmmount, MoveInput);

        //Get out of roll
        isRolling = false;

        //Apply Gravity
        p_rigidbody.velocity += Gravity;

        //Reduce speed
        if (MoveInput == Vector3.zero && StopAirMovementIfNoInput && p_rigidbody.velocity.sqrMagnitude < 100)
        {
            Vector3 ReducedSpeed = p_rigidbody.velocity;
            ReducedSpeed.x = ReducedSpeed.x / AirDecell;
            ReducedSpeed.z = ReducedSpeed.z / AirDecell;
            p_rigidbody.velocity = ReducedSpeed;
        }

        //Get set for landing
        WasOnAir = true;

        //Air Skidding  
        if (b_normalSpeed < 0 && !Grounded)
        {
            HandleGroundControl(1, (MoveInput * AirSkiddingForce) * MoveAccell);
        }

        //Max Falling Speed
        if (p_rigidbody.velocity.y < MaxFallingSpeed)
        {
            p_rigidbody.velocity = new Vector3(p_rigidbody.velocity.x, MaxFallingSpeed, p_rigidbody.velocity.z);
        }

    }

    void CheckForGround()
    {
        RayToGroundDistancecor = RayToGroundDistance;
        RayToGroundRotDistancecor = RayToGroundRotDistance;
        if (Action.Action == 0 && Grounded)
        {
            //grounder line
            RayToGroundDistancecor = Mathf.Max(RayToGroundDistance + (SpeedMagnitude * RaytoGroundSpeedRatio), RayToGroundDistance);
            RayToGroundDistancecor = Mathf.Min(RayToGroundDistancecor, RaytoGroundSpeedMax);

            //rotorline
            RayToGroundRotDistancecor = Mathf.Max(RayToGroundRotDistance + (SpeedMagnitude * RaytoGroundSpeedRatio), RayToGroundRotDistance);
            RayToGroundRotDistancecor = Mathf.Min(RayToGroundRotDistancecor, RaytoGroundRotSpeedMax);

        }
        if (EnableDebug)
        {
            Debug.DrawRay(transform.position + (transform.up * 2) + -transform.right, -transform.up * (2f + RayToGroundDistancecor), Color.yellow);
        }
        if ((Physics.Raycast(transform.position + (transform.up * 2), -transform.up, out hit, 2f + RayToGroundDistancecor, Playermask)))
        {
            GroundNormal = hit.normal;
            Grounded = true;
            GroundMovement();
        }
        else
        {
            Grounded = false;
            GroundNormal = Vector3.zero;
            AirMovement();
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(DownRay);
    }

    public void OnCollisionStay(Collision col)
    {
        Vector3 Prevnormal = GroundNormal;
        foreach (ContactPoint contact in col.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);

            //Set Middle Point
            Vector3 pointSum = Vector3.zero;
            Vector3 normalSum = Vector3.zero;
            for (int i = 0; i < col.contacts.Length; i++)
            {
                pointSum = pointSum + col.contacts[i].point;
                normalSum = normalSum + col.contacts[i].normal;
            }

            pointSum = pointSum / col.contacts.Length;
            CollisionPointsNormal = normalSum / col.contacts.Length;

            if (p_rigidbody.velocity.normalized != Vector3.zero)
            {
                CollisionPoint.position = pointSum;
            }

        }
    }

}
