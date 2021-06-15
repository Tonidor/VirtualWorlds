using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    static SimulationManager g_Instance;
    public static SimulationManager Get() { return g_Instance; }

    public Bounds worldBounds;

    [Header("World Settings")]
    [SerializeField]
    private GameObject world = null;
    [SerializeField]
    public GameObject source = null;
    [SerializeField]
    private float worldHeightMin = -5.0f;
    [SerializeField]
    private float worldHeightMax = 5.0f;

    [Header("Vehicle Settings")]
    [SerializeField]
    private int numVehicles = 20;
    [SerializeField]
    private GameObject vehicle_prefab = null;
    private GameObject vehicles;

    [Header("Source Settings")]
    [Range(0.0f, 100.0f)]
    public float fTemperatureScale = 10.0f;
    private Texture2D temperatureTex = null;

    private void Awake() {
        g_Instance = this;
    }

    void Start() {
        worldBounds = world.GetComponent<Renderer>().bounds;
        worldBounds.SetMinMax(new Vector3(worldBounds.min.x, worldHeightMin, worldBounds.min.z), new Vector3(worldBounds.max.x, worldHeightMax, worldBounds.max.z));

        createWorld();
        createVehicles();
    }

    private void createVehicles() {
        vehicles = Instantiate(new GameObject("vehicles"));

        for (int i = 0; i < numVehicles; i++) {
            Vector3 pos = new Vector3(Random.Range(worldBounds.min.x, worldBounds.max.x), 0.5f, Random.Range(worldBounds.min.z, worldBounds.max.z));
            Quaternion rot = new Quaternion();
            rot.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f);

            GameObject v = Instantiate(vehicle_prefab, pos, rot, vehicles.transform);
            v.name = "Vehicle." + i;
            v.SetActive(true);
        }
    }

    void createWorld() {
        if (temperatureTex == null) {
            temperatureTex = new Texture2D(128, 128);
        }

        Color color = new Color();

        for (int y = 0; y < temperatureTex.height; y++) {
            float fy = y;
            fy *= fTemperatureScale / temperatureTex.height;

            for (int x = 0; x < temperatureTex.width; x++) {
                float fx = x;
                fx *= fTemperatureScale / temperatureTex.width;

                color.r = Mathf.PerlinNoise(fx, fy);
                color.g = color.r;
                color.b = color.r;
                temperatureTex.SetPixel(x, y, color);
            }
        }

        temperatureTex.Apply();
        Renderer renderer = world.GetComponent<Renderer>();
        renderer.material.mainTexture = temperatureTex;
    }

    public float GetTemperature(Vector3 p) {
        p = world.transform.InverseTransformPoint(p);
        Bounds meshBounds = world.GetComponent<MeshFilter>().mesh.bounds;

        float u = 0.5f - (p.x / meshBounds.size.x);
        float v = 0.5f - (p.z / meshBounds.size.z);

        if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
            return 0.0f;

        u *= temperatureTex.width;
        v *= temperatureTex.height;

        // Draw in the texture for debugging...
        //		m_temperatureTex.SetPixel((int)u, (int)v, new Color(1, 0, 0, 1));
        //		m_temperatureTex.Apply();

        return temperatureTex.GetPixel((int)u, (int)v).r;
    }


}
