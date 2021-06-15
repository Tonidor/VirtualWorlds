using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapComponent : MonoBehaviour {
	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;

	[Header("Map Generation Settings")] [OnValueChanged("OnSettingsChanged"), Expandable]
	public MapSettings mapSettings;

	[OnValueChanged("OnSettingsChanged"), Expandable]
	public NoiseGenerator noiseGenerator;

	private TextureGenerator textureGenerator;
	public bool autoUpdate;

	[Header("Tesselation Settings")] public bool enableTesselation;

	[OnValueChanged("OnSettingsChanged")] [Range(1, 1000)]
	public int tesselation = 100;

	[OnValueChanged("OnSettingsChanged")] [Range(1, 100)]
	public float size = 5;

	public Bounds mapBounds;
	public float delta = 0.5f;

	private void Awake() {
		Init();
	}

	void Init() {
		meshFilter = transform.GetComponent<MeshFilter>();
		meshRenderer = transform.GetComponent<MeshRenderer>();
		noiseGenerator.Init(mapSettings.terrainX, mapSettings.terrainY);
		textureGenerator = new TextureGenerator();
		mapBounds = meshRenderer.bounds;
	}

	public void DrawMap(Mesh terrainMesh, Texture2D terrainTexture) {
		meshFilter.mesh = terrainMesh;
		meshRenderer.sharedMaterial.mainTexture = terrainTexture;
	}

	public void GenerateMap() {
		Init();
		Mesh terrainMesh = GenerateTerrainMesh();
		Texture2D terrainTexture = GenerateTerrainTexture();
		if (enableTesselation) Tesselate();
		DrawMap(terrainMesh, terrainTexture);
		mapBounds = meshRenderer.bounds;
	}

	public Mesh GenerateTerrainMesh() {
		Mesh mesh = meshFilter.sharedMesh;
		Vector3[] vertices = mesh.vertices;

		for (var i = 0; i < vertices.Length; i++)
			vertices[i].y = GetMeshHeight(transform.TransformPoint(vertices[i]));

		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	public void TransformWorldToUV(Vector3 p, out float u, out float v) {
		// Transform from world space to texture space...
		p = transform.InverseTransformPoint(p);
		u = 0.5f + (p.x / mapBounds.size.x);
		v = 0.5f + (p.z / mapBounds.size.z);
	}

	public void TransformUVtoWorld(float u, float v, ref Vector3 p) {
		p = transform.TransformPoint((u - 0.5f) * mapBounds.size.x, 0, (v - 0.5f) * mapBounds.size.z);
	}

	public Texture2D GenerateTerrainTexture() {
		Texture2D terrainTexture = new Texture2D(mapSettings.textureSize, mapSettings.textureSize);
		terrainTexture.name = "TemperatureMap";
		terrainTexture.wrapMode = TextureWrapMode.Repeat;

		Color color = new Color();

		Vector3 p = new Vector3(0, 0, 0);
		float u, v;
		float invh = 1.0f / terrainTexture.height;
		float invw = 1.0f / terrainTexture.width;

		for (int z = 0; z < terrainTexture.height; z++) {
			v = z * invh;

			for (int x = 0; x < terrainTexture.width; x++) {
				u = x * invw;
				TransformUVtoWorld(u, v, ref p);

				color = mapSettings.colorGradient.Evaluate(GetTemperature(p));
				terrainTexture.SetPixel(x, z, color);
			}
		}

		terrainTexture.Apply();
		return terrainTexture;
	}

	public void OnSettingsChanged() {
		if (autoUpdate) GenerateMap();
	}

	public Vector3 GetRandomPosition() {
		float x = Random.Range(mapBounds.min.x, mapBounds.max.x);
		float z = Random.Range(mapBounds.min.z, mapBounds.max.z);
		return new Vector3(x, GetMeshHeight(new Vector3(x, 0, z)), z);
	}

	public Vector3 GetRandomPositionWithinRadius(Vector3 targetPosition, int radius) {
		float x = targetPosition.x + Random.Range(-radius, radius);
		float z = targetPosition.z + Random.Range(-radius, radius);
		return new Vector3(x, GetMeshHeight(new Vector3(x, 0, z)), z);
	}

	public Vector3 GetMeshPosition(Transform targetTransform) {
		Vector3 closestPoint = mapBounds.ClosestPoint(targetTransform.position);
		Vector3 meshPosition = new Vector3(closestPoint.x, GetMeshHeight(closestPoint), closestPoint.z);
		return meshPosition;
	}

	public float GetMeshHeight(Vector3 targetPosition) {
		float height = mapSettings.meshHeightMultiplier *
		               mapSettings.meshHeightCurve.Evaluate(
			               noiseGenerator.GetValueAtPosition(targetPosition.x, targetPosition.z));
		return height;
	}

	public Vector3 GetGradient(Vector3 targetPosition) {
		float height = GetMeshHeight(targetPosition);
		
		float dx = GetMeshHeight(targetPosition + Vector3.right * delta) - height;
		float dy = GetMeshHeight(targetPosition + Vector3.forward * delta) - height;
		return new Vector3(dx, 0, dy).normalized;
	}

	public float GetTemperature(Vector3 targetPosition) {
		return noiseGenerator.GetValueAtPosition(targetPosition.x, targetPosition.z);
	}

	public bool isInsideBounds(Vector3 targetPosition) => mapBounds.Contains(targetPosition);

	private Texture2D TextureToTexture2D(Texture texture) {
		Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

		RenderTexture currentRT = RenderTexture.active;

		RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 32);
		Graphics.Blit(texture, renderTexture);

		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
		texture2D.Apply();

		RenderTexture.active = currentRT;

		return texture2D;
	}

	public void Tesselate() {
		Vector3 p = new Vector3();
		Vector2 uv = new Vector2();

		Mesh terrainMesh = meshFilter.sharedMesh;
		//mesh.Clear(false);

		Vector3[] vertices = new Vector3[(tesselation + 1) * (tesselation + 1)];
		Vector2[] uvs = new Vector2[(tesselation + 1) * (tesselation + 1)];

		int i = 0;
		float scale = size / tesselation;
		float offW = -size / 2f;
		float offD = -size / 2f;
		for (int d = 0; d <= tesselation; d++) {
			uv.y = d / (float) tesselation;

			for (int w = 0; w <= tesselation; w++) {
				float x = scale * w + offW;
				float z = scale * d + offD;
				float y = 0;

				uv.x = w / (float) tesselation;

				uvs[i] = uv;

				p.Set(x, y, z);
				vertices[i] = p; // new Vector3(w, 0, d) - new Vector3(width / 2f, 0, depth / 2f);

				i++;
			}
		}

		int[] triangles = new int[tesselation * tesselation * 2 * 3]; // 2 - polygon per quad, 3 - corners per polygon

		for (int d = 0; d < tesselation; d++) {
			for (int w = 0; w < tesselation; w++) {
				// quad triangles index.
				int ti = (d * (tesselation) + w) * 6; // 6 - polygons per quad * corners per polygon

				// First tringle
				triangles[ti] = (d * (tesselation + 1)) + w;
				triangles[ti + 1] = ((d + 1) * (tesselation + 1)) + w;
				triangles[ti + 2] = ((d + 1) * (tesselation + 1)) + w + 1;

				// Second triangle
				triangles[ti + 3] = (d * (tesselation + 1)) + w;
				triangles[ti + 4] = ((d + 1) * (tesselation + 1)) + w + 1;
				triangles[ti + 5] = (d * (tesselation + 1)) + w + 1;
			}
		}

		// Assigning vertices, triangles and UV to the mesh.
		//mesh.vertices = vertices;
		terrainMesh.triangles = triangles;


		for (i = 0; i < vertices.Length; i++) {
			float height = GetMeshHeight(transform.TransformPoint(vertices[i]));
			vertices[i].y = height;
		}

		terrainMesh.vertices = vertices;
		terrainMesh.uv = uvs;
		terrainMesh.RecalculateNormals();
		terrainMesh.RecalculateBounds();

		Debug.Log("tessi");
	}
}