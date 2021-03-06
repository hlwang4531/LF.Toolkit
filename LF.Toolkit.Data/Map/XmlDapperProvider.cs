﻿using LF.Toolkit.Data.Map;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace LF.Toolkit.Data.Map
{
    public class XmlDapperProvider : IDapperProvider
    {
        public IDictionary<string, IDapperMapping> DapperMappings { get; private set; }

        /// <summary>
        /// 映射配置,必须在使用映射前执行
        /// </summary>
        /// <param name="path"></param>
        public void Configure(string path)
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, "*.xml");
                if (files.Length > 0)
                {
                    //并发字典
                    var condict = new ConcurrentDictionary<string, IDapperMapping>();
                    //并发加载
                    files.AsParallel().ForAll(file =>
                    {
                        var xd = new XmlDocument();
                        var ms = new MemoryStream();

                        try
                        {
                            xd.Load(file);
                            xd.Save(ms);
                            ms.Position = 0;

                            //声明一个XML序列化器
                            XmlSerializer serializer = new XmlSerializer(typeof(DapperMapping));
                            //反序列化
                            var mapping = (DapperMapping)serializer.Deserialize(ms);

                            if (mapping != null)
                            {
                                var cmds = mapping.Commands.ToDictionary(i => i.CommandKey, i => i as IDapperCommand);
                                //去除换行符
                                foreach (var cmd in cmds)
                                {
                                    cmd.Value.CommandText = cmd.Value.CommandText.Trim().Trim(new char[] { '\r', '\n' });
                                }

                                mapping.CommandDictionary = cmds;
                                condict.AddOrUpdate(mapping.Type, mapping, (k, v) => condict[k] = v);
                            }
                        }
                        finally
                        {
                            if (ms != null)
                            {
                                ms.Close();
                            }
                        }
                    });

                    //转化为字典
                    DapperMappings = condict.ToDictionary(i => i.Key, i => i.Value);
                }
                else
                {
                    throw new Exception("未找到XML映射文件");
                }
            }
            else
            {
                throw new Exception("未找到XML映射文件目录");
            }
        }
    }
}
