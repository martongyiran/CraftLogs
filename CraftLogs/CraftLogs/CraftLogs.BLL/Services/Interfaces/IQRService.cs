using CraftLogs.BLL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CraftLogs.BLL.Services.Interfaces
{
    public interface IQRService
    {
        string CreateQR<T>(T data);

        void HandleQR(string scanResult); //TODO
    }
}
