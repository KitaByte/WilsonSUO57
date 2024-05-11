using System.IO;
using System.Linq;
using System.Text;

namespace Server.Custom.UOBuilder
{
    public struct UOBuilderEntity
    {
        public int E_ID { get; set; }
        public string E_Map { get; set; }
        public int E_X { get; set; }
        public int E_Y { get; set; }
        public int E_Z { get; set; }
        public int E_HUE { get; set; }

        public bool IsPlaced()
        {
            Map map = Map.Parse(E_Map);

            if (map != null)
            {
                var items = map.GetItemsInRange(new Point3D(E_X, E_Y, E_Z), 1).ToList();

                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        if (item is UOBuilderStatic)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void SaveEntity(StringBuilder sb)
        {
            sb.AppendLine($"{E_ID},{E_Map},{E_X},{E_Y},{E_Z},{E_HUE}");
        }

        public void LoadEntity(StreamReader reader)
        {
            string val = reader.ReadLine();

            if (!string.IsNullOrEmpty(val))
            {
                var vals = val.Split(',');

                if (vals.Length == 6)
                {
                    E_ID = int.Parse(vals[0]);
                    E_Map = vals[1];
                    E_X = int.Parse(vals[2]);
                    E_Y = int.Parse(vals[3]);
                    E_Z = int.Parse(vals[4]);
                    E_HUE = int.Parse(vals[5]);
                }
            }
        }
    }
}
