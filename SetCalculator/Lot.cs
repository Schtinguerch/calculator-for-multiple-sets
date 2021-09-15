using System;
using System.Collections;
using System.Collections.Generic;

namespace SetCalculator
{
    public class Lot<T> : IEnumerable<T>
    {
        public int Count => _containerList.Count;

        private List<T> _containerList;
        
        public Lot() => _containerList = new List<T>();
        
        public Lot(IEnumerable<T> startCollection)
        {
            _containerList = new List<T>();
            AddCollection(startCollection);
        }

        public void AddCollection(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                Add(item);
        }

        public void Add(T newItem)
        {   
            foreach (var oldItem in _containerList)
                if (oldItem.Equals(newItem))
                    throw new ArgumentException(
                        "Error: the lot cannot contain " +
                        " duplicate items, because the " +
                        "lots doesn't have an order");

            _containerList.Add(newItem);
        }

        public void RemoveValue(T value) =>
            _containerList.Remove(value);

        public override string ToString()
        {
            if (_containerList.Count == 0) return "{ ∅ }";

            var stringData = _containerList[0].ToString();

            for (int i = 1; i < _containerList.Count; i++)
                stringData += $", {_containerList[i]}";

            stringData = "{ " + stringData + " }";
            return stringData;
        }

        public IEnumerator<T> GetEnumerator() => _containerList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _containerList.GetEnumerator();
    }
}
