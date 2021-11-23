using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace csharp_prednaska_10
{
    [DataContract]
    public class Student
    {
        [DataMember]
        public int Age { get; set; } = 0;

        [DataMember(Name="NameOfStudent")]
        public string Name{ get; set; } = "";

        [OnSerialized]
        public void OnSerialized(StreamingContext s) => Console.WriteLine("AAA");
    }
    public class Program
    {
        public static void Serializuj(string fileName, object o)
        {
            try
            {
                var serializer = new DataContractSerializer(o.GetType());
                using (var stream = new FileStream(fileName,FileMode.Create, FileAccess.Write))
                {
                    using (XmlDictionaryWriter w = XmlDictionaryWriter.CreateBinaryWriter(stream))
                    {
                        serializer.WriteObject(w, o);
                    }
                    //serializer.WriteObject(stream, o);
                }
            }
            catch (SerializationException se)
            {
                Console.WriteLine(se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        public static Student Deserializuj(string fileName)
        {
            Student tmp = null;
            try
            {
                var serializer = new DataContractSerializer(typeof(Student));
                using (var stream = new FileStream(fileName,FileMode.Open, FileAccess.Read))
                {
                    using (XmlDictionaryReader r = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
                    {
                        tmp = serializer.ReadObject(r) as Student;
                    }
                    tmp = serializer.ReadObject(stream) as Student;
                }
            }
            catch (SerializationException se)
            {
                Console.WriteLine(se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return tmp;

        }
        public static void Main(string[] args)
        {
            Student s = new Student{ Age = 20, Name="Jano"};
            Serializuj("aaa.xml", s);

            Student b = Deserializuj("aaa.xml");
        }
    }
}
