using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Serialization
{
    interface IHasSerializableData<DataType>
    {
        DataType GetSerializableData();

        void SetFromSerializableData(DataType data);
    }
}
