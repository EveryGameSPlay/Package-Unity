﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Egsp.Core;
using Egsp.Files.Serializers;

namespace Egsp.Files
{
    public class DataProvider
    {
        /// <summary>
        /// Используемый профиль.
        /// </summary>
        public readonly DataProfile Profile;

        /// <summary>
        /// Корневая папка, куда сохраняются все файлы.
        /// </summary>
        public readonly string RootFolder;
        
        /// <summary>
        /// Расширения сохраняемых файлов.
        /// </summary>
        public readonly string Extension;
        
        /// <summary>
        /// Сериализатор, используемый провайдером.
        /// </summary>
        public ISerializer Serializer { get; set; }

        public DataProvider(DataProfile profile, string rootFolder, string extension = ".txt")
        {
            Profile = profile;
            
            if(DataProfile.ValidateProfile(profile)==false)
                throw new InvalidDataException();

            RootFolder = rootFolder + "/" + profile.Name+"/";
            Extension = extension;
            
            Serializer = new UnitySerializer();
        }
        
        public DataProvider(DataProfile profile, string rootFolder, ISerializer serializer, string extension = ".txt")
            : this(profile, rootFolder, extension)
        {
            Serializer = serializer;
        }
        
        /// <summary>
        /// Возвращает прокси для файла со свойствами.
        /// При отсутствии файла создает новый, если createDefault == true.
        /// </summary>
        public PropertyFileProxy GetPropertiesFromFile(string filePath, bool createDefault = true)
        {
            var path = RootFolder + filePath + Extension;
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());

            if (File.Exists(path))
            {
                var fs = File.Open(path, FileMode.Open, FileAccess.ReadWrite);

                return new PropertyFileProxy(fs);
            }

            if (createDefault)
            {
                var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                return new PropertyFileProxy(fs);
            }

            throw new FileNotFoundException();
        }

        /// <summary>
        /// Сохраняет любую сущность с помощью сериализации.
        /// </summary>
        public void SaveObject<T>(T entity, string file)
        {
            var path = CombineFilePath(file);
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());
            
            var data = Serializer.Serialize(entity);

            // Перезапись старого файла.
            var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            var bw = new BinaryWriter(fs);
            bw.Write(data);
            
            fs.Close();
        }
        
        /// <summary>
        /// Загружает любую сущность с помощью десериализации.
        /// </summary>
        public T LoadObject<T>(string file)
        {
            var path = CombineFilePath(file);

            if (!File.Exists(path))
                return default(T);

            var data = File.ReadAllBytes(path);

            var entity = Serializer.Deserialize<T>(data);

            return entity.Value;
        }

        public void SaveObjects<T>(string file, IEnumerable<T> entities)
        {
            var path = CombineFilePath(file);
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());
            
            var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            var binaryWriter = new BinaryWriter(fs);

            var data = Serializer.Serialize(entities.ToList());
            binaryWriter.Write(data);

            fs.Close();
        }

        /// <summary>
        /// Загружает все сущности и игнорирует несериализованные значения.
        /// </summary>
        public Option<List<T>> LoadObjects<T>(string file)
        {
            var path = CombineFilePath(file);
            if (!File.Exists(path))
                return new List<T>();

            var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
            var sr = new BinaryReader(fs);

            var list = Serializer.Deserialize<List<T>>(sr.ReadBytes(int.MaxValue));
            if(list.Value == null)
                list = Option<List<T>>.None;
            
            fs.Close();
            return list;
        }

        public string CombineFilePath(string file)
        {
            return RootFolder + file + Extension;
        }

        public string CombineDirectoryPath(string directory)
        {
            return RootFolder + directory;
        }
    }
}