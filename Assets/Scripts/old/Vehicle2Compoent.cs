using UnityEngine;

public class Vehicle2Component : MonoBehaviour
{
    public float moveSpeed = 0.01f;
    public float rotSpeed = 2.0f;
    public float probRedType = 0.5f;

    public bool enableTranslation = true;
    public bool enableRotation = true;
    public bool enableColorChange = true;
    public bool enableTemperatureSensor = true;

    private int type = 0;

    private GameObject LeftWheel = null;
    private float leftWheelEnergy = 0;
    private float leftWheelToSourceDistance = float.PositiveInfinity;

    private GameObject RightWheel = null;
    private float rightWheelEnergy = 0;
    private float rightWheelToSourceDistance = float.PositiveInfinity;

    private Quaternion updateRot = new Quaternion(0, 0, 0, 1);

    //private Color tempColor = new Color(0, 0, 0, 1);

    private SimulationManager sim;

    private Renderer renderer;

    void Awake() {
        renderer = transform.GetComponent<Renderer>();
        SetType();
        LeftWheel = transform.Find("LeftWheel").gameObject;
        RightWheel = transform.Find("RightWheel").gameObject;
        updateRot = Quaternion.AngleAxis(Random.Range(-180.0f, 180.0f), Vector3.up);
    }

    void Start() {
        sim = SimulationManager.Get();
    }

    void Update() {
        EnvironmentLock();

        Sense();
        Move();
        Change();
    }

    void SetType() {
        type = Random.value < probRedType ? 0 : 1;
        if (type == 0) {
            renderer.material.color = Color.red;
        }
        else {
            renderer.material.color = Color.blue;
        }
    }

    /// <summary>
    /// Prohibits the vehicle from exiting the environment.
    /// </summary>
    void EnvironmentLock() {
        Vector3 p = new Vector3(transform.position.x, 0, transform.position.z);
        if (sim.worldBounds.Contains(p) == false) {
            transform.position = new Vector3(Random.Range(sim.worldBounds.min.x, sim.worldBounds.max.x), 0.5f, Random.Range(sim.worldBounds.min.z, sim.worldBounds.max.z));
        }
    }

    /// <summary>
    /// Calls all functions for sensing the environment.
    /// </summary>
    void Sense() {
        if (enableTemperatureSensor) {
            SenseSource();
        }
    }

    /// <summary>
    /// Calls all functions for moving the vehicle.
    /// </summary>
    void Move() {
        if (enableTranslation) {
            Translate();
        }
        if (enableRotation) {
            Rotate();
        }
    }

    /// <summary>
    /// Calls all functions that change the vehicles state and appearance.
    /// </summary>
    void Change() {
        if (enableColorChange) {
            ChangeColor();
        }
    }

    void SenseSource() {
        leftWheelToSourceDistance = Vector3.Distance(sim.source.transform.position, LeftWheel.transform.position);
        rightWheelToSourceDistance = Vector3.Distance(sim.source.transform.position, RightWheel.transform.position);

        leftWheelEnergy = leftWheelToSourceDistance;
        rightWheelEnergy = rightWheelToSourceDistance;
    }

    void Translate() {
        if (rightWheelEnergy != leftWheelEnergy) {
            float rotationRadius = 0.5f * (leftWheelEnergy + rightWheelEnergy) / ((rightWheelEnergy - leftWheelEnergy));
            float rotationAngle = Mathf.Rad2Deg * (rightWheelEnergy - leftWheelEnergy) / 1;
            Vector3 rotationPivot = transform.position + transform.right * rotationRadius;
            transform.RotateAround(rotationPivot, transform.up, rotationAngle);
        }
        else // move in straight line...
        {
            transform.Translate(0, 0, leftWheelEnergy, Space.Self);
        }
    }

    void Rotate() {
        if (1 - Mathf.Abs(Quaternion.Dot(transform.rotation, updateRot)) < 0.1f) {
            updateRot = Quaternion.AngleAxis(Random.Range(-180.0f, 180.0f), Vector3.up);
        }

        //transform.rotation = Quaternion.Lerp(transform.rotation, updateRot, rotSpeed * energy * Time.deltaTime);
    }

    void ChangeColor() {
        //if (type == 0) {
        //    tempColor.r = tempEnergy;
        //}
        //else {
        //    tempColor.b = tempEnergy;
        //}
        //renderer.material.color = tempColor;
    }
}