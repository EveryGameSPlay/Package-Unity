using System;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Linq;
using Egsp.Files.Serializers;
using JetBrains.Annotations;

namespace Egsp.Files
 {
     /// <summary>
     /// <para>Корневая папка на разных устройствах отличается.</para>
     /// <para>
     /// Mobile - Application.persistentDataPath/Storage/;
     /// PC - Application.dataPath/Storage/
     /// </para>
     /// </summary>
     public static class Storage
     {
         /// <summary>
         /// Расширение файла для сохраняемых данных
         /// </summary>
         private const string DefaultExtension = ".txt";

         /// <summary>
         /// Папка сохраняемых данных. Оканчивается на "/"
         /// </summary>
         private static string RootFolder;

         /// <summary>
         /// Глобальные данные.
         /// </summary>
         [NotNull]
         public static DataProvider Global { get; private set; }
         
         /// <summary>
         /// Сокращение для глобальных данных.
         /// </summary>
         [NotNull]
         public static DataProvider g => Global;
         
         /// <summary>
         /// Данные принадлежащие профилю.
         /// </summary>
         [NotNull]
         public static DataProvider Local { get; set; }

         /// <summary>
         /// Сокращение для локальных данных.
         /// </summary>
         [NotNull]
         public static DataProvider l => Local;

         /// <summary>
         /// Все существующие профили.
         /// </summary>
         private static List<DataProfile> _profiles;
         
         static Storage()
         {
             
#if UNITY_ANDROID
             RootFolder = Application.persistentDataPath + "/Storage/";
#elif UNITY_IOS
             RootFolder = Application.persistentDataPath + "/Storage/";
#else 
             RootFolder = Application.dataPath + "/Storage/";
#endif
             
             Initialize();
             LoadProfiles();
         }

         private static void Initialize()
         {
             // Проверка существования папки с сохранениями
             if (Directory.Exists(RootFolder))
             {
                 // Создание отсутствующей папки с сохранениями
                 Directory.CreateDirectory(RootFolder);
                 
             }
         }

         private static void LoadProfiles()
         {
             var globalProfile = new DataProfile("Global");
             var globalProvider = new DataProvider(globalProfile, RootFolder, new UnitySerializer(), DefaultExtension);

             Global = globalProvider;

             _profiles = Global.LoadClassEntities<DataProfile>("Profiles/");
             
             // Если локальный профиль не найден, то он будет ссылаться на глобальный профиль
             if (_profiles.Count == 0)
             {
                 Local = Global;
                 return;
             }

             var localProfile = _profiles[0];
             Local = new DataProvider(localProfile, RootFolder, new UnitySerializer(),DefaultExtension);
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
         public static void SetLocal(DataProfile profile)
         {
             if(!_profiles.Contains(profile))
                 throw new Exception($"Profile {profile.Name} not exist in current list of profiles!");
             
             Local = new DataProvider(profile, RootFolder, new UnitySerializer(), DefaultExtension);
         }

         /// <summary>
         /// Получение данных о профиле.
         /// </summary>
         /// <returns></returns>
         public static PropertyFileProxy GetLocalProfileInfo()
         {
             var proxy = Local.GetProxy("info");
             return proxy;
         }
         
     }
}




