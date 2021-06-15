using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MapSettings", menuName = "MapGenerator/MapSettings")]
public class MapSettings : ScriptableObject
{
    [Header("Map Settings")]
    [Tooltip("name of the terrain")]
    public string terrainName;
    [Tooltip("submesh index of the source mesh to use")]
    public int sourceSubMeshIndex;
    [Tooltip("x scale of terrain")]
    public int terrainX;
    [Tooltip("y scale of terrain")]
    public int terrainY;
    [Tooltip("draw mode")]
    public DrawMode drawMode;
    [Tooltip("multiplier for adjusting the terrain height, only effective when a mesh is generated")]
    public float meshHeightMultiplier;
    [Tooltip("curve for adjusting the change of the mesh height")]
    public AnimationCurve meshHeightCurve;
	[Tooltip("size of the map texture")]
	public int textureSize;
	[Tooltip("color gradient for the map texture")]
    public Gradient colorGradient;
}
