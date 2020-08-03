using Unity.Entities;
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
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entity = manager.CreateEntity();
        manager.SetName(entity, "MyEntity");
        // Link entity to GO
        manager.AddComponentData(entity, GameObjectConversionUtility.GetEntityGuid(obj, 0));
        // Copy GO transform
        manager.AddComponentData(entity, new LocalToWorld{ Value = obj.transform.localToWorldMatrix });
        manager.AddComponentData(entity, new Translation{ Value = obj.transform.localPosition });
        manager.AddComponentData(entity, new Rotation{ Value = obj.transform.localRotation });

        // Register object for conversion
        Undo.RegisterCreatedObjectUndo(obj, "Create Object");
    }
}