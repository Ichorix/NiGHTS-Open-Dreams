using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointNChipCircles : MonoBehaviour
{
    [SerializeField] private bool externalDeath = false;
    [SerializeField] private float speed;
    private float t;
    private Vector3 center = Vector3.zero;
    [SerializeField] private Vector3 Destination = new Vector3(-1200, 500, 0); // Says where the item needs to go to. -1200, 500 is top left for the chip item. 

    private float xPos;
    private float yPos;

    // See the animation curves in the inspector
    [SerializeField] private AnimationCurve posVariation;
    [SerializeField] private AnimationCurve negVariation;
    private float variationMult;

    [Header("Point item instance Information")]
    [Tooltip("No idea the units these are in anymore. Actual speed is Time.time * t (controlled by speed) * pulseSpeed")]
    [SerializeField] private float pulseSpeed;
    public Material matInstance;
    public Sprite borderVariant;
    [SerializeField] Image borderReference;
    public Image image;

    void Start()
    {
        // Gets a random -1 to 1 value with one decimal to determine if it should me closer to the positive variation, negative variation, or right in the middle
        variationMult = Random.Range(-10, 10) * 0.1f;
        borderReference.sprite = borderVariant;
    }
    void Update()
    {
        t += Time.deltaTime * speed;
        if (t > 1 && !externalDeath)
        {
            Destroy(this.gameObject);
            return;
        }
        PositionCalculations();
        if(matInstance != null)
            ColorCalculations();
    }

    void PositionCalculations()
    {
        xPos = Mathf.Lerp(center.x, Destination.x, Mathf.Lerp(0, 1, t));
        yPos = Mathf.Lerp(center.y, Destination.y, Mathf.Lerp(0, 1, t));

        if(variationMult > 0)
        {
            // Var for variation rather than variable in this case
            float xVar = Mathf.Lerp(center.x, Destination.x, negVariation.Evaluate(t));
            float yVar = Mathf.Lerp(center.y, Destination.y, posVariation.Evaluate(t));

            xPos = Mathf.Lerp(xPos, xVar, variationMult);
            yPos = Mathf.Lerp(yPos, yVar, variationMult);
        }
        if(variationMult < 0)
        {
            float xVar = Mathf.Lerp(center.x, Destination.x, posVariation.Evaluate(t));
            float yVar = Mathf.Lerp(center.y, Destination.y, negVariation.Evaluate(t));

            xPos = Mathf.Lerp(xPos, xVar, Mathf.Abs(variationMult));
            yPos = Mathf.Lerp(yPos, yVar, Mathf.Abs(variationMult));
        }
        
        transform.localPosition = new Vector3(xPos, yPos, 0);
    }
    void ColorCalculations()
    {
        matInstance.SetFloat("_TimeOffset", Mathf.Abs(Mathf.Sin(Time.time * t * pulseSpeed)));
        image.material = matInstance;
    }
}
