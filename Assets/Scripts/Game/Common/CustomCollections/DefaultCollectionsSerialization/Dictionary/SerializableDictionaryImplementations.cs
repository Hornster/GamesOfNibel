//Taken from http://wiki.unity3d.com/index.php/SerializableDictionary
//Author: Fredrik Ludvigsen (Steinbitglis) 

//Modifications by: Karol Kozuch (CrazedAerialCable)

using System;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.CustomCollections.DefaultCollectionsSerialization.Dictionary
{
    // ---------------
//  String => Int
// ---------------
    [Serializable]
    public class StringIntDictionary : SerializableDictionary<string, int> { }

// ---------------
//  GameObject => Float
// ---------------
    [Serializable]
    public class GameObjectFloatDictionary : SerializableDictionary<GameObject, float> { }

//CAC modifications

//---------------------
// MenuType => MenuType
//---------------------
    /// <summary>
    /// Serializable dictionary type for two menu types.
    /// </summary>
    [Serializable]
    public class MenuTypeMenuTypeDictionary : SerializableDictionary<MenuType, MenuType> {}
//---------------------
// SkillType => bool
//---------------------
    /// <summary>
    /// Serializable dictionary type for two menu types.
    /// </summary>
    [Serializable]
    public class SkillTypeBoolDictionary : SerializableDictionary<SkillType, bool> { }
}