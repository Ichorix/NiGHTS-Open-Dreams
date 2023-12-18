using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointNChipCircles : MonoBehaviour
{
    private float t;
    public float speed;
    private Vector3 center = Vector3.zero;
    public Vector3 Destination = new Vector3(-1200, 500, 0);

    private float xPos;
    private float yPos;

    public AnimationCurve middleCurve;
    public AnimationCurve posVariation; //y pos x neg
    public AnimationCurve negVariation; //y neg x pos
    private float variationMult; // input as t

    [Header("Point item instance Information")]
    [SerializeField] private float pulseSpeed;
    public AnimationCurve pointColorBellCurve;
    public Material matInstance;
    public Image image;

    void Start()
    {
        variationMult = Random.Range(-10, 10) * 0.1f;
    }
    void Update()
    {
        t += Time.deltaTime * speed;
        if (t > 1)
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
        xPos = Mathf.Lerp(center.x, Destination.x, middleCurve.Evaluate(t));
        yPos = Mathf.Lerp(center.y, Destination.y, middleCurve.Evaluate(t));

        if(variationMult > 0)
        {
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
        matInstance.SetFloat("_TimeOffset", pointColorBellCurve.Evaluate(t * pulseSpeed));
        image.material = matInstance;
    }
}
