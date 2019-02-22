using CraftLogs.BLL.Enums;
using System;

namespace CraftLogs.BLL.Models
{
    public class QRResponse<T>
    {
        public DateTime Created { get; private set; }
        public QRTypeEnum Type { get; set; }
        public T AdditionalData { get; set; }

        public QRResponse(QRTypeEnum type, T additionalData)
        {
            Created = DateTime.Now;
            Type = type;
            AdditionalData = additionalData;
        }
    }
}
