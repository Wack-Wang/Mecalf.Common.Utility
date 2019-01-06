using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Abp.Application.Services.Dto;
using Castle.Core.Internal;
using Mecalf.Web.Framework.Services;

namespace Mecalf.Web.Framework.ClientBuilder
{
    /// <summary>
    /// 模板解释器
    /// </summary>
    public class TemplateInterpreter
    {
        private byte[] tagBeginSign;
        private byte[] tagEndSign;
        private byte[] dtoSeeTemplateBeginSign;
        private byte[] dtoSeeTemplateEndSign;
        private BinaryReader templateReader;
        private BinaryWriter templateWriter;
        private string templatePath;
        private string savePagePath;
        private int lineNumber = 1;
        private int columnNumber = 1;

        /// <summary>
        /// 编码方案
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 解释器中的所有变量
        /// </summary>
        public Dictionary<string, string> Values { get; set; }

        /// <summary>
        /// 解释器中所有的Dto
        /// </summary>
        public Dictionary<string, Type> DtoTypes { get; set; }

        public TemplateInterpreter()
        {
            tagBeginSign = new byte[] { (byte)'{', (byte)'@' };
            tagEndSign = new byte[] { (byte)'@', (byte)'}' };
            dtoSeeTemplateBeginSign = new byte[] { (byte)'{', (byte)'$' };
            dtoSeeTemplateEndSign = new byte[] { (byte)'$', (byte)'}' };
            DtoTypes = new Dictionary<string, Type>();
            Values = new Dictionary<string, string>();
            //dtoTypes["CreateDto"] = typeof(PagedAndSortedGetInput);
            Encoding = Encoding.UTF8;
            //DtoTypes["GetDto"] = typeof(PagedAndSortedGetInput);
            //ValueSet("EntityName", "Test!");
            //ValueSet("dto.prop", "Test2!");
        }

        /// <summary>
        /// 函数调用,调用解析到的函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected void FuncCall(string name, string[] args)
        {
            switch (name)
            {
                case "DtoSee":
                    {
                        DtoSee(args[0], args.Length > 1 ? args[1] : null);
                    }
                    break;
                case "LowerStr":
                    {
                        LowerStr(args[0]);
                    }
                    break;
            }
        }

        private void WriteData(string data)
        {
            templateWriter.Write(Encoding.GetBytes(data));
        }
        private void WriteData(byte data)
        {
            templateWriter.Write(data);
        }
        private void WriteData(byte[] data)
        {
            templateWriter.Write(data);
        }
        /// <summary>
        /// 变量读取,从环境中读取指定的属性
        /// </summary>
        protected void LowerStr(string name)
        {
            WriteData(Values[name].ToLowerInvariant());
        }
        /// <summary>
        /// 变量读取,从环境中读取指定的属性
        /// </summary>
        protected string ValueGet(string name)
        {
            try
            {
                return Values[name];
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException($" 给定的Key为:{name}，在文件{templatePath},在第{lineNumber}行,第{columnNumber}列", e);
            }
        }
        /// <summary>
        /// 变量读取,从环境中读取指定的属性
        /// </summary>
        protected void ValueDel(string name)
        {
            if (Values.ContainsKey(name))
            {
                Values.Remove(name);
            }
        }
        /// <summary>
        /// 变量值设置,设置环境中指定变量的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void ValueSet(string name, string value)
        {
            Values[name] = value;
        }

        /// <summary>
        /// 处理标签内容
        /// </summary>
        /// <param name="tagContent"></param>
        private void HandleTag(string tagContent)
        {
            if (tagContent.Contains("(") && tagContent.Contains(")"))
            {
                var parts = tagContent.TrimEnd(' ').Split('(', ')');
                var funcName = parts[0];
                var args = parts.Length > 1 ? parts[1].Split(',') : null;

                FuncCall(funcName, args);
            }
            else
            {
                WriteData(ValueGet(tagContent));
            }

        }

        /// <summary>
        /// 配置需要生成页面的Crud服务
        /// </summary>
        /// <typeparam name="TEntityDto"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <typeparam name="TCreateInput"></typeparam>
        /// <typeparam name="TUpdateInput"></typeparam>
        /// <typeparam name="TGetAllInput"></typeparam>
        /// <param name="service"></param>
        public void SetCrudService<TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput, TGetAllInput>(
            ICrudAppService<TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput, TGetAllInput> service)
            where TEntityDto : IEntityDto<TPrimaryKey>
            where TCreateInput : IEntityDto<TPrimaryKey>
            where TUpdateInput : IEntityDto<TPrimaryKey>
        {
            DtoTypes.Clear();
            DtoTypes["Entity"] = typeof(TEntityDto);
            DtoTypes["EntityDto"] = typeof(TEntityDto);
            DtoTypes["CreateDto"] = typeof(TCreateInput);
            DtoTypes["UpdateDto"] = typeof(TUpdateInput);
            DtoTypes["PrimaryKey"] = typeof(TPrimaryKey);
            DtoTypes["GetAllDto"] = typeof(TGetAllInput);

            var entityName = typeof(TEntityDto).Name;
            if (entityName.EndsWith("Dto"))
            {
                entityName = entityName.Substring(0, entityName.Length - 3);
            }
            ValueSet("EntityName", entityName);
        }

