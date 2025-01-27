using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate512cubes : MonoBehaviour
{

    // Prefab cube that gets spawned in.
    public GameObject cubePrefab;

    // Array that holds the 512 cubes we're spawning in.
    GameObject[] cubes = new GameObject[512];

    // Scales the height of each cube by this much.
    public float scale = 10000;

    public Color[] colorsArray = { Color.red, Color.yellow, Color.green, Color.blue, Color.magenta };
    float scaledTime;

    public float duration = 3.0f;
    private int index = 0;
    private float timer = 0.0f;
    private Color currentColor = Color.red;
    private Color startColor = Color.red;

    /* Spawns 512 instances of cubePrefab in a circle of radius 100
     * around the object this script is attached to. Each cube is
     * rotated to face towards/away the center, and each cube is a
     * child of the object this script is attached to.
     */
    void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            // Spawns a copy of cubePrefab.
            GameObject cube = Instantiate(cubePrefab);

            // Assigns this copy to it's proper position in cubes.
            cubes[i] = cube;

            // Names it properly.
            cube.name = "Cube" + i;

            // Set its parent to this object.
            cube.transform.parent = this.transform;

            /* Rotate and position the cube properly. Some attributes that
             * might come in handy include Transform.eulerAngles, Transform.forward,
             * and the Transform class in general. Make sure you're using floats
             * if you plan on doing any division.
             */

            // YOUR CODE HERE
            cube.transform.parent.eulerAngles = new Vector3(0, (360f / 512f) * i, 0);
            cube.transform.position += new Vector3(100f, 0, 0);
        }
        transform.eulerAngles = new Vector3(-90, 0, 0);
    }

    /* Every frame, we'll take the data collected in SpectrumAnalyzer
     * and use it to set the heights of our cubes. Each of the 512 data
     * points our sample array corresponds to one of our cubes. Two caveats:
     *     1. FFT values are very small, so you'll want to scale each one up
     *        (use the scale variable).
     *     2. If a FFT value is 0, you don't want the cube to disappear, so
     *        add a small base height to every cube.
     * Hint: You can access public static variables using Class.variable.
     */
    void Update()
    {
        for (int i = 0; i < 512; i++)
        {
            if (cubes != null)
            {
                GameObject cube = cubes[i];

                // YOUR CODE HERE
                if (SpectrumAnalyzer.samples[i] == 0)
                    cube.transform.localScale = new Vector3(1, 1, 1);
                else
                {
                    float yScale = SpectrumAnalyzer.samples[i] * scale;
                    if (yScale > 150.0f)
                        yScale = 150.0f;
                    cube.transform.localScale = new Vector3(1, yScale, 1);
                }

                currentColor = Color.Lerp(startColor, colorsArray[index], timer);
				//if (cube.transform.childCount > 0)
                	//cube.transform.GetChild(0).GetComponent<Renderer>().material.color = currentColor;
					cube.transform.GetComponent<Renderer>().material.color = currentColor;
            }
        }

        timer += Time.deltaTime / duration;
        if (timer >= 1.0f)
        {
            timer -= 1.0f;
            index++;
            if (index >= colorsArray.Length)
                index = 0;
            startColor = currentColor;
        }
    }
}