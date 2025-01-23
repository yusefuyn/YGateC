using System.Reflection;

namespace YGate.Mapping
{
    public static class Operations
    {
        public static T Convert<T>(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            T? returned = Activator.CreateInstance<T>();
            PropertyInfo[]? objProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[]? targetProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var objProp in objProperties)
            {
                var targetProp = Array.Find(targetProperties, p => p.Name == objProp.Name && p.PropertyType == objProp.PropertyType);
                if (targetProp != null && targetProp.CanWrite)
                {
                    var value = objProp.GetValue(obj);
                    targetProp.SetValue(returned, value);
                }
            }
            return returned;
        }

        public static List<T> ConvertToList<T>(object objs)
        {
            List<T> returnedList = new List<T>();

            if (objs == null)
                throw new ArgumentNullException(nameof(objs));

            IEnumerable<object> objectList = objs as IEnumerable<object>;

            if (objectList == null)
                throw new ArgumentException("The input is not a valid list of objects", nameof(objs));

            objectList.ToList().ForEach(obj => returnedList.Add(Convert<T>(obj)));
            return returnedList;
        }
    }
}