        /// <summary>
        /// 开始生成页面
        /// </summary>
        public void Build(string path, string savePath)
        {
            templatePath = path;
            savePagePath = savePath;
            lineNumber = 1;
            columnNumber = 1;
            templateReader = new BinaryReader(new FileStream(path, FileMode.Open));
            templateWriter = new BinaryWriter(new FileStream(savePath, FileMode.Create));
            MainLoop(templateReader, templateWriter);
        }

        /// <summary>
        /// 批量获取模板和生成页面
        /// </summary>
        /// <param name="pathDir"></param>
        /// <param name="saveDir"></param>
        public void BuildMany(string pathDir, string saveDir)
        {
            var temps = System.IO.Directory.GetFiles(pathDir);
            foreach (var path in temps)
            {
                var saveFolder = Path.Combine(saveDir, ValueGet("EntityName"));
                if (System.IO.Directory.Exists(saveFolder) == false)
                {
                    System.IO.Directory.CreateDirectory(saveFolder);
                }
                var savePath = Path.Combine(saveFolder, System.IO.Path.GetFileName(path));
                Build(path, savePath);
            }
        }

        /// <summary>
        /// 主循环，从头到尾扫描模板流。遇到 {@ 时直接调用相关的解释方法,然后解析接下来的内容.直到遇到 @} 后返回到主循环中.
        /// </summary>
        protected void MainLoop(BinaryReader reader, BinaryWriter writer, bool tracePos = true)
        {
            List<byte> tagContent = new List<byte>();//标签模式下读取到的内容全部放到这里,直到标签结束时调用对应的方法
            List<byte> buffer = new List<byte>();//所有判断模式下,数据读取到的数据都将暂存在这块

            LoopStatus status = LoopStatus.文本读取;
            byte current = 0;

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                current = reader.ReadByte();
                if (tracePos)
                {
                    if (current == '\n')
                    {
                        lineNumber++;
                        columnNumber = 1;
                    }
                    else
                    {
                        columnNumber++;
                    }
                }
                switch (status)
                {
                    case LoopStatus.文本读取:
                        {
                            //文本模式下,需要时刻判断是否有标签开始
                            if (current == tagBeginSign[0])
                            {
                                status = LoopStatus.标签头判断;
                                buffer.Clear();
                                buffer.Add(current);
                            }
                            else
                            {
                                writer.Write(current);
                            }
                        }
                        break;
                    case LoopStatus.标签头判断:
                        {
                            //判断标签头状态,如果

                            if (current == tagBeginSign[buffer.Count])
                            {
                                buffer.Add(current);
                                //如果缓存的信息已经和标签头完全匹配,则进入标签内容读取阶段
                                if (tagBeginSign.Length == buffer.Count && tagBeginSign.Except(buffer).IsNullOrEmpty())
                                {
                                    status = LoopStatus.标签内容读取;
                                }
                            }
                            else//和标签头不匹配,将缓存的内容写入到流中,并将状态回滚到文本模式
                            {
                                //假设起始标记为:{{@,结束标记为:@}},那么遇到{{{@A@}}}时,需要特殊处理
                                //如果当前的字符不能和之前已存放的数据一起组合成起始标记,则 将buffer中最前方的一个字符视为普通文本并写入到流中,
                                buffer.Add(current);
                                do
                                {
                                    WriteData(buffer[0]);
                                    buffer.RemoveAt(0);
                                } while (StartWith(tagBeginSign, buffer) == false);

                                if (buffer.Count == 0)
                                {
                                    //buffer.Add(current);
                                    writer.Write(buffer.ToArray());
                                    status = LoopStatus.文本读取;
                                    buffer.Clear();
                                }
                            }
                        }
                        break;
                    case LoopStatus.标签内容读取:
                        {
                            //标签模式下,判断是否是标签结束标记
                            if (current == tagEndSign[0])
                            {
                                status = LoopStatus.标签尾判断;
                                buffer.Clear();
                                buffer.Add(current);
                            }
                            else
                            {
                                tagContent.Add(current);
                            }
                        }
                        break;
                    case LoopStatus.标签尾判断:
                        {
                            //判断是否满足标签结束要求,当完全匹配标签尾时,将状态跳转到文本模式,


                            if (current == tagEndSign[buffer.Count])
                            {
                                buffer.Add(current);
                                //如果缓存的信息已经和标签尾完全匹配,则进入标签内容处理阶段
                                if (tagEndSign.Length == buffer.Count && tagEndSign.Except(buffer).IsNullOrEmpty())
                                {
                                    HandleTag(Encoding.GetString(tagContent.ToArray()));
                                    tagContent.Clear();
                                    status = LoopStatus.文本读取;
                                }
                            }
                            else//和标签尾不匹配,将缓存的内容写入到标签内容中,并将状态回滚到标签内容模式
                            {
                                tagContent.AddRange(buffer.ToArray());
                                buffer.Clear();
                                status = LoopStatus.标签内容读取;
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            writer.Flush();
        }

        private bool StartWith<T>(byte[] list, List<T> startList)
        {
            for (int i = 0; i < startList.Count; i++)
            {
                if (list[i].Equals(startList[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 读取实体属性遍历时,为每个属性生成内容时使用的模板
        /// </summary>
        /// <returns></returns>
        protected byte[] ReadDtoSeeTemplate()
        {
            List<byte> templateContent = new List<byte>();//标签模式下读取到的内容全部放到这里,直到标签结束时调用对应的方法
            List<byte> buffer = new List<byte>();//所有判断模式下,数据读取到的数据都将暂存在这块

            LoopStatus status = LoopStatus.文本读取;
            byte current = 0;

            while (templateReader.BaseStream.Position < templateReader.BaseStream.Length)
            {
                current = templateReader.ReadByte();
                if (current == '\n')
                {
                    lineNumber++;
                    columnNumber = 1;
                }
                else
                {
                    columnNumber++;
                }
                switch (status)
                {
                    case LoopStatus.文本读取:
                        {
                            //文本模式下,需要时刻判断是否有标签开始
                            if (current == dtoSeeTemplateBeginSign[0])
                            {
                                status = LoopStatus.标签头判断;
                                buffer.Clear();
                                buffer.Add(current);
                            }
                            else
                            {
                                WriteData(current);
                            }
                        }
                        break;
                    case LoopStatus.标签头判断:
                        {
                            //判断标签头状态,如果

                            if (current == dtoSeeTemplateBeginSign[buffer.Count])
                            {
                                buffer.Add(current);
                                //如果缓存的信息已经和标签头完全匹配,则进入模板内容读取阶段
                                if (dtoSeeTemplateBeginSign.Length == buffer.Count && dtoSeeTemplateBeginSign.Except(buffer).IsNullOrEmpty())
                                {
                                    status = LoopStatus.标签内容读取;
                                }
                            }
                            else//和标签头不匹配,将缓存的内容写入到流中,并将状态回滚到文本模式
                            {
                                buffer.Add(current);
                                WriteData(buffer.ToArray());
                                status = LoopStatus.文本读取;
                                buffer.Clear();
                            }
                        }
                        break;
                    case LoopStatus.标签内容读取:
                        {
                            //标签模式下,判断是否是标签结束标记
                            if (current == dtoSeeTemplateEndSign[0])
                            {
                                status = LoopStatus.标签尾判断;
                                buffer.Clear();
                                buffer.Add(current);
                            }
                            else
                            {
                                templateContent.Add(current);
                            }
                        }
                        break;
                    case LoopStatus.标签尾判断:
                        {
                            //判断是否满足标签结束要求,当完全匹配标签尾时,将状态跳转到文本模式,


                            if (current == dtoSeeTemplateEndSign[buffer.Count])
                            {
                                buffer.Add(current);
                                //如果缓存的信息已经和标签尾完全匹配,则进入标签内容处理阶段
                                if (dtoSeeTemplateEndSign.Length == buffer.Count && dtoSeeTemplateEndSign.Except(buffer).IsNullOrEmpty())
                                {
                                    return templateContent.ToArray();
                                }
                            }
                            else//和标签尾不匹配,将缓存的内容写入到标签内容中,并将状态回滚到标签内容模式
                            {
                                buffer.Add(current);
                                templateContent.AddRange(buffer);
                                status = LoopStatus.标签内容读取;
                                buffer.Clear();
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            throw new Exception($"模板内容有误!文件:{templatePath}");
        }


        /// <summary>
        /// 遍历Dto中的所有属性,并用指定的模板进行显示
        /// </summary>
        /// <param name="dtoName">Dto的名称</param>
        /// <param name="containsId">遍历的属性中,是否需要包含Id字段</param>
        public void DtoSee(string dtoName, string containsId)
        {
            if (DtoTypes.ContainsKey(dtoName) == false)
            {
                throw new KeyNotFoundException("找不到给定名称的Dto");
            }

            var innerTemplate = ReadDtoSeeTemplate();
            var dtoProperties = DtoTypes[dtoName].GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();

            if ("true".Equals(containsId, StringComparison.CurrentCultureIgnoreCase) == false)
            {
                var idProperty = dtoProperties.FirstOrDefault(info => info.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase));
                if (idProperty != null)
                {
                    dtoProperties.Remove(idProperty);
                }
            }
            var mStream = new MemoryStream(innerTemplate);
            foreach (var dtoProperty in dtoProperties)
            {
                ValueSet("dto.prop", dtoProperty.Name);
                ValueSet("dtoProp", dtoProperty.Name);
                MainLoop(new BinaryReader(mStream), templateWriter, false);
                mStream.Position = 0;
            }

            mStream.Dispose();
            ValueDel("dto.prop");
            ValueDel("dtoProp");

        }
    }

    enum LoopStatus
    {
        文本读取,
        标签头判断,
        标签内容读取,
        标签尾判断,
    }
}