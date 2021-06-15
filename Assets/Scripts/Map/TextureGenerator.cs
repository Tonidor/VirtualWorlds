using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class TextureGenerator
{
	/// <summary>
	/// creates a texture from a given noise map
	/// </summary>
	/// <returns>noise texture</returns>
	public Texture2D TextureFromNoiseMap(float[,] noiseMap, int width, int height) {
		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colourMap[y * width + x] = Color.Lerp(Color.white, Color.black, noiseMap[x, y]);
			}
		}
		return TextureFromColourMap(colourMap, width, height);
	}

	/// <summary>
	/// creates a texture from a given colour map
	/// </summary>
	/// <param name="colourMap">colur map to convert into a texture</param>
	/// <param name="width">texture width</param>
	/// <param name="height">texture height</param>
	/// <returns>colour texture</returns>
	public Texture2D TextureFromColourMap(Color[] colourMap, int width, int height) {
		Texture2D texture = new Texture2D(width, height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels(colourMap);
		texture.Apply();
		return texture;
	}
}