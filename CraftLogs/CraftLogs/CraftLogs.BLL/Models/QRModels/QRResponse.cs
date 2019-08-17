/*
Copyright 2018 Gyirán Márton Áron

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. 
*/

using CraftLogs.BLL.Enums;
using System;

namespace CraftLogs.BLL.Models
{
    public class QRResponse<T>
    {
        public QRTypeEnum Type { get; set; }
        public string D { get; set; }

        /// <summary>
        /// For serialization.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="additionalData"></param>
        public QRResponse(QRTypeEnum type, string additionalData)
        {
            Type = type;
            D = additionalData;
        }

    }

    public class QRResponse
    {
        public QRTypeEnum Type { get; set; }
        public string D { get; set; }

        public QRResponse(QRTypeEnum type, string additionalData)
        {
            Type = type;
            D = additionalData;
        }
    }
}
