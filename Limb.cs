using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

// SpriteShape limb that follows a rigidbody
public class Limb : MonoBehaviour
{
    private SpriteShapeController ssController;
    public Rigidbody2D rb; // Drag in Rigidbody you want limb to follow

    private void Awake()
    {
        ssController = GetComponent<SpriteShapeController>();
    }

    void Start()
    {
        AddPt1st();
        StartCoroutine(AddPtRepeating());
    }

    private void AddPt1st()
    {
        // Add vertice at rb's position
        ssController.spline.InsertPointAt(0, rb.position);

        // Set tanget of previous vertice like this to make limb curvy
        ssController.spline.SetTangentMode(1, ShapeTangentMode.Continuous);
        ssController.spline.SetLeftTangent(1, new Vector2(0, 0.1f));
        ssController.spline.SetRightTangent(1, new Vector2(0, -0.1f));
    }

    IEnumerator AddPtRepeating()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f); // Add vertice every 0.05 seconds (*)

            // Add vertice at rb's position (compensate for rb's velocity)
            ssController.spline.InsertPointAt(0, rb.position + rb.velocity * 0.025f /* (*)0.025 is half of 0.5, seems to work*/);

            // Setting the tangent of the previous vertice like this makes it curvy
            ssController.spline.SetTangentMode(1, ShapeTangentMode.Continuous);

            // NOTE: rb needs to move quickly enough. Trying to add vertices too closely throws error
            // If rb isn't moving enough you'll need to check for this and not add a Pt
        }
    }
}
