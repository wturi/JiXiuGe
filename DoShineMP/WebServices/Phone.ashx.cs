using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoShineMP.WebServices
{
    /// <summary>
    /// Phone 的摘要说明
    /// </summary>
    public class Phone : IHttpHandler
    {

        /// <summary>
        /// 接口
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            #region 参数

            #region 默认参数

            var type = context.Request["type"];
            var callback = context.Request["callback"];
            if (type == null || callback == null)
            {
                return;
            }

            #endregion

            #region 公共参数

            string jsonStr = string.Empty;//返回json初始化
            string Classify = context.Request["classify"];//子项分类
            int number = 0, number1 = 0;

            #endregion

            #region 用户

            var Phone = context.Request["phone"];//手机号
            var Pwd = context.Request["pwd"];//密码
            var Send = context.Request["send"];//短信验证码
            var Sendid = context.Request["sendid"];
            #endregion

            #region 报修

            var name = context.Request["name"];//报修人姓名
            var content = context.Request["content"];//保修内容
            var villageid = context.Request["villageid"];//社区id
            var recordid = context.Request["recordid"];//报修地址记录id
            var repairid = context.Request["repairid"];//报修id
            var response = context.Request["response"];//评价内容 
            var score = context.Request["score"];//评价内容
            #endregion

            #endregion

            switch (type)
            {
                #region 登录注册

                //登录
                case "Login":
                    if (string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Send))
                    {
                        return;
                    }
                    if (!int.TryParse(Sendid, out number))
                    {
                        return;
                    }
                    jsonStr = $"{callback}({Helper.PhoneHelp.Login(Phone, Send, number)})";
                    break;
                case "LoginOnlyPhone":
                    if (string.IsNullOrEmpty(Phone))
                    {
                        return;
                    }
                    jsonStr = $"{callback}({Helper.PhoneHelp.Login(Phone)})";
                    break;
                //注册
                case "Register":
                    break;

                //发送验证短信 
                case "PhoneSend":
                    if (string.IsNullOrEmpty(Phone))
                    {
                        return;
                    }
                    jsonStr = $"{callback}({Helper.PhoneHelp.Send(Phone)})";
                    break;

                #endregion

                #region 数据处理
                case "Data":
                    if (string.IsNullOrEmpty(Phone))
                    {
                        return;
                    }
                    switch (Classify)
                    {
                        case "LatestRepair"://报修首页
                            jsonStr = $"{callback}({JsonConvert.SerializeObject(Helper.PhoneHelp.FindDataOfLasterRepairByPhone(Phone))})";
                            break;
                        case "RepairHistory"://历史纪录
                            jsonStr = $"{callback}({JsonConvert.SerializeObject(Helper.PhoneHelp.FindDataOfRepairHistory(Phone))})";
                            break;
                        case "Repair"://报修界面
                            jsonStr = $"{callback}({JsonConvert.SerializeObject(Helper.PhoneHelp.FindDataOfRepair(Phone))})";
                            break;
                        case "RepairDetails"://报修详情
                            if (!int.TryParse(repairid, out number))
                            {
                                return;
                            }
                            jsonStr = $"{callback}({JsonConvert.SerializeObject(Helper.PhoneHelp.RepairDetail(number))})";
                            break;
                        default:
                            return;
                    }
                    break;
                #endregion

                #region 报修
                ///报修
                case "Repair":
                    if (!int.TryParse(villageid, out number) || !int.TryParse(recordid, out number1))
                    {
                        return;
                    }
                    jsonStr = $"{callback}({JsonConvert.SerializeObject(Helper.PhoneHelp.Repair(content, Phone, number, name, number1))})";
                    break;
                //评价
                case "Repairspon":
                    if (!int.TryParse(repairid, out number) || !int.TryParse(score, out number1))
                    {
                        return;
                    }
                    jsonStr = $"{callback}({JsonConvert.SerializeObject(Helper.PhoneHelp.Repairspon(number, response, number1))})";
                    break;
                #endregion

                default:
                    return;
            }
            context.Response.Write(jsonStr);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}