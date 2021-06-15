using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Noise", menuName = "MapGenerator/NoiseGenerator")]
public class NoiseGenerator : ScriptableObject
{
	[Header("Noise Settings")]

	[Tooltip("noise type")]
	public NoiseType noiseType;

	[Tooltip("scale of the noise")]
	[Min(0.0001f)]
	public float scale;

	[Tooltip("seed to create the pseudo random offsets for the octaves")]
	public int offsetSeed;

	[Tooltip("number of octaves")]
	[Range(1, 10)]
	public int octaves;

	[Tooltip("frequency of the noise")]
	[Range(0.0f, 10.0f)]
	public float frequency;

	[Tooltip("persistance of the generated values, between 0 and 1")]
	[Range(0f, 1f)]
	public float persistance;

	[Tooltip("gain of the noise")]
	[Range(0.0f, 2.0f)]
	public float gain;

	[Tooltip("lacunarity of the generated values, greater than 1")]
	[Min(1f)]
	public float lacunarity;

	[Tooltip("x offset to add to the pseudo random generated offsets of each octave")]
	[Range(-100.0f, 100.0f)]
	public float fixedOffsetX;

	[Tooltip("y offset to add to the pseudo random generated offsets of each octave")]
	[Range(-100.0f, 100.0f)]
	public float fixedOffsetY;

	[Tooltip("ridge offset, for ridge noise only")]
	[Range(-10.0f, 10.0f)]
	public float ridgeOffset;

	private Vector2[] octaveOffsets;
	private int mapWidth;
	private int mapHeight;

	public void Init(int mapWidth, int mapHeight) {
		octaveOffsets = GetOctaveOffsets(octaves, offsetSeed, fixedOffsetX, fixedOffsetY);
		this.mapWidth = mapWidth;
		this.mapHeight = mapHeight;
	}

	public float GetValueAtPosition(float x, float y) {
		switch (noiseType) {
			case NoiseType.PERLIN: return Perlin(x, y);
			case NoiseType.FBM: return FBM(x, y);
			case NoiseType.TURBULANCE: return Turbulence(x, y);
			case NoiseType.RMF: return RMF(x, y);
			default: return Perlin(x, y);
		}
	}

	/// <summary>
	/// get min and max value of a 2D array
	/// </summary>
	/// <param name="noiseMap">2D array</param>
	/// <returns>min and max value</returns>
	public (float min, float max) GetMinMaxValue(float[,] noiseMap) {
		float min = noiseMap.Cast<float>().Min();
		float max = noiseMap.Cast<float>().Max();
		return (min, max);
	}

	/// <summary>
	/// normalize noise map values to a range from 0 to 1 using the min and max noise height
	/// </summary>
	/// <returns></returns>
	public float[,] NormalizeNoiseMap(float[,] noiseMap) {
		(float, float) extremes = GetMinMaxValue(noiseMap);
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				noiseMap[x, y] = Mathf.InverseLerp(extremes.Item1, extremes.Item2, noiseMap[x, y]);
			}
		}
		return noiseMap;
	}

	/// <summary>
	/// generates random octave offsets
	/// </summary>
	/// <returns>array of octave offsets in x and y direction</returns>
	public static Vector2[] GetOctaveOffsets(int octaves, int offsetSeed, float fixedOffsetX, float fixedOffsetY) {
		System.Random prng = new System.Random(offsetSeed);
		Vector2[] octaveOffsets = new Vector2[octaves];
		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next(-100000, 100000) + fixedOffsetX;
			float offsetY = prng.Next(-100000, 100000) + fixedOffsetY;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}
		return octaveOffsets;
	}

	/// <summary>
	/// creates a perlin noise value for given coordinates
	/// </summary>
	/// <returns>noise value</returns>
	public float Perlin(float x, float y) {
		float amplitude = 1;
		float frequency = 1;
		float noiseValue = 0;

		for (int i = 0; i < octaves; i++) {
			// higher frequency -> further apart sample points -> height values change faster
			float sampleX = (x - (mapWidth / 2)) / scale * frequency + octaveOffsets[i].x;
			float sampleY = (y - (mapHeight / 2)) / scale * frequency + octaveOffsets[i].y;

			// adjusts perlin noise range to -1 to 1 so that the noise height can decrease
			float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
			// increase noise height by perlin value of each octave
			noiseValue += perlinValue * amplitude;

			// decreases every octave
			amplitude *= persistance;
			// increases every octave
			frequency *= lacunarity;
		}
		return noiseValue;
	}

	/// <summary>
	/// fractal brownian noise (summed noise) value for given coordinates
	/// </summary>
	/// <returns>noise value</returns>
	public float FBM(float x, float y) {
		float sum = 0f;
		float range = 0f;
		float amplitude = 1f;
		float f = frequency;
		x = x + fixedOffsetX;
		y = y + fixedOffsetY;

		for (int o = 0; o < octaves; o++) {
			sum += (Mathf.PerlinNoise(x * f, y * f) - 0.5f) * amplitude;
			range += amplitude;
			f *= lacunarity;
			amplitude *= gain;
		}
		return sum / range;
	}

	/// <summary>
	/// turbulence noise value for given coordinates
	/// </summary>
	/// <returns>noise value</returns>
	public float Turbulence(float x, float y) {
		float sum = 0f;
		float range = 0f;
		float amplitude = 1f;
		float f = frequency;
		x = x + fixedOffsetX;
		y = y + fixedOffsetY;

		for (int o = 0; o < octaves; o++) {
			sum += Mathf.Abs(Mathf.PerlinNoise(x * f, y * f) - 0.5f) * amplitude;
			range += amplitude;
			f *= lacunarity;
			amplitude *= gain;
		}
		return sum / range;
	}

	private float Ridgef(float h) {
		h = ridgeOffset - Mathf.Abs(h);
		return (h * h);
	}

	public float RMF(float x, float y) {
		float sum = 0.0f;
		float max = 0.0f;
		float prev = 1.0f;
		float amplitude = 0.5f;
		float maxo = Mathf.Max(Ridgef(0.0f), Ridgef(1.0f));
		float f = frequency;
		x = x + fixedOffsetX;
		y = y + fixedOffsetY;

		for (int i = 0; i < octaves; i++) {

			float n = Ridgef(Mathf.PerlinNoise(x * f, y * f) - 0.5f);
			float multiplier = amplitude * prev;
			sum += n * multiplier;
			max += maxo * multiplier;
			prev = n;
			f *= lacunarity;
			amplitude *= gain;
		}
		return (2.0f * sum / max) - 1.0f;
	}
}
