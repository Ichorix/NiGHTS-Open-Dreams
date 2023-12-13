using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class TutorialLevelScript : MonoBehaviour
{
    public float speed;
    public PathCreator currentPath;
    public float distanceTravelled;
    public EndOfPathInstruction endOfPathInstruction;
    private Vector3 pathPosition;
    private float yPosition;
    public Vector3 pathRotation;

    public Vector3 playerRotation;
    public Quaternion setRotation;

    
                        
    void Start()    ///Copied from the example project, probably useful to have
    {
        yPosition = transform.position.y;
        if (currentPath != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            currentPath.pathUpdated += OnPathChanged;
        }
    }
    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged() {
        distanceTravelled = currentPath.path.GetClosestDistanceAlongPath(transform.position);
    }

    void Update()
    {
        //Position
        float playerVerticalInput = Input.GetAxis("Vertical");
        float playerHorizontalInput = Input.GetAxis("Horizontal");

        distanceTravelled += playerHorizontalInput * speed * Time.deltaTime;
        pathPosition = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);

        yPosition += playerVerticalInput * speed * Time.deltaTime;
        transform.position = new Vector3(pathPosition.x, yPosition, pathPosition.z);

        //Rotation
        pathRotation = currentPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction).eulerAngles;
        
        playerRotation.x = transform.rotation.x; 
            playerRotation.y = pathRotation.y;          // Gets the Left/Right rotation of the track
            playerRotation.z = transform.rotation.z;

            setRotation.eulerAngles = playerRotation;   // Puts the Vector3 playerRotation into the Quaternion setRotation
            transform.rotation = setRotation;           // So that the transform.rotation can then be set to the Quaternion
            
            if(playerHorizontalInput < 0) // Checks if  you are moving backwards
            {
                //transform.rotation = new Quaternion(transform.rotation.x, -transform.rotation.y, transform.rotation.z, transform.rotation.w);
                Vector3 flippedRotation = new Vector3(transform.rotation.x, pathRotation.y + 180, transform.rotation.z);
                transform.rotation = Quaternion.Euler(flippedRotation); 
            }

                                                // I dont know how this line below works
            Vector3 verticalInput = new Vector3(transform.rotation.x, playerVerticalInput * 2, transform.rotation.z);
            transform.forward += verticalInput; // I have no idea why this line is needed
                                                // But it all works so its all good
        

    }

}
