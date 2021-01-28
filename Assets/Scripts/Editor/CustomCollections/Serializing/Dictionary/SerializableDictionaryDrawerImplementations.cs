//Taken from http://wiki.unity3d.com/index.php/SerializableDictionary
//Author: Fredrik Ludvigsen (Steinbitglis) 

//Modifications by: Karol Kozuch (CrazedAerialCable)

using Assets.Scripts.Game.Common.CustomCollections.DefaultCollectionsSerialization.Dictionary;
using Assets.Scripts.Game.Common.Enums;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Scripts.CustomCollections.Serializing.Dictionary
{
    // ---------------
    //  String => Int
    // ---------------
    [UnityEditor.CustomPropertyDrawer(typeof(StringIntDictionary))]
    public class StringIntDictionaryDrawer : SerializableDictionaryDrawer<string, int>
    {
        protected override SerializableKeyValueTemplate<string, int> GetTemplate()
        {
            return GetGenericTemplate<SerializableStringIntTemplate>();
        }
    }
    internal class SerializableStringIntTemplate : SerializableKeyValueTemplate<string, int> { }

    // ---------------
    //  GameObject => Float
    // ---------------
    [UnityEditor.CustomPropertyDrawer(typeof(GameObjectFloatDictionary))]
    public class GameObjectFloatDictionaryDrawer : SerializableDictionaryDrawer<GameObject, float>
    {
        protected override SerializableKeyValueTemplate<GameObject, float> GetTemplate()
        {
            return GetGenericTemplate<SerializableGameObjectFloatTemplate>();
        }
    }
    internal class SerializableGameObjectFloatTemplate : SerializableKeyValueTemplate<GameObject, float> { }

    //CAC modifications

    //---------------------
    // MenuType => MenuType
    //---------------------
    /// <summary>
    /// Drawer for MenuType=>MenuType dictionary.
    /// </summary>
    [UnityEditor.CustomPropertyDrawer(typeof(MenuTypeMenuTypeDictionary))]
    public class MenuTypeMenuTypeDictionaryDrawer : SerializableDictionaryDrawer<MenuType, MenuType>
    {
        protected override SerializableKeyValueTemplate<MenuType, MenuType> GetTemplate()
        {
            return GetGenericTemplate<SerializableMenuTypeMenuTypeTemplate>();
        }
    }

    internal class SerializableMenuTypeMenuTypeTemplate : SerializableKeyValueTemplate<MenuType, MenuType> {}
}
