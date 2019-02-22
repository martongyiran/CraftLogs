using CraftLogs.BLL.Enums;
using CraftLogs.BLL.Models;
using CraftLogs.BLL.Services.Interfaces;
using Newtonsoft.Json;
using System;

namespace CraftLogs.BLL.Services
{
    public class QRService : IQRService
    {
        public string CreateQR<T>(T data)
        {
            if (typeof(QuestReward) == data.GetType())
            {
                QRResponse<QuestReward> response = new QRResponse<QuestReward>(QRTypeEnum.Reward, data as QuestReward);

                return JsonConvert.SerializeObject(response);
            }

            return "";
        }

        public void HandleQR(string scanResult)
        {
            throw new NotImplementedException();
        }
    }
}
