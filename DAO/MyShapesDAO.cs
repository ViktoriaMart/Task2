using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Models;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace DAO
{
    public class MyShapesDAO
    {
        public void saveShapes(string fileName, ObservableCollection<MyShape> MyShapesCollection)
        {
            XmlSerializer formatter = new XmlSerializer(MyShapesCollection.GetType(), new Type[] { typeof(MyPolygon)});

            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                formatter.Serialize(fs, MyShapesCollection);
            }
        }

        public ObservableCollection<MyShape> getShapes(string fileName)
        {
            ObservableCollection<MyShape> MyShapesCollection;

            XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<MyShape>), new Type[] { typeof(MyPolygon)});

            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                MyShapesCollection = (ObservableCollection<MyShape>)formatter.Deserialize(fs);
            }

            return MyShapesCollection;
        }
    }
}
