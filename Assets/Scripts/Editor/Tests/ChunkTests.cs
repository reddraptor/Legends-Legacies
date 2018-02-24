using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Serialization;
using Assets.Scripts.Components;

namespace Assets.Scripts.Editor.Tests
{
    public class ChunkTests
    {
        const int TEST_REPLACEMENT_TERRAINTILE_INDEX = 2;
        TerrainTile.Indices TEST_REPLACEMENT_TILE_INDICES = new TerrainTile.Indices(2, 3);
        const string TEST_PREFAB_NAME = "Default Chunk";
        const int TEST_TILE_SET_INDEX = 1;

        GameObject chunkPrefab;
        GameObject chunkInstance;
        Chunk chunk;
        TerrainTile terrainTile;
        ChunkData chunkData;

        string saveFileName = "chuckSaveTest.sav";
        Stream testFileStream;
        BinaryFormatter serializer = new BinaryFormatter();

        [Test]
        public void CreateChunkTest()
        {
            chunkPrefab = Resources.Load("Chunks/" + TEST_PREFAB_NAME) as GameObject;
            Assert.That(chunkPrefab, Is.Not.Null);
            Assert.That(chunkPrefab, Is.TypeOf(typeof(GameObject)));

            chunkInstance = Object.Instantiate(chunkPrefab);
            Assert.That(chunkInstance, Is.Not.Null);
            Assert.That(chunkInstance, Is.TypeOf(typeof(GameObject)));

            chunk = chunkInstance.GetComponent<Chunk>();
            Assert.That(chunk, Is.Not.Null);
            Assert.That(chunk, Is.TypeOf(typeof(Chunk)));

            terrainTile = chunk.GetTerrainTileAt(TEST_REPLACEMENT_TILE_INDICES);
            Assert.That(terrainTile, Is.Not.Null);
            Assert.That(terrainTile.prefabIndex, Is.EqualTo(chunk.GetTileSet().defaultIndex));
        }

        [Test]
        public void SetTileSetTest()
        {
            if (chunk == null) CreateChunkTest();

            chunk.SetTileSet(TEST_TILE_SET_INDEX);

            Assert.That(chunk.GetTileSet(), Is.EqualTo(chunk.prefabTables.tileSetTable[TEST_TILE_SET_INDEX]));

            terrainTile = chunk.GetTerrainTileAt(TEST_REPLACEMENT_TILE_INDICES);
            Assert.That(terrainTile, Is.Not.Null);
            Assert.That(terrainTile.prefabIndex, Is.EqualTo(chunk.GetTileSet().defaultIndex));
        }

        [Test]
        public void ReplaceTileTest()
        {
            if (chunk == null) CreateChunkTest();
            
            chunk.ReplaceTile(TEST_REPLACEMENT_TILE_INDICES, TEST_REPLACEMENT_TERRAINTILE_INDEX);

            terrainTile = chunk.GetTerrainTileAt(TEST_REPLACEMENT_TILE_INDICES);
            Assert.That(terrainTile, Is.Not.Null);
            Assert.That(terrainTile.prefabIndex, Is.EqualTo(TEST_REPLACEMENT_TERRAINTILE_INDEX));
        }

        [Test]
        public void SaveChunkTest()
        {
            CreateChunkTest();
            SetTileSetTest();
            ReplaceTileTest();

            if (File.Exists("saveFileName"))
            {
                File.Delete("saveFileName");
            }

            testFileStream = File.Create(saveFileName);
            
            chunkData = chunk.GetSerializableData();

            Assert.That(chunkData.tileSetIndex, Is.EqualTo(TEST_TILE_SET_INDEX));
            Assert.That(chunkData.size, Is.EqualTo(chunk.size));
            Assert.That(
                chunkData.tilePrefabIndex[TEST_REPLACEMENT_TILE_INDICES.i, TEST_REPLACEMENT_TILE_INDICES.j],
                Is.EqualTo(TEST_REPLACEMENT_TERRAINTILE_INDEX)
                );

            
            serializer.Serialize(testFileStream, chunkData);
            testFileStream.Close();

            Object.DestroyImmediate(chunkInstance);
        }

        [Test]
        public void LoadChunkTest()
        {
            SaveChunkTest();

            Assert.That(File.Exists(saveFileName));

            testFileStream = File.OpenRead(saveFileName);
            chunkData = (ChunkData)serializer.Deserialize(testFileStream);
            Debug.Log(chunkData);

            testFileStream.Close();

            CreateChunkTest();
            chunk.SetFromSerializableData(chunkData);

            terrainTile = chunk.GetTerrainTileAt(TEST_REPLACEMENT_TILE_INDICES);
            Assert.That(terrainTile, Is.Not.Null);
            Assert.That(terrainTile.prefabIndex, Is.EqualTo(TEST_REPLACEMENT_TERRAINTILE_INDEX));
        }

    } 
}