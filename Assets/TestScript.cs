﻿using System;
using Unity.Entities;
using Unity.Scenes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Transforms;

public class TestScript : MonoBehaviour
{
    public void SpawnGameObjectAndPrefab()
    {
        UnityEngine.Assertions.Assert.IsTrue(World.DefaultGameObjectInjectionWorld.IsCreated, "No world");

        // Create GameObject
        var obj = new GameObject();
        SceneManager.MoveGameObjectToScene(obj, gameObject.scene);

        // Create matching Entity
        var world = World.DefaultGameObjectInjectionWorld;
        var manager = world.EntityManager;
        var entity = manager.CreateEntity();
        manager.SetName(entity, "MyEntity");
        // Link entity to GO
        manager.AddComponentData(entity, GameObjectConversionUtility.GetEntityGuid(obj, 0));
        // Copy GO transform
        manager.AddComponentData(entity, new LocalToWorld{ Value = obj.transform.localToWorldMatrix });
        manager.AddComponentData(entity, new Translation{ Value = obj.transform.localPosition });
        manager.AddComponentData(entity, new Rotation{ Value = obj.transform.localRotation });

        // Set the SceneTag and SceneSection on the entity so it's in the sub-scene
        var sceneGuid = new GUID(AssetDatabase.AssetPathToGUID(gameObject.scene.path));
        var sceneSystem = world.GetExistingSystem<SceneSystem>();
        var sceneEntity = sceneSystem.GetSceneEntity(sceneGuid);
        var sectionEntity = manager.GetBuffer<ResolvedSectionEntity>(sceneEntity)[0].SectionEntity;
        manager.AddSharedComponentData(entity, new SceneTag{ SceneEntity = sectionEntity });
        manager.AddSharedComponentData(entity, new SceneSection { SceneGUID = sceneGuid });

        // Register object for conversion (triggers a live-link update)
        Undo.RegisterCreatedObjectUndo(obj, "Create Object");
    }
}