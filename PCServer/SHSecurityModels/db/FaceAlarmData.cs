//卡口模型数据以及ftp中的KaKouData.json的反解析结构体

//mysql模型
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SHSecurityModels
{
    public class FaceAlarmData
    {
        public int Id { get; set; }
        public string alarmTime { get; set; }//发现时间
        public int timeStamp { get; set; }//记录时间
        public string cameraName { get; set; }//相机id
        public string position { get; set; }//发现地点
        public string alarmId { get; set; }//报警id
        public string humanId { get; set; }//人员id
        public string humanName{ get; set; }//人员id
        public string matchHumanList { get; set; }//匹配人员id信息

    }

    public class JsonFaceStruct
    {
        public int age;
        public string alarmId;
        public string alarmTime;
        public string bkgPicUrl;
        public string cameraName;
        public int cascadeAlarmId;
        public int controlId;
        public int ethnic;
        public string facePicUrl;
        public int glass;
        public string humanId;
        public string humanName;
        public List<Human> humans = new List<Human>();
        public string indexCode;
        public int listLibId;
        public int sex;
        public float similarity;
        public int smile;
        public int status;
    }
    public class Human
    {
        public string address;
        public string birthday;
        public string cityId;
        public string cityName;
        public string credentialsNum;
        public int credentialsType;
        public int ethnic;
        public string facePicUrl;
        public string humanId;
        public string humanName;
        public int listLibId;
        public string listLibName;
        public string provinceId;
        public string provinceName;
        public int sex;
        public float similarity;
        public int smile;

    }

    public class MatchSring
    {
        public List<string> list = new List<string>();
    }

}
