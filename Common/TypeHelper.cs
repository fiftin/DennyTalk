using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace Remoting
{

    public delegate bool ForEachFieldDelegate(string pathName, object value);

    public class TypeHelper
    {
        static public string FullDumpNameToShort(string fullname)
        {
            string filename = System.IO.Path.GetFileName(fullname);
            Regex r = new Regex(@"^S_(?<ppsid>\d+)_(?<id>\d+)_(?<num>\d+)_[\d_]*(?<res>\w?)\.mmk$");
            Match m = r.Match(filename);
            if (m.Success)
            {
                string res = m.Groups["res"].Value;
                res = res != "" ? "_"+res : "";
                return string.Format("$_{0}_{1}_{2}{3}", m.Groups["ppsid"], m.Groups["id"], m.Groups["num"], res);
            }
            else
            {
                if (filename.Length > 40)
                    return filename.Substring(0, 40);
                else
                    return filename;
            }
        }
        /// <summary>
        /// Определяет является ли тип <typeparamref name="type"/> встроинным типом (int, char, float, ...).
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public bool IsInternalType(Type type)
        {
            return (type == typeof(Int32)
                || type == typeof(Char)
                || type == typeof(Double)
                || type == typeof(Single)
                || type == typeof(Int16));
        }

        /// <summary>
        /// Возвращает массив полей структуры,
        /// имеющих простой тип (int, double, string, ...) или
        /// являющихся массивом простых типов
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FieldInfo[] GetNodeFields(Type type)
        {
            List<FieldInfo> result = new List<FieldInfo>();
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                Type fieldType = field.FieldType;
                if (fieldType.IsArray)
                {
                    Type elementType = fieldType.GetElementType();
                    if (!IsInternalType(elementType))
                    {
                        result.Add(field);
                    }
                }
                else if (!IsInternalType(fieldType))
                {
                    result.Add(field);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Возвращает массив полей структуры,
        /// имеющих составной тип (т.е. тоже являющихся структурами)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FieldInfo[] GetValueFields(Type type)
        {
            List<FieldInfo> result = new List<FieldInfo>();
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                Type fieldType = field.FieldType;
                if (fieldType.IsArray)
                {
                    Type elementType = fieldType.GetElementType();
                    if (IsInternalType(elementType))
                    {
                        result.Add(field);
                    }
                }
                else if (IsInternalType(fieldType))
                {
                    result.Add(field);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Возвращает тип поля по его пути
        /// </summary>
        /// <param name="type"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        static public Type GetType(Type type, string path)
        {
            string[] fieldNames = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string fieldName in fieldNames)
            {
                Regex r = new Regex(@"^([\w_][\w\d_]*)\[(\d+)\]$");
                Match m = r.Match(fieldName);
                if (m.Success)
                {
                    string arrayName = m.Groups[1].Value;
                    int arrayIndex = int.Parse(m.Groups[2].Value);
                    FieldInfo field = type.GetField(arrayName);
                    type = field.FieldType.GetElementType();
                }
                else
                {
                    FieldInfo field = type.GetField(fieldName);
                    type = field.FieldType;
                }
            }
            return type;
        }

        private static bool TryMatch(Regex r, string p, out Match m)
        {
            m = r.Match(p);
            return m.Success;
        }

        /// <summary>
        /// Возвращает значение поля объекта obj (или подъобекта) по его пути
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        static public object GetValue(object obj, string path)
        {
            Regex r1 = new Regex(@"^([\w_][\w\d_]*)\[(\d+)\]$");
            Regex r2 = new Regex(@"^([\w_][\w\d_]*)\[(\d+)\]\[(\d+)\]$");
            Regex r3 = new Regex(@"^([\w_][\w\d_]*)\[(\d+)\]\[(\d+)\]\[(\d+)\]$");
            Regex r4 = new Regex(@"^([\w_][\w\d_]*)\[(\d+)\]\[(\d+)\]\[(\d+)\]\[(\d+)\]$");

            string[] fieldNames = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string fieldName in fieldNames)
            {
                Type type = obj.GetType();
                Match m;
                object value;
                if (TryMatch(r1, fieldName, out m))
                {
                    string arrayName = m.Groups[1].Value;
                    int arrayIndex = int.Parse(m.Groups[2].Value);
                    FieldInfo field = type.GetField(arrayName);
                    value = ((Array)field.GetValue(obj)).GetValue(arrayIndex);
                }
                else if (TryMatch(r2, fieldName, out m))
                {
                    string arrayName = m.Groups[1].Value;
                    int arrayIndex = int.Parse(m.Groups[2].Value) * int.Parse(m.Groups[3].Value);
                    FieldInfo field = type.GetField(arrayName);
                    value = ((Array)field.GetValue(obj)).GetValue(arrayIndex);
                }
                else if (TryMatch(r3, fieldName, out m))
                {
                    string arrayName = m.Groups[1].Value;
                    int arrayIndex = int.Parse(m.Groups[2].Value) * int.Parse(m.Groups[3].Value) * int.Parse(m.Groups[4].Value);
                    FieldInfo field = type.GetField(arrayName);
                    value = ((Array)field.GetValue(obj)).GetValue(arrayIndex);
                }
                else if (TryMatch(r4, fieldName, out m))
                {
                    string arrayName = m.Groups[1].Value;
                    int arrayIndex = int.Parse(m.Groups[2].Value) * int.Parse(m.Groups[3].Value) * int.Parse(m.Groups[4].Value) * int.Parse(m.Groups[5].Value);
                    FieldInfo field = type.GetField(arrayName);
                    value = ((Array)field.GetValue(obj)).GetValue(arrayIndex);
                }
                else
                {
                    FieldInfo field = type.GetField(fieldName);
                    value = field.GetValue(obj);
                }
                obj = value;
            }
            return obj;
        }

        /// <summary>
        /// Для всех скалярных полей объекта и всех его подъобектов
        /// вызывает делегат <paramref name="p"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        static public bool ForEachField(object obj, string path, ForEachFieldDelegate p)
        {
            Type type = obj.GetType();
            FieldInfo[] valueFields = GetValueFields(type);
            FieldInfo[] nodeFields = GetNodeFields(type);
            foreach (FieldInfo valueField in valueFields)
            {
                string pathName = path + "\\" + valueField.Name;
                object val = valueField.GetValue(obj);
                if (valueField.FieldType.IsArray)
                {
                    Array arr = (Array)val;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        object itemVal = arr.GetValue(i);
                        if (!p(pathName + "[" + i + "]", itemVal))
                            return false;
                    }
                }
                else
                {
                    if (!p(pathName, val))
                        return false;
                }
            }
         
            foreach (FieldInfo nodeField in nodeFields)
            {
                object subObj = nodeField.GetValue(obj);
                string pathName = path + "\\" + nodeField.Name;
                Type nodeType = subObj.GetType();
                if (nodeType.IsArray)
                {
                    Array arr = (Array)subObj;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (!ForEachField(arr.GetValue(i), pathName + "[" + i + "]", p))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (!ForEachField(subObj, pathName, p))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void SetValue(object obj, string fieldPathName, object fieldValue)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (fieldPathName == null)
                throw new ArgumentNullException("fieldPathName");
            if (fieldPathName == "")
                throw new ArgumentException("", "fieldPathName");

            string[] fieldPath = fieldPathName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            object currObj = obj;
            for (int i = 0; i < fieldPath.Length; i++)
            {
                string fieldName = fieldPath[i];
                Regex r = new Regex(@"^(?<name>[\w\d]+)\[(?<index>\d+)\]$");
                Match m = r.Match(fieldName);
                if (m.Success)
                    fieldName = m.Groups["name"].Value;
                Type currType = currObj.GetType();
                FieldInfo field = currType.GetField(fieldName);

                if (i != fieldPath.Length - 1) // если НЕ последний элемент массива fieldPath
                {
                    currObj = field.GetValue(currObj);
                    if (m.Success)
                    {
                        int index = int.Parse(m.Groups["index"].Value);
                        currObj = ((Array)currObj).GetValue(index);
                    }
                }
                else
                {
                    if (m.Success) // если присвоение идет элементу массива
                    {
                        int index = int.Parse(m.Groups["index"].Value);
                        Array arr = (Array)field.GetValue(currObj);
                        arr.SetValue(fieldValue, index);
                    }
                    else
                    {
                        field.SetValue(currObj, fieldValue);
                    }
                }
            }
        }

        public static JObject GetJsonFromStructure(object structure)
        {
            //throw new NotImplementedException();
            JObject ret = new JObject();
            FieldInfo[] fields = TypeHelper.GetValueFields(structure.GetType());
            //tel.Fields.Clear();
            foreach (FieldInfo field in fields)
            {
                Type fieldType = field.FieldType;
                if (fieldType.IsArray)
                {
                    if (fieldType.GetElementType() != typeof(char))
                    {
                        object[] obj1 = field.GetCustomAttributes(typeof(MarshalAsAttribute), false);
                        MarshalAsAttribute marshalAsAttr = null;
                        if (obj1.Length > 0)
                        {
                            marshalAsAttr = (MarshalAsAttribute)obj1[0];
                        }

                        TwoDimensionalArrayAttribute attr2 = null;
                        ThreeDimensionalArrayAttribute attr3 = null;
                        object[] attrs = field.GetCustomAttributes(typeof(TwoDimensionalArrayAttribute), false);
                        if (attrs.Length > 0)
                        {
                            attr2 = (TwoDimensionalArrayAttribute)attrs[0];
                        }
                        else
                        {
                            attrs = field.GetCustomAttributes(typeof(ThreeDimensionalArrayAttribute), false);
                            if (attrs.Length > 0)
                            {
                                attr3 = (ThreeDimensionalArrayAttribute)attrs[0];
                            }
                        }

                        for (int i = 0; i < marshalAsAttr.SizeConst; i++)
                        {
                            string name;
                            if (attr2 != null)
                            {
                                int x = i / attr2.Y;
                                int y = i - x * attr2.Y;
                                name = string.Format("{0}[{1}][{2}]", field.Name, x, y);
                            }
                            else if (attr3 != null)
                            {
                                int x = i / (attr3.Z * attr3.Y);
                                int y = (i - x * (attr3.Z * attr3.Y)) / attr3.Z;
                                int z = i - x * (attr3.Z * attr3.Y) - y * attr3.Z;
                                name = string.Format("{0}[{1}][{2}][{3}]", field.Name, x, y, z);
                            }
                            else
                            {
                                name = string.Format("{0}[{1}]", field.Name, i);
                            }
                            object value = ((Array)field.GetValue(structure)).GetValue(i);
                            ret.Add(name, new JValue(value));
                        }
                    }
                    else
                    {

                        object[] obj1 = field.GetCustomAttributes(typeof(MarshalAsAttribute), false);
                        MarshalAsAttribute marshalAsAttr = null;
                        if (obj1.Length > 0)
                        {
                            marshalAsAttr = (MarshalAsAttribute)obj1[0];
                        }

                        TwoDimensionalArrayAttribute attr2 = null;
                        ThreeDimensionalArrayAttribute attr3 = null;
                        object[] attrs = field.GetCustomAttributes(typeof(TwoDimensionalArrayAttribute), false);
                        if (attrs.Length > 0)
                        {
                            attr2 = (TwoDimensionalArrayAttribute)attrs[0];
                        }
                        else
                        {
                            attrs = field.GetCustomAttributes(typeof(ThreeDimensionalArrayAttribute), false);
                            if (attrs.Length > 0)
                            {
                                attr3 = (ThreeDimensionalArrayAttribute)attrs[0];
                            }
                        }

                        if (attr2 != null)
                        {
                            char[][] chars = new char[attr2.X][];
                            for (int k = 0; k < attr2.X; k++)
                            {
                                chars[k] = new char[attr2.Y];
                            }

                            for (int i = 0; i < marshalAsAttr.SizeConst; i++)
                            {
                                int x = i / attr2.Y;
                                int y = i - x * attr2.Y;
                                chars[x][y] = (char)((Array)field.GetValue(structure)).GetValue(i);
                            }

                            for (int k = 0; k < attr2.X; k++)
                            {
                                string str = new string(chars[k]);
                                str = str.Trim('\0');
                                ret.Add(string.Format("{0}[{1}]", field.Name, k), str);
                            }
                        }
                        else if (attr3 != null)
                        {
                            char[][][] chars = new char[attr3.X][][];
                            for (int k = 0; k < attr3.X; k++)
                            {
                                chars[k] = new char[attr3.Y][];
                                for (int q = 0; q < attr3.Y; q++)
                                {
                                    chars[k][q] = new char[attr3.Z];
                                }
                            }
                            for (int i = 0; i < marshalAsAttr.SizeConst; i++)
                            {
                                int x = i / (attr3.Z * attr3.Y);
                                int y = (i - x * (attr3.Z * attr3.Y)) / attr3.Z;
                                int z = i - x * (attr3.Z * attr3.Y) - y * attr3.Z;
                                chars[x][y][z] = (char)((Array)field.GetValue(structure)).GetValue(i);
                            }

                            for (int a = 0; a < attr3.X; a++)
                            {
                                for (int b = 0; b < attr3.Y; b++)
                                {
                                    string str = new string(chars[a][b]);
                                    str = str.Trim('\0');
                                    ret.Add(string.Format("{0}[{1}][{2}]", field.Name, a, b), str);
                                }
                            }
                        }
                        else
                        {
                            char[] chars = (char[])field.GetValue(structure);
                            string str = new string(chars);
                            str = str.Trim('\0');
                            ret.Add(field.Name, str);
                        }
                    }
                }
                else
                    ret.Add(field.Name, new JValue(field.GetValue(structure)));
            }
            return ret;
        }
    }

    internal class ArrayElement
    {
        private int index;
        private Array array;

        public ArrayElement(int index, Array array)
        {
            Array = array;
            Index = index;
        }

        public Array Array
        {
            get { return array; }
            private set { array = value; }
        }

        public object Value
        {
            get { return array.GetValue(Index); }
            set { array.SetValue(value, Index); }
        }


        public int Index
        {
            get { return index; }
            private set { index = value; }
        }

    }

    internal class DummyTag
    {
    }

}
