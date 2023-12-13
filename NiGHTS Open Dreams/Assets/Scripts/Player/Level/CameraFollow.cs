using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class CameraFollow : MonoBehaviour
    {
        public LevelFollow levelFollow;
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        public float distanceTravelled, maxDistance;
        public bool isClosedPath;
        public Vector3 pathPosition, playerPosition;
        public Quaternion pathRotation, playerRotation;
        public float playerYvalue, levelYvalue, yPercent, positionBetweenYs;

        void Start()
        {  
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
            
        }
        void Update()
        {
            //If using LevelFollow
            distanceTravelled = levelFollow.distanceTravelled;
            pathCreator = levelFollow.currentPath;

            playerYvalue = levelFollow.transform.position.y;
            levelYvalue = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction).y;
            positionBetweenYs = Mathf.Lerp(levelYvalue, playerYvalue, yPercent);
            

            pathPosition = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.position = new Vector3(pathPosition.x, positionBetweenYs, pathPosition.z);

            
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            

            if(isClosedPath && distanceTravelled >= maxDistance)
            {
                distanceTravelled -= maxDistance;
            }
        }
        void FixedUpdate()
        {
            //If using NewLevelFollow
            //float movingHorizontal = Input.GetAxis("Horizontal");
            //distanceTravelled += movingHorizontal * speed * Time.deltaTime

            //transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }
        
        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() 
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
            Debug.Log("PathChanged");
        }

        /*void SetPath11()
        {
            Debug.Log("Path1");
            maxDistance = 28.85f;
            pathCreator = GameObject.Find("1-1/Path (28.85)").GetComponent<PathCreator>();
            endOfPathInstruction = (EndOfPathInstruction)0; //Loop
            isClosedPath = true;
        }
        void SetPath12()
        {
            maxDistance = 144.66f;
            pathCreator = GameObject.Find("1-2/Path (144.66)").GetComponent<PathCreator>();
            endOfPathInstruction = (EndOfPathInstruction)2; //Stop
            isClosedPath = false;
        }*/
    }
}