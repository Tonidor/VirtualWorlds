using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
	/// <summary>
	/// generates a noise map using perlin noise with octaves
	/// </summary>
	/// <param name="mapWidth">width of the map</param>
	/// <param name="mapHeight">height of the map</param>
	/// <param name="seed">seed to create the pseudo random offsets for the octaves</param>
	/// <param name="scale">scale of the noise</param>
	/// <param name="octaves">number of octaves to use</param>
	/// <param name="persistance">persistance of the generated values, between 0 and 1</param>
	/// <param name="lacunarity">lacunarity of the generated values, greater than 1</param>
	/// <param name="offset">offset to add to the pseudo random generated offsets of each octave</param>
	/// <param name="noiseType">type of noise that is applied on the map</param>
	/// <returns>noise map</returns>
	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, NoiseType noiseType = NoiseType.PERLIN) {
		float[,] noiseMap = new float[mapWidth, mapHeight];

		// creates a pseudo random offset for the octaves
		System.Random prng = new System.Random(seed);
		Vector2[] octaveOffsets = new Vector2[octaves];
		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next(-100000, 100000) + offset.x;
			float offsetY = prng.Next(-100000, 100000) + offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}

		if (scale <= 0) {
			scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {
					// higher frequency -> further apart sample points -> height values change faster
					float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

					// adjusts perlin noise range to -1 to 1 so that the noise height can decrease
					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					// increase noise height by perlin value of each octave
					noiseHeight += perlinValue * amplitude;

					// decreases every octave
					amplitude *= persistance;
					// increases every octave
					frequency *= lacunarity;
				}

				if (noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;
				}
				else if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}
				noiseMap[x, y] = noiseHeight;
			}
		}

		// normalize noise map values to a range from 0 to 1 using the min and max noise height
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
			}
		}

		return noiseMap;
	}
}
