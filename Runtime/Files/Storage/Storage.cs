using System;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace Egsp.Core
 {
     /// <summary>
     /// <para>Через данный класс осуществляется доступ к данным вне приложения.
     /// Учитывает платформу и инструментарий.</para>
     /// <para>Корневая папка на разных устройствах отличается.</para>
     /// <para>
     /// Mobile - Application.persistentDataPath/Storage/;
     /// PC - Application.dataPath/Storage/
     /// </para>
     /// </summary>
     public static partial class Storage
     {
         private const string CommonProviderName = "Common";
         
         /// <summary>
         /// Расширение файла для сохраняемых данных
         /// </summary>
         private const string DefaultExtension = ".txt";

         /// <summary>
         /// Папка сохраняемых данных. Оканчивается на "/"
         /// </summary>
         private static string RootFolder;

         /// <summary>
         /// Общие данные.
         /// </summary>
         [NotNull]
         public static DataProvider Common { get; private set; }
         
         /// <summary>
         /// Данные принадлежащие профилю. Если не было загружено ни одного профиля, то будет использоваться общий.
         /// </summary>
         [NotNull]
         public static DataProvider Current { get; set; }

         /// <summary>
         /// Все существующие профили.
         /// </summary>
         private static List<DataProfile> _profiles;

         public static ISerializer DefaultSerializer { get; private set; }
         
         static Storage()
         {

#if UNITY_EDITOR
             RootFolder = Application.dataPath + "/Storage/";
#elif UNITY_ANDROID
             RootFolder = Application.persistentDataPath + "/Storage/";
#elif UNITY_IOS
             RootFolder = Application.persistentDataPath + "/Storage/";
#else 
             RootFolder = Application.dataPath + "/Storage/";
#endif

#if ODIN_SERIALIZE
             DefaultSerializer = new OdinSerializer();
#else
             DefaultSerializer = new UnitySerializer();
#endif
             
             PrepareDirectories();
             LoadProfiles();
         }

         private static void PrepareDirectories()
         {
             // Проверка существования папки с сохранениями
             if (!Directory.Exists(RootFolder))
             {
                 // Создание отсутствующей папки с сохранениями
                 Directory.CreateDirectory(RootFolder);
             }
         }

         private static void LoadProfiles()
         {
             var commonProfile = new DataProfile(CommonProviderName);
             var commonProvider = new DataProvider(commonProfile, RootFolder, DefaultSerializer, DefaultExtension);

             Common = commonProvider;

             var profiles = Common.GetObjects<DataProfile>("Profiles/profiles");

             if (profiles.IsSome)
             {
                 var list = profiles.option;

                 if (list.Count == 0)
                 {
                     _profiles = list;
                     Current = Common;
                 }
                 else
                 {
                     var localProfile = _profiles[0];
                     Current = new DataProvider(localProfile, RootFolder, DefaultSerializer, DefaultExtension);
                 }
             }
             else
             {
                 _profiles = null;
             }
             
             // Если локальный профиль не найден, то он будет ссылаться на глобальный профиль
             if (!profiles.IsSome || profiles.option.Count == 0)
             {
                 _profiles = profiles.option;
                 Current = Common;
             }
             else
             {
                 _profiles = profiles.option;
                 var localProfile = _profiles[0];
                 Current = new DataProvider(localProfile, RootFolder, DefaultSerializer, DefaultExtension);
             }
         }

         /// <summary>
         /// Получение всех существующих профилей.
         /// Возвращается копия списка. Однако элементы списка НЕ копии!
         /// </summary>
         public static List<DataProfile> GetProfiles()
         {
             return _profiles.ToList();
         }

         /// <summary>
         /// Установка локального профиля.
         /// Если профиль не будет найдет в списке, то вылетит исключение. 
         /// </summary>
         public static void SwitchCurrentProfile(DataProfile profile)
         {
             if(!_profiles.Contains(profile))
                 throw new Exception($"Profile {profile.Name} not exist in current list of profiles!");
             
             Current = new DataProvider(profile, RootFolder, new UnitySerializer(), DefaultExtension);
         }

     }
}




